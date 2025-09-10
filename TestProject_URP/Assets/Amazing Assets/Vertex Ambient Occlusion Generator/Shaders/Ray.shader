Shader "Hidden/Amazing Assets/Vertex Ambient Occlusion Generator/Ray"
{
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite On

        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent+100"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            
            #include "UnityCG.cginc"


            sampler2D _CameraDepthTexture;


            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            fixed4 frag ( UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
            {
                float2 screenuv = vpos.xy / _ScreenParams.xy;
                float screenDepth = Linear01Depth(tex2D(_CameraDepthTexture, screenuv));
                float diff = screenDepth - Linear01Depth(vpos.z);
               
                float intersect = diff > 0 ? (1 - smoothstep(0, _ProjectionParams.w, diff)) : 0;

                intersect = intersect * intersect > 0.5 ? 1 : 0;

                
                return lerp(float4(0, 1, 0, .8), float4(1, 0, 0, .8), intersect);
            }
            ENDCG
        }
    }
}