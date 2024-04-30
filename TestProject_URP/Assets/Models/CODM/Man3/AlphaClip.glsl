#version 300 es

precision highp float;
precision highp int;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 unity_OrthoParams;
uniform 	mediump vec4 _WorldSpaceLightPos0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	vec4 unity_LightPosition0;
uniform 	vec4 unity_LightPosition1;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 hlslcc_mtx4x4unity_WorldToShadow[16];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	vec4 hlslcc_mtx4x4unity_MatrixV[4];
uniform 	mediump vec4 unity_FogColor;
uniform 	mediump float _OnePassToggle;
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec4 _LightColor0;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	mediump float _CutoffTips;
uniform 	mediump float _SpecularNoiseTiling;
uniform 	mediump float _SpecularNoiseIntensity;
uniform 	mediump float _NormalDistortTiling;
uniform 	mediump float _NormalDistortIntensity;
uniform 	mediump vec4 _TangentFlowMap_ST;
uniform 	mediump float _CoverLerp;
uniform 	mediump vec4 _CoverMainTex_ST;
uniform 	mediump float _AddShadowIntensity;
uniform 	mediump float _NormalSmoothness;
uniform 	mediump float _ScatterDirectOcclusion;
uniform 	mediump float _ScatterAmbientOcclusion;
uniform 	mediump float _AmbientOcclusionIntensity;
uniform 	vec4 unity_ObjectOffsetPosition;
uniform 	mediump vec4 _NightVisionLightMapDensity;
uniform 	mediump vec4 _Color;
uniform 	mediump float _Cutoff;
uniform 	mediump float _Metallic;
uniform 	mediump float _Specular;
uniform 	mediump float _Roughness;
uniform 	vec4 hlslcc_mtx4x4_cSmLightMatrix[4];
uniform 	mediump vec4 _cSmParam;
uniform 	mediump float _CameraExposure;
uniform 	mediump vec4 _aces_pack;
uniform 	mediump float _aces_whitePoint_scalar;
uniform 	mediump vec4 key0;
uniform 	mediump vec4 key1Delta;
uniform 	mediump vec4 key2Delta;
uniform 	mediump vec4 keyLerpData;
uniform mediump sampler2D _MainTex;
uniform lowp sampler2D _AmbientOcclusionRootMap;
uniform lowp sampler2D _CoverMainTex;
uniform lowp sampler2D _TangentFlowMap;
uniform lowp samplerCube unity_SpecCube0;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_cShadowmap;
uniform lowp sampler2D _cShadowmap;
in highp vec4 vs_TEXCOORD0;
in mediump vec4 vs_TEXCOORD1;
in mediump vec3 vs_TEXCOORD2;
in mediump vec3 vs_TEXCOORD3;
in mediump vec3 vs_TEXCOORD4;
in mediump vec3 vs_TEXCOORD7;
in highp vec3 vs_TEXCOORD9;
in mediump vec4 vs_TEXCOORD11;
in mediump vec4 vs_TEXCOORD12;
in mediump vec4 vs_TEXCOORD13;
layout(location = 0) out mediump vec4 SV_Target0;
vec2 u_xlat0;
mediump vec4 u_xlat16_0;
lowp vec2 u_xlat10_0;
vec4 u_xlat1;
mediump vec4 u_xlat16_1;
lowp float u_xlat10_1;
bool u_xlatb1;
vec4 u_xlat2;
mediump vec3 u_xlat16_2;
lowp vec2 u_xlat10_2;
vec4 u_xlat3;
mediump vec4 u_xlat16_3;
lowp vec4 u_xlat10_3;
mediump vec3 u_xlat16_4;
mediump vec3 u_xlat16_5;
mediump vec3 u_xlat16_6;
mediump vec3 u_xlat16_7;
mediump vec4 u_xlat16_8;
mediump vec4 u_xlat16_9;
mediump vec4 u_xlat16_10;
vec3 u_xlat11;
mediump vec3 u_xlat16_11;
bvec3 u_xlatb11;
vec3 u_xlat12;
mediump vec3 u_xlat16_12;
vec3 u_xlat13;
mediump vec2 u_xlat16_13;
mediump vec3 u_xlat16_14;
mediump vec3 u_xlat16_15;
mediump vec3 u_xlat16_16;
mediump vec3 u_xlat16_17;
mediump vec3 u_xlat16_18;
mediump vec3 u_xlat16_19;
mediump vec3 u_xlat16_20;
mediump float u_xlat16_21;
vec2 u_xlat22;
mediump float u_xlat16_22;
mediump vec3 u_xlat16_23;
vec3 u_xlat24;
mediump float u_xlat16_24;
lowp vec3 u_xlat10_24;
mediump vec3 u_xlat16_26;
mediump vec3 u_xlat16_43;
float u_xlat44;
mediump float u_xlat16_44;
lowp float u_xlat10_44;
bool u_xlatb44;
float u_xlat45;
mediump float u_xlat16_45;
bool u_xlatb46;
mediump float u_xlat16_51;
mediump float u_xlat16_52;
mediump float u_xlat16_59;
bool u_xlatb67;
mediump float u_xlat16_70;
mediump float u_xlat16_71;
mediump float u_xlat16_72;
mediump float u_xlat16_73;
mediump float u_xlat16_74;
mediump float u_xlat16_75;
mediump float u_xlat16_76;
float u_xlat77;
mediump float u_xlat16_77;
bool u_xlatb77;
mediump float u_xlat16_78;
bool u_xlatb78;
mediump float u_xlat16_80;
mediump float u_xlat16_81;
mediump float u_xlat16_82;
mediump float u_xlat16_83;
mediump float u_xlat16_84;
mediump float u_xlat16_85;
void main()
{
    u_xlat16_0 = texture(_MainTex, vs_TEXCOORD0.xy);
    u_xlat16_1.xyz = u_xlat16_0.xyz * _Color.xyz;
    u_xlat10_2.xy = texture(_AmbientOcclusionRootMap, vs_TEXCOORD0.xy).xy;
    u_xlat16_3.x = _CutoffTips + (-_Cutoff);
    u_xlat16_3.x = u_xlat10_2.y * u_xlat16_3.x + _Cutoff;
    u_xlat16_3.x = u_xlat16_0.w + (-u_xlat16_3.x);
#ifdef UNITY_ADRENO_ES3
    u_xlatb67 = !!(u_xlat16_3.x<0.0);
#else
    u_xlatb67 = u_xlat16_3.x<0.0;
#endif
    if((int(u_xlatb67) * int(0xffffffffu))!=0){discard;}
    u_xlat24.xy = vs_TEXCOORD0.zw * _CoverMainTex_ST.xy + _CoverMainTex_ST.zw;
    u_xlat10_3 = texture(_CoverMainTex, u_xlat24.xy);
    u_xlat16_4.x = u_xlat10_3.w * _CoverLerp;
    u_xlat16_26.xyz = (-u_xlat16_0.xyz) * _Color.xyz + u_xlat10_3.xyz;
    u_xlat16_4.xyz = u_xlat16_4.xxx * u_xlat16_26.xyz + u_xlat16_1.xyz;
	// u_xlat16_4 =  albedo
    u_xlat0.xy = vs_TEXCOORD0.xy * _TangentFlowMap_ST.xy + _TangentFlowMap_ST.zw;
    u_xlat10_0.xy = texture(_TangentFlowMap, u_xlat0.xy).xy;
	// uv.x = input.uv.x * _SpecularNoiseTiling
    u_xlat1.x = vs_TEXCOORD0.x * _SpecularNoiseTiling;
	// uv.yw = input.uv.y
    u_xlat1.yw = vs_TEXCOORD0.yy;
	//tangentFMap = texture(_TangentFlowMap,float2(uv.x,input.uv.y))
    u_xlat10_44 = texture(_TangentFlowMap, u_xlat1.xy).z;
	//uv.z = input.uv.x * _NormalDistortTiling
    u_xlat1.z = vs_TEXCOORD0.x * _NormalDistortTiling;
	//normal2 = texture(_TangentFlowMap,float2(uv.z,input.uv.y))
    u_xlat10_1 = texture(_TangentFlowMap, u_xlat1.zw).z;
	//metallic =  0.97 //-0.03+1.0
    u_xlat16_23.x = (-_Metallic) + 1.0;
	//diffuse = metallic * albedo
    u_xlat16_23.xyz = u_xlat16_23.xxx * u_xlat16_4.xyz;
	//u_xlat16_5 = albedo - 0.04
    u_xlat16_5.xyz = u_xlat16_4.xyz + vec3(-0.0399999991, -0.0399999991, -0.0399999991);
    //u_xlat16_5 = lerp(0.04,albedo,_Metallic)
	u_xlat16_5.xyz = vec3(_Metallic) * u_xlat16_5.xyz + vec3(0.0399999991, 0.0399999991, 0.0399999991);

	// t1 = t1 * 2 -1
	u_xlat16_70 = u_xlat10_44 * 2.0 + -1.0;
	// t2 = t2 * 2 -1
    u_xlat16_71 = u_xlat10_1 * 2.0 + -1.0;
	//
    u_xlat16_6.y = u_xlat16_71 * _NormalDistortIntensity;

    u_xlat16_71 = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD4.xyz);
    u_xlat16_71 = inversesqrt(u_xlat16_71);
    u_xlat16_3.xyz = vec3(u_xlat16_71) * vs_TEXCOORD4.xyz;
	// u_xlat16_3 = normalWS

    u_xlat16_71 = dot(vs_TEXCOORD12.xyz, vs_TEXCOORD12.xyz);
    u_xlat16_71 = inversesqrt(u_xlat16_71);
    u_xlat16_7.xyz = vec3(u_xlat16_71) * vs_TEXCOORD12.zyx;
    u_xlat16_8.xyz = vs_TEXCOORD12.xyz * vec3(u_xlat16_71) + vec3(-0.0, -0.0, -1.0);
	//

	//  0 * 
    u_xlat16_8.xyz = vec3(vec3(_NormalSmoothness, _NormalSmoothness, _NormalSmoothness)) * u_xlat16_8.xyz + vec3(0.0, 0.0, 1.0);
    u_xlat16_71 = dot(u_xlat16_8.xyz, u_xlat16_8.xyz);
    u_xlat16_71 = inversesqrt(u_xlat16_71);

	//
    u_xlat16_6.x = float(0.0);
    u_xlat16_6.z = float(0.0);
    u_xlat16_6.xyz = u_xlat16_8.xyz * vec3(u_xlat16_71) + u_xlat16_6.xyz;
    u_xlat16_71 = dot(u_xlat16_6.xyz, u_xlat16_6.xyz);
    u_xlat16_71 = inversesqrt(u_xlat16_71);
    u_xlat16_6.xyz = vec3(u_xlat16_71) * u_xlat16_6.xyz;

