using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace PrimerFigura
{
    class Editor
    {
        private Camera Camera;
        private Game Game;
        private Escenario Escenario;

        public string SelectedObjeto { get; set; } = "";
        public string SelectedParte { get; set; } = "";
        private bool EditModeActive = false; 

        private float rotationSpeed = 0.003f;
        private float traslationSpeed = 0.0001f;
        private float scaleSpeed = 0.0001f;

        private const float KeyDelay = 0.3f; // Delay in seconds
        private float KeyCooldown = 0.0f;
        
        public enum TransformationEditMode
        {
            Traslacion,
            Rotacion,
            Escalado
        }
        public TransformationEditMode? currentTransformationMode = null;

        public Editor(Camera camera, Game game)
        {
            Camera = camera;
            Game = game;
            SelectedObjeto = "";
            SelectedParte = "";
            Escenario = game.escenario;
        }

        public void ProcessKeyboardInput(KeyboardState keyboardState, float deltaTime)
        {
            // Actualizar el timer de cooldown
            if (KeyCooldown > 0)
                KeyCooldown -= deltaTime;

            // Inputs GLOBALES
            if (keyboardState.IsKeyDown(Keys.Tab) && KeyCooldown <= 0)
            {
                ToggleEditMode();
                KeyCooldown = KeyDelay;
            }

            if (EditModeActive)
            {
                EditModeInputs(keyboardState, deltaTime);
                return;
            }

            CameraModeInputs(keyboardState, deltaTime);
        }

        public void EditModeInputs(KeyboardState keyboardState, float deltaTime)
        {
            if (keyboardState.IsKeyDown(Keys.G) && KeyCooldown <= 0)
            {
                System.Console.WriteLine("guardando escenario");
                Escenario?.GuardarEscenario("Escenario.json");
                
                KeyCooldown = KeyDelay;
            }

            if (keyboardState.IsKeyDown(Keys.H) && KeyCooldown <= 0)
            {
                System.Console.WriteLine("cargando escenario");
                Escenario?.CargarEscenario("Escenario.json");

                KeyCooldown = KeyDelay;
            }


            if (SelectedObjeto == "" && SelectedParte == "")
            {
                EscenarioTransformations(keyboardState, deltaTime);
                return;
            }

            if (SelectedParte != "")
            {
                Parte parte = Escenario!.Objetos[SelectedObjeto].PartesLista[SelectedParte];
                ParteTransformations(parte, keyboardState, deltaTime);
                return;
            }

            if (SelectedObjeto != "")
            {
                Objeto objeto = Escenario!.Objetos[SelectedObjeto];
                ObjetoTransformations(objeto, keyboardState, deltaTime);
                return;
            }
        }

        public void ParteTransformations(Parte Parte, KeyboardState keyboardState, float deltatime)
        {
            if (currentTransformationMode == TransformationEditMode.Traslacion)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    Parte?.Trasladar(0.0f, 0.0f, traslationSpeed);
                if (keyboardState.IsKeyDown(Keys.S))
                    Parte?.Trasladar(0.0f, 0.0f, -traslationSpeed);
                if (keyboardState.IsKeyDown(Keys.D))
                    Parte?.Trasladar(traslationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.A))
                    Parte?.Trasladar(-traslationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.Space))
                    Parte?.Trasladar(0.0f, traslationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.LeftControl))
                    Parte?.Trasladar(0.0f, -traslationSpeed, 0.0f);
            }
            if (currentTransformationMode == TransformationEditMode.Rotacion)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    Parte?.Rotar(-rotationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.S))
                    Parte?.Rotar(rotationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.A))
                    Parte?.Rotar(0.0f, rotationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.D))
                    Parte?.Rotar(0.0f, -rotationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.Space))
                    Parte?.Rotar(0.0f, 0.0f, rotationSpeed);
                if (keyboardState.IsKeyDown(Keys.LeftControl))
                    Parte?.Rotar(0.0f, 0.0f, -rotationSpeed);
            }
            if (currentTransformationMode == TransformationEditMode.Escalado)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    Parte?.Escalar(1.0f + scaleSpeed);
                if (keyboardState.IsKeyDown(Keys.S))
                    Parte?.Escalar(1.0f - scaleSpeed);
            }
        }

        public void ObjetoTransformations(Objeto objeto, KeyboardState keyboardState, float deltatime)
        {
            if (currentTransformationMode == TransformationEditMode.Traslacion)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    objeto?.Trasladar(0.0f, 0.0f, traslationSpeed);
                if (keyboardState.IsKeyDown(Keys.S))
                    objeto?.Trasladar(0.0f, 0.0f, -traslationSpeed);
                if (keyboardState.IsKeyDown(Keys.D))
                    objeto?.Trasladar(traslationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.A))
                    objeto?.Trasladar(-traslationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.Space))
                    objeto?.Trasladar(0.0f, traslationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.LeftControl))
                    objeto?.Trasladar(0.0f, -traslationSpeed, 0.0f);
            }
            if (currentTransformationMode == TransformationEditMode.Rotacion)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    objeto?.Rotar(-rotationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.S))
                    objeto?.Rotar(rotationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.A))
                    objeto?.Rotar(0.0f, rotationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.D))
                    objeto?.Rotar(0.0f, -rotationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.Space))
                    objeto?.Rotar(0.0f, 0.0f, rotationSpeed);
                if (keyboardState.IsKeyDown(Keys.LeftControl))
                    objeto?.Rotar(0.0f, 0.0f, -rotationSpeed);
            }
            if (currentTransformationMode == TransformationEditMode.Escalado)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    objeto?.Escalar(1.0f + scaleSpeed);
                if (keyboardState.IsKeyDown(Keys.S))
                    objeto?.Escalar(1.0f - scaleSpeed);
            }
        }

        public void EscenarioTransformations(KeyboardState keyboardState, float deltatime)
        {
            if (currentTransformationMode == TransformationEditMode.Traslacion)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    Escenario?.Trasladar(0.0f, 0.0f, traslationSpeed);
                if (keyboardState.IsKeyDown(Keys.S))
                    Escenario?.Trasladar(0.0f, 0.0f, -traslationSpeed);
                if (keyboardState.IsKeyDown(Keys.D))
                    Escenario?.Trasladar(traslationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.A))
                    Escenario?.Trasladar(-traslationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.Space))
                    Escenario?.Trasladar(0.0f, traslationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.LeftControl))
                    Escenario?.Trasladar(0.0f, -traslationSpeed, 0.0f);
            }
            if (currentTransformationMode == TransformationEditMode.Rotacion)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    Escenario?.Rotar(-rotationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.S))
                    Escenario?.Rotar(rotationSpeed, 0.0f, 0.0f);
                if (keyboardState.IsKeyDown(Keys.A))
                    Escenario?.Rotar(0.0f, rotationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.D))
                    Escenario?.Rotar(0.0f, -rotationSpeed, 0.0f);
                if (keyboardState.IsKeyDown(Keys.Space))
                    Escenario?.Rotar(0.0f, 0.0f, rotationSpeed);
                if (keyboardState.IsKeyDown(Keys.LeftControl))
                    Escenario?.Rotar(0.0f, 0.0f, -rotationSpeed);
            }
            if (currentTransformationMode == TransformationEditMode.Escalado)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    Escenario?.Escalar(1.0f + scaleSpeed);
                if (keyboardState.IsKeyDown(Keys.S))
                    Escenario?.Escalar(1.0f - scaleSpeed);
            }
        }

        public void CameraModeInputs(KeyboardState keyboardState, float deltaTime)
        {
            float speed = Camera.Speed;
            float velocity = speed * deltaTime;

            if (keyboardState.IsKeyDown(Keys.W))
                Camera.Position += Camera.Front * velocity;

            if (keyboardState.IsKeyDown(Keys.S))
                Camera.Position -= Camera.Front * velocity;

            if (keyboardState.IsKeyDown(Keys.A))
                Camera.Position -= Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * velocity;

            if (keyboardState.IsKeyDown(Keys.D))
                Camera.Position += Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * velocity;

            if (keyboardState.IsKeyDown(Keys.LeftControl))
                Camera.Position -= Camera.Up * velocity;

            if (keyboardState.IsKeyDown(Keys.Space))
                Camera.Position += Camera.Up * velocity;

            if (keyboardState.IsKeyDown(Keys.Escape))
                Game.Close();
        }

        public bool IsEditModeActive()
        {
            return EditModeActive;
        }

        public void ToggleEditMode()
        { 
            // Cambiamos el ultimo estado del mouse en el jugo al estado del mouse en editor
            if (EditModeActive)
                Game.lastMousePosition = new Vector2(Game.MouseState.X, Game.MouseState.Y);

            Game.CursorState = Game.CursorState == CursorState.Normal ? CursorState.Grabbed : CursorState.Normal;
            EditModeActive = !EditModeActive;

            string estado = EditModeActive ? "Activado" : "Desactivado";
            System.Console.WriteLine("Modo edicion: " + estado);
        }
    }
}
