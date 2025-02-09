using UnityEngine;

public class RenderCircle : MonoBehaviour
{
    private const int steps = 1500;
    private const float radius = .15f;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void CreateCircle(int steps = steps, float radius = radius)
    {
        lineRenderer.positionCount = steps;

        for (int i = 0; i < steps; i++)
        {
            float currentRadian = ((float)i / steps) * 2 * Mathf.PI;

            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, currentPosition);
        }
    }

    public void DestroyCircle()
    {
        lineRenderer.positionCount = 0;
    }
}
