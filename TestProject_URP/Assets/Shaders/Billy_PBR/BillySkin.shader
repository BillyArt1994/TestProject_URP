Shader "Billy/Skin"
{
    Properties
    {
        _Tint("Tint",COLOR) = (0.5,0.5,0.5,1.0)
        _Albedo("Albedo",2D) = "white"{}
        [Normal]_NormalTex("Normal",2D) = "bump"{}
        [Toggle(_MRAE_MAP_ENABLED)] _MRAEMapEnabled("MRAE Map Enabled", Float) = 0
        _MRAEMap("MRAE Map", 2D) = "black" {}
        _Curvature("Curvature Map",2D) = "white" {}
        _Thickness("Thickness Map",2D) = "white" {}
        _Metalic("Metalic",Range(0,1)) = 0.0
        _Reflectivity("Reflectivity",Range(0,1)) = 0.5
        _Roughness("Roughness",Range(0,1))= 0.5
        _HeightMap("HeightMap",2D) = "black"{}
        _AO("AO",Range(0,1)) = 0.0
        [Toggle(_BENTNORMAL_ENABLED)] _BentnormalEnabled("Bent Normal Enabled",Float) = 0.0
        _Bentnormal("Bent Normal", 2D) = "bump" {}
        _Anisotropic("Anisotropic",Range(-1,1)) = 0.0
        [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _BeckManTex("_BECKMANNLUT",2D) = "white"{}
        _BRDFLUT("LUT",2D) = "white"{}
        _HorizonFade ("Horizon Fade",Range(0.0,1.0) ) =1.0
        _SkinBrdfLUT("Skin BrdfLUT",2D) = "white"{}
        _ShadowBrdfLUT("Shadow BrdfLUT",2D) = "white"{}
        _SkinToneScale("Skin Tone Scale",Range(0,1)) = 0.0
        _SkinSecondSpecScale("Skin Second Spec Scale",Range(0,1)) = 0.0
        _CurvatureScaleBias("_CurvatureScaleBias",float) = (1.0,0.0,0.0,0.0) 
        _ShadowScaleBias("_ShadowScaleBias",float) = (1.0,0.0,0.0,0.0) 


        [Toggle(_ALPHACLIP_ENABLED)] _AlphaClipEnabled("Dither Enabled",Float) = 0.0
        [Toggle(_DITHER_ENABLED)] _DitherEnabled("Dither Enabled",Float) = 0.0
        [Enum(Both, 0, Front, 1, Back, 2)]
        _CullMode ("Cull Mode", Float) = 2
        _Cutoff("Cut Off",Range(0.0,1.0)) = 0.0
        [KeywordEnum(UE4_BRDFApprox,Pre_Integration)] _IBLBrdfMode ("IBL Specular BRDF Mode",float) = 1.0
        [Toggle(DIRECTDIFFUSE)] _DirectDiffuse ("Direct Lighting Diffuse",float) = 1.0
        [Toggle(DIRECTSPECULAR)] _DirectSpecular ("Direct Lighting Specular",float) = 1.0
        [Toggle(INDIRECTDIFFUSE)] _IndirectDiffuse ("Indirect Lighting Diffuse",float) = 1.0
        [Toggle(INDIRECTSPECULAR)] _IndirectSpecular ("Indirect Lighting Specular",float) = 1.0
    }
    SubShader
    {
        Name "Opaque"
        Tags 
        {
            "RenderType"="Opaque"
            "LightMode" = "UniversalForward"
        }
        ZWrite On
        ZTest LEqual
        Cull [_CullMode]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex PBRPassVertex
            #pragma fragment PBRPassFragment
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma shader_feature DIRECTDIFFUSE   
            #pragma shader_feature DIRECTSPECULAR
            #pragma shader_feature INDIRECTDIFFUSE
            #pragma shader_feature INDIRECTSPECULAR
            #pragma multi_compile  _IBLBRDFMODE_UE4_BRDFAPPROX _IBLBRDFMODE_PRE_INTEGRATION
            #pragma shader_feature_local _ _MRAE_MAP_ENABLED
            #pragma shader_feature_local _ _DITHER_ENABLED
            #pragma shader_feature_local _ _ALPHACLIP_ENABLED
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _CUSTOM_SHADOW _CUSTOM_THICKNESS

            #define _CUSTOM_THICKNESS 1

            #include "BillySkinForwardPass.hlsl"
            #include "BillySkinInput.hlsl"

            ENDHLSL
        }


        Pass
        {
            Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull [_CullMode]

            HLSLPROGRAM
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma shader_feature_local _ALPHATEST_ON
            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment


            
            #include "BillyShadowCasterPass.hlsl"
            
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // Includes
            #include "BillyDepthOnlyPass.hlsl"
            //#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            //#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
        
    }
    //CustomEditor "CommonShaderGUI.BillySkinShaderGUI"
}
