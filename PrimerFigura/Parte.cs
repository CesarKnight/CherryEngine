using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PrimerFigura
{
    class Parte
    {
        // Esta es posicion relativa al centro de masas de objeto
        private Vector3 _offsetCoords;
        private Vector3 _rotation;
        public float _scale;

        [JsonPropertyOrder(4)]
        public List<Cara> Caras { get; set; }
        
        public float[] OffsetCoords
        {
            get
            {
                float[] posicionArray = new float[3];
                posicionArray[0] = this._offsetCoords.X;
                posicionArray[1] = this._offsetCoords.Y;
                posicionArray[2] = this._offsetCoords.Z;
                return posicionArray;
            }
            set
            {
                this._offsetCoords = new Vector3(0,0,0);
                this.Trasladar(value[0], value[1], value[2]);
            }
        }
        public float[] Rotation
        {
            get
            {
                float[] rotacionArray = new float[3];
                rotacionArray[0] = this._rotation.X;
                rotacionArray[1] = this._rotation.Y;
                rotacionArray[2] = this._rotation.Z;
                return rotacionArray;
            }
            set
            {
                this._rotation = new Vector3(0, 0, 0);
                this.Rotar(value[0], value[1], value[2]);
            }
        }

        public float Scale
        {
            get { return this._scale; }
            set 
            { 
                this._scale = 1.0f;
                this.Escalar(value);
            }
        }

        public Parte()
        {
            this._offsetCoords = new Vector3(0.0f, 0.0f, 0.0f);
            this._rotation = new Vector3(0.0f, 0.0f, 0.0f);
            this._scale = 1.0f;
            this.Caras = new List<Cara>();
        }

        public Parte(float difX, float difY, float difZ) : this()
        {
            this._offsetCoords = new Vector3(difX, difY, difZ);
        }

        public Parte(Vector3 offset) : this()
        {
            this._offsetCoords = offset;
        }

        public void dibujar(Vector3 posCentroObjeto, Matrix4 objetoTransform, Shader shader)
        {
            // Create local transformation matrices
            Matrix4 parteRotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(this._rotation.X)) *
                                  Matrix4.CreateRotationY(MathHelper.DegreesToRadians(this._rotation.Y)) *
                                  Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(this._rotation.Z));

            Matrix4 parteScale = Matrix4.CreateScale(this._scale);

            // Calculate the transformed position of this part
            Vector3 transformedPos = Vector3.TransformPosition(this._offsetCoords, objetoTransform);
            Vector3 finalPos = posCentroObjeto + transformedPos;

            // Combine transformations
            Matrix4 finalTransform = parteScale * parteRotation * objetoTransform;

            foreach (Cara cara in this.Caras)
            {
                cara.Dibujar(finalPos, finalTransform, this._scale, shader);
            }
        }

        public void Escalar(float multiplicador)
        {
            this._scale *= multiplicador;
        }

        public void Rotar(float x, float y, float z)
        {
            this._rotation.X += x;
            this._rotation.Y += y;
            this._rotation.Z += z;
        }

        public void Trasladar(float x, float y, float z)
        {
            this._offsetCoords.X += x;
            this._offsetCoords.Y += y;
            this._offsetCoords.Z += z;
        }

        public void cargarEsfera(string colorHex, int latitudeBands = 10, int longitudeBands = 10)
        {
            // Clear any existing caras
            this.Caras = new List<Cara>();

            // Calculate vertices
            List<VerticeColor> vertices = new List<VerticeColor>();
            List<uint> indices = new List<uint>();

            // Generate vertices for each point on the sphere
            for (int lat = 0; lat <= latitudeBands; lat++)
            {
                float theta = lat * MathF.PI / latitudeBands;
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);

                for (int lon = 0; lon <= longitudeBands; lon++)
                {
                    float phi = lon * 2 * MathF.PI / longitudeBands;
                    float sinPhi = MathF.Sin(phi);
                    float cosPhi = MathF.Cos(phi);

                    float x = cosPhi * sinTheta;
                    float y = cosTheta;
                    float z = sinPhi * sinTheta;

                    // Add vertex with position (scaled to 0.5 radius to match cube dimensions)
                    vertices.Add(new VerticeColor(x * 0.5f, y * 0.5f, z * 0.5f));
                }
            }

            // Generate indices for triangles
            for (int lat = 0; lat < latitudeBands; lat++)
            {
                for (int lon = 0; lon < longitudeBands; lon++)
                {
                    uint first = (uint)(lat * (longitudeBands + 1) + lon);
                    uint second = (uint)(first + longitudeBands + 1);

                    // First triangle of the quad
                    indices.Add(first);
                    indices.Add(second);
                    indices.Add(first + 1);

                    // Second triangle of the quad
                    indices.Add(second);
                    indices.Add(second + 1);
                    indices.Add(first + 1);
                }
            }

            // Create a single cara for the sphere
            Cara esfera = new Cara();
            VerticeColor[] verticesArray = vertices.ToArray();
            uint[] indicesArray = indices.ToArray();

            // Set the color for the sphere
            esfera.SetVerticesColor(verticesArray, indicesArray, colorHex);

            // Add the cara to the parte
            this.Caras.Add(esfera);
        }
        public void cargarCubo()
        {
            uint[] indices=
            { 
                0, 1, 2,
                0, 2, 3
            };

            Cara derecha = new Cara();
            VerticeColor[] c =
            {
                new VerticeColor(-0.5f, -0.5f,  0.5f),
                new VerticeColor( 0.5f, -0.5f,  0.5f),
                new VerticeColor( 0.5f,  0.5f,  0.5f),
                new VerticeColor(-0.5f,  0.5f,  0.5f)
            };
            derecha.SetVerticesColor(c,indices, "#83dea0");

            Cara trasera = new Cara();
            c =
            [
                new VerticeColor(-0.5f, -0.5f, -0.5f),
                new VerticeColor( 0.5f, -0.5f, -0.5f),
                new VerticeColor( 0.5f,  0.5f, -0.5f),
                new VerticeColor(-0.5f,  0.5f, -0.5f),
            ];
            trasera.SetVerticesColor(c, indices, "#524fd1");

            Cara izquierda = new Cara();
            c = 
            [
                new VerticeColor(-0.5f, -0.5f, -0.5f),
                new VerticeColor(-0.5f, -0.5f,  0.5f),
                new VerticeColor(-0.5f,  0.5f,  0.5f),
                new VerticeColor(-0.5f,  0.5f, -0.5f),
            ];
            izquierda.SetVerticesColor(c, indices, "#cfff33");

            Cara frontal = new Cara();
            c = 
            [
                new VerticeColor(0.5f, -0.5f, -0.5f),
                new VerticeColor(0.5f, -0.5f,  0.5f),
                new VerticeColor(0.5f,  0.5f,  0.5f),
                new VerticeColor(0.5f,  0.5f, -0.5f),
            ];
            frontal.SetVerticesColor(c, indices, "#8b33ff");

            Cara superior = new Cara();
            c = 
            [
                new VerticeColor(-0.5f,  0.5f, -0.5f),
                new VerticeColor( 0.5f,  0.5f, -0.5f),
                new VerticeColor( 0.5f,  0.5f,  0.5f),
                new VerticeColor(-0.5f,  0.5f,  0.5f),
            ];
            superior.SetVerticesColor(c, indices, "#cf3e00");

            Cara inferior = new Cara();
            c = 
            [
                new VerticeColor(-0.5f, -0.5f, -0.5f),
                new VerticeColor( 0.5f, -0.5f, -0.5f),
                new VerticeColor( 0.5f, -0.5f,  0.5f),
                new VerticeColor(-0.5f, -0.5f,  0.5f),
            ];
            inferior.SetVerticesColor(c, indices, "#ffffff");

            List<Cara> carasCubo =
            [
                frontal,
                trasera,
                izquierda,
                derecha,
                superior,
                inferior,
            ];
            this.Caras = carasCubo;
        }
        public void cargarCrossAxis()
        {
            uint[] indices =
            {
                0, 1, 2,
                0, 2, 3
            };

            Cara xAxis = new Cara();
            VerticeColor[] c =
            {
                new VerticeColor(-10.0f, 0.01f, 0.0f),
                new VerticeColor(-10.0f, 0.0f, 0.0f),
                new VerticeColor( 10.0f, 0.0f, 0.0f),
                new VerticeColor( 10.0f, 0.01f, 0.0f),
            };
            xAxis.SetVerticesColor(c, indices, "#ff0000");  //rojo


            Cara yAxis = new Cara();
            c =
            [
                new VerticeColor( 0.01f, -10.0f, 0.0f),
                new VerticeColor( 0.0f,  -10.0f, 0.0f),
                new VerticeColor( 0.0f,   10.0f, 0.0f),
                new VerticeColor( 0.01f,  10.0f, 0.0f),
            ];
            yAxis.SetVerticesColor(c, indices, "#00ff00");    //verde


            Cara zAxis = new Cara();
            c =
            [
                new VerticeColor( 0.0f, 0.01f,-10.0f),
                new VerticeColor( 0.0f, 0.0f ,-10.0f),
                new VerticeColor( 0.0f, 0.0f , 10.0f),
                new VerticeColor( 0.0f, 0.01f , 10.0f),
            ];
            zAxis.SetVerticesColor(c, indices, "#0000ff");  //azul

            List<Cara> carasCrossAxis =
            [
                xAxis,
                yAxis,
                zAxis,
            ];
            this.Caras = carasCrossAxis;
        }
    }
}