//----------------------------------------------------
    u_xlat16_3.w = 1.0;
    u_xlat16_8.x = dot(unity_SHAr, u_xlat16_3);
    u_xlat16_8.y = dot(unity_SHAg, u_xlat16_3);
    u_xlat16_8.z = dot(unity_SHAb, u_xlat16_3);
    u_xlat16_9 = u_xlat16_3.yzzx * u_xlat16_3.xyzz;
    u_xlat16_10.x = dot(unity_SHBr, u_xlat16_9);
    u_xlat16_10.y = dot(unity_SHBg, u_xlat16_9);
    u_xlat16_10.z = dot(unity_SHBb, u_xlat16_9);
    u_xlat16_71 = u_xlat16_3.y * u_xlat16_3.y;
    u_xlat16_71 = u_xlat16_3.x * u_xlat16_3.x + (-u_xlat16_71);
    u_xlat16_9.xyz = unity_SHC.xyz * vec3(u_xlat16_71) + u_xlat16_10.xyz;
    u_xlat16_8.xyz = u_xlat16_8.xyz + u_xlat16_9.xyz;
    u_xlat16_8.xyz = max(u_xlat16_8.xyz, vec3(0.0, 0.0, 0.0));
    u_xlat16_8.xyz = u_xlat16_8.xyz * _NightVisionLightMapDensity.zzz + _NightVisionLightMapDensity.www;
    u_xlat10_24.xyz = textureLod(unity_SpecCube0, u_xlat16_3.xyz, 6.0).xyz;
    u_xlat16_9.xyz = u_xlat10_24.xyz * unity_SpecCube0_HDR.xxx;
    u_xlat16_9.xyz = u_xlat16_9.xyz * _NightVisionLightMapDensity.zzz + _NightVisionLightMapDensity.www;
    // u_xlat16_9 = SH
//--------------------------------------------------------------

	u_xlat24.xyz = vs_TEXCOORD9.xyz + unity_ObjectOffsetPosition.xyz;
    u_xlat11.xyz = u_xlat24.xyz + (-unity_ShadowSplitSpheres[0].xyz);
    u_xlat12.xyz = u_xlat24.xyz + (-unity_ShadowSplitSpheres[1].xyz);
    u_xlat44 = dot(u_xlat11.xyz, u_xlat11.xyz);
    u_xlat1.x = dot(u_xlat12.xyz, u_xlat12.xyz);
    u_xlatb11.xz = lessThan(vec4(u_xlat44), unity_ShadowSplitSpheres[0].wwww).xz;
#ifdef UNITY_ADRENO_ES3
    u_xlatb11.y = !!(u_xlat1.x<unity_ShadowSplitSpheres[1].w);
#else
    u_xlatb11.y = u_xlat1.x<unity_ShadowSplitSpheres[1].w;
#endif
    u_xlat16_3.x = (u_xlatb11.x) ? float(1.0) : float(0.0);
    u_xlat16_3.y = (u_xlatb11.y) ? float(1.0) : float(0.0);
    u_xlat16_3.z = (u_xlatb11.z) ? float(-1.0) : float(-0.0);
    u_xlat16_71 = u_xlat16_3.z + u_xlat16_3.y;
    u_xlat16_3.w = max(u_xlat16_71, 0.0);
    u_xlat16_11.xy = (-u_xlat16_3.xw) + vec2(1.0, 0.0);
    u_xlat11.xy = unity_ObjectOffsetPosition.ww * u_xlat16_11.xy + u_xlat16_3.xw;
    u_xlat12.xyz = u_xlat24.yyy * hlslcc_mtx4x4unity_WorldToShadow[1].xyz;
    u_xlat12.xyz = hlslcc_mtx4x4unity_WorldToShadow[0].xyz * u_xlat24.xxx + u_xlat12.xyz;
    u_xlat12.xyz = hlslcc_mtx4x4unity_WorldToShadow[2].xyz * u_xlat24.zzz + u_xlat12.xyz;
    u_xlat12.xyz = u_xlat12.xyz + hlslcc_mtx4x4unity_WorldToShadow[3].xyz;
    u_xlat13.xyz = u_xlat24.yyy * hlslcc_mtx4x4unity_WorldToShadow[5].xyz;
    u_xlat13.xyz = hlslcc_mtx4x4unity_WorldToShadow[4].xyz * u_xlat24.xxx + u_xlat13.xyz;
    u_xlat24.xyz = hlslcc_mtx4x4unity_WorldToShadow[6].xyz * u_xlat24.zzz + u_xlat13.xyz;
    u_xlat24.xyz = u_xlat24.xyz + hlslcc_mtx4x4unity_WorldToShadow[7].xyz;
    u_xlat24.xyz = u_xlat11.yyy * u_xlat24.xyz;
    u_xlat24.xyz = u_xlat12.xyz * u_xlat11.xxx + u_xlat24.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlatb44 = !!(vec4(0.0, 0.0, 0.0, 0.0)!=vec4(unity_OrthoParams.z));
#else
    u_xlatb44 = vec4(0.0, 0.0, 0.0, 0.0)!=vec4(unity_OrthoParams.z);
