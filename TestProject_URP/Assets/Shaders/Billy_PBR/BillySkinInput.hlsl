#ifndef BIILY_SKIN_INPUT_INCLUDED    
#define BIILY_SKIN_INPUT_INCLUDED


    float4 _Tint;
    half _HorizonFade;
    half _Roughness;
    half _Metalic;    
    half _AO;
    float _Reflectivity;
    half _SkinToneScale;
    half _SkinSecondSpecScale;
    half _TranslucencyViewDependency;
    half _TranslucencyThreshold;
    half _TranslucencyScale;
    half4 _TranslucencyColor;
    half4 _fakeAmbientIntensity;
    half4 _fakeAmbientColor;
    half4 _CurvatureScaleBias;
    half4 _ShadowScaleBias;

    float4 _EmissionColor;
    float4 _Albedo_ST;
    float4 _NormalTex_ST;
    float4 _MRAEMap_ST;
    float4 _SkinBrdfLUT_ST;
    float4 _BRDFLUT_ST;
    float _Cutoff;
    float _DeepScatterFalloff;

    sampler2D _BeckManTex;

    TEXTURE2D(_Curvature);       SAMPLER(sampler_Curvature);
    TEXTURE2D(_Thickness);       SAMPLER(sampler_Thickness);
    TEXTURE2D(_MRAEMap);       SAMPLER(sampler_MRAEMap);
    TEXTURE2D(_Bentnormal);       SAMPLER(sampler_Bentnormal);
    TEXTURE2D(_Albedo);       SAMPLER(sampler_Albedo);
    TEXTURE2D(_NormalTex);       SAMPLER(sampler_NormalTex);
    TEXTURE2D(_BRDFLUT);       SAMPLER(sampler_BRDFLUT);
    TEXTURE2D(_SkinBrdfLUT);       SAMPLER(sampler_SkinBrdfLUT);
    TEXTURE2D(_ShadowBrdfLUT);       SAMPLER(sampler_ShadowBrdfLUT);
#endif