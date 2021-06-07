using UnityEngine;

public class MonoLineSegment : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private LineRenderer lineRenderer;

    private void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();   
    }

    private void Start()
    {
        var points = new Vector3[] {startPoint.position, endPoint.position};
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(points);
        SetColor(Color.blue);
    }

    public void SetSegment(LineSegment segment)
    {
        startPoint.position = segment.startPoint;
        endPoint.position = segment.endPoint;
        var points = new Vector3[] {segment.startPoint, segment.endPoint};
        lineRenderer.SetPositions(points);
    }

    public LineSegment GetLineSegment()
    {
        var lineSegment = new LineSegment(startPoint.position, endPoint.position);
        return lineSegment;
    }

    public void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}