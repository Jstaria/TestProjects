uniform vec2 position;
uniform vec2 bounds;
uniform sampler2D texture;

vec2 textureBounds;

void main()
{
    textureBounds = textureSize(texture, 0);
    position /= textureBounds.y;
    bounds /= textureBounds.y;

    vec2 uv = gl_TexCoord[0].xy;
    uv.x *= 1920.0 / 1080.0;
    uv.y = 1 - uv.y;

    vec4 color = texture2D(texture, uv);

    if (uv.x >= position.x && uv.x < position.x + bounds.x &&
        uv.y >= position.y && uv.y < position.y + bounds.y) {
        
        gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
            return;
    }

    // If not an outline pixel, keep original color

    gl_FragColor = vec4(uv,0.0,0.0);
}