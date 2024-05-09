#ifndef BILLY_SKIN_BRDF_INCLUDED
#define BILLY_SKIN_BRDF_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "BillyBRDF.hlsl"
#include "BillySkinInput.hlsl"


    float3 Billy_Skin_Lighting(BillyBRDFData brdfData,float3 normal,float3 viewDir,float curvature,float thickness,float3 vertexNormal,Light light)
    {
        half shadow = max(light.shadowAttenuation,0.0);
        return thickness;

        float3 halfDir = normalize(light.direction+viewDir);
        float3 reflDir = reflect(-viewDir, normal);
        float NdotLRaw = dot(normal,light.direction);
        float NdotVRaw = dot(normal,viewDir);
        float NdotL = saturate(dot(normal, light.direction));
        float NdotH = saturate(dot(normal, halfDir));
        float HdotL = saturate(dot(halfDir,light.direction));
        float absNdotV = max(abs(dot(normal,viewDir)),0.0001);
        float NdotV = saturate(dot(normal,viewDir));

        float G = GeometrySmith(absNdotV,NdotL,brdfData.roughness);
        float3 F = FresnelTerm(HdotL,brdfData.reflectivity);    
        float D1 = DistributionGGX(NdotH,brdfData.roughness);
        float D2 = DistributionGGX(NdotH,_SkinSecondSpecScale);
        
        float3 N_low = vertexNormal;
        float NdotL_low = dot(N_low, light.direction);
        float curvatureScaled = curvature * _CurvatureScaleBias.x +_CurvatureScaleBias.y;



        half3 normalSmoothFactor = saturate(1.0-vertexNormal);
        normalSmoothFactor *= normalSmoothFactor;
        half3 normalShadeG = normalize(lerp(normal,N_low,0.3+0.7*normalSmoothFactor));
        half3 normalShadeB = normalize(lerp(normal,N_low,normalSmoothFactor));
        half NdotLShadeG = saturate(dot(normalShadeG,light.direction));
        half NdotLShadeB = saturate(dot(normalShadeB,light.direction));
        half3 rgbNdotL = half3(saturate(NdotL_low) ,NdotLShadeG,NdotLShadeB);

        half3 diffuseBrdf = SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,half2(NdotL_low*0.5+0.5,curvatureScaled));
        diffuseBrdf += rgbNdotL;
        //float3 BlurFactor = saturate(1.0f - NdotL_low);
        //BlurFactor *= BlurFactor;
//
        //float3 gN = lerp(normal, N_low, 0.3f + 0.7f*BlurFactor);
        //float3 bN = lerp(normal, N_low, BlurFactor);
        //float3 rgbNdotL = float3( NdotL_low, dot(gN, light.direction), dot(bN, light.direction) );
        //
        //float3 LutU= (rgbNdotL*0.5+0.5);
//
        //float3 diffuseBrdf;
        //diffuseBrdf.r =SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,float2(LutU.r,curvature*(dot(light.color,float3(0.22,0.707,0.071)))*_SkinToneScale)).r;
        //diffuseBrdf.g =SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,float2(LutU.g,curvature*(dot(light.color,float3(0.22,0.707,0.071)))*_SkinToneScale)).g;
        //diffuseBrdf.b =SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,float2(LutU.b,curvature*(dot(light.color,float3(0.22,0.707,0.071)))*_SkinToneScale)).b;
//
        //float backContrib = lerp(saturate(-NdotLRaw),saturate(-NdotVRaw),_TranslucencyViewDependency);
        //float transMap = smoothstep(_TranslucencyThreshold, 1,thickness);
        //half3 translucencyColor = backContrib * _TranslucencyScale * _TranslucencyColor*transMap;
