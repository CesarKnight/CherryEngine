using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PrimerFigura
{
    class Cara
    {
        // Los vertices son renderizados en relacion a la coordenada pasada en el metodo dibujar
        public List<VerticeColor> Vertices { get; }

        private uint[] _indicesArray = [];
        
        private float[] verticesArray = [];

        [JsonPropertyOrder(-1)]
        public uint[] IndicesArray
        {
            get { return _indicesArray; }
        }


        // %%%%%%%% Elementos de OpenGL %%%%%%%%%%%%

        // en estos int se guardas punteros
        // declaramos el buffer de vertices para que sean enviados a la gpu de un saque
        int BufferObjetoVertices;
        // declaramos el objeto de array de vertices
        int VertexArrayObject;
        // declaramos el objeto de array de indices
        int ElementBufferObject;

        public Cara()
        {
            this.Vertices = new List<VerticeColor>();
        }

        [JsonConstructor]
        public Cara(List<VerticeColor> vertices, uint[] indicesArray)
        {
            this.Vertices = vertices;
            this._indicesArray = indicesArray;
            inicializarBuffers();
        }

        private void inicializarBuffers()
        {
            // Transformamos la lista a array
            verticesArray = new float[this.Vertices.Count * 6];
            int i = 0;
            foreach (VerticeColor verticeColor in this.Vertices)
            {
                verticesArray[i] = verticeColor.x;
                verticesArray[i + 1] = verticeColor.y;
                verticesArray[i + 2] = verticeColor.z;
                verticesArray[i + 3] = verticeColor.r;
                verticesArray[i + 4] = verticeColor.g;
                verticesArray[i + 5] = verticeColor.b;
                i+=6;
            }

            // asignamos la id del buffer de vertices y el buffer de indices
            BufferObjetoVertices = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            // generamos el objeto de array de vertices y asignamos a opengl
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // le decimos a la gpu que vamos a usar este buffer de vertices
            GL.BindBuffer(BufferTarget.ArrayBuffer, BufferObjetoVertices);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesArray.Length * sizeof(float), verticesArray, BufferUsageHint.DynamicDraw);

            // y este buffer de indices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indicesArray.Length * sizeof(uint), _indicesArray, BufferUsageHint.StaticDraw);

            // configuramos el puntero para los vertices dando a entender
            // que tiene 3 valores floats para los vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // y tambien 3 valores floats para el color de los vertices
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        public void SetVertices(VerticeColor[] verticecolores, uint[] indices)
        {
            foreach (VerticeColor verticeColor in verticecolores)
            {
                this.Vertices.Add(verticeColor);
            }
            this._indicesArray = indices;
            inicializarBuffers();
        }

        // recibe los vertices y los indices y un color en formato hex
        public void SetVerticesColor(VerticeColor[] verticecolores, uint[] indices, string hexColor)
        {
            foreach (VerticeColor verticeColor in verticecolores)
            {
                verticeColor.SetColor(hexColor);
                this.Vertices.Add(verticeColor);
            }
            this._indicesArray = indices;
            inicializarBuffers();
        }

        public void Dibujar(Vector3 pos, Vector3 rotacion , float escala , Shader shader)
        {
            Matrix4 Translation = Matrix4.CreateTranslation(pos);

            Matrix4 Rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotacion.X)) *
                                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotacion.Y)) *
                                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotacion.Z));

            Matrix4 Scale = Matrix4.CreateScale(escala);
            Matrix4 Transformation = Scale * Rotation * Translation;

            int modelLocation = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(modelLocation, false, ref Transformation);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, this._indicesArray.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void ChangeColor(float r, float g, float b)
        {
            foreach (VerticeColor verticeColor in this.Vertices)
            {
                verticeColor.r = r;
                verticeColor.g = g;
                verticeColor.b = b;
            }
            UpdateVertexArray();
        }

        public void ChangeColor(string Hex)
        {
            foreach (VerticeColor verticeColor in this.Vertices)
            {
                verticeColor.SetColor(Hex);
            }
            UpdateVertexArray();
        }

        public void UpdateVertexArray()
        {
            int i = 0;
            foreach (VerticeColor verticeColor in this.Vertices)
            {
                verticesArray[i] = verticeColor.x;
                verticesArray[i + 1] = verticeColor.y;
                verticesArray[i + 2] = verticeColor.z;
                verticesArray[i + 3] = verticeColor.r;
                verticesArray[i + 4] = verticeColor.g;
                verticesArray[i + 5] = verticeColor.b;
                i += 6;
            }
            // y le decimos que lo que vamos a hacer es actualizar el buffer
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, verticesArray.Length * sizeof(float), verticesArray);
        }
    }
}
