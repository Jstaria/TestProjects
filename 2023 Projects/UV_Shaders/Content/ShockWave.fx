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

float x;
float y;

float UpperFeather;
float BottomFeather;

float rippleIntensity;

float time;

float4 MainPS(in VertexShaderOutput input) : COLOR
{
	
    float2 newUV = input.UV * 2 - 1;
	
    //float len2 = length(input.UV - float2(x, 1));
    float len = length(input.UV - float2(x, y));
	
    float timer = frac(time);
    float upperRing = smoothstep(len + UpperFeather, len - BottomFeather, timer);
    //float upperRing2 = smoothstep(len2 + UpperFeather, len2 - BottomFeather, timer);
	
    float inverseRing = 1 - upperRing;
    //float inverseRing2 = 1 - upperRing2;
	
    float finalRing = upperRing * inverseRing;
    //float finalRing2 = upperRing2 * inverseRing2;
	
    float2 finalUV = input.UV - newUV * finalRing * rippleIntensity * (1 - timer);
    //float2 finalUV2 = input.UV - newUV * finalRing2 * rippleIntensity * (1 - timer);
	
    //finalUV = (input.UV) / finalUV; //* finalUV2;
	
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