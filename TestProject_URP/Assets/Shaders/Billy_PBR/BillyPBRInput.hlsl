#ifndef BIILY_PBR_INPUT_INCLUDED    
#define BIILY_PBR_INPUT_INCLUDED

    //float4 _MainPos;
    //float4 _FollowPos;
    //float4 _W_Bottom;
    //float4 _MeshH;

    float4 _Tint;
    float _HorizonFade;
    float _Roughness;
    float _Metalic;    
    float _AO;
    float _Reflectivity;
    float _Cutoff;

    float4 _EmissionColor;
    float4 _Albedo_ST;
    float4 _NormalTex_ST;
    float4 _MRAEMap_ST;
    float4 _BRDFLUT_ST;
#ifdef _CHECKVALUE
    float _ChkTargetValue;
    float _ChkTargetScale;
    float _ChkRange;
#endif

#ifdef _MIPMAP_DISPLAYER
    sampler2D _CheckTex;
    float4 _Albedo_TexelSize;
#endif

    TEXTURE2D(_MRAEMap);       SAMPLER(sampler_MRAEMap);
    TEXTURE2D(_Bentnormal);       SAMPLER(sampler_Bentnormal);
    TEXTURE2D(_Albedo);       SAMPLER(sampler_Albedo);
    TEXTURE2D(_NormalTex);       SAMPLER(sampler_NormalTex);
    TEXTURE2D(_BRDFLUT);       SAMPLER(sampler_BRDFLUT);
#endif