using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEditor.Rendering.Universal
{
    [CustomEditor(typeof(SubstancePainterLookup))]
    sealed class SubstancePainterLookupEditor : VolumeComponentEditor
    {
        SerializedDataParameter m_lookuptexture;
        SerializedDataParameter m_lookuptextureOut;
        string _exportFilePath = "";

        public override void OnEnable()
        {
            var o = new PropertyFetcher<SubstancePainterLookup>(serializedObject);
            m_lookuptexture = Unpack(o.Find(x => x.lookuptexture));
            m_lookuptextureOut = Unpack(o.Find(x => x.lookuptextureOut));
        }
        public override GUIContent GetDisplayTitle()
        {
            return EditorGUIUtility.TrTextContent("Look Up Generator");
        }

        public override void OnInspectorGUI()
        {
            SubstancePainterLookup spLookUp = (SubstancePainterLookup)target;
            PropertyField(m_lookuptexture, EditorGUIUtility.TrTextContent("Lut Input Texture"));
            PropertyField(m_lookuptextureOut, EditorGUIUtility.TrTextContent("Lut OutPut Texture"));


           // GUILayout.BeginVertical("box");
            if (GUILayout.Button("Bake Lut"))
            {
                BakeLut();
            }
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Save Path");
                _exportFilePath = EditorGUILayout.TextField(_exportFilePath);
                if (GUILayout.Button("..."))
                {
                    _exportFilePath = EditorUtility.SaveFilePanelInProject("Choose image file", "CustomBakedLut",
                        "exr", "Please enter a file name to save the image to");
                }
            }
        }

        void BakeLut()
        {

        }

    }
}