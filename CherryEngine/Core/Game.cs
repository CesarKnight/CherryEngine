using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using CherryEngine.Components;
using CherryEngine.Editor;

namespace CherryEngine.Core
{
    internal class Game : GameWindow
    {
        private Camera camera;
        public TransformationEditor editor;
        public  Vector2 lastMousePosition;
        private Shader? shader;

        public Escenario escenario;
        public bool ProductionMode = false;

        private bool ignorarPosicionInicialMouse = true;

        public Game(int ancho, int alto, string titulo)
             : base
             (
                 GameWindowSettings.Default, new NativeWindowSettings()
                 {
                     ClientSize = (ancho, alto),
                     Title = titulo,
                     WindowState = WindowState.Normal,
                 }
             )
        {
            camera = new Camera(
                new Vector3(0.0f, 0.0f, 10.0f),
                new Vector3(0.0f, 0.0f, -1.0f),
                Vector3.UnitY
            );
            editor = new TransformationEditor(camera,this);
            escenario = new Escenario(0, 0, 0);
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
            this.WindowState = WindowState.Maximized;
            CursorState = CursorState.Grabbed;

            GL.ClearColor(0.5f, 0.1f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string shaderDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\.Shaders"));
            string vertexPath = Path.Combine(shaderDir, "shader.vert");
            string fragmentPath = Path.Combine(shaderDir, "shader.frag");

            shader = new Shader(vertexPath, fragmentPath);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
                
            editor.ProcessKeyboardInput(KeyboardState, (float)args.Time);
            if (editor.IsEditModeActive())
                return;

            MouseState mouseState = MouseState;

            if (ignorarPosicionInicialMouse)
            {
                lastMousePosition = new Vector2(mouseState.X, mouseState.Y);
                ignorarPosicionInicialMouse = false;
                return;
            }

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

            if (escenario != null && shader != null)
            {
                escenario.dibujar(shader);
            }

            SwapBuffers();
        }

        // llamada a la api de GLFW para obtener el tamaño de la pantalla
        // GLFW es la libreria que OpenTK usa para crear la ventana
        protected void VerGLFW()
        {
            unsafe
            {
                var glfwWindow = GLFW.GetCurrentContext();
                if (glfwWindow == null)
                {
                    Console.WriteLine("No current GLFW context.");
                }
                else
                {
                    var monitorPrimario = GLFW.GetPrimaryMonitor();
                    var videoMode = GLFW.GetVideoMode(monitorPrimario);
                    int screenWidth = videoMode->Width;
                    int screenHeight = videoMode->Height;
                    Console.WriteLine($"Screen Width: {screenWidth}, Screen Height: {screenHeight}");
                }
            }
        }
    }
}
