#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float xSize, ySize, xDraw, yDraw;

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
	
    float vertPixSize = 1.0f / ySize;
    float horPixSize = 1.0f / xSize;
	
    float4 color;
	
    if (xDraw / xSize < .6f || yDraw / ySize < .6f)
    {
        if (xDraw / xSize < .4f || yDraw / ySize < .4f)
        {
            vertPixSize = 2.0f / ySize;
            horPixSize = 2.0f / xSize;
        }
        
        float4 aboveColor = tex2D(samplerTexture, 
            (input.TextureCoordinate) + float2(0, -vertPixSize));
        
        float4 belowColor = tex2D(samplerTexture,
            (input.TextureCoordinate) + float2(0, vertPixSize));
        
        float4 leftColor = tex2D(samplerTexture,
            (input.TextureCoordinate) + float2(-horPixSize, 0));
        
        float4 rightColor = tex2D(samplerTexture,
            (input.TextureCoordinate) + float2(horPixSize, 0));
        
        color = float4(
            (textureColor.r + aboveColor.r + belowColor.r + rightColor.r + leftColor.r) / 5,
            (textureColor.g + aboveColor.g + belowColor.g + rightColor.g + leftColor.g) / 5,
            (textureColor.b + aboveColor.b + belowColor.b + rightColor.b + leftColor.b) / 5,
            (textureColor.a + aboveColor.a + belowColor.a + rightColor.a + leftColor.a) / 5);
    }
    else
    {
 		color = float4(textureColor.r, textureColor.g, textureColor.b, textureColor.a);
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