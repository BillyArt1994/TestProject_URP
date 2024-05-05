#ifndef BIILY_PBR_INPUT_INCLUDED    
#define BIILY_PBR_INPUT_INCLUDED
    float4 _MainPos;
    float4 _FollowPos;
    float4 _W_Bottom;
    float4 _MeshH;

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
    //float4x4 _CustomShadowMatrix;
    ////sampler2D _CustomShadowTexture;
    //float4 _CustomShadowTexture_TexelSize;
    //float _CustomShadowFilterScale;
    //float _CustomShadowLightSize;
    float _ChkTargetValue;
    float _ChkTargetScale;
    float _ChkRange;

    //TEXTURE2D_SHADOW(_CustomShadowTexture);
    //SAMPLER_CMP(sampler_LinearClampCompare_CST);

    TEXTURE2D(_MRAEMap);       SAMPLER(sampler_MRAEMap);
    TEXTURE2D(_Bentnormal);       SAMPLER(sampler_Bentnormal);
    TEXTURE2D(_Albedo);       SAMPLER(sampler_Albedo);
    TEXTURE2D(_NormalTex);       SAMPLER(sampler_NormalTex);
    TEXTURE2D(_BRDFLUT);       SAMPLER(sampler_BRDFLUT);
#endif