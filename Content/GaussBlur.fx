// Pixel shader applies a one dimensional gaussian blur filter
sampler TextureSampler : register(s0);

#define SAMPLE_COUNT 15

float2 SampleOffsets[SAMPLE_COUNT];
float SampleWeights[SAMPLE_COUNT];


float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 color = 0;
    
    // Combines the pixels at the distances in offset way from the current pixel, weighted to keep current pixel most prominent.
    for (int i = 0; i < SAMPLE_COUNT; i++)
    {
        color += tex2D(TextureSampler, texCoord + SampleOffsets[i]) * SampleWeights[i];
    }
    
    return color;
}


technique GaussianBlur
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
