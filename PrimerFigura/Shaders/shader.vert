#version 330 core

layout(location = 0) in vec3 aPosition;     // en memoria las primeras 3 direcciones son de vertices
layout(location = 1) in vec3 aColor;        // y las siguientes 3 son de color

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 vertexColor; // color para el shader de fragmento

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    vertexColor = aColor; // pasamos el color al shader de fragmento
}