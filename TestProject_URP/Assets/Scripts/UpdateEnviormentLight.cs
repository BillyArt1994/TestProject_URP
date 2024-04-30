using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpdateEnviormentLight : MonoBehaviour
{
    [Range(0,360)]
    public float m_rotate;
    public Material m_skybox;
    private int RotationProperty;
    private List<ReflectionProbe> m_reflctProbe_list;
    // Start is called before the first frame update
    void Awake()
    {
        RotationProperty = Shader.PropertyToID("_Rotation"); 
        m_skybox = RenderSettings.skybox;
        m_reflctProbe_list = new List<ReflectionProbe>(GameObject.FindObjectsOfType<ReflectionProbe>());
    }

    // Update is called once per frame
    void Update()
    {
        m_skybox.SetFloat(RotationProperty, m_rotate);
        for (int i = 0; i < m_reflctProbe_list.Count; i++)
        {
            m_reflctProbe_list[i].RenderProbe();
        }
        UpdateEnviormentLightT();
    }

    [ContextMenu("test")]
    void UpdateEnviormentLightT()
    {
        DynamicGI.UpdateEnvironment();
    }
}
