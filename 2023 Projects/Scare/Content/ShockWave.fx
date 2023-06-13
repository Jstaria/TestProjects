#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
    float2 UV : TEXCOORD0;
};

sampler2D textureSampler;

float UpperFeather;
float BottomFeather;

float time;

float4 MainPS(in VertexShaderOutput input) : COLOR
{
    float2 newUV = input.UV * 2 - 1;
	
    float len = length(newUV);
    float timer = frac(time);
    float upperRing = smoothstep(len + UpperFeather, len - BottomFeather, timer);
	
    float inverseRing = 1 - upperRing;
	
    float finalRing = upperRing * inverseRing;
	
    float2 finalUV = newUV * finalRing + input.UV;
	
    float4 col = tex2D(textureSampler, finalUV);
	
    return float4(col.rgb, 1);
};

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};