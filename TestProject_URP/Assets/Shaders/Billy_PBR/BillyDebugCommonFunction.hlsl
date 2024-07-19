#ifndef BILLY_DEBUG_COMMONFUNCTION
#define BILLY_DEBUG_COMMONFUNCTION

float3 CheckColorValue(float3 color, float targetValue, float targetScale, float range)
{
    targetScale = 1.0;
    targetValue *= targetScale;
    float lum = dot(color, float3(0.2126729, 0.7151522, 0.072175));
    float3 outColor;
    outColor.g = saturate(max(range - abs(lum - targetValue), 0.0) * 10000) * 1.2; // just in range
    outColor.r = saturate(max(lum - targetValue + range, 0.0) * 10000) - outColor.g * 0.5; // over    range
    outColor.b = saturate(max(targetValue - lum + range, 0.0) * 10000) - outColor.g * 0.5; // under   range

    float rhythm = sin(lum / targetScale * 10.0 + _Time.w) * 0.35;
    outColor.g += 0.123;
    return outColor * (0.65 + rhythm);
}

#endif