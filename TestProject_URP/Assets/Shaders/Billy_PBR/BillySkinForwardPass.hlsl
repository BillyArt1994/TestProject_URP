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
    float4 shadowPos : TEXCOORD7;
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

float PercentageCloserFiltering(float3 shadowPos,float bias ,float filterradius)
{
    float PCFshadow = 0 ;

    for (int y = -1 ;y<=1 ;y++){
        for (int x =-1;x<=1 ;x++){
            float2 Offset = float2(x,y)*_CustomShadowTexture_TexelSize.xy*filterradius;
            PCFshadow += SAMPLE_TEXTURE2D_SHADOW(_CustomShadowTexture, sampler_LinearClampCompare_CST,shadowPos-float3(Offset,bias));
        }
    }
    //PCFshadow.x = SAMPLE_TEXTURE2D_SHADOW(_CustomShadowTexture, sampler_LinearClampCompare_CST,shadowPos-float3(filterradius*float2(offsetX,offsetY),bias));
    //PCFshadow.y = SAMPLE_TEXTURE2D_SHADOW(_CustomShadowTexture, sampler_LinearClampCompare_CST,shadowPos+float3(filterradius*float2(offsetX,offsetY),-bias));
    //PCFshadow.z = SAMPLE_TEXTURE2D_SHADOW(_CustomShadowTexture, sampler_LinearClampCompare_CST,shadowPos-float3(filterradius*float2(-offsetX,offsetY),bias));
    //PCFshadow.w = SAMPLE_TEXTURE2D_SHADOW(_CustomShadowTexture, sampler_LinearClampCompare_CST,shadowPos+float3(filterradius*float2(offsetX,-offsetY),-bias));
    return PCFshadow/9.0;
}
static float2 poissonDisk[32] = 
{
	float2(-0.975402, -0.0711386 ),
	float2(-0.920347, -0.41142 ),
	float2(-0.883908, 0.217872 ),
	float2(-0.884518, 0.568041 ),
	float2(-0.811945, 0.90521 ),
	float2(-0.792474, -0.779962),
	float2(-0.614856, 0.386578 ),
	float2(-0.580859, -0.208777),
	float2(-0.53795, 0.716666 ),
	float2(-0.515427, 0.0899991),
	float2(-0.454634, -0.707938),
	float2(-0.420942, 0.991272 ),
	float2(-0.261147, 0.588488 ),
	float2(-0.211219, 0.114841 ),
	float2(-0.146336, -0.259194),
	float2(-0.139439, -0.888668),
	float2(0.0116886, 0.326395 ),
	float2(0.0380566, 0.625477 ),
	float2(0.0625935, -0.50853 ),
	float2(0.125584, 0.0469069 ),
	float2(0.169469, -0.997253 ),
	float2(0.320597, 0.291055 ),
	float2(0.359172, -0.633717 ),
	float2(0.435713, -0.250832 ),
	float2(0.507797, -0.916562 ),
	float2(0.545763, 0.730216 ),
	float2(0.56859, 0.11655 ),
	float2(0.743156, -0.505173 ),
	float2(0.736442, -0.189734 ),
	float2(0.843562, 0.357036 ),
	float2(0.865413, 0.763726 ),
	float2(0.872005, -0.927 ),
};
    
float findBlocker(float3 shadowPos) 
{
    float dBlocker = 0 ;
    float2 wBlockerSearch = _CustomShadowLightSize * _CustomShadowTexture_TexelSize.xy*0.5;
    float sum = 0;
    float flag = 0;
    for(int i = 0;i<9;++i){
      float depthInShadowmap =  SAMPLE_TEXTURE2D(_CustomShadowTexture,sampler_MRAEMap, shadowPos.xy + wBlockerSearch*poissonDisk[i]).r;
      if(depthInShadowmap < shadowPos.z-0.002){
        dBlocker += depthInShadowmap;
        sum += 1.0;
        flag = 1;
      }
    }
    return flag == 1 ?  dBlocker/float(sum):1;
  }
float EstimateThicknessFromParallelShadowPoisson32(float3 shadowPos)
{
    float sum =0;
    
    for (int i = 0 ;i<32; ++i)
    {
        float2 Offset = poissonDisk[i]*_CustomShadowTexture_TexelSize.xy*_CustomShadowFilterScale;
        float zShadowMap = SAMPLE_TEXTURE2D(_CustomShadowTexture,sampler_MRAEMap, Offset+shadowPos.xy).r;
        sum += max(0, shadowPos.z -zShadowMap);
    }
    return sum*(1.0/32.0);
}


float PercentageCloserSoftShadows(float3 shadowPos,float bias)
{
    float blockerDis = findBlocker(shadowPos);
    float penumbraWidth = (shadowPos.z-blockerDis)/blockerDis * _CustomShadowLightSize;
    float shadow = PercentageCloserFiltering(shadowPos,bias,penumbraWidth);
    return shadow;
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
     
    float curvature = SAMPLE_TEXTURE2D(_Curvature,sampler_Curvature,input.uv).r;
    float thickness = 1.0 - SAMPLE_TEXTURE2D(_Thickness,sampler_Thickness,input.uv).r;

    BillyBRDFData brdfData = (BillyBRDFData)0;
    InitializeBRDFData(albedo,ao,roughness,metalic,reflectance,emissive,brdfData);

    Light mainlight = GetMainLight(TransformWorldToShadowCoord(input.positionWS.xyz) );



   
    float3 viewDir = normalize(input.viewDirWS);

    float skinToneLod = lerp(0,8,_SkinToneScale);
    float4 blurrnormalTS = SAMPLE_TEXTURE2D_LOD(_NormalTex,sampler_NormalTex,input.uv,skinToneLod);

    float4 normalTS = SAMPLE_TEXTURE2D(_NormalTex,sampler_NormalTex,input.uv);
    normalTS.xyz = UnpackNormal(normalTS);
    float3 normal = TransformTangentToWorld(normalTS,half3x3(input.tangentWS.xyz,input.BtangentWS.xyz,input.normalWS.xyz));
    normal = normalize(normal.xyz);

    #if _CSTUOM_SHADOW
    float4 lightClipPos =  mul(_CustomShadowMatrix,half4(input.positionWS,1.0));
    float3 shadowPos = ((lightClipPos.xyz/lightClipPos.w)*0.5+0.5);
    float depth_ShadowMap = SAMPLE_TEXTURE2D(_CustomShadowTexture,sampler_MRAEMap, shadowPos.xy).r;
    float bias = max(_CustomShadowBias*(1.0-dot(normal,mainlight.direction)),_CustomShadowBias);
    //float shadow = PercentageCloserFiltering(shadowPos,bias,_CustomShadowFilterScale);//shadowPos.z -bias >depth_ShadowMap ? 0:1;
    float shadow = PercentageCloserSoftShadows(shadowPos,bias);//shadowPos.z -bias >depth_ShadowMap ? 0:1;
    mainlight.shadowAttenuation = shadow;
    thickness = EstimateThicknessFromParallelShadowPoisson32(shadowPos);
    #endif 
    //return float4(mainlight.shadowAttenuation.xxx,1.0);
    
    blurrnormalTS.xyz = UnpackNormal(blurrnormalTS);
    float3 blurrNormal = TransformTangentToWorld(blurrnormalTS,half3x3(input.tangentWS.xyz,input.BtangentWS.xyz,input.normalWS.xyz));
    blurrNormal = normalize(blurrNormal.xyz);

    //return blurrNormal.xyzz;

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
    return half4(col,1.0);
}
#endif