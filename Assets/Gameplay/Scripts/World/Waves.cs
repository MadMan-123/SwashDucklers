using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Waves : MonoBehaviour
{

    Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private Vector3[] normals;
    private int[] triangles;

    [SerializeField]
    [Tooltip("Size of each grid cell in the mesh")]
    private float cellSize = 1.0f;

    [SerializeField]
    [Tooltip("Number of vertices in X direction")]
    private int gridX = 100;

    [SerializeField]
    [Tooltip("Number of vertices in Z direction")]
    private int gridZ = 100;
    [SerializeField] float waveHeight;
    [SerializeField] private int verticesRowCount;
    [SerializeField] private int verticesCount;
    [SerializeField] private int trianglesCount;
    [SerializeField] private Material material;

    // Start is called before the first frame update
    void Start()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt32;
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = material;

        }


        verticesRowCount = gridX + 1;
        verticesCount = verticesRowCount * (gridZ + 1);
        trianglesCount = 6 * gridX * gridZ;


        vertices = new Vector3[verticesCount];
        uvs = new Vector2[verticesCount];
        normals = new Vector3[verticesCount];
        triangles = new int[trianglesCount];

        
        GenerateMesh();
        UpdateMesh();
    }

   void GenerateMesh()
    {
        // Calculate total size of the mesh
        float totalWidth = (gridX - 1) * cellSize;
        float totalDepth = (gridZ - 1) * cellSize;

        // Generate vertices and UVs
        for (int z = 0; z < gridZ; z++)
        {
            for (int x = 0; x < gridX; x++)
            {
                int index = z * gridX + x;
                
                // Calculate vertex position
                // Center the mesh by subtracting half the total size
                float xPos = x * cellSize - (totalWidth * 0.5f);
                float zPos = z * cellSize - (totalDepth * 0.5f);
                vertices[index] = new Vector3(xPos, 0, zPos);
                
                // Calculate UVs (for texture mapping)
                uvs[index] = new Vector2((float)x / (gridX - 1) * 10, (float)z / (gridZ - 1) * 10);
                
                // Set default normal (facing up)
                normals[index] = Vector3.up;
            }
        }

        // Generate triangles
        int triangleIndex = 0;
        for (int z = 0; z < gridZ - 1; z++)
        {
            for (int x = 0; x < gridX - 1; x++)
            {
                int vertexIndex = z * gridX + x;
                
                // First triangle
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + gridX;
                triangles[triangleIndex + 2] = vertexIndex + 1;
                
                // Second triangle
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + gridX;
                triangles[triangleIndex + 5] = vertexIndex + gridX + 1;
                
                triangleIndex += 6;
            }
        }
    }
    public float GetWaveHeight(Vector3 pos)
    {
        // convert the world position to local wave position , based on the grid size and cell size
        float x = pos.x + (gridX * cellSize * 0.5f);
        float z = pos.z + (gridZ * cellSize * 0.5f);
        
        // Calculate the grid cell position
        int xIndex = Mathf.FloorToInt(x / cellSize);
        int zIndex = Mathf.FloorToInt(z / cellSize);
        
        // Get the vertices of the grid cell
        int vertexIndex = zIndex * gridX + xIndex;
        
        //get the y position of the vertex
        return vertices[vertexIndex].y;
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals;
        
        // Calculate bounds based on total size
        float totalWidth = (gridX - 1) * cellSize;
        float totalDepth = (gridZ - 1) * cellSize;
        Bounds bounds = new Bounds(Vector3.zero, new Vector3(totalWidth, waveHeight * 2, totalDepth));
        mesh.bounds = bounds;
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}