#endif
    u_xlat1.x = dot(u_xlat11.xy, vec2(1.0, 1.0));
    u_xlat1.x = (-u_xlat1.x) + u_xlat24.z;
    u_xlat1.x = u_xlat1.x + 1.0;
    u_xlat1.x = (u_xlatb44) ? u_xlat1.x : u_xlat24.z;
    vec3 txVec0 = vec3(u_xlat24.xy,u_xlat1.x);
    u_xlat10_1 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec0, 0.0);
    u_xlat16_71 = (-_LightShadowData.x) + 1.0;
    u_xlat16_71 = u_xlat10_1 * u_xlat16_71 + _LightShadowData.x;

 //--------------------------------------
    u_xlat24.xyz = (-vs_TEXCOORD9.xyz) + _WorldSpaceCameraPos.xyz;
    u_xlat11.x = hlslcc_mtx4x4unity_MatrixV[0].z;
    u_xlat11.y = hlslcc_mtx4x4unity_MatrixV[1].z;
    u_xlat11.z = hlslcc_mtx4x4unity_MatrixV[2].z;
    u_xlat1.x = dot(u_xlat24.xyz, u_xlat11.xyz);
    u_xlat24.xyz = vs_TEXCOORD9.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    u_xlat24.x = dot(u_xlat24.xyz, u_xlat24.xyz);
    u_xlat24.x = sqrt(u_xlat24.x);
    u_xlat24.x = (-u_xlat1.x) + u_xlat24.x;
    u_xlat1.x = unity_ShadowFadeCenterAndType.w * u_xlat24.x + u_xlat1.x;
    u_xlat1.x = u_xlat1.x * _LightShadowData.z + _LightShadowData.w;
    
#ifdef UNITY_ADRENO_ES3
    u_xlat1.x = min(max(u_xlat1.x, 0.0), 1.0);
#else
    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
#endif
    u_xlat16_71 = u_xlat1.x + u_xlat16_71;

#ifdef UNITY_ADRENO_ES3
    u_xlat16_71 = min(max(u_xlat16_71, 0.0), 1.0);
#else
    u_xlat16_71 = clamp(u_xlat16_71, 0.0, 1.0);
#endif

	// lightDir = vs_TEXCOORD9.xyz - unity_LightPosition0.xyz 
    u_xlat24.xyz = (-vs_TEXCOORD9.xyz) + unity_LightPosition0.xyz;
    u_xlat1.x = dot(u_xlat24.xyz, u_xlat24.xyz);
    u_xlat16_72 = u_xlat1.x + 9.99999997e-007;
    u_xlat16_72 = inversesqrt(u_xlat16_72);
    u_xlat16_73 = u_xlat1.x * unity_4LightAtten0.x + 1.0;
    u_xlat16_73 = float(1.0) / u_xlat16_73;
    u_xlat24.xyz = u_xlat24.xyz * vec3(u_xlat16_72);
	//u_xlat24 = lightDir 

    u_xlat11.xyz = vs_TEXCOORD9.yyy * hlslcc_mtx4x4_cSmLightMatrix[1].xyz;
    u_xlat11.xyz = hlslcc_mtx4x4_cSmLightMatrix[0].xyz * vs_TEXCOORD9.xxx + u_xlat11.xyz;
    u_xlat11.xyz = hlslcc_mtx4x4_cSmLightMatrix[2].xyz * vs_TEXCOORD9.zzz + u_xlat11.xyz;
    u_xlat11.xyz = u_xlat11.zxy + hlslcc_mtx4x4_cSmLightMatrix[3].zxy;
    u_xlat11.x = u_xlat11.x;

#ifdef UNITY_ADRENO_ES3
    u_xlat11.x = min(max(u_xlat11.x, 0.0), 1.0);
#else
    u_xlat11.x = clamp(u_xlat11.x, 0.0, 1.0);
#endif

    u_xlat1.x = (-u_xlat11.x) + 1.0;
    u_xlat44 = (u_xlatb44) ? u_xlat1.x : u_xlat11.x;
    u_xlat16_72 = dot(u_xlat11.yz, u_xlat11.yz);
    u_xlat16_72 = min(u_xlat16_72, 1.0);
    u_xlat16_72 = (-u_xlat16_72) + 1.0;
    u_xlat11.xy = u_xlat11.yz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    u_xlat16_3 = _cSmParam.wwww * vec4(-0.5, -0.5, 0.5, -0.5) + u_xlat11.xyxy;
    vec3 txVec1 = vec3(u_xlat16_3.xy,u_xlat44);
    u_xlat16_10.x = textureLod(hlslcc_zcmp_cShadowmap, txVec1, 0.0);
    vec3 txVec2 = vec3(u_xlat16_3.zw,u_xlat44);
    u_xlat16_10.y = textureLod(hlslcc_zcmp_cShadowmap, txVec2, 0.0);
    u_xlat16_3 = _cSmParam.wwww * vec4(-0.5, 0.5, 0.5, 0.5) + u_xlat11.xyxy;
    vec3 txVec3 = vec3(u_xlat16_3.xy,u_xlat44);
    u_xlat16_10.z = textureLod(hlslcc_zcmp_cShadowmap, txVec3, 0.0);
    vec3 txVec4 = vec3(u_xlat16_3.zw,u_xlat44);
    u_xlat16_10.w = textureLod(hlslcc_zcmp_cShadowmap, txVec4, 0.0);
    vec3 txVec5 = vec3(u_xlat11.xy,u_xlat44);
    u_xlat16_74 = textureLod(hlslcc_zcmp_cShadowmap, txVec5, 0.0);
    u_xlat16_75 = dot(vec4(0.200000003, 0.200000003, 0.200000003, 0.200000003), u_xlat16_10);
    u_xlat16_74 = u_xlat16_74 * 0.200000003 + u_xlat16_75;
    u_xlat11.xyz = (-vs_TEXCOORD9.xyz) + unity_LightPosition1.xyz;
    u_xlat11.xyz = u_xlat11.xyz * vs_TEXCOORD11.www;
    u_xlat16_5.xyz = u_xlat16_5.xyz * vec3(0.452399939, 0.452399939, 0.452399939) + vec3(-0.00239999872, -0.00239999872, -0.00239999872);
    
    u_xlat16_10.xyz = vec3(u_xlat16_71) * _LightColor0.xyz;



	// dot(tangentDirWS,cameraDir)
    u_xlat16_14.x = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD7.xyz);
	// dot(bitangentDir,cameraDir)
    u_xlat16_14.y = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD7.xyz);
	// dot(normalDir,cameraDir)
    u_xlat16_14.z = dot(vs_TEXCOORD4.xyz, vs_TEXCOORD7.xyz);

    u_xlat16_71 = dot(u_xlat16_14.xyz, u_xlat16_14.xyz);
    u_xlat16_71 = inversesqrt(u_xlat16_71);
    u_xlat16_14.xyz = vec3(u_xlat16_71) * u_xlat16_14.xyz;
	//u_xlat16_14 = normalize(cameraDirTS)

	//  dot(tangentDirWS,ligtPos0Dir)
    u_xlat16_15.x = dot(vs_TEXCOORD2.xyz, _WorldSpaceLightPos0.xyz);
	//  dot(tangentDirWS,ligtPos0Dir)
    u_xlat16_15.y = dot(vs_TEXCOORD3.xyz, _WorldSpaceLightPos0.xyz);
	//  dot(normalDirWS  ligtPos0Dir)
    u_xlat16_15.z = dot(vs_TEXCOORD4.xyz, _WorldSpaceLightPos0.xyz);
    u_xlat16_71 = dot(u_xlat16_15.xyz, u_xlat16_15.xyz);
    u_xlat16_71 = inversesqrt(u_xlat16_71);
    u_xlat16_15.xyz = vec3(u_xlat16_71) * u_xlat16_15.xyz;
	//u_xlat16_15 = normalize(lightPos0DirTS)


    u_xlat16_16.xy = u_xlat10_0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
	// tangentDir = tangent * 2  -1 
    u_xlat16_71 = dot(u_xlat16_16.xy, u_xlat16_16.xy);
    u_xlat16_71 = inversesqrt(u_xlat16_71);
    u_xlat16_16.xy = vec2(u_xlat16_71) * u_xlat16_16.xy;
    u_xlat16_16.z = 0.0;

    u_xlat3.yzw = u_xlat16_15.zyx * vec3(0.488602519, 0.488602519, 0.488602519);
    u_xlat3.x = 0.282094806;
    u_xlat3 = u_xlat3 * vs_TEXCOORD13;
	//  vs_TEXCOORD13 = 0.5*2-1
    u_xlat0.x = dot(u_xlat3, vec4(1.0, 1.0, 1.0, 1.0));

