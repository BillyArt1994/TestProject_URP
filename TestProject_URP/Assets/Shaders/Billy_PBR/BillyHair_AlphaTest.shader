Shader "Billy/Marschner Hair_AlphaTest"
{
    Properties
    {
        
        _Albedo("Albedo",2D) = "white"{}
        _TangentTex("Tangent Tex",2D) = "bump"{}
        _AO_Root("AO Root",2D) ="white"{}
        
        _Edge("Edge",Range(0,1))= 0.5
        _EdgeSmooth("Edge Smooth",Range(0,1))=0.55

        _RootColor("Root Color",COLOR) = (0.5,0.5,0.5,1.0)
        _TipColor("Tip Color",COLOR) = (0.5,0.5,0.5,1.0)
        _TipVariation("Tip Variation",Range(0.0,1.0)) = 0.0

        _SpecCol ("Spec Col",COLOR) = (0.5,0.5,0.5,1.0)
        _SecSpecCol ("Sec Spec Col",COLOR) = (0.5,0.5,0.5,1.0)

        _Wrap("Wrap",Range(0.0,1.0)) = 0.4
        _Roughness("Roughness",Range(0.0,1.0)) = 0.5
        _Specular("Specular",Range(0.0,10.0)) = 0.5
        _Backlit("Back Scatter Intenstity",Range(0.0,1.0)) = 1.0
        _Area("Area",float) = 0.0
        _Scatter("Transmission",Range(0.0,1.0)) = 1.0
        
        _SpecularNoiseTiling("Anisotropic",Range(0.0,10.0)) = 1.0
        _SpecularNoiseIntensity("Anisotropic Intensity",Range(0.0,1.0)) = 1.0

        _CutoffTips("Cut off Tips",Range(0,1)) =  0.1
        _Cutoff("Cut Off",Range(0,1)) = 0.5
        _Debug("Alpha Debug",Range(0,1)) = 0.1
        
        [Toggle(R_ON)] _R_ON ("R",float) = 1.0
        [Toggle(TRT_ON)] _TRT_ON ("TRT",float) = 1.0
        [Toggle(TT_ON)] _TT_ON ("TT",float) = 1.0
        [Toggle(SCATTER_ON)] _SCATTER_ON ("Scatter",float) = 1.0
        [Toggle(INDIRECTDIFFUSE_ON)] _INDIRECTDIFFUSE_ON ("Indirect Lighting Diffuse",float) = 1.0
        [Toggle(INDIRECTSPECULAR_ON)] _INDIRECTSPECULAR_ON ("Indirect Lighting Specular",float) = 1.0
    }
    SubShader
    {

       
        Pass
        {
            Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest"}
            ZWrite On
            Cull off

            HLSLPROGRAM
            #pragma vertex vertex
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma shader_feature R_ON   
            #pragma shader_feature TRT_ON
            #pragma shader_feature TT_ON
            #pragma shader_feature SCATTER_ON
            #pragma shader_feature INDIRECTDIFFUSE_ON
            #pragma shader_feature INDIRECTSPECULAR_ON
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma shader_feature _DIRECTDIFFUSE_DISPLAYER   
            #pragma shader_feature _DIRECTSPECULAR_DISPLAYER
            #pragma shader_feature _INDIRECTDIFFUSE_DISPLAYER
            #pragma shader_feature _INDIRECTSPECULAR_DISPLAYER
            #pragma shader_feature _ALBEDO_DISPLAYER
            #pragma shader_feature _METALLIC_DISPLAYER
            #pragma shader_feature _ROUNGHNESS_DISPLAYER
            #pragma shader_feature _AO_DISPLAYER
            #pragma shader_feature _NORMAL_DISPLAYER
            #pragma shader_feature _SHADOW_DISPLAYER

            #define DEPTH_PASS

            #include "Math.hlsl"
            #include "BillyHairBRDF.hlsl"
            #include "BillyHairLighting.hlsl"
            #include "BillyHairForwardPass_AlphaTest.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            ENDHLSL
        }
        
/*      
        Pass
        {
            Tags { "RenderType"="Transparent" "Queue" = "Transparent" "LightMode" = "HairPass"}
            Cull off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vertex
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma shader_feature R_ON   
            #pragma shader_feature TRT_ON
            #pragma shader_feature TT_ON
            #pragma shader_feature SCATTER_ON
            #pragma shader_feature INDIRECTDIFFUSE_ON
            #pragma shader_feature INDIRECTSPECULAR_ON
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma shader_feature _DIRECTDIFFUSE_DISPLAYER   
            #pragma shader_feature _DIRECTSPECULAR_DISPLAYER
            #pragma shader_feature _INDIRECTDIFFUSE_DISPLAYER
            #pragma shader_feature _INDIRECTSPECULAR_DISPLAYER
            #pragma shader_feature _ALBEDO_DISPLAYER
            #pragma shader_feature _METALLIC_DISPLAYER
            #pragma shader_feature _ROUNGHNESS_DISPLAYER
            #pragma shader_feature _AO_DISPLAYER
            #pragma shader_feature _NORMAL_DISPLAYER
            #pragma shader_feature _SHADOW_DISPLAYER

            #include "Math.hlsl"
            #include "BillyHairBRDF.hlsl"
            #include "BillyHairLighting.hlsl"
            #include "BillyHairForwardPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            ENDHLSL
        }
*/
        Pass
        {
           Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #define _ALPHATEST_ON 1

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Universal Pipeline keywords

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            
            #include "BillyHairShadowCasterPass.hlsl"
            ENDHLSL
        } 
    }
}
