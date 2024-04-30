using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LookDevWindow : EditorWindow
{
    public int m_rotate = 0;
    public Material m_skybox;
    public float m_exposure = 1.0f;
    private int RotationProperty;
    private int ExposureProperty;
    private bool m_rotateDirLightFlag;
    private List<ReflectionProbe> m_reflctProbe_list;

    [MenuItem("Window/Look Dev")]
    static void OpenWindow()
    {
        LookDevWindow window = (LookDevWindow)EditorWindow.GetWindow(typeof(LookDevWindow));
        window.Init();
        window.Show();
    }

    void OnEnable()
    {
        Init();
    }
    void Init()
    {
        m_reflctProbe_list = new List<ReflectionProbe>(GameObject.FindObjectsOfType<ReflectionProbe>());
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Enviroment Setting");
        m_rotateDirLightFlag = EditorGUILayout.Toggle("Rotate With Direction Light ? ", m_rotateDirLightFlag);
        m_rotate = EditorGUILayout.IntSlider("SkyBox rotation", m_rotate, 0, 360);
        RenderSettings.skybox.SetFloat("_Rotation", (float)m_rotate);
        if (m_rotateDirLightFlag)
        {
           
        }
        m_exposure = EditorGUILayout.Slider("Exposure", m_exposure, 0.0f, 8.0f);
        RenderSettings.skybox.SetFloat("_Exposure", m_exposure);
        OnInspectorUpdate();
    }

    private void OnInspectorUpdate()
    {
        if (GUI.changed)
        {
            UpdateEnviormentLight();
        }
    }
    void UpdateEnviormentLight()
    {
        DynamicGI.UpdateEnvironment();
        if (m_reflctProbe_list.Count == null) return;
        for (int i = 0; i < m_reflctProbe_list.Count; i++)
        {
            m_reflctProbe_list[i].RenderProbe();
        }
    }
}
