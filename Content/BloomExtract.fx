//Effect file to extract only the brighter parts of the screen

sampler TextureSampler : register(s0);


float BloomThreshold;
float4 PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 colour = tex2D(TextureSampler, texCoord);
    return saturate((colour - BloomThreshold) / (1 - BloomThreshold));
}


technique BloomExtract
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShader();
    }
}
