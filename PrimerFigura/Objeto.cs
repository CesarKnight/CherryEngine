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

        public void cargarCubo()
        {
            // vertices de un cubo
            float[] vertices ={
            // Cara frontal         // Colores
            -0.5f, -0.5f,  0.5f,    0.0f , 1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,    0.0f , 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,    0.0f , 1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,    0.0f , 1.0f, 0.0f,

            // Cara trasera
            -0.5f, -0.5f, -0.5f,    1.0f , 0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,    1.0f , 0.0f, 0.0f,
             0.5f,  0.5f, -0.5f,    1.0f , 0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,    1.0f , 0.0f, 0.0f,

            // Cara izquierda
            -0.5f, -0.5f, -0.5f,     0.05f , 0.0f, 0.5f,
            -0.5f, -0.5f,  0.5f,     0.05f , 0.0f, 0.5f,
            -0.5f,  0.5f,  0.5f,     0.05f , 0.0f, 0.5f,
            -0.5f,  0.5f, -0.5f,     0.05f , 0.0f, 0.5f,

            // Cara derecha
             0.5f, -0.5f, -0.5f,     1.0f , 0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,     1.0f , 0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,     1.0f , 0.0f, 0.0f,
             0.5f,  0.5f, -0.5f,     1.0f , 0.0f, 0.0f,

            // Cara superior
            -0.5f,  0.5f, -0.5f,     1.0f , 0.0f, 0.0f,
             0.5f,  0.5f, -0.5f,     1.0f , 0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,     1.0f , 0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,     1.0f , 0.0f, 0.0f,

            // Cara inferior
            -0.5f, -0.5f, -0.5f,     1.0f , 0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,     1.0f , 0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,     1.0f , 0.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,     1.0f , 0.0f, 0.0f,
            };

            // indices para dibujar el cubo usando elementos
            uint[] indices = {
            // Cara frontal
            0, 1, 2, 2, 3, 0,
            // Cara trasera
            4, 5, 6, 6, 7, 4,
            // Cara izquierda
            8, 9, 10, 10, 11, 8,
            // Cara derecha
            12, 13, 14, 14, 15, 12,
            // Cara superior
            16, 17, 18, 18, 19, 16,
            // Cara inferior
            20, 21, 22, 22, 23, 20
            };

            this.cargar(vertices, indices);
        }
        public void cargarU()
        {
            // vertices de una U hecha con bloques, rotada 90 grados alrededor del eje Y
            float[] vertices = {
                // Bloque inferior izquierdo    // COlores
                -0.5f, -0.5f, -0.5f,            1.0f , 0.0f, 0.0f,
                -0.5f, -0.5f, -0.3f,            1.0f , 0.0f, 0.0f,
                -0.5f,  1.0f, -0.3f,            1.0f , 0.0f, 0.0f,
                -0.5f,  1.0f, -0.5f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.5f, -0.5f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.5f, -0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f,  1.0f, -0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f,  1.0f, -0.5f,            1.0f , 0.0f, 0.0f,

                // Bloque inferior derecho
                -0.5f, -0.5f,  0.3f,            1.0f , 0.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,            1.0f , 0.0f, 0.0f,
                -0.5f,  1.0f,  0.5f,            1.0f , 0.0f, 0.0f,
                -0.5f,  1.0f,  0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.5f,  0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.5f,  0.5f,            1.0f , 0.0f, 0.0f,
                 0.5f,  1.0f,  0.5f,            1.0f , 0.0f, 0.0f,
                 0.5f,  1.0f,  0.3f,            1.0f , 0.0f, 0.0f,

                // Bloque central inferior
                -0.5f, -0.5f, -0.3f,            1.0f , 0.0f, 0.0f,
                -0.5f, -0.5f,  0.3f,            1.0f , 0.0f, 0.0f,
                -0.5f, -0.3f,  0.3f,            1.0f , 0.0f, 0.0f,
                -0.5f, -0.3f, -0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.5f, -0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.5f,  0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.3f,  0.3f,            1.0f , 0.0f, 0.0f,
                 0.5f, -0.3f, -0.3f,            1.0f , 0.0f, 0.0f,
            };

            // indices para dibujar la U usando elementos
            uint[] indices = {
                // Bloque inferior izquierdo
                0, 1, 2, 2, 3, 0,
                0, 1, 5, 5, 4, 0,
                1, 2, 6, 6, 5, 1,
                2, 3, 7, 7, 6, 2,
                3, 0, 4, 4, 7, 3,
                4, 5, 6, 6, 7, 4,

                // Bloque inferior derecho
                8, 9, 10, 10, 11, 8,
                8, 9, 13, 13, 12, 8,
                9, 10, 14, 14, 13, 9,
                10, 11, 15, 15, 14, 10,
                11, 8, 12, 12, 15, 11,
                12, 13, 14, 14, 15, 12,

                // Bloque central inferior
                16, 17, 18, 18, 19, 16,
                16, 17, 21, 21, 20, 16,
                17, 18, 22, 22, 21, 17,
                18, 19, 23, 23, 22, 18,
                19, 16, 20, 20, 23, 19,
                20, 21, 22, 22, 23, 20
            };

            this.cargar(vertices, indices);
        }
    }
}
