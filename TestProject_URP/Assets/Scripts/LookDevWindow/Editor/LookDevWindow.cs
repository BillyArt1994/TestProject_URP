using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class LookDevWindow : EditorWindow
{
    public enum DisplayerElement
    {
        None = 0,
        Albedo = 1 << 0,
        Metallic = 1 << 1,
        Rounghness = 1 << 2,
        AO = 1 << 3,
        Normal = 1 << 4,
        DirectionDiffuse = 1 << 5,
        DirectionSpecular = 1 << 6,
        IndirectionDiffuse = 1 << 7,
        IndirectionSpecular = 1 << 8,
        Shadow = 1 << 9,
        TextureDensity = 1 << 10
    }

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
    public DisplayerElement m_displayElem;


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
        m_displayElem = (DisplayerElement)EditorGUILayout.EnumPopup("View Mode", m_displayElem);
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
                SceneView.duringSceneGui -= TurnTable;
                SceneView.duringSceneGui += TurnTable;
            }
            else if(m_turnTableFlag == false)
            {
                SceneView.duringSceneGui -= TurnTable;
            }
            ViewDisplayElement(m_displayElem);
        }
    }

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
   
    }

    void ViewDisplayElement(DisplayerElement elem)
    {
        if (elem == DisplayerElement.TextureDensity)
        {
            
            Texture2D mipmapChecker = EditorGUIUtility.Load("Assets/Textures/ColorCheck/TextureDensityCheck.dds") as Texture2D;
            Shader.SetGlobalTexture("_CheckTex", mipmapChecker);

            foreach (UnityEditor.SceneView view in UnityEditor.SceneView.sceneViews)
            {                
                view.SetSceneViewShaderReplace(Shader.Find("Unlit/DensityVisualization"), "RenderType");
            }


        }
        else
        {
            Shader.SetGlobalTexture("_CheckTex", null);
            foreach (UnityEditor.SceneView view in UnityEditor.SceneView.sceneViews)
            {
                view.SetSceneViewShaderReplace(null, "RenderType");
            }
        }

        Shader.SetKeyword(GlobalKeyword.Create("_ALBEDO_DISPLAYER"), elem == DisplayerElement.Albedo);
        Shader.SetKeyword(GlobalKeyword.Create("_METALLIC_DISPLAYER"), elem == DisplayerElement.Metallic);
        Shader.SetKeyword(GlobalKeyword.Create("_ROUNGHNESS_DISPLAYER"), elem == DisplayerElement.Rounghness);
        Shader.SetKeyword(GlobalKeyword.Create("_AO_DISPLAYER"), elem == DisplayerElement.AO);
        Shader.SetKeyword(GlobalKeyword.Create("_NORMAL_DISPLAYER"), elem == DisplayerElement.Normal);
        Shader.SetKeyword(GlobalKeyword.Create("_SHADOW_DISPLAYER"), elem == DisplayerElement.Shadow);
        Shader.SetKeyword(GlobalKeyword.Create("_DIRECTDIFFUSE_DISPLAYER"), elem == DisplayerElement.DirectionDiffuse);
        Shader.SetKeyword(GlobalKeyword.Create("_DIRECTSPECULAR_DISPLAYER"), elem == DisplayerElement.DirectionSpecular);
        Shader.SetKeyword(GlobalKeyword.Create("_INDIRECTDIFFUSE_DISPLAYER"), elem == DisplayerElement.IndirectionDiffuse);
        Shader.SetKeyword(GlobalKeyword.Create("_INDIRECTSPECULAR_DISPLAYER"), elem == DisplayerElement.IndirectionSpecular);
    }
}
