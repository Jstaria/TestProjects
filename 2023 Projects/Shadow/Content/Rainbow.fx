#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler2D TextureSampler;

float time;

struct VertexShaderOutput
{
	float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 col = tex2D(TextureSampler, input.TextureCoordinates);
	
    float red = (float) sin(time) / 2 + .5f;
    float green = (float) sin(time + 2) / 2 + .5f;
    float blue = (float) sin(time + 4) / 2 + .5f;
    
    float avgCol = (col.r + col.g + col.b) / 2;

    float3 mulCol = input.Color.rgb * float3((red + col.r) / 2, (green + col.g) / 2, (blue + col.b) / 2) * avgCol;

    return float4(mulCol, col.a);
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};