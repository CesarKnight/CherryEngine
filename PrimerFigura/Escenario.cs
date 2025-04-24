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
        private Vector3 _rotation;
        private float _scale;
        
        [JsonPropertyOrder(4)]
        public Dictionary<string, Objeto> Objetos { get; set; }

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
                this.Rotar(value[0], value[1], value[2]);
            }
        }

        public float Scale
        {
            get { return this._scale; }
            set
            {
                this.Escalar(value);
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
        public Escenario(float x, float y, float z):this()
        {
            this._posicion = new Vector3(x, y, z);
        }
        public Escenario(Vector3 posicion):this()
        {
            this._posicion = posicion;
        }

        public void dibujar(Shader shader)
        {
            foreach (var objeto in Objetos)
            {
                objeto.Value.dibujar(_posicion, shader);
            }
        }

        public void Escalar(float multiplicador)
        {
            this._scale += multiplicador;
            Matrix4 scaleTrans = Matrix4.CreateScale(this._scale);
            foreach (var objeto in Objetos)
            {
                Vector4 objetoRelativePos = new Vector4(
                    objeto.Value.OffsetCoords[0],
                    objeto.Value.OffsetCoords[1],
                    objeto.Value.OffsetCoords[2],
                    1.0f
                );
                Vector4 newObjetoOffset = objetoRelativePos * scaleTrans;
                objeto.Value.OffsetCoords = [newObjetoOffset.X, newObjetoOffset.Y, newObjetoOffset.Z];
                objeto.Value.Scale = this._scale;
            }
        }

        public void Rotar(float x, float y, float z)
        {
            this._rotation.X += x;
            this._rotation.Y += y;
            this._rotation.Z += z;
            Matrix4 rotationTrans = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(this._rotation.X)) *
                                         Matrix4.CreateRotationY(MathHelper.DegreesToRadians(this._rotation.Y)) *
                                         Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(this._rotation.Z));
            foreach (var objeto in Objetos)
            {
                Vector4 objetoRelativePos = new Vector4(
                    objeto.Value.OffsetCoords[0],
                    objeto.Value.OffsetCoords[1],
                    objeto.Value.OffsetCoords[2],
                    1.0f
                );
                Vector4 newObjetoOffset = objetoRelativePos * rotationTrans;
                objeto.Value.OffsetCoords = [newObjetoOffset.X, newObjetoOffset.Y, newObjetoOffset.Z];
                objeto.Value.Rotation = [this._rotation.X, this._rotation.Y, this._rotation.Z];
            }
        }

        public void Trasladar(float x, float y, float z)
        {
            this._posicion.X += x;
            this._posicion.Y += y;
            this._posicion.Z += z;
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
            Objeto cubitos = new Objeto(0.0f, 0.0f, 3.0f);
            cubitos.cargarCubos();
            this.Objetos.Add("U", cubitos);


            Objeto U2 = new Objeto(-5.0f, 0.0f, 0.0f);
            U2.cargarCubos();
            U2.Rotation = [0.0f, 45.0f, 0.0f];
            this.Objetos.Add("U2", U2);

            Objeto eje = new Objeto(new Vector3(0,0,0));
            eje.cargarAxis();
            this.Objetos.Add("Ejes", eje);
        }
    }
}