#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif

    u_xlat16_71 = u_xlat0.x + -1.0;
    u_xlat16_71 = _ScatterDirectOcclusion * u_xlat16_71 + 1.0;
    u_xlat16_0.x = u_xlat16_71 * u_xlat16_71;
    u_xlat16_0.x = u_xlat16_0.x * u_xlat16_0.x;
    u_xlat16_0.x = u_xlat16_71 * u_xlat16_0.x;
    u_xlat16_71 = dot(u_xlat16_6.xyz, u_xlat16_15.xyz);

#ifdef UNITY_ADRENO_ES3
    u_xlat16_71 = min(max(u_xlat16_71, 0.0), 1.0);
#else
    u_xlat16_71 = clamp(u_xlat16_71, 0.0, 1.0);
#endif

    u_xlat16_75 = min(u_xlat16_0.x, u_xlat16_71);
    u_xlat16_12.xyz = u_xlat16_4.xyz + vec3(-0.991999984, -0.805999994, -0.518000007);

    u_xlat16_12.xyz = u_xlat16_12.xyz * vec3(0.5, 0.5, 0.5) + vec3(0.991999984, 0.805999994, 0.518000007);
	u_xlat16_17.xyz = vec3(u_xlat16_71) + u_xlat16_12.xyz;


#ifdef UNITY_ADRENO_ES3
    u_xlat16_17.xyz = min(max(u_xlat16_17.xyz, 0.0), 1.0);
#else
    u_xlat16_17.xyz = clamp(u_xlat16_17.xyz, 0.0, 1.0);
#endif

    u_xlat16_17.xyz = vec3(u_xlat16_75) * u_xlat16_17.xyz;
    u_xlat16_18.xyz = u_xlat16_23.xyz * u_xlat16_10.xyz;



	// perceptualRoughness = max(roughness,0.04)
    u_xlat16_75 = max(_Roughness, 0.00392156886);
    u_xlat16_75 = min(u_xlat16_75, 1.0);
    u_xlat16_75 = u_xlat16_75 * u_xlat16_75;
	// roughness = perceptualRoughness * perceptualRoughness

    u_xlat16_76 = dot(u_xlat16_14.xyz, u_xlat16_15.xyz);// VdotL
    u_xlat16_80 = dot(u_xlat16_16.xy, u_xlat16_15.xy);// TdotL
    u_xlat16_22 = -abs(u_xlat16_80) + 1.0;
    u_xlat16_22 = sqrt(u_xlat16_22);
    u_xlat16_44 = abs(u_xlat16_80) * -0.0187292993 + 0.0742610022;
    u_xlat16_44 = u_xlat16_44 * abs(u_xlat16_80) + -0.212114394;
    u_xlat16_44 = u_xlat16_44 * abs(u_xlat16_80) + 1.57072878;
    u_xlat16_1.x = u_xlat16_22 * u_xlat16_44;
    u_xlat1.x = u_xlat16_1.x * -2.0 + 3.14159274;
#ifdef UNITY_ADRENO_ES3
    u_xlatb77 = !!(u_xlat16_80<(-u_xlat16_80));
#else
    u_xlatb77 = u_xlat16_80<(-u_xlat16_80);
#endif
    u_xlat1.x = u_xlatb77 ? u_xlat1.x : float(0.0);
    u_xlat22.x = u_xlat16_44 * u_xlat16_22 + u_xlat1.x;
    u_xlat16_81 = dot(u_xlat16_16.xy, u_xlat16_14.xy);//TdotV
    u_xlat16_82 = (-u_xlat16_81) * u_xlat16_81 + 1.0;//1- TdotV*TdotV
    u_xlat16_82 = max(u_xlat16_82, 0.0);
    u_xlat16_82 = sqrt(u_xlat16_82);

    u_xlat16_44 = -abs(u_xlat16_81) + 1.0;
    u_xlat16_44 = sqrt(u_xlat16_44);
    u_xlat16_1.x = abs(u_xlat16_81) * -0.0187292993 + 0.0742610022;
    u_xlat16_1.x = u_xlat16_1.x * abs(u_xlat16_81) + -0.212114394;
    u_xlat16_1.x = u_xlat16_1.x * abs(u_xlat16_81) + 1.57072878;
    u_xlat16_77 = u_xlat16_44 * u_xlat16_1.x;
    u_xlat77 = u_xlat16_77 * -2.0 + 3.14159274;
#ifdef UNITY_ADRENO_ES3
    u_xlatb78 = !!(u_xlat16_81<(-u_xlat16_81));
#else
    u_xlatb78 = u_xlat16_81<(-u_xlat16_81);
#endif
    u_xlat77 = u_xlatb78 ? u_xlat77 : float(0.0);
    u_xlat22.y = u_xlat16_1.x * u_xlat16_44 + u_xlat77;
    u_xlat22.xy = (-u_xlat22.xy) + vec2(1.57079637, 1.57079637);
    u_xlat16_83 = (-u_xlat22.y) + u_xlat22.x;
    u_xlat16_83 = abs(u_xlat16_83) * 0.5;
    u_xlat16_83 = cos(u_xlat16_83);
    u_xlat16_15.xyz = (-u_xlat16_16.xyz) * vec3(u_xlat16_80) + u_xlat16_15.xyz;// T* TdotL +L
    u_xlat16_19.xyz = (-u_xlat16_16.xyz) * vec3(u_xlat16_81) + u_xlat16_14.xyz;// T* TdotV +V
    u_xlat16_15.x = dot(u_xlat16_15.xyz, u_xlat16_19.xyz);// dot(T* TdotL +L, T* TdotV +V)
    u_xlat16_15.xy = u_xlat16_15.xx * vec2(0.5, 17.0) + vec2(0.5, -16.7800007);
    u_xlat16_15.x = u_xlat16_15.x;
#ifdef UNITY_ADRENO_ES3
    u_xlat16_15.x = min(max(u_xlat16_15.x, 0.0), 1.0);
#else
    u_xlat16_15.x = clamp(u_xlat16_15.x, 0.0, 1.0);
