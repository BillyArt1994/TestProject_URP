using UnityEngine.Rendering.Universal;

namespace UnityEditor.Rendering.Universal
{
    [CustomEditor(typeof(Tonemapping))]
    sealed class TonemappingEditor : VolumeComponentEditor
    {
        SerializedDataParameter m_Mode;

        // HDR Mode.
        SerializedDataParameter m_NeutralHDRRangeReductionMode;
        SerializedDataParameter m_HueShiftAmount;
        SerializedDataParameter m_HDRDetectPaperWhite;
        SerializedDataParameter m_HDRPaperwhite;
        SerializedDataParameter m_HDRDetectNitLimits;
        SerializedDataParameter m_HDRMinNits;
        SerializedDataParameter m_HDRMaxNits;
        SerializedDataParameter m_HDRAcesPreset;

        /////////////////UE4_ACES_BEGIN/////////////////
        SerializedDataParameter m_Slope;
        SerializedDataParameter m_Toe;
        SerializedDataParameter m_Shoulder;
        SerializedDataParameter m_BlackClip;
        SerializedDataParameter m_WhiteClip;
        /////////////////UE4_ACES_END/////////////////

        /// <summary>
        /// FILMIC
        SerializedDataParameter m_ACES_A;
        SerializedDataParameter m_ACES_B;
        SerializedDataParameter m_ACES_C;
        SerializedDataParameter m_ACES_D;
        SerializedDataParameter m_ACES_E;
        /// </summary>
        /// 

        /// <summary>
        /// FILMIC
        SerializedDataParameter m_A;
        SerializedDataParameter m_B;
        SerializedDataParameter m_C;
        SerializedDataParameter m_D;
        SerializedDataParameter m_E;
        SerializedDataParameter m_F;
        SerializedDataParameter m_G;
        SerializedDataParameter m_H;
        SerializedDataParameter m_I;

        /// </summary>

        public override bool hasAdditionalProperties => true;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<Tonemapping>(serializedObject);
            m_Mode = Unpack(o.Find(x => x.mode));

            m_NeutralHDRRangeReductionMode = Unpack(o.Find(x => x.neutralHDRRangeReductionMode));
            m_HueShiftAmount = Unpack(o.Find(x => x.hueShiftAmount));
            m_HDRDetectPaperWhite = Unpack(o.Find(x => x.detectPaperWhite));
            m_HDRPaperwhite = Unpack(o.Find(x => x.paperWhite));
            m_HDRDetectNitLimits = Unpack(o.Find(x => x.detectBrightnessLimits));
            m_HDRMinNits = Unpack(o.Find(x => x.minNits));
            m_HDRMaxNits = Unpack(o.Find(x => x.maxNits));
            m_HDRAcesPreset = Unpack(o.Find(x => x.acesPreset));

            /////////////////UE4_ACES_BEGIN/////////////////
            m_Slope = Unpack(o.Find(x => x.slope));
            m_Toe = Unpack(o.Find(x => x.toe));
            m_Shoulder = Unpack(o.Find(x => x.shoulder));
            m_BlackClip = Unpack(o.Find(x => x.blackClip));
            m_WhiteClip = Unpack(o.Find(x => x.whiteClip));
            /////////////////UE4_ACES_END///////////////////
           
            /// FILM///
            m_ACES_A = Unpack(o.Find(x => x.ACES_A));
            m_ACES_B = Unpack(o.Find(x => x.ACES_B));
            m_ACES_C = Unpack(o.Find(x => x.ACES_C));
            m_ACES_D = Unpack(o.Find(x => x.ACES_D));
            m_ACES_E = Unpack(o.Find(x => x.ACES_E));
            ///


            /// OverWatch2///
            m_A = Unpack(o.Find(x => x.A));
            m_B = Unpack(o.Find(x => x.B));
            m_C = Unpack(o.Find(x => x.C));
            m_D = Unpack(o.Find(x => x.D));
            m_E = Unpack(o.Find(x => x.E));
            m_F = Unpack(o.Find(x => x.F));
            m_G = Unpack(o.Find(x => x.G));
            m_H = Unpack(o.Find(x => x.H));
            ///
        }

