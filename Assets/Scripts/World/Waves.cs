using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Waves : MonoBehaviour
{

    Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private Vector3[] normals;
    private int[] triangles;

    [SerializeField]
    private float width = 100.0f;

    [SerializeField]
    private float depth = 100.0f;

    [SerializeField]
    private int SizeX = 100;

    [SerializeField]
    private int SizeZ = 100;

    [SerializeField] float waveHeight;
    [SerializeField] private int verticesRowCount;
    [SerializeField] private int verticesCount;
    [SerializeField] private int trianglesCount;

    // Start is called before the first frame update
    void Start()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }


        verticesRowCount = SizeX + 1;
        verticesCount = verticesRowCount * (SizeZ + 1);
        trianglesCount = 6 * SizeX * SizeZ;


        vertices = new Vector3[verticesCount];
        uvs = new Vector2[verticesCount];
        normals = new Vector3[verticesCount];
        triangles = new int[trianglesCount];


    }

    // Update is called once per frame
    void Update()
    {
        if (!mesh) return;
        // Set the vertices of the mesh
        int vertexIndex = 0;
        for (int z = 0; z <= SizeZ; ++z)
        {
            float percentageZ = (float)z / (float)SizeZ;
            float startZ = percentageZ * depth;

            for (int x = 0; x <= SizeX; ++x)
            {
                float percentageX = (float)x / (float)SizeX;
                float startX = percentageX * width;


                //Generate waves by adding several diffrent sine waves in diffrent directions and magnitudes
                float height = Mathf.Sin(Time.time + x) * waveHeight * 0.5f;
                height = height + Mathf.Sin(Time.time + x * 2f) * waveHeight * 0.5f;
                height = height + Mathf.Sin(Time.time + z + x) * waveHeight;
                height = height + Mathf.Sin(Time.time + z * 0.2f) * waveHeight;
                float heightPercentage = height / waveHeight;

                vertices[vertexIndex] = new Vector3(startX, height, startZ);
                uvs[vertexIndex] =
                    new Vector2(); // No texturing so just set to zero - could be expanded in the future
                normals[vertexIndex] =
                    Vector3.up; // These should be set based on heights of terrain but we can use Recalulated normals on mesh to calculate for us
                ++vertexIndex;
            }
        }

        // Setup the indexes so they are in the correct order and will render correctly
        vertexIndex = 0;
        int trianglesIndex = 0;
        for (int z = 0; z < SizeZ; ++z)
        {
            for (int x = 0; x < SizeX; ++x)
            {
                vertexIndex = x + (verticesRowCount * z);

                triangles[trianglesIndex++] = vertexIndex;
                triangles[trianglesIndex++] = vertexIndex + verticesRowCount;
                triangles[trianglesIndex++] = (vertexIndex + 1) + verticesRowCount;
                triangles[trianglesIndex++] = (vertexIndex + 1) + verticesRowCount;
                triangles[trianglesIndex++] = vertexIndex + 1;
                triangles[trianglesIndex++] = vertexIndex;
            }
        }

        // Assign all of the data that has been created to the mesh and update it
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        //mesh.colors = colours;
        mesh.normals = normals;
        mesh.RecalculateNormals();
        mesh.UploadMeshData(false);
    }
}