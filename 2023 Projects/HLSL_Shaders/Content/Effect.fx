// DiffuseLighting.fx

// Define the input and output structures for the vertex and pixel shaders
struct VertexInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
};

struct VertexOutput
{
    float4 Position : SV_POSITION;
    float3 Normal : TEXCOORD0;
};

// User-defined variables
matrix World;
matrix View;
matrix Projection;
matrix WorldInverseTranspose;

// Diffuse lighting parameters
float3 DiffuseLightingDirection = float3(1, 0 ,0);
float3 DiffuseColor = (1, 1, 1);
float AmbientIntensity = .1;;
float DiffuseIntensity = 1;

// Vertex shader function
VertexOutput DiffuseVertexShader(VertexInput input)
{
    VertexOutput output;

    // Transform vertex position and normal to world space
    output.Position = mul(mul(input.Position, World), mul(View, Projection));
    output.Normal = normalize(mul(input.Normal, (float3x3) WorldInverseTranspose));

    return output;
}

// Pixel shader function
float4 DiffusePixelShader(VertexOutput input) : SV_TARGET
{
    // Calculate diffuse lighting
    float3 normalizedLightDir = normalize(DiffuseLightingDirection);
    float diffuseFactor = max(0, dot(normalizedLightDir, input.Normal));
    float3 diffuseLight = DiffuseColor * DiffuseIntensity * diffuseFactor;

    // Combine ambient and diffuse lighting (ambient is multiplied by a constant value for simplicity)
    float3 finalColor = (AmbientIntensity * DiffuseColor) + diffuseLight;

    return float4(finalColor, 1);
}

technique DiffuseTechnique
{
    pass Pass1
    {
        // Vertex shader to use
        VertexShader = compile vs_3_0 DiffuseVertexShader();

        // Pixel shader to use
        PixelShader = compile ps_3_0 DiffusePixelShader();
    }
}