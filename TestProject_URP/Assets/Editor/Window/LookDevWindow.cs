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
    private float m_rotateTemp = 0.0f;
    public bool m_turnTableFlag = false;
    public Transform m_turntable;
    public float m_speed = 1.0f;

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
        m_exposure = EditorGUILayout.Slider("Exposure", m_exposure, 0.0f, 8.0f);
        RenderSettings.skybox.SetFloat("_Exposure", m_exposure);
        m_turnTableFlag = EditorGUILayout.Toggle("turnTable ? ", m_turnTableFlag);
        m_turntable = (Transform)EditorGUILayout.ObjectField(m_turntable, typeof(Transform), true);
        m_speed = EditorGUILayout.FloatField("ËÙ¶È",m_speed);
        OnInspectorUpdate();
    }

    private void OnInspectorUpdate()
    {
        if (GUI.changed)
        {
            UpdateEnviormentLight();
            if (m_rotateDirLightFlag)
            {
                RotateDirectLight();
            }
            if (m_turnTableFlag == true)
            {
                //EditorApplication.update += TurnTable;
                SceneView.duringSceneGui += TurnTable;
            }
            else if(m_turnTableFlag == false)
            {
                SceneView.duringSceneGui -= TurnTable;
            }
        }
    }

    //private void OnFocus()
    //{
    //    SceneView.duringSceneGui 
    //}



    void UpdateEnviormentLight()
    {
        DynamicGI.UpdateEnvironment();
        if (m_reflctProbe_list.Count == null) return;
        for (int i = 0; i < m_reflctProbe_list.Count; i++)
        {
            if (m_reflctProbe_list[i] == null) continue;
            m_reflctProbe_list[i].RenderProbe();
        }
    }

    void RotateDirectLight()
    {
        var directLight = RenderSettings.sun;
        directLight.transform.Rotate(new Vector3(0, -(m_rotate - m_rotateTemp), 0), Space.World);
        m_rotateTemp = m_rotate;
    }

    void TurnTable(SceneView sceneView)
    {
        if (m_turntable != null) 
        {
            m_turntable.Rotate(Vector3.up, m_speed * Time.deltaTime, Space.World);
        }
        sceneView.Repaint();
    }
}