#endif
    u_xlat16_15.x = sqrt(u_xlat16_15.x);

    u_xlat16_13.xy = vec2(u_xlat16_70) * vec2(vec2(_SpecularNoiseIntensity, _SpecularNoiseIntensity)) + vec2(-0.0700000003, 0.140000001);
    u_xlat16_20.x = sin(u_xlat16_13.x);//sin(Alpha[0])
    u_xlat16_21 = cos(u_xlat16_13.x);//cos(Alpha[0])
    u_xlat16_70 = u_xlat16_15.x * u_xlat16_21;// CosHalfPhi *ca
    u_xlat16_59 = u_xlat16_81 * u_xlat16_20.x;//sa * SinThetaV
    u_xlat16_70 = u_xlat16_70 * u_xlat16_82 + u_xlat16_59;// CosHalfPhi *ca*sqrt(1 - SinThetaV * SinThetaV)+sa * SinThetaV)
    
    u_xlat16_84 = u_xlat16_20.x + u_xlat16_20.x;
    u_xlat16_85 = u_xlat16_75 * 1.41421354;// Roughness * sqrt(2.0)
    u_xlat16_20.x = u_xlat16_15.x * u_xlat16_85;

    u_xlat16_80 = u_xlat16_80 + u_xlat16_81;
    u_xlat16_22 = (-u_xlat16_84) * u_xlat16_70 + u_xlat16_80;//2sa * (CosHalfPhi *ca*sqrt(1 - SinThetaV * SinThetaV)+sa * SinThetaV)+
    u_xlat16_22 = u_xlat16_22 * u_xlat16_22;
    u_xlat16_22 = u_xlat16_22 * -0.5;
    u_xlat16_1.x = u_xlat16_20.x * u_xlat16_20.x;
    u_xlat16_22 = u_xlat16_22 / u_xlat16_1.x;

    u_xlat16_22 = u_xlat16_22 * 1.44269502;
    u_xlat16_22 = exp2(u_xlat16_22);

    u_xlat16_1.x = u_xlat16_20.x * 2.50662827;// 
    u_xlat16_22 = u_xlat16_22 / u_xlat16_1.x;// Mp

    //---------------------------------------------------
    u_xlat16_70 = u_xlat16_15.x * 0.25;// 0.25 * CosHalfPhi
    //------------------Np-------------------------

    //----------------------------------------------------
    u_xlat16_15.x = u_xlat16_76 * 0.5 + 0.5;
    u_xlat16_15.x = sqrt(u_xlat16_15.x);
    u_xlat16_1.x = (-u_xlat16_15.x) + 1.0;
    u_xlat16_77 = u_xlat16_1.x * u_xlat16_1.x;
    u_xlat16_77 = u_xlat16_77 * u_xlat16_77;
    u_xlat16_1.x = u_xlat16_1.x * u_xlat16_77;
    u_xlat16_1.x = u_xlat16_1.x * 0.953479409 + 0.0465205684;
    //-------------------FP-------------------------------

    u_xlat16_70 = u_xlat16_22 * u_xlat16_70; // Mp * Np
    u_xlat16_70 = u_xlat16_1.x * u_xlat16_70;// FP * Np
    u_xlat16_76 = (-u_xlat16_76);// -VdotL
#ifdef UNITY_ADRENO_ES3
    u_xlat16_76 = min(max(u_xlat16_76, 0.0), 1.0);
#else
    u_xlat16_76 = clamp(u_xlat16_76, 0.0, 1.0);
#endif
    u_xlat16_76 = (-u_xlat16_76) + 1.0;//
    u_xlat16_70 = u_xlat16_70 * u_xlat16_76;// (1 - saturate(-VdotL))*Mp * Np * Fp
    u_xlat16_76 = _Specular + _Specular; // 2Specular
    u_xlat16_70 = u_xlat16_70 * u_xlat16_76;//(1 - saturate(-VdotL))*Mp * Np * Fp * 2Specular

    u_xlat16_20.xyz = u_xlat16_10.xyz * vec3(u_xlat16_70);
    u_xlat16_70 = u_xlat16_75 + u_xlat16_75;
    u_xlat16_22 = (-u_xlat16_83) * 0.5 + 1.0;
    u_xlat16_1.x = u_xlat16_22 * u_xlat16_22;
    u_xlat16_1.x = u_xlat16_1.x * u_xlat16_1.x;
    u_xlat16_22 = u_xlat16_22 * u_xlat16_1.x;
    u_xlat16_22 = u_xlat16_22 * 0.953479409 + 0.0465205684;
    u_xlat16_15.x = (-u_xlat16_22) + 1.0;
    u_xlat16_1.x = u_xlat16_15.x * u_xlat16_15.x;
    u_xlat16_22 = u_xlat16_22 * u_xlat16_1.x;
    u_xlat16_80 = (-u_xlat16_13.y) + u_xlat16_80;
    u_xlat16_1.x = u_xlat16_80 * u_xlat16_80;
    u_xlat16_1.x = u_xlat16_1.x * -0.5;
    u_xlat16_77 = u_xlat16_70 * u_xlat16_70;
    u_xlat16_1.x = u_xlat16_1.x / u_xlat16_77;
    u_xlat16_1.x = u_xlat16_1.x * 1.44269502;
    u_xlat16_1.x = exp2(u_xlat16_1.x);

    u_xlat16_78 = u_xlat16_75 * 5.01325655;	// roughness *5.0



    u_xlat16_1.x = u_xlat16_1.x / u_xlat16_78;
    u_xlat16_70 = u_xlat16_15.y * 1.44269502;
    u_xlat16_70 = exp2(u_xlat16_70);
    u_xlat16_75 = 0.800000012 / u_xlat16_83;
    u_xlat16_4.xyz = log2(u_xlat16_4.xyz);



    u_xlat16_43.xyz = u_xlat16_4.xyz * vec3(u_xlat16_75);
    u_xlat16_43.xyz = exp2(u_xlat16_43.xyz);
    u_xlat16_70 = u_xlat16_1.x * u_xlat16_70;
    u_xlat16_70 = u_xlat16_22 * u_xlat16_70;
    u_xlat16_43.xyz = u_xlat16_43.xyz * vec3(u_xlat16_70);
    u_xlat16_10.xyz = u_xlat16_10.xyz * u_xlat16_43.xyz;
    u_xlat16_10.xyz = u_xlat16_20.xyz * vec3(u_xlat16_71) + u_xlat16_10.xyz;
    //
    u_xlat16_10.xyz = max(u_xlat16_10.xyz, vec3(0.0, 0.0, 0.0));
    u_xlat16_10.xyz = u_xlat16_0.xxx * u_xlat16_10.xyz;
    u_xlat16_10.xyz = u_xlat16_18.xyz * u_xlat16_17.xyz + u_xlat16_10.xyz;
    u_xlat16_70 = u_xlat10_2.x + -1.0;



    u_xlat16_70 = _AmbientOcclusionIntensity * u_xlat16_70 + 1.0;
    u_xlat3.yzw = u_xlat16_7.xyz * vec3(0.488602519, 0.488602519, 0.488602519);
    u_xlat3.x = 0.282094806;
    u_xlat3 = u_xlat3 * vs_TEXCOORD13;
    u_xlat0.x = dot(u_xlat3, vec4(1.0, 1.0, 1.0, 1.0));

#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat16_71 = u_xlat0.x + -1.0;
    u_xlat16_71 = _ScatterAmbientOcclusion * u_xlat16_71 + 1.0;
    u_xlat16_0.x = u_xlat16_71 * u_xlat16_71;
    u_xlat16_0.x = u_xlat16_0.x * u_xlat16_0.x;
    u_xlat16_0.x = u_xlat16_71 * u_xlat16_0.x;
    u_xlat16_5.xyz = u_xlat16_9.xyz * u_xlat16_5.xyz;
    u_xlat16_5.xyz = u_xlat16_23.xyz * u_xlat16_8.xyz + u_xlat16_5.xyz;
    u_xlat16_5.xyz = vec3(u_xlat16_70) * u_xlat16_5.xyz;

	// light0Dir
    u_xlat16_7.x = dot(vs_TEXCOORD2.xyz, u_xlat24.xyz);
    u_xlat16_7.y = dot(vs_TEXCOORD3.xyz, u_xlat24.xyz);
    u_xlat16_7.z = dot(vs_TEXCOORD4.xyz, u_xlat24.xyz);
    u_xlat16_70 = dot(u_xlat16_7.xyz, u_xlat16_7.xyz);
    u_xlat16_70 = inversesqrt(u_xlat16_70);
    u_xlat16_7.xyz = vec3(u_xlat16_70) * u_xlat16_7.xyz;
	// 

	u_xlat2.yzw = u_xlat16_7.zyx * vec3(0.488602519, 0.488602519, 0.488602519);
    u_xlat2.x = 0.282094806;
    u_xlat2 = u_xlat2 * vs_TEXCOORD13;
    u_xlat22.x = dot(u_xlat2, vec4(1.0, 1.0, 1.0, 1.0));
