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
        Vector3 posicion;
        Dictionary< string, Parte> partesLista;

        public Objeto(float centroX, float centroY, float centroZ)
        {
            this.posicion = new Vector3(centroX, centroY, centroZ);
            this.partesLista = new Dictionary<string, Parte> ();
        }

        public Objeto(float centroX, float centroY, float centroZ, Dictionary< string , Parte> partes)
        {
            this.posicion = new Vector3(centroX, centroY, centroZ);
            this.partesLista = partes;
        }

        public Vector3 getPosicion() {
            return this.posicion;
        }

        // cambia la posicion del centro del objeto y de todas sus partes
        public void setPosicion(Vector3 nuevaPos)
        {
            Vector3 dif = this.posicion - nuevaPos;
            foreach (var parte in partesLista)
            {
                parte.Value.SetPosicion(parte.Value.GetPosicion() - dif);
            }
        }


        // Cambia la posicion de una parte en relacion a la posicion del objeto
        public void setPartePosicionRelativo(string nombre, float xDif, float yDif, float zDif)
        {
            if (this.partesLista.ContainsKey(nombre))
            {
                Vector3 antiguaPos = this.partesLista[nombre].GetPosicion();
                Vector3 nuevaPos =
                (
                    antiguaPos.X + xDif,
                    antiguaPos.Y + yDif,
                    antiguaPos.Z + zDif
                );
                this.partesLista[nombre].SetPosicion(nuevaPos);
            }
        }

        public void dibujar(Shader shader)
        {
            foreach (var parte in partesLista)
            {
                parte.Value.dibujar(shader);
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

        public void cargarCubo()
        {
            Parte cubo = new Parte(this.posicion);
            cubo.cargarCubo();
            this.añadirParte("Cubo", cubo);
        }

        public void cargarAxis()
        {
            Parte axis = new Parte(this.posicion);
            axis.cargarCrossAxis();
            this.añadirParte("Axis", axis);
        }
       
    }
}
