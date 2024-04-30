Shader "UnSkyCubemap(Removed Tint)" {
Properties {
	[Gamma] _Exposure ("Exposure", Range(0, 8)) = 0.0
	[Gamma] _ExposureExtentionValue ("Down Value", Range(-0.5 , 1)) = 1.0
	[Gamma] _Saturation("Saturation" ,Range(0.0,1.0)) = 1.0
	_Rotation ("Rotation", Range(0, 360)) = 0
	[NoScaleOffset] _Tex ("Cubemap   (HDR)", Cube) = "grey" {}
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off

	Pass {
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0

		#include "UnityCG.cginc"

		samplerCUBE _Tex;
		half4 _Tex_HDR;
		half _Exposure;
		float _Rotation;
		half _ExposureExtentionValue;
		half _Saturation;

		float3 RotateAroundYInDegrees (float3 vertex, float degrees)
		{
			float alpha = degrees * UNITY_PI / 180.0;
			float sina, cosa;
			sincos(alpha, sina, cosa);
			float2x2 m = float2x2(cosa, -sina, sina, cosa);
			return float3(mul(m, vertex.xz), vertex.y).xzy;
		}
		
		struct appdata_t {
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float3 texcoord : TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f vert (appdata_t v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			float3 rotated = RotateAroundYInDegrees(v.vertex, _Rotation);
			o.vertex = UnityObjectToClipPos(rotated);
			o.texcoord = v.vertex.xyz;
			return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
			half4 tex = texCUBE (_Tex, i.texcoord);
			half luminance = 0.2125 *tex.r+0.7154*tex.g+0.0721*tex.b;
			half3 c = DecodeHDR (tex, _Tex_HDR);
			c = lerp(luminance,c,_Saturation);
			c = c  * unity_ColorSpaceDouble.rgb;
			c *= (_Exposure + (0.23 * _ExposureExtentionValue));
			return half4(c, 1);
		}
		ENDCG 
	}
} 	


Fallback Off

}
