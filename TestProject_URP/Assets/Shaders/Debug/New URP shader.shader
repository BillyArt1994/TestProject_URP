Shader "Custom/CustomURPShader"
{
    Properties
    {
        _Tint("Tint",COLOR) = (0.5,0.5,0.5,1.0)
        _Albedo("Albedo",2D) = "white"{}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Cull off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vertex
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma shader_feature DIRECTDIFFUSE   
            #pragma shader_feature DIRECTSPECULAR
            #pragma shader_feature INDIRECTDIFFUSE
            #pragma shader_feature INDIRECTSPECULAR
            #pragma multi_compile  _IBLBRDFMODE_UE4_BRDFAPPROX _IBLBRDFMODE_PRE_INTEGRATION
            #pragma shader_feature_local _LIGHTMODE_STANDARD _LIGHTMODE_SKIN
            #pragma shader_feature_local _ _MRAE_MAP_ENABLED
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float4 _Tint;
            TEXTURE2D(_Albedo);       SAMPLER(sampler_Albedo);


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

            };

            Varyings vertex (Attributes input)
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
                return output;
            }

            float4 frag (Varyings input) : SV_Target
            {
                float4 albedo = SAMPLE_TEXTURE2D(_Albedo,sampler_Albedo,input.uv);
                albedo.xyz *= _Tint.xyz;
                clip(albedo.a-0.2);
                return albedo.bbbb;
            }

            ENDHLSL
        }
    }
}
