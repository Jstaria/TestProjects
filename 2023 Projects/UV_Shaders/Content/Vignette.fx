#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D Sampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

float time;

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 newUV = input.UV;
    float4 fragColor = tex2D(Sampler, newUV);
	
    float3 col = input.Color.rgb;
	
    float vignette = distance(newUV, 0.5);
	
    vignette = lerp(1, 1.5 * sin(time), vignette);
	
    float3 Color = (100, 0.0, .125);
    
    col = lerp(col * Color, col, vignette);
    //col *= vignette;

    col *= tex2D(Sampler, newUV).rgb;
	
    fragColor = float4(col, 1);
	
    return fragColor;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};