using UnityEngine;

public class LineSegment
{
    public Vector2 startPoint;
    public Vector2 endPoint;

    public LineSegment(Vector2 startPoint, Vector2 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }
}