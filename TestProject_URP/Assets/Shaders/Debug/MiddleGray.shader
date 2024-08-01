Shader "Billy/Debug/MiddleGray"
{
    Properties
    {
        _Tint("Tint",COLOR) = (0.4627,0.4627,0.4627,1.0)
        _Albedo("Albedo",2D) = "white"{}
        [HideInInspector][Toggle(_NORMAL_MAP_ENABLED)] _NORMALMapEnabled("Normal Map Enabled", Float) = 0
        [HideInInspector][Normal]_NormalTex("Normal",2D) = "bump"{}
        [HideInInspector][Toggle(_MRAE_MAP_ENABLED)] _MRAEMapEnabled("MRAE Map Enabled", Float) = 0
        [HideInInspector]_MRAEMap("MRAE Map", 2D) = "black" {}
        _Metalic("Metalic",Range(0,1)) = 0.0
        _Reflectivity("Reflectivity",Range(0,1)) = 0.5
        _Roughness("Roughness",Range(0,1))= 1.0
        [HideInInspector]_HeightMap("HeightMap",2D) = "black"{}
        [HideInInspector]_AO("AO",Range(0,1)) = 1.0
        [HideInInspector][Toggle(_BENTNORMAL_ENABLED)] _BentnormalEnabled("Bent Normal Enabled",Float) = 0.0
        [HideInInspector]_Bentnormal("Bent Normal", 2D) = "bump" {}
        [HideInInspector]_Anisotropic("Anisotropic",Range(-1,1)) = 0.0
        [HideInInspector][HDR] _EmissionColor("Color", Color) = (0,0,0)

        [HideInInspector]_HorizonFade ("Horizon Fade",Range(0.0,1.5) ) = 0.2
        [HideInInspector]_BRDFLUT("LUT",2D) = "white"{}

        [HideInInspector][Toggle(_ALPHACLIP_ENABLED)] _AlphaClipEnabled("AlphaClip Enabled",Float) = 0.0
        [HideInInspector][Toggle(_DITHER_ENABLED)] _DitherEnabled("Dither Enabled",Float) = 0.0
        [HideInInspector][Enum(Both, 0, Front, 1, Back, 2)]
        [HideInInspector]_CullMode ("Cull Mode", Float) = 2
        [HideInInspector]_Cutoff("Cut Off",Range(0.0,1.0)) = 0.0
        
        [KeywordEnum(UE4_BRDFApprox,Pre_Integration)] _IBLBrdfMode ("IBL Specular BRDF Mode",float) = 0.0

		[HideInInspector][Toggle(_CHECKVALUE)]_CheckValue("> Measure The Output Value", Float) = 1
        [HideInInspector]_ChkTargetValue(" ORANGE-GREEN-BLUE", Range(-0.1, 5.0)) = 0.18
        [HideInInspector][Enum(x0.01,0.01, x0.1,0.1, x1,1.0, x10,10.0, x100,100.0, x1000,1000.0, x10000,10000.0)]_ChkTargetScale("    (Higher - Hit - Lower)", Range( 0.001, 1000.0)) = 1.0
        [HideInInspector][PowerSlider(2.0)]_ChkRange(" Tolerance", Range(0.0032, 10.0)) = 0.02
    }
    SubShader
    {
        Name "Opaque"
        Tags 
        {
         "RenderType"="Opaque"
       //  "LightMode" = "UniversalForward"
        }
        ZWrite On
        ZTest LEqual
        Cull off



        Pass
        {
            HLSLPROGRAM
            #pragma vertex PBRPassVertex
            #pragma fragment PBRPassFragment
            //#define _CHECKVALUE 0 

            #include "../Billy_PBR/BillyPBRLighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 texcoord     : TEXCOORD0;
                float4 color        : COLOR;
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
                float4 color         :TEXCOORD8;
            };
    
            Varyings PBRPassVertex (Attributes input)
            {
                Varyings output = (Varyings)0;
                //float3 mainPos =mul(unity_WorldToObject, _MainPos).xyz;
                //float3 follow = mul(unity_WorldToObject, _FollowPos).xyz;
                //float3 offDir = follow - mainPos;
                //float3 followVert = input.positionOS.xyz + offDir;
                //float3 wPos = mul(unity_ObjectToWorld, input.positionOS.xyz).xyz;
                //float mask = (wPos.y - _W_Bottom) / max(0.00001, _MeshH);
                //float3 pos = lerp(input.positionOS.xyz, followVert, mask);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS,input.tangentOS);
                output.normalWS.xyz = normalInput.normalWS;
                output.tangentWS.xyz = normalInput.tangentWS;
                output.BtangentWS.xyz = normalInput.bitangentWS;
                output.positionWS = vertexInput.positionWS;
                output.viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
                output.uv = input.texcoord;
                output.screenPos = ComputeScreenPos(output.positionCS);
                output.color = input.color;
                #ifdef _MIPMAP_DISPLAYER
                // 128 is check tex size,divide 128 is to macth checkTex size to AlbedoTex size and to get unity texture density
                // *8 is to push the mipmap 3 level down
                output.uv_Check = (input.texcoord*_Albedo_ST.xy+_Albedo_ST.zw)*(_Albedo_TexelSize.zw/128*8);
                #endif
                return output;
            }

            float4 PBRPassFragment (Varyings input) : SV_Target
            {
                float4 albedo = float4(0.18,0.18,0.18,1.0);//SAMPLE_TEXTURE2D(_Albedo,sampler_Albedo,input.uv*_Albedo_ST.xy+_Albedo_ST.zw);
                //albedo.xyz *= _Tint.xyz;
                
                float ao,roughness,metalic,reflectance,emissive;
                reflectance = 0.5;//_Reflectivity;
                ao = 1.0;
                roughness = 1.0;//_Roughness;
                metalic = 0.0;//_Metalic;
                emissive = 0.0;//1.0;
                BillyBRDFData brdfData = (BillyBRDFData)0;
                InitializeBRDFData(albedo,ao,roughness,metalic,reflectance,emissive,brdfData);
                Light mainlight = GetMainLight(TransformWorldToShadowCoord(input.positionWS.xyz));
                float3 viewDir = normalize(input.viewDirWS);

                float3 normal; 
                normal = normalize(input.normalWS.xyz);

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

                return col;

            }
            ENDHLSL
        }
/*
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull [_CullMode]

            HLSLPROGRAM
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma shader_feature_local _ALPHATEST_ON
            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #pragma multi_compile_local _ _CUSTOM_SHADOW
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "../Billy_PBR/BillyShadowCasterPass.hlsl"
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
            ZTest LEqual
            Cull [_CullMode]

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // Includes
            #include "../Billy_PBR/BillyDepthOnlyPass.hlsl"
            ENDHLSL
        }
 */
        
    }


   
    //CustomEditor "CommonShaderGUI.BillyPBRShaderGUI"
}
