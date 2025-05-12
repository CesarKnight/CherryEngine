using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using OpenTK.Mathematics;
using CherryEngine.Core;

namespace CherryEngine.Components
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
                posicionArray[0] = _posicion.X;
                posicionArray[1] = _posicion.Y;
                posicionArray[2] = _posicion.Z;
                return posicionArray; 
            }
            set {
                _posicion = new Vector3(0,0,0);
                Trasladar(value[0], value[1], value[2]);
            }
        }

        public float[] Rotation
        {
            get
            {
                float[] rotacionArray = new float[3];
                rotacionArray[0] = _rotation.X;
                rotacionArray[1] = _rotation.Y;
                rotacionArray[2] = _rotation.Z;
                return rotacionArray;
            }
            set
            {
                _rotation = new Vector3(0, 0, 0);
                Rotar(value[0], value[1], value[2]);
            }
        }

        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = 1.0f;
                Escalar(value);
            }
        }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        };

        public Escenario()
        {
            _posicion = new Vector3(0.0f, 0.0f, 0.0f);
            _rotation = new Vector3(0.0f, 0.0f, 0.0f);
            _scale = 1.0f;
            Objetos = new Dictionary<string, Objeto>(); 
        }
        public Escenario(float x, float y, float z):this()
        {
            _posicion = new Vector3(x, y, z);
        }
        public Escenario(Vector3 posicion):this()
        {
            _posicion = posicion;
        }

        public void dibujar(Shader shader)
        {
            Matrix4 escenarioRotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_rotation.X)) *
                                      Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotation.Y)) *
                                      Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(_rotation.Z));

            Matrix4 escenarioScale = Matrix4.CreateScale(_scale);

            Matrix4 escenarioTransform = escenarioScale * escenarioRotation;

            foreach (var objeto in Objetos)
            {
                objeto.Value.dibujar(_posicion, escenarioTransform, shader);
            }
        }

        public void Escalar(float multiplicador)
        {
            _scale *= multiplicador;           
        }

        public void Rotar(float x, float y, float z)
        {
            _rotation.X += x;
            _rotation.Y += y;
            _rotation.Z += z;
        }

        public void Trasladar(float x, float y, float z)
        {
            _posicion.X += x;
            _posicion.Y += y;
            _posicion.Z += z;
        }

        public void GuardarEscenario(string filePath, bool inProduction)
        {
            string jsonString = JsonSerializer.Serialize(this, options);
            
            if (!inProduction)
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string shaderDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\.Escenarios"));
                filePath = Path.Combine(shaderDir, filePath);
            }

            File.WriteAllText(filePath, jsonString);
        }

        public bool CargarEscenario(string filePath, bool inProduction)
        {
            if (! inProduction)
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string shaderDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\.Escenarios"));
                filePath = Path.Combine(shaderDir, filePath);
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine("El archivo no existe.");
                return false;
            }
            string jsonString = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(jsonString))
            {
                Console.WriteLine("El archivo está vacío.");
                return false;
            }
            var EscenarioCargado = JsonSerializer.Deserialize<Escenario>(jsonString, options);

            if(EscenarioCargado == null)
            {
                Console.WriteLine("Error al deserializar el archivo.");
                return false;
            }

            _posicion = new Vector3(
                EscenarioCargado.Posicion[0],
                EscenarioCargado.Posicion[1],
                EscenarioCargado.Posicion[2]
            );
            _rotation = new Vector3(
                EscenarioCargado.Rotation[0],
                EscenarioCargado.Rotation[1],
                EscenarioCargado.Rotation[2]
            );
            _scale = EscenarioCargado.Scale;

            Objetos = EscenarioCargado.Objetos;
            return true;
        }

        public void CargarEscenarioPrueba()
        {
            Objeto cubitos = new Objeto(0.0f, 0.0f, 3.0f);
            cubitos.cargarCubos();
            cubitos.Scale = 2f;
            cubitos.Rotar(45, 45, 30);
            Objetos.Add("U", cubitos);

            Objeto U2 = new Objeto(5.0f, 0.0f, 0.0f);
            U2.cargarCubos();
            Objetos.Add("U2", U2);

            Objeto eje = new Objeto(new Vector3(0,0,0));
            eje.cargarAxis();
            Objetos.Add("Ejes", eje);

            Objeto esfera = new Objeto(0, 1, 0);
            esfera.CargarEsfera();
            Objetos.Add("Esfera", esfera);
        }
    }
}
