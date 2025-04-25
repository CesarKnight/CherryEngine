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

        private string SelectedObjeto;
        private string SelectedParte;

        private const float KeyDelay = 0.3f; // Delay in seconds
        private float KeyCooldown = 0.0f;

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
            float speed = Camera.Speed;
            float velocity = speed * deltaTime;

            // Update cooldown timers
            if (KeyCooldown > 0)
                KeyCooldown -= deltaTime;

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

            if (keyboardState.IsKeyDown(Keys.Tab) && KeyCooldown <= 0)
            {
                Game.CursorState = Game.CursorState == CursorState.Normal ? CursorState.Grabbed : CursorState.Normal;
                KeyCooldown = KeyDelay;
            }

            if (keyboardState.IsKeyDown(Keys.G) && KeyCooldown <= 0)
            {
                System.Console.WriteLine("guardando escenario");
                Escenario?.GuardarEscenario("Escenario.json");
                KeyCooldown = KeyDelay;
            }
        }
    }
}
