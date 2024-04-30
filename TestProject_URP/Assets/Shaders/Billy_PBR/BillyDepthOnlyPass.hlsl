#ifndef BILLY_DEPTHONLY_PASS
#define BILLY_DEPTHONLY_PASS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

struct DepthOnlyAttributes
{
	float4 positionOS : POSITION;
	half3 normalOS : NORMAL;

};

struct DepthOnlyVaryings
{
	float4 positionCS : SV_POSITION;
	float4 posSS : TEXCOORD2;
};

DepthOnlyVaryings DepthOnlyVertex(DepthOnlyAttributes i)
{
	DepthOnlyVaryings o = (DepthOnlyVaryings)0;
	o.positionCS = TransformObjectToHClip(i.positionOS);
	return o;
}

half4 DepthOnlyFragment (DepthOnlyVaryings i) : SV_TARGET
{
	return i.positionCS.zzzz;
}

#endif