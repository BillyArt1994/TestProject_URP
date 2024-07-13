#ifndef UNIVERSAL_FORWARD_SKIN_PASS_INCLUDED
#define UNIVERSAL_FORWARD_SKIN_PASS_INCLUDED

#include "BillySkinLighting.hlsl"
#include "BillySkinInput.hlsl"
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
    float4 shadowCoord   : TEXCOORD6;
    float4 positionCS    : SV_POSITION;
    float4 screenPos : TEXCOORD8;
};

Varyings PBRPassVertex (Attributes input)
{
    Varyings output = (Varyings)0;
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    output.positionCS = vertexInput.positionCS;
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS,input.tangentOS);
    output.normalWS.xyz = normalInput.normalWS;
    output.tangentWS.xyz = normalInput.tangentWS;
    output.BtangentWS.xyz = normalInput.bitangentWS;
    output.positionWS = vertexInput.positionWS;
    output.viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
    output.shadowCoord = GetShadowCoord(vertexInput);
    output.uv = input.texcoord;
    output.screenPos = ComputeScreenPos(vertexInput.positionCS);
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
    roughness = _Roughness;
    metalic = _Metalic;
    emissive = mraeMap.a;
    #else
    ao = _AO;
    roughness = _Roughness;
    metalic = _Metalic;
    emissive = 1.0;
    #endif

    float deltaWorldNormal = fwidth(input.normalWS);// length(abs(ddx(normalInput.normalWS))+abs(ddy(normalInput.normalWS)));
    float deltaWorldPos = fwidth(input.positionWS);// length(abs(ddx(vertexInput.positionWS))+abs(ddy(vertexInput.positionWS)));

    float curvature = 1.0;//(deltaWorldNormal/deltaWorldPos) * 0.01*_CurvatureFactore;//SAMPLE_TEXTURE2D(_Curvature,sampler_Curvature,input.uv).r;
    float thickness = 1.0 - SAMPLE_TEXTURE2D(_Thickness,sampler_Thickness,input.uv).r;

    BillyBRDFData brdfData = (BillyBRDFData)0;
    InitializeBRDFData(albedo,ao,roughness,metalic,reflectance,emissive,brdfData);

    float4 normalTS = SAMPLE_TEXTURE2D(_NormalTex,sampler_NormalTex,input.uv);
    normalTS.xyz = UnpackNormal(normalTS);
    float3 normal = TransformTangentToWorld(normalTS,half3x3(input.tangentWS.xyz,input.BtangentWS.xyz,input.normalWS.xyz));
    normal = normalize(normal.xyz);

    #if defined(_CUSTOM_THICKNESS)
    Light mainlight = GetMainLight(TransformWorldToShadowCoord(input.positionWS.xyz),input.normalWS.xyz);
    thickness = mainlight.thickness;
    #else
    Light mainlight = GetMainLight(TransformWorldToShadowCoord(input.positionWS.xyz));
    #endif

    float3 viewDir = normalize(input.viewDirWS);

    float skinToneLod = lerp(0,8,_SkinToneScale);
    float4 blurrnormalTS = SAMPLE_TEXTURE2D_LOD(_NormalTex,sampler_NormalTex,input.uv,skinToneLod);

    blurrnormalTS.xyz = UnpackNormal(blurrnormalTS);
    float3 blurrNormal = TransformTangentToWorld(blurrnormalTS,half3x3(input.tangentWS.xyz,input.BtangentWS.xyz,input.normalWS.xyz));
    blurrNormal = normalize(blurrNormal.xyz);

    float3 col = Billy_Skin_Lighting(brdfData,normal,viewDir,curvature,thickness,blurrNormal,mainlight);

    half3 additionalLightsSumResult = 0;

    #ifdef _ADDITIONAL_LIGHTS
    uint pixelLightCount = GetAdditionalLightsCount();  
    for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
    {
        Light light = GetAdditionalLight(lightIndex, input.positionWS,half4(1,1,1,1));
        additionalLightsSumResult += Billy_Skin_AddLighting( brdfData,normal,viewDir,curvature,thickness,blurrNormal,light);
    }
    #endif

    col.xyz += additionalLightsSumResult;
    #if _ALPHACLIP_ENABLED
        #ifdef _DITHER_ENABLED
        float2 screen_uv = input.screenPos.xy/input.screenPos.w;
        float2 pixelPos = screen_uv*_ScreenParams.xy;
        DitherAlpha(pixelPos,_Cutoff);
        #else
        clip(_Cutoff - albedo.a);
        #endif
    #endif

    #if defined(_ALBEDO_DISPLAYER)
    col = albedo;
    #endif
    #if defined(_METALLIC_DISPLAYER)
    col = metalic;
    #endif
    #if defined(_ROUNGHNESS_DISPLAYER)
    col = roughness;
    #endif
    #if defined(_AO_DISPLAYER)
    col = ao;
    #endif
    #if defined(_NORMAL_DISPLAYER)
    col = half4(normal,1.0);
    #endif

    return half4(col,1.0);
}
#endif