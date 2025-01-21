using _3D_Programming;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;

public struct Triangle
{
    public Vector3 normal;
    public int faceCount;

    public Vector3[] vertices;
    public int[] indices;
}

public class CubeMarching
{
    private float scale = 10;
    private Random random = new Random();
    private int width = 10;
    private int length = 10;
    private int height = 10;
    private float heightThreshhold = .5f;

    private float zOffset = 0;

    private FastNoiseLite noise = new FastNoiseLite();

    // Geometric Info
    private VertexBuffer vertexBuffer; // Graphics buffer
    private GraphicsDevice GraphicsDevice = Game1._graphics.GraphicsDevice;

    private float[,,] heights;

    private List<VertexPositionColorNormal> vertices = new List<VertexPositionColorNormal>();
    private List<Triangle> triangles = new List<Triangle>();

    public int vertexCount {  get { return vertices.Count; } }
    public int triangleCount {  get { return triangles.Count; } }

    // Start is called before the first frame update

    public VertexBuffer VertexBuffer { get { return vertexBuffer; } }

    public CubeMarching() : this(10,10,10,.5f, 10, .05f) { }

    public CubeMarching(int width, int height, int length, float heightThreshhold, float scale, float frequency)
    {
        this.width = width;
        this.height = height;
        this.length = length;
        this.heightThreshhold = heightThreshhold;
        this.scale = scale;

        this.noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        this.noise.SetFrequency(frequency);
    }

    #region Generic

    public void Update()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
        {
            heightThreshhold += .01f;
            heightThreshhold = MathF.Min(heightThreshhold, .99f);

            UpdateAll();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
        {
            heightThreshhold -= .01f;
            heightThreshhold = MathF.Max(heightThreshhold, 0.01f);
            UpdateAll();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
        {
            zOffset += .1f;

            UpdateAll();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
        {
            zOffset -= .1f;

            UpdateAll();
        }
    }

    public void UpdateAll()
    {
        SetHeights();
        MarchCubes();
    }

    private void MarchCubes()
    {
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    float[] cubeCorners = new float[8];

                    for (int i = 0; i < 8; i++)
                    {
                        Vector3 corner = new Vector3(x, y, z) + CubeTable.Corners[i];
                        cubeCorners[i] = heights[(int)corner.X, (int)corner.Y, (int)corner.Z];
                    }

                    MarchCube(new Vector3(x, y, z), cubeCorners);
                }
            }
        }

        for (int index = 0; index < triangles.Count; index++)
        {
            if (triangles[index].faceCount == 0)
            {
                triangles.Remove(triangles[index]);
                index--;
                continue;
            }

            for (int i = 0; i < 3; i++)
            {
                VertexPositionColorNormal vertex = vertices[triangles[index].indices[i]];
                vertex.Normal = triangles[index].normal;
                vertices[triangles[index].indices[i]] = vertex;
            }
        }

        // An array of drawables we will give to the GPU
        if (triangleCount == 0)
            return;

        vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorNormal), vertices.Count, BufferUsage.WriteOnly);
        vertexBuffer.SetData<VertexPositionColorNormal>(vertices.ToArray());
    }

    private void MarchCube(Vector3 position, float[] cubeCorners)
    {
        int configIndex = GetConfigurationIndex(cubeCorners);

        // Will not generate cubes behind camera
        // float dotProduct = Vector3.Dot(position - cam.transform.position, cam.transform.forward);

        if (configIndex == 0 || configIndex == 255) //|| dotProduct < 0)
        {
            return;
        }

        int edgeIndex = 0;

        for (int t = 0; t < 5; t++)
        {
            triangles.Add(new Triangle());
            Triangle triangle = triangles[triangles.Count - 1]; 
            triangle.vertices = new Vector3[3];
            triangle.indices = new int[3];
            triangle.normal = new Vector3(0, 0, 0);
            triangle.faceCount = 0;

            for (int v = 0; v < 3; v++)
            {
                int triTableValue = CubeTable.Triangles[configIndex, edgeIndex];
                
                if (triTableValue == -1)
                {
                    return;
                }

                Vector3 edgeStart = position * scale + CubeTable.Edges[triTableValue, 0] * scale;
                Vector3 edgeEnd = position * scale + CubeTable.Edges[triTableValue, 1] * scale;

                Vector3 vertex;

                // Middle of the edge
                vertex = Vector3.Lerp(edgeStart, edgeEnd, (heightThreshhold - cubeCorners[GetEdgeEndVertex(CubeTable.Edges[triTableValue, 0])]) /
                    (cubeCorners[GetEdgeEndVertex(CubeTable.Edges[triTableValue, 1])] - cubeCorners[GetEdgeEndVertex(CubeTable.Edges[triTableValue, 0])]));

                Vector3 rand = Vector3.Normalize(new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()));

                vertices.Add(new VertexPositionColorNormal(vertex, Color.Red, Vector3.Zero));

                triangle.vertices[v] = vertex;
                triangle.indices[v] = vertices.Count - 1;

                edgeIndex++;
            }

            Vector3 edge1 = triangle.vertices[1] - triangle.vertices[0];
            Vector3 edge2 = triangle.vertices[2] - triangle.vertices[0];

            Vector3 Normal = Vector3.Normalize(Vector3.Cross(edge2, edge1));

            triangle.normal = Normal;
            triangle.faceCount = 1;

            triangles[triangles.Count - 1] = triangle;
        }
    }

    private int GetEdgeEndVertex(Vector3 pos)
    {
        for (int i = 0; i < CubeTable.Corners.Length; i++)
        {
            if (pos == CubeTable.Corners[i]) { return i; }
        }

        return default;
    }

    private int GetConfigurationIndex(float[] cubeCorners)
    {
        int configIndex = 0;

        // Starting byte
        // 0000 0000

        for (int i = 0; i < 8; i++)
        {
            // Will replace a single bit with a 1 if above threshhold
            if (cubeCorners[i] > heightThreshhold)
            {
                configIndex |= 1 << i;
            }
        }

        return configIndex;
    }

    private void SetHeights()
    {
        heights = new float[width + 1, height + 1, length + 1];

        // Loop through each height vertex
        for (int x  = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height + 1; y++)
            {
                for (int z = 0; z < length + 1; z++)
                {
                    float currentHeight = noise.GetNoise(x, y, z + zOffset);

                    // Checking the depth value and assigning it(I'm pretty sure)
                    float newHeight = y <= currentHeight - 0.5f ? 0f : 
                                      y > currentHeight + 0.5f ? 1f :
                                      y > currentHeight ? y - currentHeight : 
                                      currentHeight - y;

                    heights[x, y, z] = currentHeight;

                    // 3D Noise
                    //float currentHeight = (float)NoiseS3D.Noise(x * noiseResolution / 15, y * noiseResolution / 15, z * noiseResolution / 15) / Mathf.PerlinNoise(x * noiseResolution, z * noiseResolution);
                    //heights[x, y, z] = currentHeight;
                }
            }
        }
    }

    #endregion
}
