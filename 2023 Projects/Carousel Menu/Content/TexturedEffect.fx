// TexturedEffect.fx

// Input structure for the vertex shader
struct VertexInput
{
    float4 Position : POSITION0; // Vertex position (3D)
    float2 TexCoord : TEXCOORD0; // Texture coordinates (UV)
};

// Output structure for the vertex shader
struct VertexOutput
{
    float4 Position : SV_POSITION0; // Transformed vertex position (screen space)
    float2 TexCoord : TEXCOORD0; // Texture coordinates (UV)
};

// Texture sampler
sampler TextureSampler = sampler_state
{
    Texture = <Texture>; // The texture to sample from
    MagFilter = LINEAR; // Magnification filter (linear for smooth results)
    MinFilter = LINEAR; // Minification filter (linear for smooth results)
    MipFilter = LINEAR; // Mipmapping filter (linear for smooth results)
    AddressU = CLAMP; // Texture addressing mode in U direction (clamp to edge)
    AddressV = CLAMP; // Texture addressing mode in V direction (clamp to edge)
};

// Declare World, View, and Projection matrices as shader parameters
float4x4 World; // World matrix to transform from model to world space
float4x4 View; // View matrix to transform from world to view space
float4x4 Projection; // Projection matrix to transform from view to screen space

// Vertex shader
VertexOutput TexturedVertexShader(VertexInput input)
{
    VertexOutput output;

    // Transform the vertex position (Billboard)
    // First, transform the vertex from model space to world space using the World matrix
    output.Position = mul(input.Position, World);
    
    // Next, transform the vertex from world space to view space using the View matrix
    output.Position = mul(output.Position, View);
    
    // Finally, transform the vertex from view space to screen space using the Projection matrix
    output.Position = mul(output.Position, Projection);

    // Pass the texture coordinates to the pixel shader
    output.TexCoord = input.TexCoord;

    return output;
}

// Pixel shader
float4 TexturedPixelShader(VertexOutput input) : COLOR0
{
    // Sample the texture using the input texture coordinates
    float4 color = tex2D(TextureSampler, input.TexCoord);

    // Return the sampled color as the final pixel color
    return color;
}

// Technique for rendering textured quads
technique Textured
{
    pass P0
    {
        // Set the vertex and pixel shaders for this technique
        VertexShader = compile vs_3_0 TexturedVertexShader(); // Vertex shader
        PixelShader = compile ps_3_0 TexturedPixelShader(); // Pixel shader
    }
}
