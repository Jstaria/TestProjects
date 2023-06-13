#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

int countInt;
sampler samplerTexture;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 TextureCoordinate : TEXCOORD0;
	float4 Color : COLOR0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 col = tex2D(samplerTexture, input.TextureCoordinate);
	
    float3 mulCol = float3(col.r, col.g, col.b);
	
    float4 aboveColor = tex2D(samplerTexture,
            (input.TextureCoordinate) + float2(0, -2));
        
    float4 belowColor = tex2D(samplerTexture,
            (input.TextureCoordinate) + float2(0, 2));
        
    float4 leftColor = tex2D(samplerTexture,
            (input.TextureCoordinate) + float2(-2, 0));
        
    float4 rightColor = tex2D(samplerTexture,
            (input.TextureCoordinate) + float2(2, 0));
        
    float4 color = float4(
            (col.r + aboveColor.r + belowColor.r + rightColor.r + leftColor.r) / 5,
            (col.g + aboveColor.g + belowColor.g + rightColor.g + leftColor.g) / 5,
            (col.b + aboveColor.b + belowColor.b + rightColor.b + leftColor.b) / 5,
            (col.a + aboveColor.a + belowColor.a + rightColor.a + leftColor.a) / 5);
	
    if (countInt % 2)
    {
        col.rgb += -1;
    }
    else
    {
        col.rgb += 1;
    }
	
    return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};