        public override void OnInspectorGUI()
        {
            PropertyField(m_Mode);
            /////////////////UE4_ACES_BEGIN/////////////////
            if (m_Mode.value.intValue == (int)TonemappingMode.UE4_ACES)
            {
                UnityEngine.GUILayout.BeginVertical("box");
                UnityEngine.GUILayout.BeginHorizontal();

                PropertyField(m_Slope);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_Slope.value.floatValue = 0.88f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_Toe);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_Toe.value.floatValue = 0.55f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_Shoulder);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_Shoulder.value.floatValue = 0.26f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_BlackClip);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_BlackClip.value.floatValue = 0.0f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_WhiteClip);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_WhiteClip.value.floatValue = 0.04f;
                }
                UnityEngine.GUILayout.EndHorizontal();
                UnityEngine.GUILayout.EndVertical();

            }
            /////////////////UE4_ACES_END////////////////////
            ///


            ///FILM
            if (m_Mode.value.intValue == (int)TonemappingMode.FILM)
            {
                UnityEngine.GUILayout.BeginVertical("box");
                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_ACES_A);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_ACES_A.value.floatValue = 2.51f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_ACES_B);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_ACES_B.value.floatValue = 0.03f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_ACES_C);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_ACES_C.value.floatValue = 2.43f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_ACES_D);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_ACES_D.value.floatValue = 0.59f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_ACES_E);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_ACES_E.value.floatValue = 0.14f;
                }
                UnityEngine.GUILayout.EndHorizontal();
                UnityEngine.GUILayout.EndVertical();
            }
            //////////////////////////////////////////////////
            ///

            ///Over Watch2
            if (m_Mode.value.intValue == (int)TonemappingMode.OverWatch2)
            {
                UnityEngine.GUILayout.BeginVertical("box");
                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_A);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_A.value.floatValue = 1.16541f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_B);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_B.value.floatValue = 0.1f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_C);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_C.value.floatValue = 2.5f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_D);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_D.value.floatValue = 0.3f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_E);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_E.value.floatValue = 0.3f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_F);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_F.value.floatValue = 0.01f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_G);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_G.value.floatValue = 0.1f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.BeginHorizontal();
                PropertyField(m_H);
                if (UnityEngine.GUILayout.Button("Reset"))
                {
                    m_H.value.floatValue = 0.1f;
                }
                UnityEngine.GUILayout.EndHorizontal();

                UnityEngine.GUILayout.EndVertical();
            }
            //////////////////////////////////////////////////

                // Display a warning if the user is trying to use a tonemap while rendering in LDR
                var asset = UniversalRenderPipeline.asset;
            if (asset != null && !asset.supportsHDR)
            {
                EditorGUILayout.HelpBox("Tonemapping should only be used when working with High Dynamic Range (HDR). Please enable HDR through the active Render Pipeline Asset.", MessageType.Warning);
                return;
            }

            if (PlayerSettings.allowHDRDisplaySupport && m_Mode.value.intValue != (int)TonemappingMode.None)
            {
                EditorGUILayout.LabelField("HDR Output");
                int hdrTonemapMode = m_Mode.value.intValue;
                
                if (hdrTonemapMode == (int)TonemappingMode.Neutral)
                {
                    PropertyField(m_NeutralHDRRangeReductionMode);
                    PropertyField(m_HueShiftAmount);

                    PropertyField(m_HDRDetectPaperWhite);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectPaperWhite.value.boolValue))
                    {
                        PropertyField(m_HDRPaperwhite);
                    }
                    EditorGUI.indentLevel--;

                    PropertyField(m_HDRDetectNitLimits);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectNitLimits.value.boolValue))
                    {
                        PropertyField(m_HDRMinNits);
                        PropertyField(m_HDRMaxNits);
                    }
                    EditorGUI.indentLevel--;
                }
                if (hdrTonemapMode == (int)TonemappingMode.ACES)
                {
                    PropertyField(m_HDRAcesPreset);

                    PropertyField(m_HDRDetectPaperWhite);
                    EditorGUI.indentLevel++;
                    using (new EditorGUI.DisabledScope(m_HDRDetectPaperWhite.value.boolValue))
                    {
                        PropertyField(m_HDRPaperwhite);
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
}
