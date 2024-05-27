#ifndef UNIVERSAL_FORWARD_PBR_PASS_INCLUDED
#define UNIVERSAL_FORWARD_PBR_PASS_INCLUDED

#include "BillyPBRLighting.hlsl"
#include "BillyBRDF.hlsl"
#include "BillyPBRInput.hlsl"
#include "BillyCommonFunction.hlsl"

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
};
struct Varyings
{
    float2 uv      : TEXCOORD0;
    float3 positionWS    : TEXCOORD1;
    float3 normalWS      : TEXCOORD2;
    float4 tangentWS     : TEXCOORD3;
    float4 BtangentWS    : TEXCOORD4;
    float3 viewDirWS     : TEXCOORD5;
    float4 positionCS    : SV_POSITION;
    float4 screenPos     :TEXCOORD7;
};

Varyings PBRPassVertex (Attributes input)
{
    Varyings output = (Varyings)0;
    float3 mainPos =mul(unity_WorldToObject, _MainPos).xyz;
    float3 follow = mul(unity_WorldToObject, _FollowPos).xyz;
    float3 offDir = follow - mainPos;
    float3 followVert = input.positionOS.xyz + offDir;
    float3 wPos = mul(unity_ObjectToWorld, input.positionOS.xyz).xyz;
    float mask = (wPos.y - _W_Bottom) / max(0.00001, _MeshH);
    float3 pos = lerp(input.positionOS.xyz, followVert, mask);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(pos);
    output.positionCS = vertexInput.positionCS;
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS,input.tangentOS);
    output.normalWS.xyz = normalInput.normalWS;
    output.tangentWS.xyz = normalInput.tangentWS;
    output.BtangentWS.xyz = normalInput.bitangentWS;
    output.positionWS = vertexInput.positionWS;
    output.viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
    output.uv = input.texcoord;
    output.screenPos = ComputeScreenPos(output.positionCS);
    return output;
}

float4 PBRPassFragment (Varyings input) : SV_Target
{
    float4 albedo = SAMPLE_TEXTURE2D(_Albedo,sampler_Albedo,input.uv);
    albedo.xyz *= _Tint.xyz;
    float ao,roughness,metalic,reflectance,emissive;
    reflectance = _Reflectivity;
    #ifdef _MRAE_MAP_ENABLED
    float4 mraeMap = SAMPLE_TEXTURE2D(_MRAEMap,sampler_MRAEMap,input.uv);
    ao = mraeMap.b;
    roughness = mraeMap.g;
    metalic = mraeMap.r;
    emissive = mraeMap.a;
    #else
    ao = _AO;
    roughness = _Roughness;
    metalic = _Metalic;
    
    emissive = 1.0;
    #endif
    BillyBRDFData brdfData = (BillyBRDFData)0;
    InitializeBRDFData(albedo,ao,roughness,metalic,reflectance,emissive,brdfData);

    Light mainlight = GetMainLight(TransformWorldToShadowCoord(input.positionWS.xyz));

    float3 viewDir = normalize(input.viewDirWS);

    float4 normalTS = SAMPLE_TEXTURE2D(_NormalTex,sampler_NormalTex,input.uv);
    normalTS.xyz = UnpackNormal(normalTS);

    float3 normal = TransformTangentToWorld(normalTS,half3x3(input.tangentWS.xyz,input.BtangentWS.xyz,input.normalWS.xyz));
    normal = normalize(normal.xyz);
    float4 col = Billy_PBS_Lighting(brdfData,normal,viewDir,input.normalWS.xyz,mainlight);
    half3 additionalLightsSumResult = 0;

    #ifdef _ADDITIONAL_LIGHTS
    uint pixelLightCount = GetAdditionalLightsCount();  
    for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
    {
        Light light = GetAdditionalLight(lightIndex, input.positionWS,half4(1,1,1,1));
        additionalLightsSumResult += Billy_PBS_AddLighting( brdfData,normal,viewDir,input.normalWS.xyz,light);
    }
    #endif
    col.xyz += additionalLightsSumResult;


    #ifdef _CHECKVALUE
    col.xyz = CheckColorValue(col.xyz, _ChkTargetValue, _ChkTargetScale, _ChkRange);
    #endif

    #if _ALPHACLIP_ENABLED
        #ifdef _DITHER_ENABLED
        float2 screen_uv = input.screenPos.xy/input.screenPos.w;
        float2 pixelPos = screen_uv*_ScreenParams.xy;
        DitherAlpha(pixelPos,_Cutoff);
        #else
        clip(albedo.a -_Cutoff);
        #endif
    #endif
    return col;

}
#endif