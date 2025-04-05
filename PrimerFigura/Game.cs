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
            camera = new Camera(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.UnitY);
        }

       
        // el objeto shader
        Shader shader;

        // Lista de objetos

        List<Objeto> objetos = new List<Objeto>();

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
            // Color de fondo
            GL.ClearColor(0.5f, 0.1f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            Objeto cubo = new Objeto(5, 0, 0);
            cubo.cargar(verticesCubo, indicesCubo);
            objetos.Add(cubo);

            Objeto cubo1 = new Objeto(5, 0, 2);
            cubo1.cargar(verticesCubo, indicesCubo);
            objetos.Add(cubo1);

            Objeto U = new Objeto(5, 0, 4);
            U.cargar(verticesU, indicesU);
            objetos.Add(U);

            Objeto U1 = new Objeto(5, 0, -2);
            U1.cargar(verticesU, indicesU);
            objetos.Add(U1);

            // compilamos el shader
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string shaderDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\Shaders"));
            string vertexPath = Path.Combine(shaderDir, "shader.vert");
            string fragmentPath = Path.Combine(shaderDir, "shader.frag");

            shader = new Shader(vertexPath, fragmentPath);
        }

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

            foreach(Objeto objeto in objetos)
            {
                objeto.dibujar(shader);
            }

            SwapBuffers();
        }


        // vertices de un cubo
        float[] verticesCubo ={
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
        uint[] indicesCubo = {
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

        // vertices de una U hecha con bloques
        float[] verticesU = {
                // Bloque inferior izquierdo
                -0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f, -0.3f,
                -0.5f,  1.0f, -0.3f,
                -0.5f,  1.0f, -0.5f,
                 0.5f, -0.5f, -0.5f,
                 0.5f, -0.5f, -0.3f,
                 0.5f,  1.0f, -0.3f,
                 0.5f,  1.0f, -0.5f,

                // Bloque inferior derecho
                -0.5f, -0.5f,  0.3f,
                -0.5f, -0.5f,  0.5f,
                -0.5f,  1.0f,  0.5f,
                -0.5f,  1.0f,  0.3f,
                 0.5f, -0.5f,  0.3f,
                 0.5f, -0.5f,  0.5f,
                 0.5f,  1.0f,  0.5f,
                 0.5f,  1.0f,  0.3f,

                // Bloque central inferior
                -0.5f, -0.5f, -0.3f,
                -0.5f, -0.5f,  0.3f,
                -0.5f, -0.3f,  0.3f,
                -0.5f, -0.3f, -0.3f,
                 0.5f, -0.5f, -0.3f,
                 0.5f, -0.5f,  0.3f,
                 0.5f, -0.3f,  0.3f,
                 0.5f, -0.3f, -0.3f
            };

        // indices para dibujar la U usando elementos
        uint[] indicesU = {
                // Bloque inferior izquierdo
                0, 1, 2, 2, 3, 0,
                0, 1, 5, 5, 4, 0,
                1, 2, 6, 6, 5, 1,
                2, 3, 7, 7, 6, 2,
                3, 0, 4, 4, 7, 3,
                4, 5, 6, 6, 7, 4,

                // Bloque inferior derecho
                8, 9, 10, 10, 11, 8,
                8, 9, 13, 13, 12, 8,
                9, 10, 14, 14, 13, 9,
                10, 11, 15, 15, 14, 10,
                11, 8, 12, 12, 15, 11,
                12, 13, 14, 14, 15, 12,

                // Bloque central inferior
                16, 17, 18, 18, 19, 16,
                16, 17, 21, 21, 20, 16,
                17, 18, 22, 22, 21, 17,
                18, 19, 23, 23, 22, 18,
                19, 16, 20, 20, 23, 19,
                20, 21, 22, 22, 23, 20
        };
    }
}
