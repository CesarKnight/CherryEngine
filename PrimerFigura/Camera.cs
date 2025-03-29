using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PrimerFigura
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Front { get; set; }
        public Vector3 Up { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Speed { get; set; }
        public float Sensitivity { get; set; }

        public Camera(Vector3 position, Vector3 front, Vector3 up)
        {
            Position = position;
            Front = front;
            Up = up;
            Pitch = 0.0f;
            Yaw = -90.0f;
            Speed = 2.5f;
            Sensitivity = 0.1f;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public void ProcessKeyboardInput(KeyboardState keyboardState, float deltaTime)
        {
            float velocity = Speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.W))
                Position += Front * velocity;
            if (keyboardState.IsKeyDown(Keys.S))
                Position -= Front * velocity;
            if (keyboardState.IsKeyDown(Keys.A))
                Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * velocity;
            if (keyboardState.IsKeyDown(Keys.D))
                Position += Vector3.Normalize(Vector3.Cross(Front, Up)) * velocity;
        }

        public void ProcessMouseMovement(float xOffset, float yOffset)
        {
            xOffset *= Sensitivity;
            yOffset *= Sensitivity;

            Yaw += xOffset;
            Pitch -= yOffset;

            Pitch = Math.Clamp(Pitch, -89.0f, 89.0f);

            Vector3 front;
            front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            Front = Vector3.Normalize(front);
        }
    }
}
