uniform int positionsAndWidths;
uniform sampler2D texture;

void main()
{
    vec2 uv = gl_TexCoord[0].xy;

    vec4 color = texture2D(texture, uv);

    int positionX = int(extractRange(positionsAndWidths, 0, 14));
    int positionY = int(extractRange(positionsAndWidths, 13, 14));
    int width = int(extractRange(positionsAndWidths, 27, 10));
    int height = int(extractRange(positionsAndWidths, 37, 10));

    if (uv.x >= positionX && uv.x <= positionX + width && uv.y >= positionY && uv.y <= positionY + height) {
        
        gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
            return;
    }

    // If not an outline pixel, keep original color
    gl_FragColor = color;
}