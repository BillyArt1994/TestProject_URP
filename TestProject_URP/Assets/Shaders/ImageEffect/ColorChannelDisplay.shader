Shader "ImageEffect/ColorChannelDisplay"
{
    Properties
    {
        [KeywordEnum(RGBA,R,G,B,A,Gray)] _ChannelDisplay ("Chanel Display mode", Float) = 0
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _CHANNELDISPLAY_RGBA _CHANNELDISPLAY_R _CHANNELDISPLAY_G _CHANNELDISPLAY_B _CHANNELDISPLAY_A _CHANNELDISPLAY_GRAY

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                #ifdef _CHANNELDISPLAY_RGBA
                col = col;
                #elif _CHANNELDISPLAY_GRAY
                col = dot(col.xyz,float3(0.299,0.587,0.114));
                #elif _CHANNELDISPLAY_R
                col = float4(col.rrr,0.0);
                #elif _CHANNELDISPLAY_G
                col = float4(col.ggg,0.0);
                #elif _CHANNELDISPLAY_B
                col = float4(col.bbb,0.0);
                #elif _CHANNELDISPLAY_A
                col = float4(col.aaa,0.0);
                #endif
                return col;
            }
            ENDCG
        }
    }
}
