using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LookDevFreeCamera)), CanEditMultipleObjects]
public class LookDevFreeCameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LookDevFreeCamera m_lookDevFreeCamera = (LookDevFreeCamera)target;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        if (m_lookDevFreeCamera.m_isActive)
        {
            buttonStyle.normal.textColor = Color.green; // 修改按钮文本颜色
            GUI.color = new Color32(0, 255, 0, 255);
        }
        
        if(GUILayout.Button(new GUIContent("Activate Game View Sync", "Only Run In Editor Mode"), buttonStyle))
        {
            m_lookDevFreeCamera.m_isActive = !m_lookDevFreeCamera.m_isActive;
        }
    }
}
