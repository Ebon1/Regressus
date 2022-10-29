sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float4 MainPS(float2 uv : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, uv);
    float2 pos = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float2 dir = (uv - pos) * 0.1;
    for (int i = 1; i < 5; i++)
    {
        float alpha = 1 - i / 5;
        color += tex2D(uImage0, uv + dir * i * uProgress) * alpha * uProgress;
        color += tex2D(uImage0, uv - dir * i * uProgress) * alpha * uProgress;
    }
    return color;
}
Technique technique1
{
    pass Flash
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}