//
        //float transmittance = exp2(_DeepScatterFalloff * thickness * thickness);
	    //float minusNDotL = -dot(NdotL_low, light.direction);
        //transmittance *= saturate(minusNDotL + 0.3);
        
        half3 rgbShadow = SAMPLE_TEXTURE2D(_ShadowBrdfLUT,sampler_ShadowBrdfLUT,half2(shadow,NdotL_low*_ShadowScaleBias.x+_ShadowScaleBias.y));
        float3 kd = (1-F);

        float3 directdiffuse = brdfData.diffuse*rgbShadow*kd*diffuseBrdf*light.distanceAttenuation*light.color;

        float3 directspecular = (D1*1.5+D2*0.5)*F*G*PI*NdotL*light.distanceAttenuation *light.color*rgbShadow;

        float mip_roughness = (brdfData.perceptualRoughness * (1.7 - 0.7 * brdfData.perceptualRoughness))*UNITY_SPECCUBE_LOD_STEPS;
        brdfData.ao *= HorizonOcclusion(reflDir,normal,vertexNormal,_HorizonFade);
        half specularAO = computeSpecOcclusion(NdotV , brdfData.ao , brdfData.roughness);
        float4 encodedIrradiance  = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0,samplerunity_SpecCube0,reflDir,mip_roughness);
        float3 irradianceEnv = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
        irradianceEnv *= HorizonOcclusion(reflDir,normal,vertexNormal,_HorizonFade)*specularAO;   
        float3 irKs = fresnelSchlickRoughness(NdotV,brdfData.reflectivity,brdfData.roughness);
        //return irKs;
        float3 irKd = (1.0 - irKs);

        #ifdef _IBLBRDFMODE_UE4_BRDFAPPROX
                float2 iblSPecularBrdf = EnvBRDFApprox(brdfData.perceptualRoughness,NdotV);
                float3 indirectSpecular = irradianceEnv*(brdfData.reflectivity * iblSPecularBrdf.x + iblSPecularBrdf.y)*brdfData.ao;
        #else
                float2 envBRDF = SAMPLE_TEXTURE2D(_BRDFLUT, sampler_BRDFLUT,float2(NdotV,brdfData.perceptualRoughness)).rg;
                float3 indirectSpecular = irradianceEnv*(brdfData.reflectivity*envBRDF.x+envBRDF.y)*brdfData.ao;
        #endif

        float3 indirectDiffuse;
        float3 ambientMN = normalize(lerp(normal,N_low,0.3)) ;
        indirectDiffuse = brdfData.diffuse*(SampleSH(normal))*brdfData.ao;
        indirectDiffuse.r = (brdfData.diffuse*(SampleSH(N_low))*brdfData.ao).r;
        indirectDiffuse.g = (brdfData.diffuse*(SampleSH(ambientMN))*brdfData.ao).g;
        indirectDiffuse.b = (brdfData.diffuse*(SampleSH(normal))*brdfData.ao).b;

        float3 col = 0.0.xxx;
        #ifdef DIRECTDIFFUSE
        col += directdiffuse;
        #endif 
        #ifdef DIRECTSPECULAR 
        col += directspecular;
        #endif  
        #ifdef INDIRECTDIFFUSE 
        col += indirectDiffuse;
        #endif  
        #ifdef INDIRECTSPECULAR 
        col += indirectSpecular;
        #endif  
        return col;
    }

    float3 Billy_Skin_AddLighting(BillyBRDFData brdfData,float3 normal,float3 viewDir,float curvature,float thickness,float3 vertexNormal,Light light)
    {
        half shadow = max(  light.shadowAttenuation,0.0);
        float3 halfDir = normalize(light.direction+viewDir);
        float3 reflDir = reflect(-viewDir, normal);
        float NdotLRaw = dot(normal,light.direction);
        float NdotVRaw = dot(normal,viewDir);
        float NdotL = saturate(dot(normal, light.direction));
        float NdotH = saturate(dot(normal, halfDir));
        float HdotL = saturate(dot(halfDir,light.direction));
        float absNdotV = max(abs(dot(normal,viewDir)),0.0001);
        float NdotV = saturate(dot(normal,viewDir));

        float G = GeometrySmith(absNdotV,NdotL,brdfData.roughness);
        float3 F = FresnelTerm(HdotL,brdfData.reflectivity);    
        float D1 = DistributionGGX(NdotH,brdfData.roughness);
        float D2 = DistributionGGX(NdotH,_SkinSecondSpecScale);
        
        float3 N_low = vertexNormal;
        float NdotL_low = saturate(dot(N_low, light.direction));
        float3 BlurFactor = saturate(1.0f - NdotL_low);
        BlurFactor *= BlurFactor;

        float3 gN = lerp(normal, N_low, 0.3f + 0.7f*BlurFactor);
        float3 bN = lerp(normal, N_low, BlurFactor);
        float3 rgbNdotL = float3( NdotL_low, saturate(dot(gN, light.direction)) , saturate(dot(bN, light.direction)) );
        
        float3 LutU= (rgbNdotL*0.5+0.5);


        float3 diffuseBrdf;
        diffuseBrdf.r =SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,float2(LutU.r*shadow,curvature*(dot(light.color,float3(0.22,0.707,0.071)))*_SkinToneScale)).r;
        diffuseBrdf.g =SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,float2(LutU.g*shadow,curvature*(dot(light.color,float3(0.22,0.707,0.071)))*_SkinToneScale)).g;
        diffuseBrdf.b =SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,float2(LutU.b*shadow,curvature*(dot(light.color,float3(0.22,0.707,0.071)))*_SkinToneScale)).b;
        //return diffuseBrdf.xyzz;
        //float3 rgbCur = SAMPLE_TEXTURE2D(_SkinBrdfLUT,sampler_SkinBrdfLUT,float2(NdotL_low,curvature)).rgb*0.5-0.25;
        //diffuseBrdf = saturate(rgbCur+rgbNdotL) ;

        //float3 rgbShadow = SAMPLE_TEXTURE2D(_ShadowBrdfLUT,sampler_ShadowBrdfLUT,float2(shadow,NdotL_low));
        //diffuseBrdf *= rgbShadow;

        //float transmittance = exp2(thickness*thickness);//exp2(0.01*thickness*thickness);
        //float minusNDotL =  -dot(N_low, light.direction);
        //float deepScatterFactor = transmittance*saturate(minusNDotL+0.3)*_TranslucencyScale;
       // return transmittance.xxxx;

        float backContrib = lerp(saturate(-NdotLRaw),saturate(-NdotVRaw),_TranslucencyViewDependency);
        float transMap = smoothstep(_TranslucencyThreshold, 1,thickness);
        half3 translucencyColor = backContrib * _TranslucencyScale * _TranslucencyColor*transMap;
       // return translucencyColor.xyzz;

        float3 kd = (1-F);
        //float3 deepScatter= deepScatterFactor*(brdfData.diffuse*kd*light.distanceAttenuation *light.color);
        float3 directdiffuse =  brdfData.diffuse*kd*diffuseBrdf*light.distanceAttenuation *light.color*shadow +translucencyColor;
        float3 directspecular = (D1*1.5+D2*0.5)*F*G*PI*NdotL*light.color*light.distanceAttenuation*shadow;


        //return thickness;


       // float backContrib = lerp(saturate(-NdotLRaw),saturate(-NdotVRaw),_TranslucencyViewDependency);
       // float transMap = smoothstep(_TranslucencyThreshold, 1,thickness);
       // float transmittance = exp2(thickness*thickness);
       // float minusNDotL =  -dot(N_low, light.direction);
//
       // 
       // //half3 translucencyColor = backContrib * _TranslucencyScale * _TranslucencyColor*transMap;
       // half3 translucencyColor = transMap* ;
       // return translucencyColor.xyzz;

        float3 col = 0.0.xxx;
        #ifdef DIRECTDIFFUSE
        col += directdiffuse;
        #endif 
        #ifdef DIRECTSPECULAR 
        col += directspecular;
        #endif  
        return col;
    }

#endif