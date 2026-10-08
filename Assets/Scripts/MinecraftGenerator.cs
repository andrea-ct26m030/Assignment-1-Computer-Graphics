using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class QuaddieTheCube : MonoBehaviour
{
    [SerializeField] private Texture2D atlasTexture;

    // Tile coordinates from the top-left, starting at zero.
    [SerializeField] private Vector2Int sideTile = new Vector2Int(3, 0);
    [SerializeField] private Vector2Int topTile = new Vector2Int(1, 9);
    [SerializeField] private Vector2Int bottomTile = new Vector2Int(2, 0);

    [SerializeField] private float width = 5f;
    [SerializeField] private float height = 3f;
    [SerializeField] private float depth = 3f;

    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        var meshRenderer = GetComponent<MeshRenderer>();

        var material = new Material(
            Shader.Find("Universal Render Pipeline/Lit")
        );

        material.SetTexture("_BaseMap", atlasTexture);
        meshRenderer.sharedMaterial = material;

        float x = width / 2f;
        float y = height / 2f;
        float z = depth / 2f;

        var corners = new Vector3[]
        {
            new Vector3(-x, -y, -z), // 0
            new Vector3( x, -y, -z), // 1
            new Vector3( x,  y, -z), // 2
            new Vector3(-x,  y, -z), // 3
            new Vector3(-x, -y,  z), // 4
            new Vector3( x, -y,  z), // 5
            new Vector3( x,  y,  z), // 6
            new Vector3(-x,  y,  z), // 7
        };

        var faceCorners = new int[]
        {
            0, 3, 2, 1, // Back
            5, 6, 7, 4, // Front
            1, 2, 6, 5, // Right
            4, 7, 3, 0, // Left
            3, 7, 6, 2, // Top
            4, 0, 1, 5, // Bottom
        };

        var vertices = new Vector3[24];
        var triangles = new int[36];
        var uvs = new Vector2[24];

        for (int face = 0; face < 6; face++)
        {
            int v = face * 4;
            int t = face * 6;

            for (int i = 0; i < 4; i++)
                vertices[v + i] = corners[faceCorners[v + i]];

            triangles[t]     = v;
            triangles[t + 1] = v + 1;
            triangles[t + 2] = v + 2;
            triangles[t + 3] = v;
            triangles[t + 4] = v + 2;
            triangles[t + 5] = v + 3;

            Vector2Int tile = sideTile;

            if (face == 4)
                tile = topTile;
            else if (face == 5)
                tile = bottomTile;

            SetFaceUVs(uvs, v, tile);
        }

        var mesh = new Mesh
        {
            name = "Minecraft Cube",
            vertices = vertices,
            triangles = triangles,
            uv = uvs
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }

    private void SetFaceUVs(
        Vector2[] uvs,
        int vertexStart,
        Vector2Int tile)
    {
        const float tileSize = 1f / 16f;

        float uMin = tile.x * tileSize;
        float uMax = (tile.x + 1) * tileSize;

        float vMin = 1f - (tile.y + 1) * tileSize;
        float vMax = 1f - tile.y * tileSize;

        uvs[vertexStart]     = new Vector2(uMin, vMin);
        uvs[vertexStart + 1] = new Vector2(uMin, vMax);
        uvs[vertexStart + 2] = new Vector2(uMax, vMax);
        uvs[vertexStart + 3] = new Vector2(uMax, vMin);
    }
}