#ifndef BILLY_HAIR_FORWARD_PASS_INCLUDED
#define BILLY_HAIR_FORWARD_PASS_INCLUDED

#include "Math.hlsl"
#include "BillyHairBRDF.hlsl"
#include "BillyHairLighting.hlsl"
#include "BillyHairInput.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float2 texcoord1     : TEXCOORD1;
    float4 color        : COLOR;
};

struct Varyings
{
    float2 uv      : TEXCOORD0;
    float2 uv1      : TEXCOORD7;
    float3 positionWS    : TEXCOORD1;
    float3 normalWS      : TEXCOORD2;
    float4 tangentWS     : TEXCOORD3;
    float4 BtangentWS    : TEXCOORD4;
    float3 viewDirWS     : TEXCOORD5;
    float4 shadowCoord   : TEXCOORD6;
    float4 tangentOS   : TEXCOORD8;
    float3 bitangentOS   : TEXCOORD9;
    float4 positionCS    : SV_POSITION;
    float4 color         : TEXCOORD10;
};

Varyings vertex (Attributes input)
{
    Varyings output = (Varyings)0;
    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    output.positionCS = vertexInput.positionCS;
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS,input.tangentOS);
    output.normalWS.xyz = normalInput.normalWS;
    output.tangentWS.xyz = normalInput.tangentWS;
    output.tangentOS = input.tangentOS;
    real sign = real(input.tangentOS.w) * GetOddNegativeScale();
    output.bitangentOS = real3(cross(input.normalOS.xyz, float3(input.tangentOS.xyz))) * sign;
    output.BtangentWS.xyz = normalInput.bitangentWS;
    output.positionWS = vertexInput.positionWS;
    output.viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
    output.shadowCoord = GetShadowCoord(vertexInput);
    output.uv = input.texcoord;
    output.uv1 = input.texcoord1;
    output.color = input.color;
    return output;
}

float4 frag (Varyings input , half face : VFACE) : SV_Target
{
    Light mainlight = GetMainLight(input.shadowCoord);
    float4 albedo = tex2Dlod(_Albedo,float4(input.uv,0.0,1));
    albedo.xyz *= _TipColor.xyz;
    float4 uv;
    uv.x = input.uv.x *_SpecularNoiseTiling;
    uv.yw = input.uv.yy;
    uv.z = input.uv.x *_NormalDistortTiling;
    float4 flowMap0 = tex2D(_TangentTex,input.uv);
    float4 flowMap1 = tex2D(_TangentTex,uv.xy);
    float4 flowMap2 = tex2D(_TangentTex,uv.zw);
    float ao = input.color.g;
    float EdgeSmooth = _Edge <= _EdgeSmooth ? _Edge - 0.01 : _EdgeSmooth;
    float rootFlow =SmoothStep(_Edge,EdgeSmooth , saturate(lerp(flowMap0.b,min(flowMap0.g,flowMap0.b),_TipVariation)) );
    albedo.xyz = lerp(_RootColor,albedo.xyz,1-rootFlow);

    //return float4(albedo);

    float t1 = 0;
    float t2 = flowMap2.z *2.0 -1.0;
    half faceFlag = face == 0 ? -1: 1;
    float3 T = float3(0.0,1,0.0);
     T.xy = lerp(T.xy,float2(0.5,flowMap0.y),flowMap0.a);
    T.z = 0;

    float3 V = float3(dot(input.viewDirWS,input.tangentWS.xyz),dot(input.viewDirWS,input.BtangentWS.xyz),dot(input.viewDirWS,input.normalWS.xyz));
    V = normalize(V);
    float3 N = T;
    float3 L = float3(dot(mainlight.direction,input.tangentWS.xyz),dot(mainlight.direction,input.BtangentWS.xyz),dot(mainlight.direction,input.normalWS.xyz));
    L = normalize(L.xyz);
    float3 SpecularColor = albedo;
    float Specular = _Specular;
    float roughness = max(_Roughness,0.04);
    float Backlit = _Backlit;
    float Area = _Area;

    float Scatter =  _Scatter;
    float3 col = Billy_Hair_Lighting(albedo,V,N,L,t1,Specular,roughness,Backlit,Area,Scatter,ao,mainlight);
    
    half3 additionalLightsSumResult = 0;

    #ifdef _ADDITIONAL_LIGHTS
    uint pixelLightCount = GetAdditionalLightsCount();  
    for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
    {
        Light light = GetAdditionalLight(lightIndex, input.positionWS,half4(1,1,1,1));
        L = float3(dot(light.direction,input.tangentWS.xyz),dot(light.direction,input.BtangentWS.xyz),dot(light.direction,input.normalWS.xyz));
        L = normalize(L.xyz);
        additionalLightsSumResult += Billy_Hair_AddLighting(albedo,V,N,L,t1,Specular,roughness,Backlit,Area,Scatter,ao,light);
    }
    #endif
    col.xyz += additionalLightsSumResult;

    float4 rootAO = flowMap0.bbbb;//tex2D(_AO_Root,input.uv);
    float r = _CutoffTips - _Cutoff;
    r = rootAO.y * r + _Cutoff;
    float ar = flowMap0.a - r;
   // flowMap0.a =  flowMap0.a/(r+0.001);
    //flowMap0.a = saturate(flowMap0.a);
    //return float4(col,albedo.a);

    #if defined(DEPTH_PASS)
    clip(flowMap0.a -_Cutoff );
    #endif
    
    #if defined(_ALBEDO_DISPLAYER)
    col.xyz = albedo;
    #endif
    #if defined(_METALLIC_DISPLAYER)
    col.xyz = 0.0;
    #endif
    #if defined(_ROUNGHNESS_DISPLAYER)
    col.xyz = roughness;
    #endif
    #if defined(_AO_DISPLAYER)
    col.xyz = ao;
    #endif
    #if defined(_NORMAL_DISPLAYER)
    col.xyz = N;
    #endif


    return float4(col.xyz,saturate(flowMap0.a* _CutoffTips) );
}

#endif