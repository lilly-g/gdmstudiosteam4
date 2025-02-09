using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    
    [SerializeField] private float _cooldown;
    private bool _active = true;
    private float _inactiveTime = 0f;

    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;
    private RenderCircle circleRenderer;

    private Color initialColor;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleRenderer = GetComponent<RenderCircle>();

        initialColor = spriteRenderer.color;
    }

    void Update()
    {
        if (!_active)
        {
            _inactiveTime += Time.deltaTime;
        }

        if (_inactiveTime >= _cooldown)
        {
            _inactiveTime = 0f;
            spriteRenderer.color = initialColor;
            _active = true;
        }
    }

    public void OnGrapple()
    {
        circleRenderer.CreateCircle();
    }

    public void OnRelease()
    {
        _active = false;
        spriteRenderer.color = Color.grey;
        circleRenderer.DestroyCircle();
    }

    public bool IsActive()
    {
        return _active;
    }
}
