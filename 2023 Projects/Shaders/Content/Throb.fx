#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Position within a sin curve that you're in
float SINLOC;

float4 filterColor;

texture Texture;
sampler TextureSanpler = sampler_state
{
    Texture = <Texture>;
};


sampler2D samplerTexture;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
};

// Pixel shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 textureColor = tex2D(samplerTexture, input.TextureCoordinate);
	
    float4 color;
	
    if (textureColor.a != 0) 
    {
        color = float4(
            textureColor.r + (textureColor.r - filterColor.r) * SINLOC,
            textureColor.g + (textureColor.g - filterColor.g) * SINLOC,
            textureColor.b + (textureColor.b - filterColor.b) * SINLOC,
            textureColor.a);
    }
    else
    {
        color = float4(textureColor.rgba);
    }
    return color * filterColor;
}

technique BasicColorDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
};