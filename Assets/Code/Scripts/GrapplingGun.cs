using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private LayerMask grappleableLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Player Controller:")]
    [SerializeField] private PlayerController playerController;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;

    [Header("No Launch To Point")]
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 _grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [HideInInspector] public GameObject grappledObj;
    [HideInInspector] public bool launchToPoint = true;

    private void Start()
    {
        m_camera = Camera.main;

        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {

        #if UNITY_EDITOR

        //grapple is set on button press
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           TryGrapple();
        }

        #endif

        //grapple is released on button press or when reaching grapple point
        if (grappleRope.isGrappling && (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space) || Vector2.Distance(gunHolder.position, _grapplePoint) < 1))
        {
            releaseGrapple();
        }

        if (grappleRope.isGrappling)
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            SetGrapplePoint();
            RotateGun(mousePos, true);
        }

        if (grappleRope.isGrappling)
        {
            if (grappleRope.enabled)
            { 
                RotateGun(_grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public bool SetGrapple()
    {
        Collider2D[] taggedPoints = Physics2D.OverlapCircleAll(firePoint.position, maxDistance, LayerMask.GetMask("GrapplePoint"));

        if (taggedPoints.Length > 0)
        {
            Collider2D grapplePoint = null;
            float minDistance = 999999999999999999;

            foreach (var point in taggedPoints)
            {
                if (point.gameObject.GetComponent<GrapplePoint>().IsActive())
                {
                    Vector2 distanceVector = point.transform.position - firePoint.position;
                    float distanceFloat = Vector2.Distance(point.transform.position, firePoint.position);
                    RaycastHit2D _hit;

                    _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, Mathf.Infinity, ~playerLayer);

                    if ((distanceFloat <= minDistance) && (_hit && (1 << _hit.collider.gameObject.layer) == grappleableLayer))
                    {
                        minDistance = distanceFloat;
                        grapplePoint = point;
                    }
                }
            }

            if (grapplePoint != null)
            {
                if (grapplePoint.gameObject.CompareTag("Slingshot"))
                {
                    launchToPoint = true;
                    
                }
                else{
                    launchToPoint = false;
                }

                grappledObj = grapplePoint.gameObject;
                grappledObj.GetComponent<GrapplePoint>().OnGrapple();
                
                SetGrapplePoint();
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return false;
        }

        /*
        //uses a raycast from player to mouse position to find grapplepoint
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

        RaycastHit2D _hit;
        _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, Mathf.Infinity, ~playerLayer);

        //only grapples if hit object has correct layer
        if (_hit && (1 << _hit.collider.gameObject.layer) == grappleableLayer)
        {
            //launches or swings depending on type of grapplepoint
            if (_hit.collider.gameObject.CompareTag("Slingshot"))
            {
                launchToPoint = true;
            }
            else{
                launchToPoint = false;
            }

            if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
            {
                grappledObj = _hit.collider.gameObject;
                SetGrapplePoint();
                grappleRope.enabled = true;
            }
        }
        */
    }

    public void SetGrapplePoint()
    {
        _grapplePoint = grappledObj.GetComponent<Collider2D>().bounds.center;
        grappleDistanceVector = _grapplePoint - (Vector2)gunPivot.position;
    }

    public void TryGrapple()
    {
        if (SetGrapple())
        {
            grappleRope.enabled = true;
        }
    }

    /*
    public bool CanGrapple()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;

        RaycastHit2D _hit;
        _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized, Mathf.Infinity, ~playerLayer);

        return (_hit && (1 << _hit.collider.gameObject.layer) == grappleableLayer);
    }
    */

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        
        //spring joint is only set up if not launching to point
        if (!launchToPoint)
        {
            m_springJoint2D.distance = Vector2.Distance(_grapplePoint, gunPivot.position);
            m_springJoint2D.frequency = targetFrequncy;

            m_springJoint2D.connectedAnchor = _grapplePoint;
            m_springJoint2D.enabled = true;
        }

        playerController.Grappled();
    }

    public void releaseGrapple()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
            
        playerController.GrappleReleased();
        grappledObj.GetComponent<GrapplePoint>().OnRelease();
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

}
