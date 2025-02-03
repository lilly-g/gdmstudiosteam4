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

        private Vector2 boxSize = new Vector2(1.35f, 0.03f);
        private float boxCastDistance = .94f;

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

            if (_time < frameDashed + _stats.DashTime)
            {
                isDashing = false;
            }

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
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            
            ApplyMovement();
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private float _frameLeftGrapple = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.BoxCast(transform.position, new Vector2(boxSize.x, _stats.GrounderDistance), 0, -transform.up, boxCastDistance, _stats.GroundLayer);
            bool ceilingHit = Physics2D.BoxCast(transform.position, new Vector2(boxSize.x, _stats.GrounderDistance), 0, transform.up, boxCastDistance, _stats.GroundLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

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
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;
        
        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.linearVelocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        //Controls horizontal movement
        private void HandleDirection()
        {
            var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;

            if (_grapple.grappleRope.isGrappling)
            {
                //if swinging on grapple, use grapple values for acceleration. Otherwise, prohibit horizontal movement.
                if (!_grapple.launchToPoint)
                {
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeedGrappled, _stats.GrappleAcceleration * Time.fixedDeltaTime);
                }
            }
            //decelerate to 0 if not inputting
            else if (_frameInput.Move.x == 0)
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            //if player is travelling faster than max speed, horizontal input should only affect their movement if it is
            //resisting current movement. Prevents player from slowing down when holding "forward".
            else if (Mathf.Abs(_frameVelocity.x) > _stats.MaxSpeed)
            {
                if ((_frameVelocity.x > 0 && _frameInput.Move.x > 0) || (_frameVelocity.x < 0 && _frameInput.Move.x < 0))
                {
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
                }
                else{
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
                }
            }
            //accelerate to max speed
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            //if dashing, apply dash force
            if (_time < frameDashed + _stats.DashTime)
            {
                _frameVelocity = dashDirection * _stats.DashSpeed;
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
            //otherwise apply gravity
            else
            {
                //gravity does not apply during grapplegrace
                if (_time > _frameLeftGrapple + _stats.GrappleGrace)
                {
                    var inAirGravity = _stats.FallAcceleration;
                    if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                    _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
                }
            }
        }

        #endregion

        #region Other

        private float frameDashed = float.MinValue;
        private Vector2 dashDirection;
        public bool isDashing;

        //called once when grapple begins
        public void Grappled()
        {
            //when launching, provide inital boost towards point
            if (_grapple.launchToPoint)
            {
                _frameVelocity = _grapple.grappleDistanceVector.normalized * 2;
            }
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

            //enables mid-air jump after releasing grapple
            _coyoteUsable = true;
            _frameLeftGrounded = _time;
        }

        public void Dash()
        {
            dashDirection = _frameInput.Move;
            isDashing = true;
            frameDashed = _time;
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