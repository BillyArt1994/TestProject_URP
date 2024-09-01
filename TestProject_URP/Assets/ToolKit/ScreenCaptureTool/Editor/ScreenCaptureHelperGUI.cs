using UnityEditor;
using UnityEngine;

namespace BillyToolKit
{
    [CustomEditor(typeof(ScreenCaptureHelper))]
    public class ScreenCaptureHelperGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Screen Capture"))
            {
                var sc = (ScreenCaptureHelper)target;
                sc.ScreenCaptureFunction();
            }
        }
    }
}