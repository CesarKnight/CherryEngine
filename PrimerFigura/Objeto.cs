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
        public Dictionary< string, Parte> PartesLista { get; set; }
      
        [JsonPropertyOrder(-1)]
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
                _offsetCoords.X = value[0];
                _offsetCoords.Y = value[1];
                _offsetCoords.Z = value[2];
            }
        }

        public Objeto()
        {
            this._offsetCoords = new Vector3(0.0f, 0.0f, 0.0f);
            this.PartesLista = new Dictionary<string, Parte>();
        }

        public Objeto(float centroX, float centroY, float centroZ)
        {
            this._offsetCoords = new Vector3(centroX, centroY, centroZ);
            this.PartesLista = new Dictionary<string, Parte> ();
        }

        public Objeto(Vector3 posicion)
        {
            this._offsetCoords = posicion;
            this.PartesLista = new Dictionary<string, Parte>();
        }

        public void dibujar(Vector3 posCentroEscenario, Shader shader)
        {
            foreach (var parte in PartesLista)
            {
                parte.Value.dibujar(posCentroEscenario + this._offsetCoords, shader);
            }
            
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
            Parte cubo = new Parte(-4.0f,0.0f,0.0f);
            cubo.cargarCubo();
            this.añadirParte("Cubo1", cubo);

            Parte cubo1 = new Parte(4.0f,0.0f,0.0f);
            cubo1.cargarCubo();
            this.añadirParte("Cubo2", cubo1);
        }

        public void cargarAxis()
        {
            Parte axis = new Parte(0.0f, 0.0f, 0.0f);
            axis.cargarCrossAxis();
            this.añadirParte("Axis", axis);
        }
    }
}
