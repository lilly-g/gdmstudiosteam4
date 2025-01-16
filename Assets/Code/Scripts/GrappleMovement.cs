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
    private GameObject grappleThrown;
    private GrappleThrown grappleScript;
    private Rigidbody2D player;
    private DistanceJoint2D joint;
    private bool thrown;
    public Transform grappleHeld;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject grappleProjectile;
    [SerializeField] RopeController rope;
    [SerializeField] private float grappleDistance = 1f;

    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        grappleHeld = transform.GetChild(0);
        player = GetComponent<Rigidbody2D>();
    }

   void Update()
    {

        //Get mouse position and find direction between player and mouse
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;

        //Calculate angle and rotate held grapple
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        grappleHeld.eulerAngles = new Vector3(0, 0, angle);

        //if grapple has been thrown and has hit a wall, fix maximum joint distance
        if (grappleThrown != null && !grappleScript.IsAirBorne() && joint.enabled == false)
        {
            joint.connectedBody = grappleThrown.GetComponent<Rigidbody2D>();
            joint.distance = Vector2.Distance(transform.position, grappleThrown.transform.position);
            joint.enabled = true;
        }

        //when primary fire is pressed and grapple is not thrown, throw grapple
        if (grappleHeld.gameObject.activeSelf && Input.GetButtonDown("Fire1"))
        {
            grappleThrown = Instantiate(grappleProjectile, transform.position, grappleHeld.rotation);
            grappleScript = grappleThrown.GetComponent<GrappleThrown>();
            grappleScript.originObj = this.transform;
            grappleScript.move(direction);

            grappleHeld.gameObject.SetActive(false);

            rope.setUpLine(this.transform, grappleThrown.transform);
        }
        //if grapple has been thrown, when secondary fire is pressed, return grapple to player
        else if (!(grappleHeld.gameObject.activeSelf) && Input.GetButtonDown("Fire2"))
        {
            Destroy(grappleThrown);
            joint.enabled = false;
            grappleHeld.gameObject.SetActive(true);
        }
        //if grapple has been thrown and has hit a wall, when primary fire is pressed, pull player to grapple
        else if (!(grappleHeld.gameObject.activeSelf) && !grappleScript.IsAirBorne() && Input.GetButtonDown("Fire1"))
        {
            joint.distance = grappleDistance;
            Vector3 pull_direction = (grappleThrown.transform.position - transform.position).normalized * 50;
            player.linearVelocity = new Vector2(pull_direction.x, pull_direction.y);
        }
        
    }
}
