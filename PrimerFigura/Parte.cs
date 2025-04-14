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
        // Debe ser la posicion en relacion al objeto padre
        public Vector3 posicion { get; set; }
        // Vertices: Son puntos donde se unen las lineas para formar una figura
        float[] vertices = [];
        // Indices: Es el orden en el que se unen los vertices para formar la figura
        uint[] indices = [];

        // en este objeto se guardan los vertices para que sean enviados a la gpu de un saque
        // el int es solo la id del objeto
        int BufferObjetoVertices;
        // declaramos el objeto de array de vertices
        int VertexArrayObject;

        int ElementBufferObject;

        public Parte(float centroX, float centroY, float centroZ)
        {
            this.posicion = new Vector3(centroX, centroY, centroZ);
        }

        public Parte(float centroX, float centroY, float centroZ, float[] vertices, uint[] indices)
        {
            this.posicion = new Vector3(centroX, centroY, centroZ);
            this.vertices = vertices;
            this.indices = indices;

            inicializarBuffers();
        }

        private void inicializarBuffers()
        {
            // asignamos la id del buffer de vertices
            BufferObjetoVertices = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            // generamos el objeto de array de vertices y asignamos a opengl
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // le decimos a la gpu que vamos a usar este buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, BufferObjetoVertices);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // configuramos el puntero para los vertices dando a entender
            // que tiene 3 valores floats para los vertices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // y tambien 3 valores floats para el color de los vertices
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        public void cargar(float[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;

            inicializarBuffers();
        }

        public void dibujar(Shader shader)
        {
            Matrix4 model = Matrix4.CreateTranslation(posicion);
            int modelLocation = GL.GetUniformLocation(shader.Handle, "model");
            GL.UniformMatrix4(modelLocation, false, ref model);
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void borrar()
        {
            GL.DeleteBuffer(BufferObjetoVertices);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
        }
    }
}
