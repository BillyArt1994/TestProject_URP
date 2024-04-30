using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditorInternal.VR;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace CommonShaderGUI
{
    class BillySkinShaderGUI : BillyShaderGUI
    {
        protected MaterialProperty curvature;
        protected MaterialProperty thickness;
        protected MaterialProperty skinBrdfLUT;
        protected MaterialProperty shadowBrdfLUT;
        protected MaterialProperty skinToneScale;
        protected MaterialProperty skinSecondSpecScale;
        protected MaterialProperty translucencyViewDependency;
        protected MaterialProperty translucencyThreshold;
        protected MaterialProperty translucencyScale;
        protected MaterialProperty translucencyColor;
        static class BillySkinBaseProperties
        {
            public static readonly string _Curvature = nameof(_Curvature);
            public static readonly string _Thickness = nameof(_Thickness);
            public static readonly string _SkinBrdfLUT = nameof(_SkinBrdfLUT);
            public static readonly string _ShadowBrdfLUT = nameof(_ShadowBrdfLUT);
            public static readonly string _SkinToneScale = nameof(_SkinToneScale);
            public static readonly string _SkinSecondSpecScale = nameof(_SkinSecondSpecScale);
            public static readonly string _TranslucencyViewDependency = nameof(_TranslucencyViewDependency);
            public static readonly string _TranslucencyThreshold = nameof(_TranslucencyThreshold);
            public static readonly string _TranslucencyScale = nameof(_TranslucencyScale);
            public static readonly string _TranslucencyColor = nameof(_TranslucencyColor);

        }
        public override void FindProperties(MaterialProperty[] props)
        {
            base.FindProperties(props);
            curvature = FindProperty(BillySkinBaseProperties._Curvature, props, false);
            thickness = FindProperty(BillySkinBaseProperties._Thickness, props, false);
            skinBrdfLUT = FindProperty(BillySkinBaseProperties._SkinBrdfLUT, props, false);
            shadowBrdfLUT = FindProperty(BillySkinBaseProperties._ShadowBrdfLUT, props, false);
            skinToneScale = FindProperty(BillySkinBaseProperties._SkinToneScale, props, false);
            skinSecondSpecScale = FindProperty(BillySkinBaseProperties._SkinSecondSpecScale, props, false);
            translucencyViewDependency = FindProperty(BillySkinBaseProperties._TranslucencyViewDependency, props, false);
            translucencyThreshold = FindProperty(BillySkinBaseProperties._TranslucencyThreshold, props, false);
            translucencyScale = FindProperty(BillySkinBaseProperties._TranslucencyScale, props, false);
            translucencyColor = FindProperty(BillySkinBaseProperties._TranslucencyColor, props, false);
        }

        public override void OnOpenGUI()
        {
            base.OnOpenGUI();
        }

        public override void DrawSurfacePropertise(Material material)
        {
            base.DrawSurfacePropertise(material);
            EditorGUILayout.BeginVertical("Box");
            m_editor.ShaderProperty(skinBrdfLUT,"Skin Brdf Lut");
            m_editor.ShaderProperty(shadowBrdfLUT,"Shadow Brdf Lut");
            m_editor.ShaderProperty(curvature, "Curvature Map");
            m_editor.ShaderProperty(thickness, "Thickness Map");
            m_editor.ShaderProperty(skinToneScale, "SkinTone Scale");
            m_editor.ShaderProperty(skinSecondSpecScale, "Skin Second Spec Scale");
            m_editor.ShaderProperty(translucencyViewDependency, "Translucency View Dependency");
            m_editor.ShaderProperty(translucencyThreshold, "Translucency Threshold");
            m_editor.ShaderProperty(translucencyScale, "Translucency Scale");
            m_editor.ShaderProperty(translucencyColor, "Translucency Color");
            EditorGUILayout.EndVertical();
        }

        public override void RefreshKeywords()
        {
            base.RefreshKeywords();
        }
        public override void SetDefaultTexture()
        {
            base.SetDefaultTexture();
            //Texture2D tex = EditorGUIUtility.Load("Assets/Textures/LUT/Pre-Integrated Skin Shading.png") as Texture2D;
            //if (tex != null) skinBrdfLUT.textureValue = tex;
        }
    }
}

