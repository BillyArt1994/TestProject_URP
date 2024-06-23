#ifndef BILLY_HAIR_LIGHTING_INCLUDED
#define BILLY_HAIR_LIGHTING_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "BillyHairBRDF.hlsl"
#include "BillyHairInput.hlsl"

    float3 Billy_Hair_Lighting(float3 SpecularColor, float3 V,float3 N,float3 L,float t1,float Specular,float Roughness,float Backlit,float Area,float Scatter,float ao,Light light)
    {

        half Shadow = max(light.shadowAttenuation,0.0);
        //Shadow = min(Shadow,ao*5);
        float3 reflDir = reflect(-V, N);
        float3 directdiffuse = 0.0.xxx;
        float3 directspecular= 0.0.xxx;
        const float VoL       = dot(V,L);
        const float SinThetaL = dot(N,L);
        const float SinThetaV = dot(N,V);


        float CosThetaD = cos(0.5 * abs(asin( SinThetaV ) - asin( SinThetaL)));
    
        const float3 Lp = L - SinThetaL * N;
        const float3 Vp = V - SinThetaV * N;
        const float CosPhi = dot(Lp,Vp) * rsqrt(dot(Lp,Lp) * dot(Vp,Vp) + 1e-4);
        const float CosHalfPhi =  sqrt(saturate(0.5 + 0.5 * CosPhi));

        float3 S = 0;
        float n = 1.55;
        float n_prime = 1.19 / CosThetaD + 0.36 * CosThetaD;
        float Shift = 0.035;

        float Alpha[3] = {
            -Shift * 2+t1,
            Shift+t1,
            Shift * 4+t1,
        };

        float B[3] = {
            Area + pow2(Roughness),
            Area + pow2(Roughness) / 2,
            Area + pow2(Roughness) * 2,
        };
        #ifdef R_ON
        // R
        {
            const float sa = sin(Alpha[0]);
            const float ca = cos(Alpha[0]);
            float Shift = 2*sa* (ca * CosHalfPhi * saturate(sqrt(1 - SinThetaV * SinThetaV) ) + sa * SinThetaV);
            float Mp = Hair_G(B[0] * sqrt(2.0) * CosHalfPhi, SinThetaL + SinThetaV - Shift);
            float Np = 0.25 * CosHalfPhi;
            float Fp = Hair_F(sqrt(saturate( 0.5 + 0.5 * VoL)));
            directspecular += Specular * Mp * Np * Fp *lerp(1,Backlit,saturate(-VoL))*_SpecCol;
            
        }
        #endif

        #ifdef TRT_ON
        // TRT
        if(1) 
        {
            float Mp = Hair_G(B[2], SinThetaL + SinThetaV - Alpha[2]);
            float f = Hair_F(CosThetaD * 0.5);
            float Fp = pow2(1 - f) * f;
            float3 Tp = pow(_SecSpecCol, 0.8 / CosThetaD);
            float Np = exp(17 * CosPhi - 16.78);
            directspecular += Mp * Np * Fp * Tp;
           
        }
        #endif

        #ifdef TT_ON
        //TT
        if(1) 
        {
            float Mp = Hair_G(B[1], SinThetaL + SinThetaV - Alpha[1]);
            float a = 1 / n_prime;
            float h = CosHalfPhi * (1 + a * (0.6 - 0.8 * CosPhi));
            float f = Hair_F(CosThetaD * sqrt(saturate( 1 - h*h)));
            float Fp = pow2(1 - f);
            float3 Tp = pow(SpecularColor, 0.5 * sqrt(1 - pow2(h * a)) / CosThetaD);
            float Np = exp(-3.65 * CosPhi - 3.98);
            directspecular += Mp * Np * Fp * Tp * Backlit;
           // return Mp * Np * Fp * Tp * Backlit;

        }
        #endif

        #ifdef SCATTER_ON
        if(1) 
        {
            float KajiyaDiffuse = 1 - abs(dot(N, L));
            float3 FakeNormal = normalize(V - N * dot(V, N));
            N = FakeNormal;
            float Wrap = _Wrap;
            float NoL = saturate((dot(N, L) + Wrap) / pow2(1 + Wrap));

            float DiffuseScatter =  lerp(NoL, KajiyaDiffuse, 0.33) * Scatter;
            float Luma = Luminance(SpecularColor);
            float3 ScatterTint = pow(SpecularColor / Luma, 1-Shadow);
            directdiffuse = sqrt(SpecularColor) * DiffuseScatter * ScatterTint;
          // return directdiffuse;
        }
        #endif


        float3 col = 0.0.xxx;
        directspecular = directspecular*light.color* Shadow;
        directdiffuse = directdiffuse*light.color* Shadow;
        col = (directdiffuse+ directspecular);

        //return directdiffuse;

        float mip_roughness = (Roughness * (1.7 - 0.7 * Roughness))*UNITY_SPECCUBE_LOD_STEPS;
        float4 encodedIrradiance  = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0,samplerunity_SpecCube0,reflDir,mip_roughness);
        float3 irradianceEnv = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
        float3 indirectDiffuse = SpecularColor*(SampleSH(N)) * ao;
        float3 indirectSpecular = irradianceEnv*(SpecularColor*0.4524-0.0024) *ao;
        col  += indirectSpecular +indirectDiffuse;

        #ifdef _DIRECTDIFFUSE_DISPLAYER
        col = directdiffuse;
        #endif 

        #ifdef _DIRECTSPECULAR_DISPLAYER 
        col = directspecular;
        #endif  
        #if defined( _INDIRECTDIFFUSE_DISPLAYER)
        col = indirectDiffuse;
        #endif  

        #if defined( _INDIRECTSPECULAR_DISPLAYER )
        col = indirectSpecular;
        #endif  

        #if defined( _SHADOW_DISPLAYER)
        col = Shadow;
        #endif

        return col;
    }

    float3 Billy_Hair_AddLighting (float3 SpecularColor, float3 V,float3 N,float3 L,float t1,float Specular,float Roughness,float Backlit,float Area,float Scatter,float ao,Light light)
    {
        half Shadow = max(light.distanceAttenuation * light.shadowAttenuation,0.0);
        float3 directdiffuse = 0.0.xxx;
        float3 directspecular= 0.0.xxx;
        const float VoL       = dot(V,L);
        const float SinThetaL = dot(N,L);
        const float SinThetaV = dot(N,V);


        float CosThetaD = cos(0.5 * abs(asin( SinThetaV ) - asin( SinThetaL)));
    
        const float3 Lp = L - SinThetaL * N;
        const float3 Vp = V - SinThetaV * N;
        const float CosPhi = dot(Lp,Vp) * rsqrt(dot(Lp,Lp) * dot(Vp,Vp) + 1e-4);
        const float CosHalfPhi =  sqrt(saturate(0.5 + 0.5 * CosPhi));

        float3 S = 0;
        float n = 1.55;
        float n_prime = 1.19 / CosThetaD + 0.36 * CosThetaD;
        float Shift = 0.035;

        float Alpha[3] = {
            -Shift * 2+t1,
            Shift+t1,
            Shift * 4+t1,
        };

        float B[3] = {
            Area + pow2(Roughness),
            Area + pow2(Roughness) / 2,
            Area + pow2(Roughness) * 2,
        };
        #ifdef R_ON
        // R
        {
            const float sa = sin(Alpha[0]);
            const float ca = cos(Alpha[0]);
            float Shift = 2*sa* (ca * CosHalfPhi * saturate(sqrt(1 - SinThetaV * SinThetaV) ) + sa * SinThetaV);
            float Mp = Hair_G(B[0] * sqrt(2.0) * CosHalfPhi, SinThetaL + SinThetaV - Shift);
            float Np = 0.25 * CosHalfPhi;
            float Fp = Hair_F(sqrt(saturate( 0.5 + 0.5 * VoL)));
            directspecular += Specular * Mp * Np * Fp *lerp(1,Backlit,saturate(-VoL))*_SpecCol;
            //return  CosHalfPhi;
        }
        #endif

        #ifdef TRT_ON
        // TRT
        if(1) 
        {
            float Mp = Hair_G(B[2], SinThetaL + SinThetaV - Alpha[2]);
            float f = Hair_F(CosThetaD * 0.5);
            float Fp = pow2(1 - f) * f;
            float3 Tp = pow(_SecSpecCol, 0.8 / CosThetaD);
            float Np = exp(17 * CosPhi - 16.78);
            directspecular += Mp * Np * Fp * Tp;
            //return float4(Np.xxx,albedo.a);
        }
        #endif

        #ifdef TT_ON
        //TT
        if(1) 
        {
            float Mp = Hair_G(B[1], SinThetaL + SinThetaV - Alpha[1]);
            float a = 1 / n_prime;
            float h = CosHalfPhi * (1 + a * (0.6 - 0.8 * CosPhi));
            float f = Hair_F(CosThetaD * sqrt(saturate( 1 - h*h)));
            float Fp = pow2(1 - f);
            float3 Tp = pow(_SecSpecCol, 0.5 * sqrt(1 - pow2(h * a)) / CosThetaD);
            float Np = exp(-3.65 * CosPhi - 3.98);
            directspecular += Mp * Np * Fp * Tp * Backlit;
        }
        #endif

        #ifdef SCATTER_ON
        if(1) 
        {
            float KajiyaDiffuse = 1 - abs(dot(N, L));
            float3 FakeNormal = normalize(V - N * dot(V, N));
            N = FakeNormal;
            float Wrap = _Wrap;
            float NoL = saturate((dot(N, L) + Wrap) / pow2(1 + Wrap));

            float DiffuseScatter =  lerp(NoL, KajiyaDiffuse, 0.33) * Scatter;
            float Luma = Luminance(SpecularColor);
            float3 ScatterTint = pow(SpecularColor / Luma, 1-Shadow);
            directdiffuse = sqrt(SpecularColor) * DiffuseScatter * ScatterTint;         
        }
        #endif


        float3 col = 0.0.xxx;
        col += directdiffuse + directspecular;
        col *= light.color * Shadow;

        #if defined( _SHADOW_DISPLAYER)
        col = Shadow;
        #endif


        return col;
    }

#endif