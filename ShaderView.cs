using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Threading.Tasks;

enum ShaderStatus
{
    OK = 1,
    NotInitilized = 0,
    FileNotFound = -1,
    UnknownShaderFileType = -2,
}


class View
{
    private int Width;
    private int Height;
    private int VWidth;
    private int VHeight;

    private int RayTracingProgramID;
    private int RayTracingVertexShader;
    private int RayTracingFragmentShader;

    private ShaderStatus Status = ShaderStatus.NotInitilized;

    public OpenTK.Vector3 LightSourcePosition = new OpenTK.Vector3(2.0f, 4.0f, -4.0f);
    public OpenTK.Vector3 CameraPosition = new OpenTK.Vector3(0.0F, 0.0F, -8.0f);

    public const int CUBE_SIDES_COUNT = 6;

    public const int CUBE_MAX_COUNT = 10;

    public int CUBE_COUNT = 0;
    public OpenTK.Vector3[] CubePositions = new OpenTK.Vector3[CUBE_MAX_COUNT];
    public int[] CubeMaterials = new int[CUBE_MAX_COUNT];
    public float[] CubeSizes = new float[CUBE_MAX_COUNT];

    public struct STriangle
    {
        public OpenTK.Vector3 v1;
        public OpenTK.Vector3 v2;
        public OpenTK.Vector3 v3;

        public int MaterialIdx;
    }

    public struct SCube
    {
        STriangle[] CubeTriangles;
    }

    public struct Side
    {
        public OpenTK.Vector3 up;
        public OpenTK.Vector3 left;
        public OpenTK.Vector3 right;
        public OpenTK.Vector3 bottom;
        public OpenTK.Vector3 align;
    }

    public View(int width, int height, int vwidth, int vheight)
    {
        Width = Math.Abs(width);
        Height = Math.Abs(height);

        VWidth = Math.Abs(vwidth);
        VHeight = Math.Abs(vheight);

        InitShader();

        InitView(vwidth, vheight);
    }

    private void InitShader()
    {
        RayTracingProgramID = GL.CreateProgram();
 
        LoadShader("../../Shaders//RayTracing.vert", RayTracingProgramID, out RayTracingVertexShader);
        LoadShader("../../Shaders/RayTracing.frag", RayTracingProgramID, out RayTracingFragmentShader);

        GL.LinkProgram(RayTracingProgramID);

        int status = 0;
        GL.GetProgram(RayTracingProgramID, GetProgramParameterName.LinkStatus, out status);
        Console.WriteLine(GL.GetProgramInfoLog(RayTracingProgramID));
    }
    public void LoadShader(string filename, int program, out int address)
    {
        address = -1;
        ShaderType type;
        switch (Path.GetExtension(filename))
        {
            case ".vert":
                {
                    type = ShaderType.VertexShader;
                }; break;
            case ".frag":
                {
                    type = ShaderType.FragmentShader;
                }; break;

            default:
                {
                    Status = ShaderStatus.UnknownShaderFileType;
                    return;
                };
        }

        LoadShader(filename, type, program, out address);
    }

    public void LoadShader(string filename, ShaderType type, int program, out int address)
    {
        address = GL.CreateShader(type);

        StreamReader Reader = new StreamReader(filename);

        GL.ShaderSource(address, Reader.ReadToEnd());

        GL.CompileShader(address);
        GL.AttachShader(program, address);

        Console.WriteLine(GL.GetShaderInfoLog(address));

        Reader.Close();

        Status = ShaderStatus.OK;
    }

    public void InitView(int ViewWidth = 800, int ViewHeight = 600)
    {
        GL.ShadeModel(ShadingModel.Smooth);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(0, Width, 0, Height, -1, 1);
        GL.Viewport(0, 0, VWidth, VHeight);
    }

    private void SetUniform3(string name, OpenTK.Vector3 value)
    {
        GL.Uniform3(GL.GetUniformLocation(RayTracingProgramID, name), value);
    }

    private void SetUniform2(string name, OpenTK.Vector2 value)
    {
        GL.Uniform2(GL.GetUniformLocation(RayTracingProgramID, name), value);
    }

    private void SetUniform1(string name, float value)
    {
        GL.Uniform1(GL.GetUniformLocation(RayTracingProgramID, name), value);
    }

    private void SetUniform1(string name, int value)
    {
        GL.Uniform1(GL.GetUniformLocation(RayTracingProgramID, name), value);
    }

