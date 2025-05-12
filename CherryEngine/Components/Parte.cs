using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CherryEngine.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CherryEngine.Components
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
                posicionArray[0] = _offsetCoords.X;
                posicionArray[1] = _offsetCoords.Y;
                posicionArray[2] = _offsetCoords.Z;
                return posicionArray;
            }
            set
            {
                _offsetCoords = new Vector3(0,0,0);
                Trasladar(value[0], value[1], value[2]);
            }
        }
        public float[] Rotation
        {
            get
            {
                float[] rotacionArray = new float[3];
                rotacionArray[0] = _rotation.X;
                rotacionArray[1] = _rotation.Y;
                rotacionArray[2] = _rotation.Z;
                return rotacionArray;
            }
            set
            {
                _rotation = new Vector3(0, 0, 0);
                Rotar(value[0], value[1], value[2]);
            }
        }

        public float Scale
        {
            get { return _scale; }
            set 
            {
                _scale = 1.0f;
                Escalar(value);
            }
        }

        public Parte()
        {
            _offsetCoords = new Vector3(0.0f, 0.0f, 0.0f);
            _rotation = new Vector3(0.0f, 0.0f, 0.0f);
            _scale = 1.0f;
            Caras = new List<Cara>();
        }

        public Parte(float difX, float difY, float difZ) : this()
        {
            _offsetCoords = new Vector3(difX, difY, difZ);
        }

        public Parte(Vector3 offset) : this()
        {
            _offsetCoords = offset;
        }

        public void dibujar(Vector3 posCentroObjeto, Matrix4 objetoTransform, Shader shader)
        {
            // Create local transformation matrices
            Matrix4 parteRotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.X)) *
                                  Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.Y)) *
                                  Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation.Z));

            Matrix4 parteScale = Matrix4.CreateScale(_scale);

            // Calculate the transformed position of this part
            Vector3 transformedPos = Vector3.TransformPosition(_offsetCoords, objetoTransform);
            Vector3 finalPos = posCentroObjeto + transformedPos;

            // Combine transformations
            Matrix4 finalTransform = parteScale * parteRotation * objetoTransform;

            foreach (Cara cara in Caras)
            {
                cara.Dibujar(finalPos, finalTransform, _scale, shader);
            }
        }

        public void Escalar(float multiplicador)
        {
            _scale *= multiplicador;
        }

        public void Rotar(float x, float y, float z)
        {
            _rotation.X += x;
            _rotation.Y += y;
            _rotation.Z += z;
        }

        public void Trasladar(float x, float y, float z)
        {
            _offsetCoords.X += x;
            _offsetCoords.Y += y;
            _offsetCoords.Z += z;
        }

        public void cargarEsfera(string colorHex, int latitudeBands = 10, int longitudeBands = 10)
        {
            // Clear any existing caras
            Caras = new List<Cara>();

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
            Caras.Add(esfera);
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
            Caras = carasCubo;
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
            Caras = carasCrossAxis;
        }
    }
}
