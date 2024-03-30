// Fragment shader
uniform float time;
uniform float intensity;
uniform bool isActive;
uniform sampler2D texture;
void main()
{
    float threshold = 0;
    // Get the current UV coordinates
    vec2 uv = gl_TexCoord[0].xy;

    // Get the color of the current pixel
    vec4 color = texture2D(texture, uv);
    float alpha = 0.5 + 0.5 * cos(time * 5.0) / 2;
    //alpha = min(alpha, 0.5);
    // Check if the alpha value is above the threshold
    if (color.a == threshold)
    {
        // Check neighbors for lower alpha values
        //vec4 topLeft = texture2D(texture, uv + vec2(-1, -1) / textureSize(texture, 0));
        vec4 top = texture2D(texture, uv + vec2(0, -1) / textureSize(texture, 0));
        //vec4 topRight = texture2D(texture, uv + vec2(1, -1) / textureSize(texture, 0));
        vec4 left = texture2D(texture, uv + vec2(-1, 0) / textureSize(texture, 0));
        vec4 right = texture2D(texture, uv + vec2(1, 0) / textureSize(texture, 0));
        //vec4 bottomLeft = texture2D(texture, uv + vec2(-1, 1) / textureSize(texture, 0));
        vec4 bottom = texture2D(texture, uv + vec2(0, 1) / textureSize(texture, 0));
        //vec4 bottomRight = texture2D(texture, uv + vec2(1, 1) / textureSize(texture, 0));

        // Check if any neighbor has lower alpha
        if (color.a == 0 && (top.a > 0 || left.a > 0 || right.a > 0 || bottom.a > 0) && isActive)
        {
            // Set the color to the outline color (e.g., black)
            gl_FragColor = vec4(1.0, 1.0, 1.0, alpha * intensity);
            return;
        }
    }

    // If not an outline pixel, keep original color
    gl_FragColor = color;
}