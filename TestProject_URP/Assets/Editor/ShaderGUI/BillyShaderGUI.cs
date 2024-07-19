using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace CommonShaderGUI
{
    public class BillyShaderGUI : ShaderGUI
    {
        protected MaterialEditor m_editor;
        protected Material m_materials;

        [Flags]
        protected enum BillyShaderExpandable
        {
            BaseInputs = 1 << 0,
            SurfaceInputs = 1 << 1,
            SurfaceOptions = 1 << 2,
            DebugOptions = 1 << 3,
        };

        protected readonly MaterialHeaderScopeList m_materialScopeList = new MaterialHeaderScopeList(uint.MaxValue);

        BillyShaderExpandable m_filter;
        uint materialFilter => uint.MaxValue;

        public bool m_FirstTimeApply = true;

        #region Variables
        protected MaterialProperty albdeo { get; set; }
        protected MaterialProperty tint { get; set; }

        protected MaterialProperty mraeMapEnabled { get; set; }
        protected MaterialProperty MAREMap { get; set; }
        protected MaterialProperty metalic { get; set; }
        protected MaterialProperty reflectivity { get; set; }
        protected MaterialProperty roughness { get; set; }
        protected MaterialProperty ao { get; set; }
        protected MaterialProperty normalMapEnabled { get; set; }
        protected MaterialProperty normal { get; set; }
        protected MaterialProperty anisotropic { get; set; }
        protected MaterialProperty horizonFade { get; set; }
        protected MaterialProperty emissionColor { get; set; }
        protected MaterialProperty bentNormalEnabled { get; set; }
        protected MaterialProperty bentNormal { get; set; }
        protected MaterialProperty brdfLut { get; set; }
        protected MaterialProperty cutoff { get; set; }
        protected MaterialProperty ditherEnabled { get; set; }
        protected MaterialProperty alphaClipEnabled { get; set; }
        protected MaterialProperty cullMode { get; set; }
        protected MaterialProperty show_directDiffuse { get; set; }
        protected MaterialProperty show_directSpecular { get; set; }
        protected MaterialProperty show_indirectDiffuse { get; set; }
        protected MaterialProperty show_indirectSpecular { get; set; }
        protected MaterialProperty IBL_BrdfMode { get; set; }
        protected MaterialProperty CheckValue { get; set; }
        protected MaterialProperty ChkTargetValue { get; set; }
        protected MaterialProperty ChkTargetScale { get; set; }
        protected MaterialProperty ChkRange { get; set; }
        #endregion

        static class BillyShaderBaseProperties
        {
            public static readonly string _Albedo = nameof(_Albedo);
            public static readonly string _Tint = nameof(_Tint);
            public static readonly string _MRAEMapEnabled = nameof(_MRAEMapEnabled);
            public static readonly string _NORMALMapEnabled = nameof(_NORMALMapEnabled);
            public static readonly string _MRAEMap = nameof(_MRAEMap);
            public static readonly string _Metalic = nameof(_Metalic);
            public static readonly string _Reflectivity = nameof(_Reflectivity);
            public static readonly string _Roughness = nameof(_Roughness);
            public static readonly string _AO = nameof(_AO);
            public static readonly string _BentnormalEnabled = nameof(_BentnormalEnabled);
            public static readonly string _Bentnormal = nameof(_Bentnormal);
            public static readonly string _EmissionColor = nameof(_EmissionColor);
            public static readonly string _HorizonFade = nameof(_HorizonFade);
            public static readonly string _NormalTex = nameof(_NormalTex);
            public static readonly string _BRDFLUT = nameof(_BRDFLUT);

            public static readonly string _AlphaClipEnabled = nameof(_AlphaClipEnabled);
            public static readonly string _DitherEnabled = nameof(_DitherEnabled);
            public static readonly string _CullMode = nameof(_CullMode);
            public static readonly string _Cutoff = nameof(_Cutoff);
            public static readonly string _DirectDiffuse = nameof(_DirectDiffuse);
            public static readonly string _DirectSpecular = nameof(_DirectSpecular);
            public static readonly string _IndirectDiffuse = nameof(_IndirectDiffuse);
            public static readonly string _IndirectSpecular = nameof(_IndirectSpecular);
            public static readonly string _IBLBrdfMode = nameof(_IBLBrdfMode);
            public static readonly string _Anisotropic = nameof(_Anisotropic);
            public static readonly string _CheckValue = nameof(_CheckValue);
            public static readonly string _ChkTargetValue = nameof(_ChkTargetValue);
            public static readonly string _ChkTargetScale = nameof(_ChkTargetScale);
            public static readonly string _ChkRange = nameof(_ChkRange);
        }

        public virtual void FindProperties(MaterialProperty[] props)
        {
            albdeo = FindProperty(BillyShaderBaseProperties._Albedo, props, false);
            MAREMap = FindProperty(BillyShaderBaseProperties._MRAEMap, props, false);
            bentNormal = FindProperty(BillyShaderBaseProperties._Bentnormal, props, false);
            tint = FindProperty(BillyShaderBaseProperties._Tint, props, false);
            mraeMapEnabled = FindProperty(BillyShaderBaseProperties._MRAEMapEnabled, props, false);
            normalMapEnabled = FindProperty(BillyShaderBaseProperties._NORMALMapEnabled, props, false);
            bentNormalEnabled = FindProperty(BillyShaderBaseProperties._BentnormalEnabled, props, false);
            metalic = FindProperty(BillyShaderBaseProperties._Metalic, props, false);
            reflectivity = FindProperty(BillyShaderBaseProperties._Reflectivity, props, false);
            roughness = FindProperty(BillyShaderBaseProperties._Roughness, props, false);
            ao = FindProperty(BillyShaderBaseProperties._AO, props, false);
            emissionColor = FindProperty(BillyShaderBaseProperties._EmissionColor, props, false);
            horizonFade = FindProperty(BillyShaderBaseProperties._HorizonFade, props, false);
            normal = FindProperty(BillyShaderBaseProperties._NormalTex, props, false);
            brdfLut = FindProperty(BillyShaderBaseProperties._BRDFLUT, props, false);
            alphaClipEnabled = FindProperty(BillyShaderBaseProperties._AlphaClipEnabled, props, false);
            ditherEnabled = FindProperty(BillyShaderBaseProperties._DitherEnabled, props, false);
            cullMode = FindProperty(BillyShaderBaseProperties._CullMode, props, false);
            cutoff = FindProperty(BillyShaderBaseProperties._Cutoff, props, false);
            show_directDiffuse = FindProperty(BillyShaderBaseProperties._DirectDiffuse, props, false);
            show_directSpecular = FindProperty(BillyShaderBaseProperties._DirectSpecular, props, false);
            show_indirectDiffuse = FindProperty(BillyShaderBaseProperties._IndirectDiffuse, props, false);
            show_indirectSpecular = FindProperty(BillyShaderBaseProperties._IndirectSpecular, props, false);
            IBL_BrdfMode = FindProperty(BillyShaderBaseProperties._IBLBrdfMode, props, false);
            anisotropic = FindProperty(BillyShaderBaseProperties._Anisotropic, props, false);
            //CheckValue = FindProperty(BillyShaderBaseProperties._CheckValue, props, false);
            //ChkTargetValue = FindProperty(BillyShaderBaseProperties._ChkTargetValue, props, false);
            //ChkTargetScale = FindProperty(BillyShaderBaseProperties._ChkTargetScale, props, false);
            //ChkRange = FindProperty(BillyShaderBaseProperties._ChkRange, props, false);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            m_editor = materialEditor;
            m_materials = materialEditor.target as Material;
            FindProperties(properties);
            //OnOpenGUI();
            if (m_FirstTimeApply)
            {
                OnOpenGUI();
                SetDefaultTexture();
                m_FirstTimeApply = false;
            }
            ShaderPropertiesGUI();
        }

        public virtual void OnOpenGUI()
        {
            m_filter = (BillyShaderExpandable)(materialFilter);

            if (m_filter.HasFlag(BillyShaderExpandable.BaseInputs))
                m_materialScopeList.RegisterHeaderScope(EditorGUIUtility.TrTextContent("Base Input"), (uint)BillyShaderExpandable.BaseInputs, DrawBaseProperties);

            if (m_filter.HasFlag(BillyShaderExpandable.SurfaceOptions))
                m_materialScopeList.RegisterHeaderScope(EditorGUIUtility.TrTextContent("Surface Options"), (uint)BillyShaderExpandable.SurfaceOptions, DrawSurfaceOptions);

            if (m_filter.HasFlag(BillyShaderExpandable.SurfaceInputs))
                m_materialScopeList.RegisterHeaderScope(EditorGUIUtility.TrTextContent("Surface Input"), (uint)BillyShaderExpandable.SurfaceInputs, DrawSurfacePropertise);
            
            if (m_filter.HasFlag(BillyShaderExpandable.DebugOptions))
                m_materialScopeList.RegisterHeaderScope(EditorGUIUtility.TrTextContent("Debug Options Propertise"), (uint)BillyShaderExpandable.DebugOptions, DrawDebugOptionsPropertise);
        }

        public void ShaderPropertiesGUI()
        {
            m_materialScopeList.DrawHeaders(m_editor, m_materials);

        }
        public void DrawBaseProperties(Material material)
        {
            EditorGUILayout.BeginVertical("Box");
            GUIContent albedoLabel = new GUIContent(albdeo.displayName);
            m_editor.TexturePropertySingleLine(albedoLabel, albdeo, tint);
            m_editor.TextureScaleOffsetProperty(albdeo);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        public void DrawSurfaceOptions(Material material)
        {
            EditorGUILayout.BeginVertical("Box");
            m_editor.ShaderProperty(cullMode, "Cull Mode");
            m_editor.ShaderProperty(alphaClipEnabled, "Alpha Clip");
            if (material.GetFloat("_AlphaClipEnabled") != 0)
            {
                m_editor.ShaderProperty(ditherEnabled, "Dither Transparency");
                m_editor.ShaderProperty(cutoff, "Cut off");
                
            }
            EditorGUILayout.EndVertical();
        }

        public virtual void DrawSurfacePropertise(Material material)
        {
            EditorGUILayout.BeginVertical("Box");
            m_editor.ShaderProperty(normalMapEnabled, "Normal Map Enabled");
            if (material.GetFloat("_NORMALMapEnabled") != 0)
            {

                m_editor.TextureProperty(normal, "Normal Map");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            m_editor.ShaderProperty(mraeMapEnabled, "MRAE Map Enabled");
            EditorGUILayout.Space();

            if (material.GetFloat("_MRAEMapEnabled") != 0)
            {

                m_editor.TextureProperty(MAREMap, "MROE Map (R:Metallic G:Roughness B:AO A:Emission)");
            }
            else
            {
                m_editor.ShaderProperty(metalic, "Metallic");
                m_editor.ShaderProperty(roughness, "Roughness");
                m_editor.ShaderProperty(ao, "Occlusion Strength");

            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            m_editor.ShaderProperty(bentNormalEnabled, "Bent Normal Enabled");
            EditorGUILayout.EndVertical();

            if (material.GetFloat("_BentnormalEnabled") != 0)
            {
                m_editor.TextureProperty(bentNormal, "Bent Normal");
            }
            EditorGUILayout.BeginVertical("Box");
            m_editor.ShaderProperty(emissionColor, "Emission Color");
            m_editor.ShaderProperty(reflectivity, "Reflectivity");
            m_editor.ShaderProperty(anisotropic, "Anisotropic");
            m_editor.ShaderProperty(horizonFade, "Horizon Occlusion Fade");
            EditorGUILayout.EndVertical();
        }
        public void DrawDebugOptionsPropertise(Material material)
        {
            EditorGUILayout.BeginVertical("Box");
            m_editor.ShaderProperty(IBL_BrdfMode, "IBL SpecularBrdf Mode");
            EditorGUILayout.EndVertical();
            //EditorGUILayout.BeginVertical("Box");
            //m_editor.ShaderProperty(CheckValue, "> Measure The Output Value");
            //m_editor.ShaderProperty(ChkTargetValue, "ORANGE-GREEN-BLUE");
            //m_editor.ShaderProperty(ChkTargetScale, "(Higher - Hit - Lower)");
            //m_editor.ShaderProperty(ChkRange, "Tolerance");
            //EditorGUILayout.EndVertical();
        }

        public virtual void RefreshKeywords()
        {
            if (m_materials.HasProperty("_MRAEMapEnabled"))
            {
                if (m_materials.GetFloat("_MRAEMapEnabled") != 0)
                {
                    m_materials.EnableKeyword("_MRAE_MAP_ENABLED");
                }
                else
                {
                    m_materials.DisableKeyword("_MRAE_MAP_ENABLED");
                }
            }

            if (m_materials.HasProperty("_DitherEnabled"))
            {
                if (m_materials.GetFloat("_DitherEnabled") != 0)
                {
                    m_materials.EnableKeyword("_DITHER_ENABLED");
                }
                else
                {
                    m_materials.DisableKeyword("_DITHER_ENABLED");
                }
            }

            if (m_materials.HasProperty("_AlphaClipEnabled"))
            {
                if (m_materials.GetFloat("_AlphaClipEnabled") != 0)
                {
                    m_materials.EnableKeyword("_ALPHACLIP_ENABLED");
                }
                else
                {
                    m_materials.DisableKeyword("_ALPHACLIP_ENABLED");
                }
            }
            //if (m_materials.HasProperty("_CHECKVALUE"))
            //{
            //    if (m_materials.GetFloat("_CHECKVALUE") != 0)
            //    {
            //        m_materials.EnableKeyword("_CHECKVALUE");
            //    }
            //    else
            //    {
            //        m_materials.DisableKeyword("_CHECKVALUE");
            //    }
            //}
        }

        public virtual void SetDefaultTexture()
        {
            Texture2D tex = EditorGUIUtility.Load("Assets/Textures/LUT/IBL_brdf_Lut.png") as Texture2D;
            if (tex != null) brdfLut.textureValue = tex;
        }
    }
}