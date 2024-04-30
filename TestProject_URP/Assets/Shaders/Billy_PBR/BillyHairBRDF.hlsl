#ifndef BILLY_HAIR_BRDF_INCLUDED
#define BILLY_HAIR_BRDF_INCLUDED

#include "Math.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

float Hair_G(float B, float Theta)
{
	return exp(-0.5 * pow2(Theta) / (B * B)) / (sqrt(2 * PI) * B);
}

float Hair_F(float CosTheta)
{
	const float n = 1.55;
	const float F0 = pow2((1 - n) / (1 + n));
	return F0 + (1 - F0) * pow5(1 - CosTheta);
}

float3 KajiyaKayDiffuseAttenuation(float3 Albedo,float Scatter, float3 L, float3 V, half3 N, float Shadow)
{
	// Use soft Kajiya Kay diffuse attenuation
	float KajiyaDiffuse = 1 - abs(dot(N, L));

	float3 FakeNormal = normalize(V - N * dot(V, N));
	//N = normalize( DiffuseN + FakeNormal * 2 );
	N = FakeNormal;

	// Hack approximation for multiple scattering.
	float Wrap = 1;
	float NoL = saturate((dot(N, L) + Wrap) / pow2(1 + Wrap));
	float DiffuseScatter = (1 / PI) * lerp(NoL, KajiyaDiffuse, 0.33) * Scatter;
	float Luma = Luminance(Albedo);
	float3 ScatterTint = pow(Albedo / Luma, 1 - Shadow);
	return sqrt(Albedo) * DiffuseScatter * ScatterTint;
}

#endif