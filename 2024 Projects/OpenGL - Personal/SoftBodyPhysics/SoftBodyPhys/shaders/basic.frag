#version 430

out vec4 FragColor;  // Output color of the fragment

uniform sampler2D screenTexture;  // The texture containing the rendered scene
in vec2 TexCoords;  // Texture coordinates from the vertex shader

void main()
{

    vec2 uv = TexCoords;
    // Sample the color from the texture at the current fragment position
    vec4 screenColor = texture(screenTexture, uv);
    
    // Example: blend with a color (adjust as needed)
    FragColor = mix(screenColor, vec4(1.0, 1.0, 1.0, 1.0), 0.01);  // Blend with white
}