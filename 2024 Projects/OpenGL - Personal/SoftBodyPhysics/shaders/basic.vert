#version 430

layout(location = 0) in vec3 aPos;  // Vertex position
layout(location = 1) in vec2 aTexCoord;  // Vertex texture coordinate

out vec2 TexCoords;  // Output texture coordinates to the fragment shader

void main()
{
    gl_Position = vec4(aPos, 1.0);
    TexCoords = aTexCoord;  // Pass texture coordinates to the fragment shader
}