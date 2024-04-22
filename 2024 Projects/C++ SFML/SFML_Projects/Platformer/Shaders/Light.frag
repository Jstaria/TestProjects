uniform sampler2D texture;

uniform vec2 position;
uniform vec2 size;

void main()
{
    vec2 uv = gl_FragCoord.xy / position;

    gl_FragColor = vec4(uv, 0.0, 1.0);
}