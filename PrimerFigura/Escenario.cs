using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using OpenTK.Mathematics;

namespace PrimerFigura
{
    class Escenario
    {
        private Vector3 _posicion;
        public Dictionary<string, Objeto> Objetos { get; set; }

        [JsonPropertyOrder(-1)]
        public float[] Posicion
        {
            get {
                float[] posicionArray = new float[3];
                posicionArray[0] = this._posicion.X;
                posicionArray[1] = this._posicion.Y;
                posicionArray[2] = this._posicion.Z;
                return posicionArray; 
            }
            set {
                _posicion.X = value[0];
                _posicion.Y = value[1];
                _posicion.Z = value[2];
            }
        }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        };

        public Escenario()
        {
            this._posicion = new Vector3(0.0f, 0.0f, 0.0f);
            this.Objetos = new Dictionary<string, Objeto>(); 
        }

        public Escenario(Vector3 posicion)
        {
            this._posicion = posicion;
            this.Objetos = new Dictionary<string, Objeto>();
        }


        public void dibujar(Shader shader)
        {
            foreach (var objeto in Objetos)
            {
                objeto.Value.dibujar(_posicion, shader);
            }
        }

        public void GuardarEscenario(string nombreArchivo)
        {
            string jsonString = JsonSerializer.Serialize(this, options);

            System.Console.WriteLine("Archivo guardado: \n" + jsonString);
            File.WriteAllText(nombreArchivo, jsonString);
        }

        public void CargarEscenario(string nombreArchivo)
        {
            string jsonString = File.ReadAllText(nombreArchivo);
            System.Console.WriteLine("leyendo archivo: \n" + jsonString);
            if (string.IsNullOrEmpty(jsonString))
                throw new Exception("El archivo está vacío o no existe.");

            var EscenarioCargado = JsonSerializer.Deserialize<Escenario>(jsonString, options);
            if(EscenarioCargado == null)
                throw new Exception("Error al deserializar el archivo.");

            this._posicion = new Vector3(
                EscenarioCargado.Posicion[0],
                EscenarioCargado.Posicion[1],
                EscenarioCargado.Posicion[2]
            );
            this.Objetos = EscenarioCargado.Objetos;
        }

        public void CargarEscenarioPrueba()
        {
            Objeto cubitos = new Objeto(0.0f, 0.0f, 0.0f);
            cubitos.cargarCubos();
            this.Objetos.Add("Cubitos", cubitos);

            Objeto eje = new Objeto(new Vector3(0,0,0));
            eje.cargarAxis();
            this.Objetos.Add("Ejes", eje);
        }
    }
}
