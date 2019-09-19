using UnityEngine;
using System.Collections;


// Interpolation between points with a Catmull-Rom spline
public class CatmullRomSpline : MonoBehaviour
{
    // Has to be at least 4 points
    public Transform[] _controlPointsList;
    // Are we making a line or a loop?
    public bool _isLooping = true;

    public int SpanCount { get; private set; } // Number of control points


    //Display without having to press play
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        SetValues();

        Vector3 prevPos = ValueAt(0);

        const int resolution = 1000;
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            
            Vector3 pos = ValueAt(t);
            
            // Draw this line segment
            Gizmos.DrawLine(prevPos, pos);

            prevPos = pos;
        }
    }

    private void Awake()
    {
        SetValues();
    }

    private void SetValues()
    {
        SpanCount = _isLooping ? _controlPointsList.Length : _controlPointsList.Length - 3;
    }

    public Vector3 GetInitialPosition()
    {
        if (_controlPointsList != null && _controlPointsList.Length > 0)
        {
            return _controlPointsList[0].position;
        }
        return Vector3.zero;
    }

    public Quaternion GetInitialRotation()
    {
        if (_controlPointsList != null && _controlPointsList.Length > 0)
        {
            return _controlPointsList[0].rotation;
        }
        return Quaternion.identity;
    }

    public Vector3 ValueAt(float t)
    {
        int n = SpanCount;
        float u = t * n;
        int i = (t >= 1f) ? (n - 1) : (int)u;
        u -= i;
        return ValueAt(i, u);
    }
    
    /// <returns>The value of the spline at position u of the specified span</returns>
    public Vector3 ValueAt(int span, float u)
    {
        return Calculate(_isLooping ? span : (span + 1), u);
    }

    public Vector3 Calculate(int i, float u)
    {
        int n = _controlPointsList.Length;
        float u2 = u * u;
        float u3 = u2 * u;
        Vector3 result = _controlPointsList[i].position * (1.5f * u3 - 2.5f * u2 + 1f); // p1
        if (_isLooping || i > 0)
        {
            result += _controlPointsList[(n + i - 1) % n].position * (-0.5f * u3 + u2 - 0.5f * u); // p0
        }
        if (_isLooping || i < (n - 1))
        {
            result += _controlPointsList[(i + 1) % n].position * (-1.5f * u3 + 2f * u2 + 0.5f * u); // p2
        }
        if (_isLooping || i < (n - 2))
        {
            result += _controlPointsList[(i + 2) % n].position * (0.5f * u3 - 0.5f * u2); // p3
        }
        return result;
    }

    public Vector3 DerivativeAt(float t)
    {
        int n = SpanCount;
        float u = t * n;
        int i = (t >= 1f) ? (n - 1) : (int)u;
        u -= i;
        return DerivativeAt(i, u);
    }

    public Vector3 DerivativeAt(int span, float u)
    {
        return Derivative(_isLooping ? span : (span + 1), u);
    }

    /// <summary>
    /// Calculates the derivative of the catmullrom spline for the given span (i) at the given position (u).
    /// </summary>
    /// <param name="i">The span(0 LE 1 L spanCount) spanCount = continuous? points.length : points.length - degree</param>
    /// <param name="u">The position (0 LE u LE 1) on the spanThe position (0 LE u LE 1) on the span</param>
    /// <returns></returns>
    public Vector3 Derivative(int i, float u)
    {
        int n = _controlPointsList.Length;
        float u2 = u * u;
        Vector3 result = _controlPointsList[i].position * (-u * 5 + u2 * 4.5f);
        if (_isLooping || i > 0)
        {
            result += _controlPointsList[(n + i - 1) % n].position * (-0.5f + u * 2 - u2 * 1.5f);
        }
        if (_isLooping || i < (n - 1))
        {
            result += _controlPointsList[(i + 1) % n].position * (0.5f + u * 4 - u2 * 4.5f);
        }
        if (_isLooping || i < (n - 2))
        {
            result += _controlPointsList[(i + 2) % n].position * (-u + u2 * 1.5f);
        }
        return result;
    }
}