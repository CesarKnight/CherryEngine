using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CherryEngine.Components
{
    class VerticeColor
    {
        public float x {get; set;}
        public float y {get; set;}
        public float z {get; set;}
        public float r {get; set;}
        public float g {get; set;}
        public float b {get; set;}

        public VerticeColor()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
            r = 1.0f;
            g = 1.0f;
            b = 1.0f;
        }

        public VerticeColor(float x, float y, float z, float r, float g, float b)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public VerticeColor(float x, float y, float z, string hexColor)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            Vector3 color = HexToVector3(hexColor);
            r = color.X;
            g = color.Y;
            b = color.Z;
        }

        public VerticeColor(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            //color por defecto blanco
            r = 1.0f;
            g = 1.0f;
            b = 1.0f;
        }

        public void SetColor(string hexColor)
        {
            Vector3 color = HexToVector3(hexColor);
            r = color.X;
            g = color.Y;
            b = color.Z;
        }

        public void SetColor(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        private Vector3 HexToVector3(string hexColor)
        {
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);

            if (hexColor.Length != 6)
                throw new ArgumentException("Hex color must be 6 characters long.");

            int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            return new Vector3(r / 255f, g / 255f, b / 255f);
        }
    }
}
