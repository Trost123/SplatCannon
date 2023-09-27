using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BlobMeshGenerator : MonoBehaviour
{
    public int resolution = 32; // Number of vertices per axis
    public float radius = 1.0f; // Blob radius
    public float noiseScale = 0.1f; // Scale of the noise
    public float noiseStrength = 0.2f; // Strength of the noise

    private MeshFilter meshFilter;
    private Mesh mesh;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        GenerateRandomParams();
        GenerateBlobMesh();
    }

    void GenerateRandomParams()
    {
        resolution = Random.Range(3, 30);
        noiseScale = Random.Range(1f, 100f);
        noiseStrength = Random.Range(0.2f, 0.5f);
    }
    
    void OnValidate()
    {
        if (meshFilter != null)
        {
            GenerateBlobMesh();
        }
    }

    void GenerateBlobMesh()
    {
        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[resolution * resolution * 6];

        for (int i = 0, z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, i++)
            {
                float u = x / (float)resolution;
                float v = z / (float)resolution;
                Vector3 pointOnSphere = GetPointOnSphere(u, v);
                float noise = Mathf.PerlinNoise(pointOnSphere.x * noiseScale, pointOnSphere.y * noiseScale);
                pointOnSphere *= radius + noise * noiseStrength;
                vertices[i] = pointOnSphere;
                uv[i] = new Vector2(u, v);
            }
        }

        int triangleIndex = 0;
        for (int z = 0, i = 0; z < resolution; z++, i++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                int vertIndex = i;
                int nextRowVertIndex = i + resolution + 1;
                triangles[triangleIndex++] = vertIndex;
                triangles[triangleIndex++] = nextRowVertIndex + 1;
                triangles[triangleIndex++] = vertIndex + 1;
                triangles[triangleIndex++] = vertIndex;
                triangles[triangleIndex++] = nextRowVertIndex;
                triangles[triangleIndex++] = nextRowVertIndex + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    Vector3 GetPointOnSphere(float u, float v)
    {
        float theta = 2 * Mathf.PI * u;
        float phi = Mathf.PI * (1 - v);
        float x = Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = Mathf.Cos(phi);
        float z = Mathf.Sin(phi) * Mathf.Sin(theta);
        return new Vector3(x, y, z);
    }
}
