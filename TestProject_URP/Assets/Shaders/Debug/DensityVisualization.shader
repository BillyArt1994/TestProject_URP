Shader "Billy/Debug/DensityVisualization"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CheckTex("Checker Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma multi_compile _ MIPMAP_MODE
             #pragma multi_compile _ OVERLAY_MODE
            #pragma vertex vert
            #pragma fragment frag



            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
               
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Albedo;
            sampler2D _CheckTex;
            float4 _MainTex_ST;
            float4 _Albedo_ST;
            float4 _MainTex_TexelSize;
            float4 _Albedo_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 texST = _Albedo_TexelSize.z > 0.01 ?  _Albedo_ST : _MainTex_ST;
                float2 texSize = _Albedo_TexelSize.z > 0.01 ?  _Albedo_TexelSize.zw : _MainTex_TexelSize.zw;
                o.uv = v.uv;//* texST.xy + texST.zw;           
         
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
            fixed4 colChecker = tex2D(_CheckTex, i.uv);
               
                //return colChecker;

#ifdef OVERLAY_MODE
            fixed4 colOri = _Albedo_TexelSize.z > 0.01 ? tex2D(_Albedo, i.uv) : tex2D(_MainTex, i.uv);
            //return colOri * colChecker * 2.0;
            return lerp(colOri, colChecker, 0.5);
#else 
            return colChecker;
#endif
                
              
               
            }
            ENDCG
        }
    }
}
