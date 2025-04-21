using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PrimerFigura
{
    class Objeto
    {
        // Estas son coordenadas relativas al centro de masa del escenario
        private Vector3 offsetCoords;
        Dictionary< string, Parte> partesLista;

        public Objeto(float centroX, float centroY, float centroZ)
        {
            this.offsetCoords = new Vector3(centroX, centroY, centroZ);
            this.partesLista = new Dictionary<string, Parte> ();
        }

        public Objeto(Vector3 posicion)
        {
            this.offsetCoords = posicion;
            this.partesLista = new Dictionary<string, Parte>();
        }

        public void dibujar(Vector3 posCentroEscenario, Shader shader)
        {
            foreach (var parte in partesLista)
            {
                parte.Value.dibujar(posCentroEscenario + this.offsetCoords, shader);
            }
            
        }

        public void añadirParte(string nombre, Parte nuevaParte)
        {
            this.partesLista.Add(nombre, nuevaParte);
        }

        public void borrarParte(string nombre)
        {
            this.partesLista.Remove(nombre);
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
