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
                posicionArray[0] = this._offsetCoords.X;
                posicionArray[1] = this._offsetCoords.Y;
                posicionArray[2] = this._offsetCoords.Z;
                return posicionArray;
            }
            set
            {
                this._offsetCoords = new Vector3(0, 0, 0);
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


        public Objeto()
        {
            this._offsetCoords = new Vector3(0.0f, 0.0f, 0.0f);
            this._rotation = new Vector3(0.0f, 0.0f, 0.0f);
            this._scale = 1.0f;
            this.PartesLista = new Dictionary<string, Parte>();
        }

        public Objeto(float centroX, float centroY, float centroZ): this()
        {
            this._offsetCoords = new Vector3(centroX, centroY, centroZ);
        }

        public Objeto(Vector3 posicion): this()
        {
            this._offsetCoords = posicion;
        }

        public void dibujar(Vector3 posCentroEscenario, Matrix4 EscenarioRotacion, Shader shader)
        {
            Matrix4 Rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(this._rotation.X)) *
                                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(this._rotation.Y)) *
                                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(this._rotation.Z));

            Matrix4 finalRotation = Rotation * EscenarioRotacion;
            foreach (var parte in PartesLista)
            {
                parte.Value.dibujar(posCentroEscenario + this._offsetCoords, finalRotation, shader);
            }
        }

        public void Escalar(float multiplicador)
        {
            this._scale *= multiplicador;  // Multiply instead of add
            foreach (var parte in PartesLista)
            {
                Vector4 originalPos = new Vector4(
                    parte.Value.OffsetCoords[0] ,
                    parte.Value.OffsetCoords[1] ,
                    parte.Value.OffsetCoords[2] ,
                    1.0f
                );

                Vector4 newPos = originalPos * Matrix4.CreateScale(this.Scale);
                parte.Value.OffsetCoords = [newPos.X, newPos.Y, newPos.Z];
                parte.Value.Escalar(multiplicador);
            }
        }

        public void Rotar(float x, float y, float z)
        {
            this._rotation.X += x;
            this._rotation.Y += y;
            this._rotation.Z += z;

            Matrix4 rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(x)) *
                               Matrix4.CreateRotationY(MathHelper.DegreesToRadians(y)) *
                               Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(z));

            foreach (var parte in PartesLista)
            {
                Vector4 originalPos = new Vector4(
                    parte.Value.OffsetCoords[0],
                    parte.Value.OffsetCoords[1],
                    parte.Value.OffsetCoords[2],
                    1.0f
                );

                Vector4 newPos = originalPos * rotation;
                parte.Value.OffsetCoords = [newPos.X, newPos.Y, newPos.Z];
            }
        }

        public void Trasladar(float x, float y, float z)
        {
            this._offsetCoords.X += x;
            this._offsetCoords.Y += y;
            this._offsetCoords.Z += z;
        }

        public void añadirParte(string nombre, Parte nuevaParte)
        {
            this.PartesLista.Add(nombre, nuevaParte);
        }
      
        public void borrarParte(string nombre)
        {
            this.PartesLista.Remove(nombre);
        }
        
        public void cargarCubos()
        {
            Parte cubo = new Parte(-1.0f,-1.0f,0.0f);
            cubo.cargarCubo();
            this.añadirParte("Cubo1", cubo);

            Parte cubo1 = new Parte(0.0f,-1.0f,0.0f);
            cubo1.cargarCubo();
            cubo1.Scale = 0.5f;
            this.añadirParte("Cubo2", cubo1);

            Parte cubo2 = new Parte(1.0f, -1.0f, 0.0f);
            cubo2.cargarCubo();
            cubo2.Rotation = [0.0f, -45.0f, 0.0f];
            this.añadirParte("Cubo3", cubo2);

            Parte cubo3 = new Parte(1.0f, 0.0f, 0.0f);
            cubo3.cargarCubo();
            this.añadirParte("Cubo4", cubo3);

            Parte cubo4 = new Parte(-1.0f, 0.0f, 0.0f);
            cubo4.cargarCubo();
            this.añadirParte("Cubo5", cubo4);

            Parte cubo5 = new Parte(-1.0f, 1.0f, 0.0f);
            cubo5.cargarCubo();
            this.añadirParte("Cubo6", cubo5);

            Parte cubo6 = new Parte(1.0f, 1.0f, 0.0f);
            cubo6.cargarCubo();
            this.añadirParte("Cubo7", cubo6);
        }

        public void cargarAxis()
        {
            Parte axis = new Parte(0.0f, 0.0f, 0.0f);
            axis.cargarCrossAxis();
            this.añadirParte("Axis", axis);
        }
    }
}
