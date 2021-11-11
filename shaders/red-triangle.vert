#version 330 core
in vec3 pos;
in vec3 color;

out vec3 Color;
//uniform mat4 trans;

void main()
{
    Color = color;
    gl_Position = vec4(pos.x, pos.y, pos.z, 1.0);
}