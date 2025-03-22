using System;
using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private GrapplingGun _grapple;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        [SerializeField] private Vector2 boxSize = new Vector2(1.35f, 0.03f);
        [SerializeField] private float boxCastDistance = .94f;
        [SerializeField] private Vector2 wallCheckSize = new Vector2(0.03f, 1.5f);
        [SerializeField] private float wallCheckDistance = .5f;


        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position - transform.up * boxCastDistance, boxSize);
            Gizmos.DrawWireCube(transform.position + transform.up * boxCastDistance, boxSize);
            Gizmos.DrawWireCube(transform.position + transform.right * wallCheckDistance, wallCheckSize);
            Gizmos.DrawWireCube(transform.position - transform.right * wallCheckDistance, wallCheckSize);
        }

        private void Awake()
        {
            _grapple = GetComponentInChildren<GrapplingGun>();
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;

            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.Space),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }

            updateDirection();
        }

        private void FixedUpdate()
        {
            if (isDashing && _time >= frameDashed + _stats.DashTime)
            {
                isDashing = false;
                EndDash();
            }
            else if (startedDash && _time >= frameDashed + _stats.DashBuffer){
                startedDash = false;
                Dash();
            }

            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            
            ApplyMovement();
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private float _frameLeftGrapple = float.MinValue;
        private bool _jumping;
        [HideInInspector] public bool _grounded;
        [HideInInspector] public bool facingRight = true;
        [HideInInspector] public MovingPlatform _platform = null;

        bool wallHitLeft = false;
        bool wallHitRight = false;
        bool bufferWallSliding = false;
        public bool isWallSliding = false;
        float _frameLeftWall = float.MinValue;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground, Ceiling, and Walls
            bool groundHit = Physics2D.BoxCast(transform.position, new Vector2(boxSize.x, _stats.GrounderDistance), 0, -transform.up, boxCastDistance, _stats.GroundLayer);
            bool ceilingHit = Physics2D.BoxCast(transform.position, new Vector2(boxSize.x, _stats.GrounderDistance), 0, transform.up, boxCastDistance, _stats.GroundLayer);
            wallHitLeft = Physics2D.BoxCast(transform.position, wallCheckSize, 0, -transform.right, wallCheckDistance, _stats.ClimbableLayer);
            wallHitRight = Physics2D.BoxCast(transform.position, wallCheckSize, 0, transform.right, wallCheckDistance, _stats.ClimbableLayer);

            // Hit a Ceiling.
            if (ceilingHit && !_grounded) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            //Sliding on Wall
            bufferWallSliding = (((wallHitLeft && _frameInput.Move.x == -1) || (wallHitRight && _frameInput.Move.x == 1)) && !_grounded && _frameVelocity.y < 0);

            if (!isWallSliding && bufferWallSliding)
            {
                isWallSliding = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
            }
            else if (isWallSliding && !bufferWallSliding)
            {
                isWallSliding = false;
                _frameLeftWall = _time;
            }

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            if (_jumping && _frameVelocity.y < 0){
                _jumping = false;
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && (_time < _frameLeftGrounded + _stats.CoyoteTime);
        private bool CanUseWallCoyote => _coyoteUsable && !isWallSliding && (_time < _frameLeftWall + _stats.CoyoteTime);
        private bool HasGrappleGrace => !_grounded && _time < _frameLeftGrapple + _stats.GrappleGrace;
        
        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.linearVelocity.y > 0 && _jumping) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if ((wallHitLeft || wallHitRight) || CanUseWallCoyote){
                ExecuteWallJump();
            }
            else if (_grounded || CanUseCoyote || HasGrappleGrace){
                ExecuteJump();
            }

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            SetJump();
            _frameVelocity.y = (_time < _frameLeftGrapple + _stats.BoostJumpWindow) ? _stats.BoostJumpPower : _stats.JumpPower;
            Jumped?.Invoke();
        }

        private void ExecuteWallJump()
        {
            SetJump();
            isWallSliding = false;

            float wallKickOff = _stats.JumpPower/2f;
            _frameVelocity.y = _stats.JumpPower;
            _frameVelocity.x = wallHitLeft ? wallKickOff : -wallKickOff;
            Jumped?.Invoke();
        }

        private void SetJump()
        {
            _jumping = true;
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
        }

        #endregion

        #region Horizontal

        //Controls horizontal movement
        private void HandleDirection()
        {
            var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            var acceleration = _grounded ? _stats.GroundAcceleration : _stats.AirAcceleration;

            if (_grapple.grappleRope.isGrappling)
            {
                //if swinging on grapple, use grapple values for acceleration. Otherwise, prohibit horizontal movement.
                if (!_grapple.launchToPoint)
                {
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeedGrappled, _stats.GrappleAcceleration * Time.fixedDeltaTime);
                }
            }
            //if dashing, don't decelerate
            else if (isDashing){

            }
            //decelerate to 0 if not inputting
            else if (_frameInput.Move.x == 0)
            {
                //if on platform, move with platform
                if (_platform != null)
                {
                    _frameVelocity.x = _platform.getDirection();
                }
                else{
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
                }
            }
            //if player is travelling faster than max speed, horizontal input should only affect their movement if it is
            //resisting current movement. Prevents player from abruptly slowing down when holding "forward".
            else if (Mathf.Abs(_frameVelocity.x) > _stats.MaxSpeed)
            {
                if ((_frameVelocity.x > 0 && _frameInput.Move.x > 0) || (_frameVelocity.x < 0 && _frameInput.Move.x < 0))
                {
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
                }
                else{
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, acceleration * Time.fixedDeltaTime);
                }
            }
            //accelerate to max speed
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            //if dashing, don't apply gravity
            if (_time < frameDashed + _stats.DashTime * 1.5f)
            {
                _frameVelocity = Vector2.MoveTowards(_frameVelocity, Vector2.zero, _stats.DashDeceleration * Time.fixedDeltaTime);
            }
            //if launching to grapple, apply launch force
            else if (_grapple.grappleRope.isGrappling && _grapple.launchToPoint)
            {
                _frameVelocity = Vector2.MoveTowards(_frameVelocity, _grapple.grappleDistanceVector.normalized * _stats.MaxSpeedGrappled, _stats.GrappleAcceleration * Time.fixedDeltaTime);
            }
            //if on ground, apply grounding force
            else if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            //if sliding on wall, apply wallslide force
            else if (isWallSliding)
            {
                _frameVelocity.y = 0;
            }
            //otherwise apply gravity
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Other

        private float frameDashed = float.MinValue;
        private Vector2 dashDirection;
        [HideInInspector] public bool isDashing;
        [HideInInspector] public bool startedDash;

        //called once when grapple begins
        public void Grappled()
        {
            _jumping = false;
        }

        //called once when grapple is released
        public void GrappleReleased()
        {
            //set time for grapplegrace if launching. Otherwise reset vertical velocity
            if (_grapple.launchToPoint)
            {
                _frameLeftGrapple = _time;
            }
            else{
                //this gets rid of any gravitational force built up while grappled
                _frameVelocity.y = 0;

                //prevents player from launching too quickly off of swing
                if (Mathf.Abs(_frameVelocity.x) > _stats.MaxSpeed)
                {
                    _frameVelocity.x  *= .75f;
                }
            }

            //prevents additional downward force from being applied after grapple
            _endedJumpEarly = false;

            //enables mid-air jump after releasing grapple
            _coyoteUsable = true;
            _frameLeftGrounded = _time;
        }

        public void StartDash()
        {
            _jumping = false;
            _endedJumpEarly = false;
            isWallSliding = false;
            startedDash = true;
            frameDashed = _time;
        }

        public void Dash(){
            isDashing = true;
            frameDashed = _time;
            if (_frameInput.Move.y <= 0){
                dashDirection = new Vector2(facingRight ? 1 : -1, 0);
            }
            else{
                dashDirection = _frameInput.Move.normalized;
            }
            _frameVelocity = dashDirection * _stats.DashSpeed;
        }

        public void EndDash(){
            
        }

        public void updateDirection(){
            if (FrameInput.x > 0){
                facingRight = true;
            }
            else if (FrameInput.x < 0){
                facingRight = false;
            }
        }

        #endregion

        private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }