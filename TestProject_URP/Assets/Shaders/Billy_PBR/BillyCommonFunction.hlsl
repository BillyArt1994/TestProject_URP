#ifndef BILLY_COMMONFUNCTION
#define BILLY_COMMONFUNCTION

void DitherAlpha (float2 pixelPos,float cutout)
{
    float4x4 thresholdMatrix =
    {  1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
      13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
       4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
      16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
    };
    float4x4 _RowAccess = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
    //float2 pos = IN.screenPos.xy / IN.screenPos.w;
    //pos *= _ScreenParams.xy; // pixel position
    clip(cutout - thresholdMatrix[fmod(pixelPos.x, 4)] * _RowAccess[fmod(pixelPos.y, 4)]);
}

#endif