#ifdef UNITY_ADRENO_ES3
    u_xlat22.x = min(max(u_xlat22.x, 0.0), 1.0);
#else
    u_xlat22.x = clamp(u_xlat22.x, 0.0, 1.0);
#endif
    u_xlat16_70 = u_xlat22.x + -1.0;
    u_xlat16_70 = _ScatterDirectOcclusion * u_xlat16_70 + 1.0;
    u_xlat16_22 = u_xlat16_70 * u_xlat16_70;
    u_xlat16_22 = u_xlat16_22 * u_xlat16_22;
    u_xlat16_22 = u_xlat16_70 * u_xlat16_22;
    u_xlat16_70 = u_xlat16_74 * u_xlat16_72 + -1.0;
    u_xlat16_70 = _AddShadowIntensity * u_xlat16_70 + 1.0;
    u_xlat16_70 = u_xlat16_70 * u_xlat16_73;

 

	// dot(tangent , lightDir0)
    u_xlat16_71 = dot(u_xlat16_6.xyz, u_xlat16_7.xyz);

#ifdef UNITY_ADRENO_ES3
    u_xlat16_71 = min(max(u_xlat16_71, 0.0), 1.0);
#else
    u_xlat16_71 = clamp(u_xlat16_71, 0.0, 1.0);
#endif
    u_xlat16_72 = min(u_xlat16_22, u_xlat16_71);
    u_xlat16_8.xyz = vec3(u_xlat16_71) + u_xlat16_12.xyz;



#ifdef UNITY_ADRENO_ES3
    u_xlat16_8.xyz = min(max(u_xlat16_8.xyz, 0.0), 1.0);
#else
    u_xlat16_8.xyz = clamp(u_xlat16_8.xyz, 0.0, 1.0);
#endif
    u_xlat16_8.xyz = vec3(u_xlat16_72) * u_xlat16_8.xyz;
    u_xlat16_9.xyz = u_xlat16_23.xyz * unity_LightColor0.xyz;
    u_xlat16_9.xyz = vec3(u_xlat16_70) * u_xlat16_9.xyz;
	// cameraTS lightTS
    u_xlat16_72 = dot(u_xlat16_14.xyz, u_xlat16_7.xyz);


    u_xlat16_73 = dot(u_xlat16_16.xy, u_xlat16_7.xy);
    u_xlat16_1.x = -abs(u_xlat16_73) + 1.0;
    u_xlat16_1.x = sqrt(u_xlat16_1.x);
    u_xlat16_2.x = abs(u_xlat16_73) * -0.0187292993 + 0.0742610022;
    u_xlat16_2.x = u_xlat16_2.x * abs(u_xlat16_73) + -0.212114394;
    u_xlat16_2.x = u_xlat16_2.x * abs(u_xlat16_73) + 1.57072878;
    u_xlat16_24 = u_xlat16_1.x * u_xlat16_2.x;
    u_xlat24.x = u_xlat16_24 * -2.0 + 3.14159274;
#ifdef UNITY_ADRENO_ES3
    u_xlatb46 = !!(u_xlat16_73<(-u_xlat16_73));
#else
    u_xlatb46 = u_xlat16_73<(-u_xlat16_73);
#endif
    u_xlat24.x = u_xlatb46 ? u_xlat24.x : float(0.0);
    u_xlat1.x = u_xlat16_2.x * u_xlat16_1.x + u_xlat24.x;
    u_xlat1.x = (-u_xlat1.x) + 1.57079637;
    u_xlat16_74 = (-u_xlat22.y) + u_xlat1.x;
    u_xlat16_74 = abs(u_xlat16_74) * 0.5;
    u_xlat16_74 = cos(u_xlat16_74);

    u_xlat16_7.xyz = (-u_xlat16_16.xyz) * vec3(u_xlat16_73) + u_xlat16_7.xyz;
    u_xlat16_7.x = dot(u_xlat16_7.xyz, u_xlat16_19.xyz);
    u_xlat16_7.xy = u_xlat16_7.xx * vec2(0.5, 17.0) + vec2(0.5, -16.7800007);
    u_xlat16_7.x = u_xlat16_7.x;

#ifdef UNITY_ADRENO_ES3
    u_xlat16_7.x = min(max(u_xlat16_7.x, 0.0), 1.0);
#else
    u_xlat16_7.x = clamp(u_xlat16_7.x, 0.0, 1.0);
#endif
    u_xlat16_7.x = sqrt(u_xlat16_7.x);


    u_xlat16_51 = u_xlat16_7.x * u_xlat16_21;
    u_xlat16_51 = u_xlat16_51 * u_xlat16_82 + u_xlat16_59;
    u_xlat16_75 = u_xlat16_7.x * u_xlat16_85;
    u_xlat16_73 = u_xlat16_81 + u_xlat16_73;
    u_xlat16_1.x = (-u_xlat16_84) * u_xlat16_51 + u_xlat16_73;
    u_xlat16_1.x = u_xlat16_1.x * u_xlat16_1.x;
    u_xlat16_1.x = u_xlat16_1.x * -0.5;
    u_xlat16_2.x = u_xlat16_75 * u_xlat16_75;
    u_xlat16_1.x = u_xlat16_1.x / u_xlat16_2.x;
    u_xlat16_1.x = u_xlat16_1.x * 1.44269502;
    u_xlat16_1.x = exp2(u_xlat16_1.x);
	// exp(u_xlat16_1.x)
    u_xlat16_2.x = u_xlat16_75 * 2.50662827;
    u_xlat16_1.x = u_xlat16_1.x / u_xlat16_2.x;


    u_xlat16_7.x = u_xlat16_7.x * 0.25;


    u_xlat16_51 = u_xlat16_72 * 0.5 + 0.5;
    u_xlat16_51 = sqrt(u_xlat16_51);
    u_xlat16_2.x = (-u_xlat16_51) + 1.0;


    u_xlat16_24 = u_xlat16_2.x * u_xlat16_2.x;
    u_xlat16_24 = u_xlat16_24 * u_xlat16_24;

    u_xlat16_2.x = u_xlat16_2.x * u_xlat16_24;
    u_xlat16_2.x = u_xlat16_2.x * 0.953479409 + 0.0465205684;
    // u_xlat16_2 =  0.0465 + (1 -  0.0465) * pow5(1 - CosTheta);


    u_xlat16_7.x = u_xlat16_1.x * u_xlat16_7.x;
    u_xlat16_7.x = u_xlat16_2.x * u_xlat16_7.x;



    u_xlat16_72 = (-u_xlat16_72);
#ifdef UNITY_ADRENO_ES3
    u_xlat16_72 = min(max(u_xlat16_72, 0.0), 1.0);
#else
    u_xlat16_72 = clamp(u_xlat16_72, 0.0, 1.0);
