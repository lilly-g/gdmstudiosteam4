using UnityEngine;

public class GrappleMovement : MonoBehaviour
{
    /*
    Notes on current implementation:
    - Player does not come to a complete stop when grappling into a wall but wobbles a bit before coming to rest.
    Makes grappling to wall a bit imprecise.
    - Grappling up ledge is odd. By timing release of hook player can slide up wall and into the air. Becomes more
    dramatic the greater the angle.
    - Given current player controls it is difficult to properly swing when suspended by grapple. Should be addressed.
    */

    private Vector3 mousePos;
    private Vector3 direction;

    private bool thrown;
    public bool grappled;

    private GrappleThrown grappleThrown;
    private PlayerController playerController;
    private Rigidbody2D player;
    private DistanceJoint2D joint;
    private Transform grappleHeld;
    private Camera mainCamera;

    [SerializeField] GameObject grappleProjectile;
    [SerializeField] RopeController rope;
    [SerializeField] private float grappleDistance = 1f;

    void Start()
    {
        mainCamera = Camera.main;

        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;

        player = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        grappleHeld = transform.GetChild(0);
    }

    void Update()
    {

        //Get mouse position and find direction between player and mouse
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - transform.position);
        direction.z = 0;
        direction.Normalize();
        
        //Calculate angle and rotate held grapple
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        grappleHeld.eulerAngles = new Vector3(0, 0, angle);

        //if grapple has been thrown and has hit a wall, fix maximum joint distance
        if (grappled && joint.enabled == false)
        {
            joint.connectedBody = grappleThrown.GetComponent<Rigidbody2D>();
            joint.distance = Vector2.Distance(transform.position, grappleThrown.transform.position);
            joint.enabled = true;
        }

        //when primary fire is pressed and grapple is not thrown, throw grapple
        if (Input.GetButtonDown("Fire1") && !thrown)
        {
            throwGrapple();
        }
        //if grapple has been thrown, when secondary fire is pressed, return grapple to player
        else if (Input.GetButtonDown("Fire2") && thrown)
        {
            returnGrapple();
        }
        //if grapple has been thrown and has hit a wall, when primary fire is pressed, pull player to grapple
        else if (Input.GetButtonDown("Fire1") && grappled)
        {
            joint.distance = grappleDistance;
            pullPlayer((grappleThrown.transform.position - transform.position).normalized, 50);
        }
    }

    public void throwGrapple()
    {
        grappleThrown = Instantiate(grappleProjectile, transform.position, grappleHeld.rotation).GetComponent<GrappleThrown>();
        grappleThrown.originObj = this;
        grappleThrown.move(direction);

        grappleHeld.gameObject.SetActive(false);

        rope.setUpLine(this.transform, grappleThrown.transform);

        thrown = true;
    }

    public void pullPlayer(Vector3 direction, int strength)
    {
        Vector3 pull_direction = direction * strength;
        player.linearVelocity = new Vector2(pull_direction.x, pull_direction.y);
    }

    public void returnGrapple()
    {
        Destroy(grappleThrown.gameObject);
        joint.enabled = false;
        grappleHeld.gameObject.SetActive(true);

        thrown = false;
        grappled = false;

        playerController.GrappleReleased();
    }

    public Vector3 getDirection()
    {
        return direction;
    }
}
