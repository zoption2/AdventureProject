using UnityEngine;

public class CatmullRomCurve : MonoBehaviour
{
    public Vector3[] controlPoints; // an array of control points to define the curve

    void OnDrawGizmos()
    {
        // Draw a gizmo representation of the curve in the Unity Editor
        Gizmos.color = Color.red;
        for (int i = 0; i < controlPoints.Length - 3; i++)
        {
            Vector3[] curvePoints = GetCurvePoints(controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], 20);
            for (int j = 0; j < curvePoints.Length - 1; j++)
            {
                Gizmos.DrawLine(curvePoints[j], curvePoints[j + 1]);
            }
        }
    }

    public Vector3[] GetCurvePoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int pointCount)
    {
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float t = (float)i / (pointCount - 1);
            Vector3 point = GetPoint(t, p0, p1, p2, p3);
            points[i] = point;
        }

        return points;
    }

    private Vector3 GetPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 point = new Vector3();
        float t2 = t * t;
        float t3 = t2 * t;

        point = 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );

        return point;
    }
}