#endif
    u_xlat16_72 = (-u_xlat16_72) + 1.0;
    u_xlat16_72 = u_xlat16_72 * u_xlat16_7.x;

    u_xlat16_72 = u_xlat16_76 * u_xlat16_72;
    u_xlat16_72 = u_xlat16_70 * u_xlat16_72;

	//specular * Lightcolor
    u_xlat16_17.xyz = vec3(u_xlat16_72) * unity_LightColor0.xyz;




    u_xlat16_1.x = (-u_xlat16_74) * 0.5 + 1.0;
    u_xlat16_2.x = u_xlat16_1.x * u_xlat16_1.x;
    u_xlat16_2.x = u_xlat16_2.x * u_xlat16_2.x;
    u_xlat16_1.x = u_xlat16_1.x * u_xlat16_2.x;
    u_xlat16_1.x = u_xlat16_1.x * 0.953479409 + 0.0465205684;
	// u_xlat16_1 =  0.0465 + (1 -  0.0465) * pow5(1 - CosTheta);

    u_xlat16_72 = (-u_xlat16_1.x) + 1.0;
    u_xlat16_2.x = u_xlat16_72 * u_xlat16_72;

    u_xlat16_1.x = u_xlat16_1.x * u_xlat16_2.x;
	// Fp


    u_xlat16_72 = (-u_xlat16_13.y) + u_xlat16_73;
    u_xlat16_2.x = u_xlat16_72 * u_xlat16_72;
    u_xlat16_2.x = u_xlat16_2.x * -0.5;
    u_xlat16_2.x = u_xlat16_2.x / u_xlat16_77;

    u_xlat16_2.x = u_xlat16_2.x * 1.44269502;
    u_xlat16_2.x = exp2(u_xlat16_2.x);

    u_xlat16_2.x = u_xlat16_2.x / u_xlat16_78;	//  ?/ (rounghness*5.0)

	
    u_xlat16_72 = u_xlat16_7.y * 1.44269502;
    u_xlat16_72 = exp2(u_xlat16_72);

	// Np
    u_xlat16_7.x = 0.800000012 / u_xlat16_74;


    u_xlat16_7.xyz = u_xlat16_4.xyz * u_xlat16_7.xxx;
    u_xlat16_7.xyz = exp2(u_xlat16_7.xyz);//Tp
    u_xlat16_72 = u_xlat16_2.x * u_xlat16_72;// Mp * Np
    u_xlat16_72 = u_xlat16_1.x * u_xlat16_72; //Fp * Np
    u_xlat16_7.xyz = u_xlat16_7.xyz * vec3(u_xlat16_72); //  Tp*Np

    u_xlat16_7.xyz = u_xlat16_7.xyz * unity_LightColor0.xyz;
    u_xlat16_7.xyz = vec3(u_xlat16_70) * u_xlat16_7.xyz;
    u_xlat16_7.xyz = u_xlat16_17.xyz * vec3(u_xlat16_71) + u_xlat16_7.xyz;

	/////////////////////////////////////////////
    u_xlat16_7.xyz = max(u_xlat16_7.xyz, vec3(0.0, 0.0, 0.0));
    u_xlat16_7.xyz = vec3(u_xlat16_22) * u_xlat16_7.xyz;
    u_xlat16_7.xyz = u_xlat16_9.xyz * u_xlat16_8.xyz + u_xlat16_7.xyz;
    u_xlat16_8.x = dot(vs_TEXCOORD2.xyz, u_xlat11.xyz);
    u_xlat16_8.y = dot(vs_TEXCOORD3.xyz, u_xlat11.xyz);
    u_xlat16_8.z = dot(vs_TEXCOORD4.xyz, u_xlat11.xyz);
    u_xlat16_70 = dot(u_xlat16_8.xyz, u_xlat16_8.xyz);
    u_xlat16_70 = inversesqrt(u_xlat16_70);
    u_xlat16_8.xyz = vec3(u_xlat16_70) * u_xlat16_8.xyz;
    u_xlat2.yzw = u_xlat16_8.zyx * vec3(0.488602519, 0.488602519, 0.488602519);
    u_xlat2.x = 0.282094806;
    u_xlat2 = u_xlat2 * vs_TEXCOORD13;
    u_xlat22.x = dot(u_xlat2, vec4(1.0, 1.0, 1.0, 1.0));


#ifdef UNITY_ADRENO_ES3
    u_xlat22.x = min(max(u_xlat22.x, 0.0), 1.0);
#else
    u_xlat22.x = clamp(u_xlat22.x, 0.0, 1.0);
#endif
    u_xlat16_70 = u_xlat22.x + -1.0;
    u_xlat16_70 = _ScatterDirectOcclusion * u_xlat16_70 + 1.0;
    u_xlat16_22 = u_xlat16_70 * u_xlat16_70;
    u_xlat16_22 = u_xlat16_22 * u_xlat16_22;
    u_xlat16_22 = u_xlat16_70 * u_xlat16_22;
    u_xlat16_70 = dot(u_xlat16_6.xyz, u_xlat16_8.xyz);


#ifdef UNITY_ADRENO_ES3
    u_xlat16_70 = min(max(u_xlat16_70, 0.0), 1.0);
#else
    u_xlat16_70 = clamp(u_xlat16_70, 0.0, 1.0);
#endif
    u_xlat16_71 = min(u_xlat16_22, u_xlat16_70);
    u_xlat16_6.xyz = vec3(u_xlat16_70) + u_xlat16_12.xyz;
#ifdef UNITY_ADRENO_ES3
    u_xlat16_6.xyz = min(max(u_xlat16_6.xyz, 0.0), 1.0);
#else
    u_xlat16_6.xyz = clamp(u_xlat16_6.xyz, 0.0, 1.0);
#endif
    u_xlat16_6.xyz = vec3(u_xlat16_71) * u_xlat16_6.xyz;

    u_xlat16_9.xyz = u_xlat16_23.xyz * unity_LightColor1.xyz;
    u_xlat16_9.xyz = u_xlat16_9.xyz * vs_TEXCOORD11.yyy;
    u_xlat16_71 = dot(u_xlat16_14.xyz, u_xlat16_8.xyz);
    u_xlat16_72 = dot(u_xlat16_16.xy, u_xlat16_8.xy);
    u_xlat16_1.x = -abs(u_xlat16_72) + 1.0;
    u_xlat16_1.x = sqrt(u_xlat16_1.x);
    u_xlat16_23.x = abs(u_xlat16_72) * -0.0187292993 + 0.0742610022;
    u_xlat16_23.x = u_xlat16_23.x * abs(u_xlat16_72) + -0.212114394;
    u_xlat16_23.x = u_xlat16_23.x * abs(u_xlat16_72) + 1.57072878;
    u_xlat16_45 = u_xlat16_1.x * u_xlat16_23.x;
    u_xlat45 = u_xlat16_45 * -2.0 + 3.14159274;
#ifdef UNITY_ADRENO_ES3
    u_xlatb67 = !!(u_xlat16_72<(-u_xlat16_72));
#else
    u_xlatb67 = u_xlat16_72<(-u_xlat16_72);
#endif
    u_xlat45 = u_xlatb67 ? u_xlat45 : float(0.0);
    u_xlat1.x = u_xlat16_23.x * u_xlat16_1.x + u_xlat45;
    u_xlat1.x = (-u_xlat1.x) + 1.57079637;
    u_xlat16_73 = (-u_xlat22.y) + u_xlat1.x;
    u_xlat16_73 = abs(u_xlat16_73) * 0.5;
    u_xlat16_73 = cos(u_xlat16_73);
    u_xlat16_8.xyz = (-u_xlat16_16.xyz) * vec3(u_xlat16_72) + u_xlat16_8.xyz;
    u_xlat16_8.x = dot(u_xlat16_8.xyz, u_xlat16_19.xyz);
    u_xlat16_8.xy = u_xlat16_8.xx * vec2(0.5, 17.0) + vec2(0.5, -16.7800007);
    u_xlat16_8.x = u_xlat16_8.x;



#ifdef UNITY_ADRENO_ES3
    u_xlat16_8.x = min(max(u_xlat16_8.x, 0.0), 1.0);
#else
    u_xlat16_8.x = clamp(u_xlat16_8.x, 0.0, 1.0);
