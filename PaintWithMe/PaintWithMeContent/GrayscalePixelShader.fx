float4x4 World;
float4x4 View;
float4x4 Projection;

sampler ColorMapSampler : register(s0);

float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR
{
	float4 color = tex2D(ColorMapSampler, Tex); 
    float3 colrgb = color.rgb;
    float greycolor = dot(colrgb, float3(0.3, 0.59, 0.11));

	colrgb.rgb = dot(greycolor, float3(0.3, 0.59, 0.11));

    return float4(colrgb.rgb, color.a);
}

technique GrayscaleProcess
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
