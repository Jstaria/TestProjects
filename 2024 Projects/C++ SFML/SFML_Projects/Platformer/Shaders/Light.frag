
void main()
{
    vec2 uv = gl_TexCoord[0].xy * 2.0 - 1.0;

    float d = length(uv);

    vec4 color = vec4(0.0,0.0,0.0,d);

    // If not an outline pixel, keep original color

    gl_FragColor = color;
}