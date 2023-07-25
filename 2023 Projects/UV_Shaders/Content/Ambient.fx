#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;
	
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(input.Position, View);
    output.Position = mul(viewPosition, Projection);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
	return AmbientColor * AmbientIntensity;
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};