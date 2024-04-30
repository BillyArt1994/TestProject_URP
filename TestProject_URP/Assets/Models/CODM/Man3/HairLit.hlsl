float Vis_Hair(float B, float Theta) {
	return exp(-0.5 * Square(Theta) / (B * B)) / (sqrt(2 * Pi) * B);
}

float F_Hair(float CosTheta) {
	const float n = 1.55;
	const float F0 = Square((1 - n) / (1 + n));
	return F0 + (1 - F0) * pow5(1 - CosTheta);
}

float3 HairLit(float3 L, float3 V, half3 N, float3 SpecularColor, float Specular, float Roughness,float Backlit, float Scatter, float Area, float Shadow) {
	Scatter = Scatter / 10;
	const float VoL       = dot(V,L);
	const float SinThetaL = dot(N,L);
	const float SinThetaV = dot(N,V);
	float CosThetaD = cos(0.5 * abs(asinFast( SinThetaV ) - asinFast( SinThetaL)));

	const float3 Lp = L - SinThetaL * N;
	const float3 Vp = V - SinThetaV * N;
	const float CosPhi = dot(Lp,Vp) * rsqrt(dot(Lp,Lp) * dot(Vp,Vp) + 1e-4);
	const float CosHalfPhi = sqrt(saturate(0.5 + 0.5 * CosPhi));

    float3 S = 0;
	float n = 1.55;
	float n_prime = 1.19 / CosThetaD + 0.36 * CosThetaD;
	float Shift = 0.035;

	float Alpha[3] = {
		-Shift * 2,
		Shift,
		Shift * 4,
	};

	float B[3] = {
		Area + Square(Roughness),
		Area + Square(Roughness) / 2,
		Area + Square(Roughness) * 2,
	};

	// R
	if(1) 
	{
		const float sa = sin(Alpha[0]);
		const float ca = cos(Alpha[0]);
		float Shift = 2*sa* (ca * CosHalfPhi * sqrt(1 - SinThetaV * SinThetaV) + sa * SinThetaV);
		float Mp = Vis_Hair(B[0] * sqrt(2.0) * CosHalfPhi, SinThetaL + SinThetaV - Shift);
		float Np = 0.25 * CosHalfPhi;
		float Fp = F_Hair(sqrt(saturate( 0.5 + 0.5 * VoL)));
		S += Specular * Mp * Np * Fp * lerp(1, Backlit, saturate(-VoL));
	}

	// TRT
	if(1) 
	{
		float Mp = Vis_Hair(B[2], SinThetaL + SinThetaV - Alpha[2]);
		float f = F_Hair(CosThetaD * 0.5);
		float Fp = Square(1 - f) * f;
		float3 Tp = pow(SpecularColor, 0.8 / CosThetaD);
		float Np = exp(17 * CosPhi - 16.78);
		S += Mp * Np * Fp * Tp;
	}

	// TT
	if(1) 
	{
		float Mp = Vis_Hair(B[1], SinThetaL + SinThetaV - Alpha[1]);
		float a = 1 / n_prime;
		float h = CosHalfPhi * (1 + a * (0.6 - 0.8 * CosPhi));
		float f = F_Hair(CosThetaD * sqrt(saturate( 1 - h*h)));
		float Fp = Square(1 - f);
		float3 Tp = pow(SpecularColor, 0.5 * sqrt(1 - Square(h * a)) / CosThetaD);
		float Np = exp(-3.65 * CosPhi - 3.98);
		S += Mp * Np * Fp * Tp * Backlit;
	}
	
    // Scatter
	if(1) 
	{
		float KajiyaDiffuse = 1 - abs(dot(N, L));

		float3 FakeNormal = normalize(V - N * dot(V, N));
		N = FakeNormal;

		float Wrap = 1;
		float NoL = saturate((dot(N, L) + Wrap) / Square(1 + Wrap));
		float DiffuseScatter = Inv_Pi * lerp(NoL, KajiyaDiffuse, 0.33) * Scatter;
		float Luma = Luminance(SpecularColor);
		float3 ScatterTint = pow(SpecularColor / Luma, 1 - Shadow);
		S += sqrt(SpecularColor) * DiffuseScatter * ScatterTint;
	}
	S = -min(-S, 0);
	return S;
}