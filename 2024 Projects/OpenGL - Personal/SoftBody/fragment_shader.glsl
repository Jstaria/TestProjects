#version 330 core

in vec3 vertexColor; // Color received from vertex shader
out vec4 fragColor;  // Output color of the fragment

void main()
{
    fragColor = vec4(vertexColor, 1.0); // Set the output color with full opacity
}