#ifndef BILLY_HAIR_INPUT_INCLUDED
#define BILLY_HAIR_INPUT_INCLUDED

float _EdgeSmooth;
float _Edge;
float _Wrap;
float4 _RootColor;
float _TipVariation;
float4 _TipColor;
float4 _SpecCol;
float4 _SpecularColor;
float4 _SecSpecCol;
float _Specular;
float _Roughness;
float _Backlit;
float _Area;
float _Cutoff;
float _CutoffTips;
float _Debug;
float _Scatter;
float _SpecularNoiseTiling;
float _SpecularNoiseIntensity;
float _NormalDistortTiling;
float _NormalDistortIntensity;
float _NormalSmoothness;
sampler2D _Albedo;      
sampler2D _AO_Root;     
sampler2D _TangentTex;

#endif