using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PrimerFigura
{
    class Parte
    {
        // Esta es posicion absoluta
        private Vector3 posicion;
        private List<Cara> Caras;

        public Parte(float centroX, float centroY, float centroZ)
        {
            this.posicion = new Vector3(centroX, centroY, centroZ);
            this.Caras = [];
        }

        public Parte(Vector3 pos)
        {
            this.posicion = pos;
            this.Caras = [];
        }

        public Parte(float centroX, float centroY, float centroZ, List<Cara> caras)
        {
            this.posicion = new Vector3(centroX, centroY, centroZ);
            this.Caras = caras;
        }


        public void SetCaras(List<Cara> caras)
        {
            this.Caras = caras;
        }

        public void dibujar(Shader shader)
        {
            foreach(Cara cara in this.Caras)
            {
                cara.Dibujar(shader);
            }

        }
        public Vector3 GetPosicion()
        {
            return this.posicion;
        }

        // Cambia la centro de la parte y de todas sus caras
        public void SetPosicion(Vector3 nuevaPos)
        {
            Vector3 dif = this.posicion - nuevaPos;

            foreach (Cara cara in this.Caras)
            {
                cara.Posicion += dif;
            }
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
            SetCaras(carasCubo);
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
            SetCaras(carasCrossAxis);
        }
    }
}
