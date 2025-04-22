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
                _offsetCoords.X = value[0];
                _offsetCoords.Y = value[1];
                _offsetCoords.Z = value[2];
            }
        }

        public Parte()
        {
            this._offsetCoords = new Vector3(0.0f, 0.0f, 0.0f);
            this.Caras = new List<Cara>();
        }

        public Parte(float difX, float difY, float difZ)
        {
            this._offsetCoords = new Vector3(difX, difY, difZ);
            this.Caras = [];
        }

        public Parte(Vector3 offset)
        {
            this._offsetCoords = offset;
            this.Caras = [];
        }


        public void dibujar(Vector3 posCentroObjeto, Shader shader)
        {
            foreach(Cara cara in this.Caras)
            {
                cara.Dibujar(posCentroObjeto + this._offsetCoords, shader);
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
