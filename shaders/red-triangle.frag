﻿#version 330 core

in vec3 Color;

out vec4 result;

void main()
{
    result = vec4(Color, 1);
}