uniform sampler2D texture;

uniform vec2 position;
uniform vec2 size;

void main()
{
    //vec2 center = position / size.yy;
    vec2 uv = (gl_FragCoord.xy * 2.0 - size) / 1080;;

    float d = 1 - length(uv) * 2;

    gl_FragColor = vec4(1.0,1.0,0.6, d - .3);
}