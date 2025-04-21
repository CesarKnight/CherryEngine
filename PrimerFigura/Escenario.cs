using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace PrimerFigura
{
    class Escenario
    {
        private Vector3 posicion;
        private Dictionary<string, Objeto> Objetos;

        public Escenario()
        {
            this.posicion = new Vector3(0.0f, 0.0f, 0.0f);
            this.Objetos = [];
        }

        public Escenario(Vector3 posicion)
        {
            this.posicion = posicion;
            this.Objetos = [];
        }

        public void SetDiccionaryObjetos(Objeto[] ObjetosArray)
        {
            int i = 1;
            foreach (Objeto objeto in ObjetosArray)
            {
                this.Objetos.Add("Objeto" + i, objeto);
            }
        }

        public void dibujar(Shader shader)
        {
            foreach (var objeto in Objetos)
            {
                objeto.Value.dibujar(posicion, shader);
            }
        }

        public void CargarEscenarioPrueba()
        {
            Objeto cubitos = new Objeto(0.0f, 0.0f, 0.0f);
            cubitos.cargarCubos();
            cubitos.cargarAxis();
            this.Objetos.Add("Cubitos", cubitos);
        }
    }
}