    private STriangle[] CreateCube(OpenTK.Vector3 Position, float size)
    {
        STriangle[] Triangles = new STriangle[2 * CUBE_SIDES_COUNT];
        Side[] sides = new Side[6];

        // back
        sides[0].up = new OpenTK.Vector3(0, size, 0);
        sides[0].left = new OpenTK.Vector3(-size, 0, 0);
        sides[0].right = new OpenTK.Vector3(size, 0, 0);
        sides[0].bottom = new OpenTK.Vector3(0, -size, 0);
        sides[0].align = new OpenTK.Vector3(0, 0, size);

        // front
        sides[1].up = new OpenTK.Vector3(0, size, 0);
        sides[1].left = new OpenTK.Vector3(-size, 0, 0);
        sides[1].right = new OpenTK.Vector3(size, 0, 0);
        sides[1].bottom = new OpenTK.Vector3(0, -size, 0);
        sides[1].align = new OpenTK.Vector3(0, 0, -size);

        // left
        sides[2].up = new OpenTK.Vector3(0, size, 0);
        sides[2].left = new OpenTK.Vector3(0, 0, size);
        sides[2].right = new OpenTK.Vector3(0, 0, -size);
        sides[2].bottom = new OpenTK.Vector3(0, -size, 0);
        sides[2].align = new OpenTK.Vector3(-size, 0, 0);


        // right
        sides[3].up = new OpenTK.Vector3(0, -size, 0);
        sides[3].left = new OpenTK.Vector3(0, 0, size);
        sides[3].right = new OpenTK.Vector3(0, 0, -size);
        sides[3].bottom = new OpenTK.Vector3(0, size, 0);
        sides[3].align = new OpenTK.Vector3(size, 0, 0);


        // top
        sides[4].up = new OpenTK.Vector3(0, 0, size);
        sides[4].left = new OpenTK.Vector3(-size, 0, 0);
        sides[4].right = new OpenTK.Vector3(size, 0, 0);
        sides[4].bottom = new OpenTK.Vector3(0, 0, -size);
        sides[4].align = new OpenTK.Vector3(0, size, 0);


        // bottom
        sides[5].up = new OpenTK.Vector3(0, 0, size);
        sides[5].left = new OpenTK.Vector3(-size, 0, 0);
        sides[5].right = new OpenTK.Vector3(size, 0, 0);
        sides[5].bottom = new OpenTK.Vector3(0, 0, -size);
        sides[5].align = new OpenTK.Vector3(0, -size, 0);


        for (int i = 0; i < CUBE_SIDES_COUNT; i++) {
            Triangles[2 * i + 0].v1 = Position + sides[i].align + sides[i].right + sides[i].up;
            Triangles[2 * i + 0].v2 = Position + sides[i].align + sides[i].left + sides[i].bottom;
            Triangles[2 * i + 0].v3 = Position + sides[i].align + sides[i].right + sides[i].bottom;

            Triangles[2 * i + 1].v1 = Position + sides[i].align + sides[i].right + sides[i].up;
            Triangles[2 * i + 1].v2 = Position + sides[i].align + sides[i].left + sides[i].up;
            Triangles[2 * i + 1].v3 = Position + sides[i].align + sides[i].left  + sides[i].bottom;
        }

        return Triangles;
    }

    private void LoadCubes()
    {
        STriangle[] Triangles = CreateCube(new OpenTK.Vector3(0), 0);

        for(int i = 0; i < CUBE_COUNT; i++)
        {
            STriangle[] NextTriangles = CreateCube(CubePositions[i], CubeSizes[i]);
            Triangles = Triangles.Union(NextTriangles).ToArray();
        }

        for (int i = CUBE_COUNT; i < CUBE_MAX_COUNT; i++)
        {
            STriangle[] NextTriangles = CreateCube(new OpenTK.Vector3(0), 0);
            Triangles = Triangles.Union(NextTriangles).ToArray();
        }

        for (int i = 1; i < Triangles.Length; i++)
        {
            SetUniform3("CubeTriangles[" + i.ToString() + "].v1", Triangles[i].v1);
            SetUniform3("CubeTriangles[" + i.ToString() + "].v2", Triangles[i].v2);
            SetUniform3("CubeTriangles[" + i.ToString() + "].v3", Triangles[i].v3);
            SetUniform1("CubeTriangles[" + i.ToString() + "].MaterialIdx", CubeMaterials[((i - 1) / (2 * CUBE_SIDES_COUNT))]);
        }
    }

    private void UpdateUniforms()
    {
        SetUniform3("LIGHT_POSITION",    LightSourcePosition);

        SetUniform3("uCamera.Position",  CameraPosition);
        SetUniform3("uCamera.View",      new OpenTK.Vector3(0.0f, 0.0f, 1.0f));
        SetUniform3("uCamera.Up",        new OpenTK.Vector3(0.0f, 1.0f, 0.0f));
        SetUniform3("uCamera.Side",      new OpenTK.Vector3(1.0f, 0.0f, 0.0f));

        SetUniform2("uCamera.Scale",     new OpenTK.Vector2(1.0f, 1.0f));

        LoadCubes();
    }

    public void DrawQuads()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(RayTracingProgramID);

        UpdateUniforms();

        GL.Begin(PrimitiveType.Quads);

        GL.Vertex2(-Width, -Height);
        GL.Vertex2(Width, -Height);
        GL.Vertex2(Width, Height);
        GL.Vertex2(-Width, Height);

        GL.End();
    }
}