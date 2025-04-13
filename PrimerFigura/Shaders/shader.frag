#version 330 core
out vec4 FragColor;

in vec3 vertexColor; //entra el color del vertex shader

void main()
{
    FragColor = vec4(vertexColor, 1.0); //asigna el color al fragmento aunque con alpha 1.0
}