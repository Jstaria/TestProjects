uniform sampler2D texture;

uniform vec2 position;
uniform vec2 size;

void main()
{
    position.y = -position.y;
    vec2 uv = gl_FragCoord.xy / vec2(1920,1080) * 2.0 - 1.0;
    vec2 screenCenter = vec2(1920,1080) / 2;
    vec2 offset = position;
    uv -= offset / vec2(1920,1080);

    float d = 1 - length(uv);

    gl_FragColor = vec4(uv, 0.0, d);
}