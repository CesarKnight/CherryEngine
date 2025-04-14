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

        public void setPosicion(float x, float y, float z)
        {
            Vector3 antiguaPosicion = this.posicion;
            this.posicion = new Vector3(x, y, z);

            foreach (var parte in partesLista)
            {
                parte.Value.posicion += this.posicion - antiguaPosicion;
            }
        }
        public void dibujar(Shader shader)
        {
            foreach (var parte in partesLista)
            {
                parte.Value.dibujar(shader);

            }
        }

        // Cambia la posicion de una parte en relacion a la posicion del objeto
        public void setPartePosicionRelativo(string nombre, float xDif, float yDif, float zDif)
        {
            if (this.partesLista.ContainsKey(nombre))
            {
                this.partesLista[nombre].posicion = new Vector3(
                    this.posicion.X + xDif,
                    this.posicion.Y + yDif,
                    this.posicion.Z + zDif
                );
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

       
    }
}
