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
    class Objeto
    {
        // Estas son coordenadas relativas al centro de masa del escenario
        private Vector3 _offsetCoords;
        private Vector3 _rotation;
        private float _scale;

        [JsonPropertyOrder(4)]
        public Dictionary< string, Parte> PartesLista { get; set; }
        
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
                _offsetCoords = new Vector3(0, 0, 0);
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


        public Objeto()
        {
            _offsetCoords = new Vector3(0.0f, 0.0f, 0.0f);
            _rotation = new Vector3(0.0f, 0.0f, 0.0f);
            _scale = 1.0f;
            PartesLista = new Dictionary<string, Parte>();
        }

        public Objeto(float centroX, float centroY, float centroZ): this()
        {
            _offsetCoords = new Vector3(centroX, centroY, centroZ);
        }

        public Objeto(Vector3 posicion): this()
        {
            _offsetCoords = posicion;
        }

        public void dibujar(Vector3 posCentroEscenario, Matrix4 escenarioTransform, Shader shader)
        {
            // Create local transformation matrices
            Matrix4 objetoRotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.X)) *
                                   Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.Y)) *
                                   Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation.Z));

            Matrix4 objetoScale = Matrix4.CreateScale(_scale);

            // Calculate the transformed position of this object
            Vector3 transformedPos = Vector3.TransformPosition(_offsetCoords, escenarioTransform);
            Vector3 finalPos = posCentroEscenario + transformedPos;

            // Combine transformations (escenario transformation is applied first, then object's local transformation)
            Matrix4 combinedTransform = objetoScale * objetoRotation * escenarioTransform;

            foreach (var parte in PartesLista)
            {
                parte.Value.dibujar(finalPos, combinedTransform, shader);
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

        public void añadirParte(string nombre, Parte nuevaParte)
        {
            PartesLista.Add(nombre, nuevaParte);
        }
      
        public void borrarParte(string nombre)
        {
            PartesLista.Remove(nombre);
        }
        
        public void cargarCubos()
        {
            Parte cubo = new Parte(-1.0f,-1.0f,0.0f);
            cubo.cargarCubo();
            añadirParte("Cubo1", cubo);

            Parte cubo1 = new Parte(0.0f,-1.0f,0.0f);
            cubo1.cargarCubo();
            cubo1.Scale = 0.5f;
            añadirParte("Cubo2", cubo1);

            Parte cubo2 = new Parte(1.0f, -1.0f, 0.0f);
            cubo2.cargarCubo();
            cubo2.Rotation = [0.0f, -45.0f, 0.0f];
            añadirParte("Cubo3", cubo2);

            Parte cubo3 = new Parte(1.0f, 0.0f, 0.0f);
            cubo3.cargarCubo();
            añadirParte("Cubo4", cubo3);

            Parte cubo4 = new Parte(-1.0f, 0.0f, 0.0f);
            cubo4.cargarCubo();
            añadirParte("Cubo5", cubo4);

            Parte cubo5 = new Parte(-1.0f, 1.0f, 0.0f);
            cubo5.cargarCubo();
            añadirParte("Cubo6", cubo5);

            Parte cubo6 = new Parte(1.0f, 1.0f, 0.0f);
            cubo6.cargarCubo();
            añadirParte("Cubo7", cubo6);
        }

        public void cargarAxis()
        {
            Parte axis = new Parte(0.0f, 0.0f, 0.0f);
            axis.cargarCrossAxis();
            añadirParte("Axis", axis);
        }

        public void CargarEsfera()
        {
            Parte esfera = new Parte(0.0f, 0.0f, 0.0f);
            esfera.cargarEsfera("#FFFFFF");
            añadirParte("Esfera", esfera);
        }
    }
}
