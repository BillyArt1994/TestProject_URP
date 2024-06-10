#ifndef MATH_INCLUDED
#define MATH_INCLUDED

#define PI 3.1415926
#define INV_PI 1.0/3.1415926

float pow2 (float x)
{
  return x*x;
}

float pow5 (float x)
{
    return x*x*x*x*x;
}

float Remap(float x ,float t1,float t2,float s1 ,float s2)
{
  return (x - t1) / (t2 - t1) * (s2 - s1) + s1;
}

float SmoothStep(float edge0, float edge1, float x)
{
    float t = clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
    return t * t * (3.0 - 2.0 * t);
}

#endif