using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PrimerFigura
{
    internal class Game : GameWindow
    {
        private Camera camera;
        private Vector2 lastMousePosition;

        public Game(int ancho, int alto, string titulo)
            : base
            (
                GameWindowSettings.Default, new NativeWindowSettings()
                {
                    ClientSize = (ancho, alto),
                    Title = titulo
                }
            )
        {
            camera = new Camera(new Vector3(-3.0f, 0.0f, 1.0f), new Vector3(0.0f, 2.0f, -1.0f), Vector3.UnitY);
        }

        // en este objeto se guardan los vertices para que sean enviados a la gpu de un saque
        // el int es solo la id del objeto
        int BufferObjetoVertices;

        // declaramos el objeto de array de vertices

        int VertexArrayObject;

        // el objeto shader
        Shader shader;

        // vertices de un cubo
        float[] vertices ={
            // Cara frontal
            -0.5f, -0.5f,  0.5f, 
             0.5f, -0.5f,  0.5f, 
             0.5f,  0.5f,  0.5f, 
            -0.5f,  0.5f,  0.5f, 

            // Cara trasera
            -0.5f, -0.5f, -0.5f, 
             0.5f, -0.5f, -0.5f, 
             0.5f,  0.5f, -0.5f, 
            -0.5f,  0.5f, -0.5f, 

            // Cara izquierda
            -0.5f, -0.5f, -0.5f, 
            -0.5f, -0.5f,  0.5f, 
            -0.5f,  0.5f,  0.5f, 
            -0.5f,  0.5f, -0.5f, 

            // Cara derecha
             0.5f, -0.5f, -0.5f, 
             0.5f, -0.5f,  0.5f, 
             0.5f,  0.5f,  0.5f, 
             0.5f,  0.5f, -0.5f, 

            // Cara superior
            -0.5f,  0.5f, -0.5f, 
             0.5f,  0.5f, -0.5f, 
             0.5f,  0.5f,  0.5f, 
            -0.5f,  0.5f,  0.5f, 

            // Cara inferior
            -0.5f, -0.5f, -0.5f, 
             0.5f, -0.5f, -0.5f, 
             0.5f, -0.5f,  0.5f, 
            -0.5f, -0.5f,  0.5f  
        };

        // indices para dibujar el cubo usando elementos
        uint[] indices = {
            // Cara frontal
            0, 1, 2, 2, 3, 0,
            // Cara trasera
            4, 5, 6, 6, 7, 4,
            // Cara izquierda
            8, 9, 10, 10, 11, 8,
            // Cara derecha
            12, 13, 14, 14, 15, 12,
            // Cara superior
            16, 17, 18, 18, 19, 16,
            // Cara inferior
            20, 21, 22, 22, 23, 20
        };

        int ElementBufferObject;


        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            camera.ProcessKeyboardInput(KeyboardState, (float)args.Time);

            var mouseState = MouseState;
            var deltaX = mouseState.X - lastMousePosition.X;
            var deltaY = mouseState.Y - lastMousePosition.Y;
            lastMousePosition = new Vector2(mouseState.X, mouseState.Y);

            camera.ProcessMouseMovement(deltaX, deltaY);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f);

            int viewLocation = GL.GetUniformLocation(shader.Handle, "view");
            GL.UniformMatrix4(viewLocation, false, ref view);

            int projectionLocation = GL.GetUniformLocation(shader.Handle, "projection");
            GL.UniformMatrix4(projectionLocation, false, ref projection);

            GL.BindVertexArray(VertexArrayObject);

            // Dibujar el cubo 
            Matrix4 model = Matrix4.CreateTranslation(0.0f, -2.0f, 0.0f);
            int modelLocation = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(modelLocation, false, ref model);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            shader.Dispose();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.5f, 0.1f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // asignamos la id del buffer de vertices
            BufferObjetoVertices = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            // generamos el objeto de array de vertices y asignamos a opengl
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // le decimos a la gpu que vamos a usar este buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, BufferObjetoVertices);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // configuramos el puntero para los vertices dando a entender que tiene 3 valores floats
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // compilamos el shader
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string shaderDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\Shaders"));
            string vertexPath = Path.Combine(shaderDir, "shader.vert");
            string fragmentPath = Path.Combine(shaderDir, "shader.frag");

            shader = new Shader(vertexPath, fragmentPath);
        }

    }


    public class Shader
    {
        public int Handle;

        public Shader(String vertexPath, String fragmentPath)
        {
            int VertexShader, FragmentShader;

            // se leen los archivos de los shaders
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);

            //se compilan los shaders 
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);

            // se verifica que se haya compilado correctamente
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success1);
            if (success1 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            // creamos el programa de shaders y lo linkeamos
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success2);
            if (success2 == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            // una vez linkeado los shaders se pueden borrar para liberar memoria
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