#endif
    u_xlat16_8.x = sqrt(u_xlat16_8.x);
    u_xlat16_52 = u_xlat16_8.x * u_xlat16_21;
    u_xlat16_52 = u_xlat16_52 * u_xlat16_82 + u_xlat16_59;
    u_xlat16_74 = u_xlat16_8.x * u_xlat16_85;
    u_xlat16_72 = u_xlat16_81 + u_xlat16_72;
    u_xlat16_44 = (-u_xlat16_84) * u_xlat16_52 + u_xlat16_72;
    u_xlat16_44 = u_xlat16_44 * u_xlat16_44;
    u_xlat16_44 = u_xlat16_44 * -0.5;
    u_xlat16_1.x = u_xlat16_74 * u_xlat16_74;
    u_xlat16_44 = u_xlat16_44 / u_xlat16_1.x;
    u_xlat16_44 = u_xlat16_44 * 1.44269502;
    u_xlat16_44 = exp2(u_xlat16_44);
    u_xlat16_1.x = u_xlat16_74 * 2.50662827;
    u_xlat16_44 = u_xlat16_44 / u_xlat16_1.x;
    u_xlat16_8.x = u_xlat16_8.x * 0.25;
    u_xlat16_52 = u_xlat16_71 * 0.5 + 0.5;
    u_xlat16_52 = sqrt(u_xlat16_52);
    u_xlat16_1.x = (-u_xlat16_52) + 1.0;
    u_xlat16_23.x = u_xlat16_1.x * u_xlat16_1.x;
    u_xlat16_23.x = u_xlat16_23.x * u_xlat16_23.x;
    u_xlat16_1.x = u_xlat16_1.x * u_xlat16_23.x;
    u_xlat16_1.x = u_xlat16_1.x * 0.953479409 + 0.0465205684;
    u_xlat16_8.x = u_xlat16_44 * u_xlat16_8.x;
    u_xlat16_8.x = u_xlat16_1.x * u_xlat16_8.x;
    u_xlat16_71 = (-u_xlat16_71);
#ifdef UNITY_ADRENO_ES3
    u_xlat16_71 = min(max(u_xlat16_71, 0.0), 1.0);
#else
    u_xlat16_71 = clamp(u_xlat16_71, 0.0, 1.0);
#endif
    u_xlat16_71 = (-u_xlat16_71) + 1.0;
    u_xlat16_71 = u_xlat16_71 * u_xlat16_8.x;
    u_xlat16_71 = u_xlat16_76 * u_xlat16_71;
    u_xlat16_71 = u_xlat16_71 * vs_TEXCOORD11.y;
    u_xlat16_8.xzw = vec3(u_xlat16_71) * unity_LightColor1.xyz;
    u_xlat16_44 = (-u_xlat16_73) * 0.5 + 1.0;
    u_xlat16_1.x = u_xlat16_44 * u_xlat16_44;
    u_xlat16_1.x = u_xlat16_1.x * u_xlat16_1.x;
    u_xlat16_44 = u_xlat16_44 * u_xlat16_1.x;
    u_xlat16_44 = u_xlat16_44 * 0.953479409 + 0.0465205684;
    u_xlat16_71 = (-u_xlat16_44) + 1.0;
    u_xlat16_1.x = u_xlat16_71 * u_xlat16_71;
    u_xlat16_44 = u_xlat16_44 * u_xlat16_1.x;
    u_xlat16_71 = (-u_xlat16_13.y) + u_xlat16_72;
    u_xlat16_1.x = u_xlat16_71 * u_xlat16_71;
    u_xlat16_1.x = u_xlat16_1.x * -0.5;
    u_xlat16_1.x = u_xlat16_1.x / u_xlat16_77;
    u_xlat16_1.x = u_xlat16_1.x * 1.44269502;
    u_xlat16_1.x = exp2(u_xlat16_1.x);
    u_xlat16_1.x = u_xlat16_1.x / u_xlat16_78;
    u_xlat16_71 = u_xlat16_8.y * 1.44269502;
    u_xlat16_71 = exp2(u_xlat16_71);
    u_xlat16_72 = 0.800000012 / u_xlat16_73;


    u_xlat16_4.xyz = u_xlat16_4.xyz * vec3(u_xlat16_72);
    u_xlat16_4.xyz = exp2(u_xlat16_4.xyz);
    u_xlat16_71 = u_xlat16_1.x * u_xlat16_71;
    u_xlat16_71 = u_xlat16_44 * u_xlat16_71;
    u_xlat16_4.xyz = u_xlat16_4.xyz * vec3(u_xlat16_71);
    u_xlat16_4.xyz = u_xlat16_4.xyz * unity_LightColor1.xyz;
    u_xlat16_4.xyz = u_xlat16_4.xyz * vs_TEXCOORD11.yyy;
    u_xlat16_4.xyz = u_xlat16_8.xzw * vec3(u_xlat16_70) + u_xlat16_4.xyz;
    u_xlat16_4.xyz = max(u_xlat16_4.xyz, vec3(0.0, 0.0, 0.0));
    u_xlat16_4.xyz = vec3(u_xlat16_22) * u_xlat16_4.xyz;
    u_xlat16_4.xyz = u_xlat16_9.xyz * u_xlat16_6.xyz + u_xlat16_4.xyz;

    u_xlat16_4.xyz = u_xlat16_4.xyz + u_xlat16_7.xyz;
    u_xlat16_4.xyz = u_xlat16_4.xyz + u_xlat16_10.xyz;
    u_xlat16_4.xyz = u_xlat16_5.xyz * u_xlat16_0.xxx + u_xlat16_4.xyz;

    u_xlat16_4.xyz = max(u_xlat16_4.xyz, vec3(0.00100000005, 0.00100000005, 0.00100000005));
    u_xlat16_5.xyz = (-u_xlat16_4.xyz) + unity_FogColor.xyz;
    u_xlat16_0.xyz = u_xlat16_5.xyz * vs_TEXCOORD1.www + u_xlat16_4.xyz;

#ifdef UNITY_ADRENO_ES3
    u_xlatb1 = !!(0.0<_OnePassToggle);
#else
    u_xlatb1 = 0.0<_OnePassToggle;
#endif

//-------------------------------------------
// u_xlatb1 == OnePass Tone Mapping
    if(u_xlatb1){
        u_xlat16_4.x = dot(u_xlat16_0.xyz, vec3(0.212500006, 0.715399981, 0.0720999986));
        u_xlat16_26.xy = u_xlat16_4.xx * keyLerpData.xz + keyLerpData.yw;
#ifdef UNITY_ADRENO_ES3
        u_xlat16_26.xy = min(max(u_xlat16_26.xy, 0.0), 1.0);
#else
        u_xlat16_26.xy = clamp(u_xlat16_26.xy, 0.0, 1.0);
#endif
        u_xlat16_1 = key1Delta * u_xlat16_26.xxxx + key0;
        u_xlat16_1 = key2Delta * u_xlat16_26.yyyy + u_xlat16_1;
        u_xlat16_4.x = u_xlat16_4.x * u_xlat16_1.w + (-u_xlat16_4.x);
        u_xlat16_4.xyz = u_xlat16_0.xyz * u_xlat16_1.www + (-u_xlat16_4.xxx);
        u_xlat16_4.xyz = u_xlat16_1.xyz * u_xlat16_4.xyz;
        u_xlat16_4.xyz = u_xlat16_4.xyz * vec3(_CameraExposure);
        u_xlat16_2.xyz = min(u_xlat16_4.xyz, vec3(_aces_whitePoint_scalar));
        u_xlat16_11.xyz = _aces_pack.xxx * u_xlat16_2.xyz + _aces_pack.yyy;
        u_xlat16_11.xyz = u_xlat16_2.xyz * u_xlat16_11.xyz;
        u_xlat16_12.xyz = _aces_pack.zzz * u_xlat16_2.xyz + _aces_pack.www;
        u_xlat16_2.xyz = u_xlat16_2.xyz * u_xlat16_12.xyz + vec3(0.140000001, 0.140000001, 0.140000001);
        u_xlat16_2.xyz = u_xlat16_11.xyz / u_xlat16_2.xyz;
        u_xlat16_4.xyz = max(u_xlat16_2.xyz, vec3(9.99999975e-005, 9.99999975e-005, 9.99999975e-005));
        u_xlat16_4.xyz = log2(u_xlat16_4.xyz);
        u_xlat16_4.xyz = u_xlat16_4.xyz * vec3(0.45449999, 0.45449999, 0.45449999);
        u_xlat16_4.xyz = exp2(u_xlat16_4.xyz);
    } else {
        u_xlat16_4.xyz = u_xlat16_0.xyz;
    //ENDIF
    }
//----------------------------------------
    SV_Target0.xyz = u_xlat16_4.xyz;
    SV_Target0.w = u_xlat16_0.w;
    return;
}