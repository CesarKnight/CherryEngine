using System;
using System.Collections.Generic;
using System.Drawing;
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
        // el objeto shader  
        Shader? shader;
        // Escenario  
        Escenario? escenario0;

        public Game(int ancho, int alto, string titulo)
             : base
             (
                 GameWindowSettings.Default, new NativeWindowSettings()
                 {
                     ClientSize = (ancho, alto),
                     Title = titulo,
                     WindowState = WindowState.Normal,
                     //Location = new Vector2i(500, 310),  
                 }
             )
        {
            camera = new Camera(
                new Vector3(0.0f, 0.0f, -2.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                Vector3.UnitY
            );
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            shader?.Dispose();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            // Inicializamos el escenario en 0,0,0
            escenario0 = new Escenario(new Vector3(0, 0, 0));
            escenario0.CargarEscenarioPrueba();
            // Color de fondo  
            GL.ClearColor(0.5f, 0.1f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            CursorState = CursorState.Grabbed;

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
                Close();
            if (KeyboardState.IsKeyDown(Keys.Tab))
                CursorState = CursorState == CursorState.Normal ? CursorState.Grabbed : CursorState.Normal;

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

            shader?.Use();

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f), Size.X / (float)Size.Y, 0.1f, 100.0f
            );

            if (shader != null)
            {
                int viewLocation = GL.GetUniformLocation(shader.Handle, "view");
                GL.UniformMatrix4(viewLocation, false, ref view);

                int projectionLocation = GL.GetUniformLocation(shader.Handle, "projection");
                GL.UniformMatrix4(projectionLocation, false, ref projection);
            }

            if (escenario0 != null && shader != null)
            {
                escenario0.dibujar(shader);
            }

            SwapBuffers();
        }
    }
}
