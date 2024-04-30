using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Rendering.Universal
{
    public sealed class SubstancePainterLookup : VolumeComponent, IPostProcessComponent
    {
        [Tooltip("Manually registration your substance lut as sRGBf here.")]
        public TextureParameter lookuptexture = new TextureParameter(null);

        [Tooltip("Manually registration your substance Out lut here.")]
        public TextureParameter lookuptextureOut = new TextureParameter(null);

        public bool IsActive()
        {
            var defaultState = new Texture2D(1024, 128, DefaultFormat.HDR, 0);
            return lookuptexture != defaultState;
        }

        public bool IsTileCompatible() => false;

    }
}