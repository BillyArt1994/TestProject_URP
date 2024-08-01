#ifndef BILLY_SHADOWCASTER_PASS_INCLUDED
#define BILLY_SHADOWCASTER_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"
#include "BillyPBRInput.hlsl"

float3 _LightDirection;
float3 _LightPosition;

struct Attributes
{
    float4 posOS : POSITION;
    float3 normalOS : NORMAL;
    float2 uv : TEXCOORD0 ;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv : TEXCOORD0;
    float4 posCS : SV_POSITION;
};

float4 GetShadowPositionHClip(Attributes i)
{
    float3 positionWS = TransformObjectToWorld(i.posOS.xyz);
    float3 normalWS = TransformObjectToWorldNormal(i.normalOS);

    #if _CASTING_PUNCTUAL_LIGHT_SHADOW
        float3 lightDirectionWS = normalize(_LightPosition - positionWS);
    #else
        float3 lightDirectionWS = _LightDirection;
    #endif

    #if defined(_THICKFROMSHADOW_ENABLED)
        float4 positionCS = TransformWorldToHClip(positionWS);
    #else
        float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
    #endif
        
        
    #if UNITY_REVERSED_Z
        positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #else
        positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #endif

    return positionCS;
}

Varyings ShadowPassVertex(Attributes i)
{
    Varyings o;
    UNITY_SETUP_INSTANCE_ID(i)
    o.posCS = GetShadowPositionHClip(i);
    o.uv = i.uv;
    return o;
}

half4 ShadowPassFragment(Varyings i) : SV_Target
{
    half alpha = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, i.uv).a;
    #if defined(_ALPHATEST_ON)
        alpha *= _Tint.a;
        clip(alpha - _Cutoff);
    #endif
    #if defined(LOD_FADE_CROSSFADE)
        LODFadeCrossFade(i.posCS);
    #endif
    return 0;
}

#endif