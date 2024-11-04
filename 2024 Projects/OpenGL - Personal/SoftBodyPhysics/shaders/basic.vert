#version 330

layout(location = 0) in vec3 inPos;  // Vertex position
layout(location = 1) in vec2 inTexCoord;  // Vertex texture coordinate

uniform mat4 MVP;

out vec2 TexCoords;  // Output texture coordinates to the fragment shader

void main()
{
    gl_Position = vec4(inPos.x, inPos.y, 0.0, 1.0);
    TexCoords = inTexCoord;  // Pass texture coordinates to the fragment shader
}