using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ElectricTrapController : MonoBehaviour
{
    [HideInInspector] public Vector2[] Points;

    public GameObject LeftLightPS;
    public GameObject RightLightPS;

    [HideInInspector] public Color ColorTop;
    [HideInInspector] public Color ColorMiddle;
    [HideInInspector] public Color ColorBottom;

    [HideInInspector] [Min(0.01f)]public float Thickness = 1f;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private Vector2[] _uvs;
    private int[] _lineIndices;

    private MeshRenderer _renderer;

    public ElectricTrapController()
    {
        Points = new Vector2[]
        { 
            new Vector2(0f,  0f),
            new Vector2(0f,  1f),
            new Vector2(2f,  1f),
            new Vector2(2f,  2f)
        };

        _vertices = new Vector3[]
        {
            new Vector3(0f, -0.5f, 0f),
            new Vector3(0f,  0.5f, 0f)
        };

        _uvs = new Vector2[]
        { 
            new Vector2(0f, 0f),
            new Vector2(0f, 1f)
        };

        _lineIndices = new int[]
        { 
            0,
            1
        };
    }

    private void Start()
    {
        GenerateMesh();
    }

    private void Reset()
    {
        GenerateMesh();
    }

    public void SetPointPosition(int i, Vector2 p)
    {    
        Vector2 deltaMove = p - Points[i];
        Points[i] = p;

        if (i % 3 == 0)
        {
            if (i + 1 < Points.Length)
            {
                Points[i + 1] += deltaMove;
            }

            if (i - 1 >= 0)
            {
                Points[i - 1] += deltaMove;
            }
        }

        GenerateMesh();               
    }

    void SetParticlePosition()
    {
        if (LeftLightPS != null && RightLightPS != null)
        {
            LeftLightPS.transform.position = transform.TransformPoint(Points[0]);
            RightLightPS.transform.position = transform.TransformPoint(Points[3]);

            ParticleSystemRenderer lPsr = LeftLightPS.GetComponent<ParticleSystemRenderer>();
            ParticleSystemRenderer rPsr = RightLightPS.GetComponent<ParticleSystemRenderer>();

            lPsr.sharedMaterial.SetColor("_cBottom", ColorBottom);
            lPsr.sharedMaterial.SetColor("_cMiddle", ColorMiddle);
            lPsr.sharedMaterial.SetColor("_cTop", ColorTop);

            rPsr.sharedMaterial.SetColor("_cBottom", ColorBottom);
            rPsr.sharedMaterial.SetColor("_cMiddle", ColorMiddle);
            rPsr.sharedMaterial.SetColor("_cTop", ColorTop);
        }
    }

    private OrientedPoint GetBezierPoint(float t)
    {
        Vector3 p0 = Points[0];
        Vector3 p1 = Points[1];
        Vector3 p2 = Points[2];
        Vector3 p3 = Points[3];
        
        Vector3 position = Mathf.Pow(1f - t, 3f) * p0 +
            3 * t * Mathf.Pow(1f - t, 2) * p1 +
            3 * Mathf.Pow(t, 2f) * (1f - t) * p2 +
            Mathf.Pow(t, 3f) * p3;
        
        Vector3 tangent = (p0 * -Mathf.Pow(1f - t, 2f) +
            p1 * (3 * Mathf.Pow(1f - t, 2) - 2 * (1f - t)) +
            p2 * (-3 * Mathf.Pow(t, 2f) + 2 * t) +
            p3 * Mathf.Pow(t, 2f)).normalized;

        return new OrientedPoint(position, tangent);
    }

    public void GenerateMesh()
    {
        if (_mesh == null)
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = _mesh;
        }

        _mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        int edgeAmount = EvenlySpacedPoints(1).Length * 2;

        for (int ring = 0; ring < edgeAmount; ring++)
        {
            float t = ring / (edgeAmount - 1f);
            OrientedPoint p = GetBezierPoint(t);

            for (int i = 0; i < _vertices.Length; i++)
            {
                vertices.Add(p.LocalToWorldPosition(_vertices[i] * Thickness));
                uvs.Add(new Vector2(t, _uvs[i].y));
            }
        }        
        
        for (int ring = 0; ring < edgeAmount - 1; ring++)
        {
            int rootIndex = ring * _vertices.Length;
            int rootIndexNext = (ring + 1) * _vertices.Length;

            for (int line = 0; line < _lineIndices.Length - 1; line++)
            {
                int lineIndexA = _lineIndices[line];
                int lineIndexB = _lineIndices[(line + 1)];

                int currentA = rootIndex + lineIndexA;
                int currentB = rootIndex + lineIndexB;
                int nextA = rootIndexNext + lineIndexA;
                int nextB = rootIndexNext + lineIndexB;

                triangles.Add(currentA);
                triangles.Add(nextB);
                triangles.Add(nextA);

                triangles.Add(currentA);
                triangles.Add(currentB);
                triangles.Add(nextB);
            }
        }

        _mesh.SetVertices(vertices);
        _mesh.SetTriangles(triangles, 0);
        _mesh.SetUVs(0, uvs);
        _mesh.RecalculateNormals();

        if (_renderer == null)
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        _renderer.sharedMaterial.SetFloat("_Frequency", edgeAmount);
        _renderer.sharedMaterial.SetColor("_cBottom", ColorBottom);
        _renderer.sharedMaterial.SetColor("_cMiddle", ColorMiddle);
        _renderer.sharedMaterial.SetColor("_cTop", ColorTop);

        SetParticlePosition();
    }

    public Vector2[] EvenlySpacedPoints(float spacing)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        evenlySpacedPoints.Add(Points[0]);
        Vector3 previousPoint = Points[0];
        float dstSinceLastEvenPoint = 0;
       
        Vector2[] p = Points;
        float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
        float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2f;
        int divisions = Mathf.CeilToInt(estimatedCurveLength * 10);
        float t = 0;

        while (t <= 1)
        {
            t += 1f / divisions;
            OrientedPoint pointOnCurve = GetBezierPoint(t);
            dstSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve.Position);

            while (dstSinceLastEvenPoint >= spacing)
            {
                float overshootDst = dstSinceLastEvenPoint - spacing;
                Vector3 newEvenlySpacedPoint = pointOnCurve.Position + (previousPoint - pointOnCurve.Position).normalized * overshootDst;
                evenlySpacedPoints.Add(newEvenlySpacedPoint);
                dstSinceLastEvenPoint = overshootDst;
                previousPoint = newEvenlySpacedPoint;
            }

            previousPoint = pointOnCurve.Position;
        }        

        return evenlySpacedPoints.ToArray();
    }
}
