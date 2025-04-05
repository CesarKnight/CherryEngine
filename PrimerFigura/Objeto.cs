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
        float centroX, centroY, centroZ;
        float[] vertices = [];
        uint[] indices = [];

        // en este objeto se guardan los vertices para que sean enviados a la gpu de un saque
        // el int es solo la id del objeto
        int BufferObjetoVertices;
        // declaramos el objeto de array de vertices
        int VertexArrayObject;

        int ElementBufferObject;

        public Objeto(float centroX, float centroY, float centroZ)
        {
            this.centroX = centroX;
            this.centroY = centroY;
            this.centroZ = centroZ;
        }

        public Objeto(float centroX, float centroY, float centroZ, float[] vertices , uint[] indices)
        {
            this.centroX = centroX;
            this.centroY = centroY;
            this.centroZ = centroZ;
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

            // configuramos el puntero para los vertices dando a entender que tiene 3 valores floats
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }

        public void cargar(float[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;

            inicializarBuffers();
        }


        public void dibujar(Shader shader)
        {
            Matrix4 model = Matrix4.CreateTranslation(centroX, centroY, centroZ);   
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
