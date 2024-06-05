#ifndef BILLY_PBR_BRDF_INCLUDED
#define BILLY_PBR_BRDF_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "BillyDebugCommonFunction.hlsl"
#include "BillyPBRInput.hlsl"
#include "BillyBRDF.hlsl"

    float4 Billy_PBS_Lighting (BillyBRDFData brdfData,float3 normal,float3 viewDir,float3 vertexNormal,Light light)
	{
       // return brdfData.ao;
        half shadow = max(light.shadowAttenuation,0.0);
        shadow = ApplyMicroShadow(brdfData.ao,normal,light.direction,shadow);
        float3 halfDir = normalize(light.direction+viewDir);
        float3 reflDir = reflect(-viewDir, normal);
        float NdotL = saturate(dot(normal, light.direction));
        float NdotH = saturate(dot(normal, halfDir));
        float HdotL = saturate(dot(halfDir,light.direction));
        float HdotV = saturate(dot(halfDir,viewDir));
        float absNdotV = max(abs(dot(normal,viewDir)),0.0001);
        float NdotV = saturate(dot(normal,viewDir));

        float D =   DistributionGGX(NdotH,brdfData.roughness);
        float G = GeometrySmith(absNdotV,NdotL,brdfData.roughness);
        float3 F = FresnelTerm(HdotL,brdfData.reflectivity);    
        float3 directspecular = D*G*F*PI*NdotL*light.color*shadow; 

        float3 kd = (1-F);
        float3 directdiffuse = brdfData.diffuse*kd*NdotL*light.color*shadow;

        float mip_roughness = (brdfData.perceptualRoughness * (1.7 - 0.7 * brdfData.perceptualRoughness))*UNITY_SPECCUBE_LOD_STEPS;
        brdfData.ao *= HorizonOcclusion(reflDir,normal,vertexNormal,_HorizonFade);
        half specularAO = computeSpecOcclusion(NdotV , brdfData.ao , brdfData.roughness);
        float4 encodedIrradiance  = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0,samplerunity_SpecCube0,reflDir,mip_roughness);
        float3 irradianceEnv = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
        irradianceEnv *= HorizonOcclusion(reflDir,normal,vertexNormal,_HorizonFade)*specularAO;   
        float3 irKs = fresnelSchlickRoughness(NdotV,brdfData.reflectivity,brdfData.roughness);
        float3 irKd = (1.0 - irKs);
        #ifdef _IBLBRDFMODE_UE4_BRDFAPPROX
                float2 iblSPecularBrdf = EnvBRDFApprox(brdfData.perceptualRoughness,NdotV);
                float3 indirectSpecular = irradianceEnv*(brdfData.reflectivity * iblSPecularBrdf.x + iblSPecularBrdf.y)*brdfData.ao;
        #else
                float2 envBRDF = SAMPLE_TEXTURE2D(_BRDFLUT, sampler_BRDFLUT,float2(NdotV,brdfData.perceptualRoughness)).rg;
                float3 indirectSpecular = irradianceEnv*(irKs *envBRDF.x+envBRDF.y)*brdfData.ao;
        #endif

        float3 indirectDiffuse = brdfData.diffuse*SampleSH(normal)*irKd*brdfData.ao;

        float3 col = 0.0.xxx;
        col = directdiffuse+ directspecular+indirectDiffuse+indirectSpecular;

        #if defined( _SHADOW_DISPLAYER)
        col = shadow;
        #endif

        #if defined( _DIRECTDIFFUSE_DISPLAYER)
        col =  directdiffuse;
        #endif 

        #if defined( _DIRECTSPECULAR_DISPLAYER )
        col = directspecular;
        #endif  

        #if defined( _INDIRECTDIFFUSE_DISPLAYER )
        col = indirectDiffuse;
        #endif  

        #if defined( _INDIRECTSPECULAR_DISPLAYER )
        col = indirectSpecular;
        #endif  

        return float4(col,1.0);
    }


    float3 Billy_PBS_AddLighting(BillyBRDFData brdfData,float3 normal,float3 viewDir,float3 vertexNormal,Light light)
    {
        half shadow = max(light.distanceAttenuation * light.shadowAttenuation,0.0);
        shadow = ApplyMicroShadow(brdfData.ao,normal,light.direction,shadow);
        float3 halfDir = normalize(light.direction+viewDir);
        float3 reflDir = reflect(-viewDir, normal);
        float NdotL = saturate(dot(normal, light.direction));
        float NdotH = saturate(dot(normal, halfDir));
        float HdotL = saturate(dot(halfDir,light.direction));
        float HdotV = saturate(dot(halfDir,viewDir));
        float absNdotV = max(abs(dot(normal,viewDir)),0.0001);
        float NdotV = saturate(dot(normal,viewDir));

        float D =   DistributionGGX(NdotH,brdfData.roughness);
        float G = GeometrySmith(absNdotV,NdotL,brdfData.roughness);
        float3 F = FresnelTerm(HdotL,brdfData.reflectivity);    
        float3 directspecular = D*G*F*PI*NdotL*light.color*shadow; 
        float3 kd = (1-F);
        float3 directdiffuse = brdfData.diffuse*kd*NdotL*light.color*shadow;

        float3 col = directdiffuse + directspecular;
        #if defined( _DIRECTDIFFUSE_DISPLAYER)
        col = directdiffuse;
        #endif 

        #if defined( _DIRECTSPECULAR_DISPLAYER )
        col = directspecular;
        #endif  

        return col;
    }

 #endif