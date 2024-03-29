attribute vec4 position;
attribute vec4 color;
varying vec4 fragColor;

void main()
{
    gl_Position = position;
    fragColor = color;
}