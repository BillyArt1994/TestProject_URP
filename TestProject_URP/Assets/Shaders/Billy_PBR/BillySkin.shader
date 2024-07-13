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
        _DeepScale("DeepScale",float) = 1.0
        _DeepScatterFalloff("_DeepScatterFalloff",float) = 1.0
        _TranslucencyColor("_TranslucencyColor",COLOR) = (0.5,0.5,0.5,1.0)

        _CurvatureFactore("_CurvatureFactore",float) = 1.0

        [Toggle(_ALPHACLIP_ENABLED)] _AlphaClipEnabled("AlphaClip Enabled",Float) = 0.0
        [Toggle(_DITHER_ENABLED)] _DitherEnabled("Dither Enabled",Float) = 0.0
        [Enum(Both, 0, Front, 1, Back, 2)]
        _CullMode ("Cull Mode", Float) = 2
        _Cutoff("Cut Off",Range(0.0,1.0)) = 0.0
        [KeywordEnum(UE4_BRDFApprox,Pre_Integration)] _IBLBrdfMode ("IBL Specular BRDF Mode",float) = 1.0
		[Toggle(_CHECKVALUE)]_CheckValue("> Measure The Output Value", Float) = 0
        _ChkTargetValue(" ORANGE-GREEN-BLUE", Range(-0.1, 5.0)) = 0.1842
        [Enum(x0.01,0.01, x0.1,0.1, x1,1.0, x10,10.0, x100,100.0, x1000,1000.0, x10000,10000.0)]_ChkTargetScale("    (Higher - Hit - Lower)", Range( 0.001, 1000.0)) = 1.0
        [PowerSlider(2.0)]_ChkRange(" Tolerance", Range(0.0032, 10.0)) = 0.045
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
            #pragma target 3.0
            #pragma vertex PBRPassVertex
            #pragma fragment PBRPassFragment
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma shader_feature _ _CHECKVALUE
            #pragma shader_feature _ _DIRECTDIFFUSE_DISPLAYER   
            #pragma shader_feature _ _DIRECTSPECULAR_DISPLAYER
            #pragma shader_feature _ _INDIRECTDIFFUSE_DISPLAYER
            #pragma shader_feature _ _INDIRECTSPECULAR_DISPLAYER
            #pragma shader_feature _ _ALBEDO_DISPLAYER
            #pragma shader_feature _ _METALLIC_DISPLAYER
            #pragma shader_feature _ _ROUNGHNESS_DISPLAYER
            #pragma shader_feature _ _AO_DISPLAYER
            #pragma shader_feature _ _NORMAL_DISPLAYER
            #pragma shader_feature _ _SHADOW_DISPLAYER
            #pragma multi_compile  _IBLBRDFMODE_UE4_BRDFAPPROX _IBLBRDFMODE_PRE_INTEGRATION
            #pragma shader_feature_local _ _MRAE_MAP_ENABLED
            #pragma shader_feature_local _ _DITHER_ENABLED
            #pragma shader_feature_local _ _ALPHACLIP_ENABLED
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _CUSTOM_THICKNESS
            #pragma multi_compile_local _ _CUSTOM_SHADOW

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
            #pragma multi_compile_local _ _CUSTOM_SHADOW

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
            ENDHLSL
        }
        
    }
    //CustomEditor "CommonShaderGUI.BillySkinShaderGUI"
}
