using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SplitterLineSegments : MonoBehaviour
{
    [SerializeField] private MonoLineSegment lineSegmentPrefab;
    
    public void ApplySplitLineSegments()
    {
        var lineSegments = new List<LineSegment>();
        var monoLineSegments = GetComponentsInChildren<MonoLineSegment>();
        foreach (var monoSegment in monoLineSegments)
            lineSegments.Add(monoSegment.GetLineSegment());

        var splitLineSegmentsTask = new Task<List<LineSegment>>(() => SplitLineSegments(lineSegments));
        splitLineSegmentsTask.Start();

        splitLineSegmentsTask.GetAwaiter().OnCompleted(() => 
        {
            foreach (var oldSegment in monoLineSegments)
                Destroy(oldSegment.gameObject);

            var monoLines = new List<MonoLineSegment>();
            foreach (var lineSegment in splitLineSegmentsTask.Result)
            {
                var monoLine = Instantiate(lineSegmentPrefab, Vector3.zero, Quaternion.identity);
                monoLine.SetSegment(lineSegment);
                monoLine.transform.SetParent(this.transform);
                monoLines.Add(monoLine);
            }
            StartCoroutine(BlinkLineSegments(monoLines));
        });
    }

    public static List<LineSegment> SplitLineSegments(List<LineSegment> lineSegments)
    {
        var newLineSegments = new List<LineSegment>();
        foreach (var firstLine in lineSegments)
        {
            var crossPoints = new List<Vector2>();
            foreach (var secondLine in lineSegments)
            {
                if (firstLine == secondLine)
                    continue;

                var prod1 = Vector3.Cross(firstLine.endPoint - firstLine.startPoint, secondLine.startPoint - firstLine.startPoint);
                var prod2 = Vector3.Cross(firstLine.endPoint - firstLine.startPoint, secondLine.endPoint - firstLine.startPoint);
                var prod3 = Vector3.Cross(secondLine.endPoint - secondLine.startPoint, firstLine.startPoint - secondLine.startPoint);
                var prod4 = Vector3.Cross(secondLine.endPoint - secondLine.startPoint, firstLine.endPoint - secondLine.startPoint);
                if (Mathf.Sign(prod1.z) == Mathf.Sign(prod2.z) || prod1.z == 0 || prod2.z == 0 || Mathf.Sign(prod3.z) == Mathf.Sign(prod4.z) || prod3.z == 0 || prod4.z == 0)
                    continue;

                var proportionCross = Mathf.Abs(prod2.z) / (Mathf.Abs(prod2.z) + Mathf.Abs(prod1.z));
                var crossPoint = secondLine.endPoint - proportionCross * (secondLine.endPoint - secondLine.startPoint);
                crossPoints.Add(crossPoint);
            }

            if (crossPoints.Count == 0)
                newLineSegments.Add(firstLine);
            else
            {
                var sortedCrossPoints = crossPoints.OrderBy(point => (point - firstLine.startPoint).magnitude).ToArray();
                var firstPoint = firstLine.startPoint;
                for (int i = 0; i < sortedCrossPoints.Length; i++)
                {
                    var newLine = new LineSegment(firstPoint, sortedCrossPoints[i]);
                    newLineSegments.Add(newLine);
                    firstPoint = sortedCrossPoints[i];
                }
                var endLine = new LineSegment(firstPoint, firstLine.endPoint);
                newLineSegments.Add(endLine);
            }
        }
        return newLineSegments;
    }

    private IEnumerator BlinkLineSegments(List<MonoLineSegment> lineSegments)
    {
        foreach (var lineSegment in lineSegments)
        {
            lineSegment.SetColor(Color.red);
            yield return new WaitForSeconds(0.2f);
            lineSegment.SetColor(Color.blue);
        }
    }
}
