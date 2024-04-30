#ifndef BILLY_BRDF_INCLUDED
#define BILLY_BRDF_INCLUDED

#include "Math.hlsl"

struct BillyBRDFData
{
    half3 albedo;
    half3 diffuse;
    half3 reflectivity;
    half perceptualRoughness;
    half roughness;
    half emissive;
    half ao;
};

inline void InitializeBRDFData(float3 albedo , float ao,float roughness,float metallic,float reflectance ,float emissive,out BillyBRDFData outBRDFData)
{
    outBRDFData.albedo = albedo;
    half dielectricSpec = 0.16*reflectance*reflectance;
    outBRDFData.diffuse = albedo*(1-metallic);
    outBRDFData.perceptualRoughness = roughness;
    outBRDFData.roughness = max(outBRDFData.perceptualRoughness*outBRDFData.perceptualRoughness,0.0078125);
    outBRDFData.reflectivity = lerp(dielectricSpec,albedo,metallic);
    outBRDFData.emissive = emissive;
    outBRDFData.ao = ao;
}

inline half DistributionGGX(float NdotH, float roughness)
{
    float a2 = roughness * roughness;
    float NdotH2 = NdotH * NdotH;
    float nom = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;
    return nom / max(denom,0.000001);
}

inline float D_GGX_Anisotropic(float NoH, const float3 h,
    const float3 t, const float3 b, float at, float ab) {
    float ToH = dot(t, h);
    float BoH = dot(b, h);
    float a2 = at * ab;
    float3 v = float3(ab * ToH, at * BoH, a2 * NoH);
    float v2 = dot(v, v);
    float w2 = a2 / v2;
    return a2 * w2 * w2 * (1.0 / PI);
}

inline float GeometrySmith(float NdotV,float NdotL ,float roughness)
{
    float k = roughness;
    float G_SmithL = NdotL * (1.0 - k) + k;
    float G_SmithV = NdotV * (1.0 - k) + k;
    float G = 0.5 / ( G_SmithL * G_SmithV );
    return G;
}

inline half3 FresnelTerm (half cosA,half3 F0 )
{
    half t = pow (1.0 - cosA,5.0);   // ala Schlick interpoliation
    return F0 + (1.0-F0) * t;
}

inline half3 FresnelSchlickApprox (float HdotV, float3 F0)
{
    float3 fresnel = F0 +(1.0-F0)*exp2((-5.55473 * HdotV - 6.98316) * HdotV);
    return fresnel;
}

inline float3 fresnelSchlickRoughness(float NdotV, float3 F0, float roughness)
{
    return F0 + (max(1.0 - roughness, F0) - F0) * pow(1.0 - NdotV, 5.0);
}

inline float  KS_Skin_Specular(float3 normal , float3 L,float3 V, float roughness,float rho_s,sampler2D beckmannTex)
{
    float result = 0.0;  
	float ndotl = dot( normal, L ); 
    if( ndotl > 0.0 ) {    
        float3 h = L + V; 
        // Unnormalized half-way vector    
        float3 H = normalize( h );    
        float ndoth = dot( normal, H );  
        float HdotV = dot(H,V);  
        float PH = pow( 2.0*tex2D(beckmannTex,float2(ndoth,roughness)), 10.0 );    
        float F = FresnelTerm( HdotV, 0.028 );    
        float frSpec = max( PH * F / dot( h, h ), 0 );    
        result = ndotl * rho_s * frSpec; 
        // BRDF * dot(N,L) * rho_s  
    }  
    return result;
}

half2 EnvBRDFApprox(half Roughness, half NoV)
{
        // [ Lazarov 2013, "Getting More Physical in Call of Duty: Black Ops II" ]
        // Adaptation to fit our G term.
        const half4 c0 = { -1, -0.0275, -0.572, 0.022 };
        const half4 c1 = { 1, 0.0425, 1.04, -0.04 };
        half4 r = Roughness * c0 + c1;
        half a004 = min(r.x * r.x, exp2(-9.28 * NoV)) * r.x + r.y;
        half2 AB = half2(-1.04, 1.04) * a004 + r.zw;
        return AB;
}

half HorizonOcclusion(half3 R, half3 normalWS, half3 vertexNormal, half horizonFade)
{
    //half3 R = reflect(-V, normalWS);
    half specularOcclusion = saturate(1.0 + horizonFade * dot(R, vertexNormal));
    // smooth it
    return specularOcclusion * specularOcclusion;
}

half computeSpecOcclusion ( half NdotV , half AO , half roughness )
{
    return saturate (pow( NdotV + AO , exp2 ( -16.0 * roughness - 1.0 )) - 1.0 + AO );
}

#endif