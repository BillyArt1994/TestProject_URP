using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;

[ExecuteInEditMode]
//[RequireComponent(typeof(Camera), typeof(UniversalAdditionalCameraData))]
public class CustomShadow : MonoBehaviour
{
    public TexSize m_shadowMapSize = TexSize._1024;
    public Transform m_target;
    public Vector3 m_offest;
    public Light m_light;
    public float m_radius = 1f;
    public float m_sceneCaptureDistance = 4f;
    [Range(0.0f,5.0f)]
    public float m_depthBias = 0.1f;
    [Range(0.0f, 5.0f)]
    public float m_filterScale = 1.0f;
    CustomShadowCasterFeature m_customShadow;
    private bool dirty = true;

    public static CustomShadow m_Instance;

    public enum TexSize
    {
        _64 = 1 << 6,
        _128 = 1 << 7,
        _256 = 1 << 8,
        _512 = 1 << 9,
        _1024 = 1 << 10,
        _2048 = 1 << 11,
        _4096 = 1 << 12,
    }

    CustomShadow()
    {
        m_Instance = this;
    }

    private void Awake()
    {
        m_customShadow = CustomShadowCasterFeature.m_Instance;
        if (m_light != null)
        {
            if (m_light.gameObject.GetComponent<CustomShadowCasterFeature>() == null) 
            {
                m_light.gameObject.AddComponent<CustomShadowLightHelper>();
            }
        }
            
        //m_customShadow.Init(1024, 1024);
        SetFocus();
    }

    void OnEnable()
    {
        if (m_target == null)
        {
            //target = this.transform;
        }

        m_customShadow.Init((int)m_shadowMapSize, (int)m_shadowMapSize);
    }

    void OnValidate()
    {
        SetFocus();
        SetShadowBias();
        m_customShadow.Init((int)m_shadowMapSize, (int)m_shadowMapSize);
       // Shader.SetGlobalVector("_CustomShadowParams", new Vector4(m_depthBias, 0.0f, 0.0f, 0.0f));
    }

    public void SetFocus()
    {
        m_customShadow.Projection(m_radius, m_sceneCaptureDistance, m_depthBias);
        CheckVisibility(m_light);
    }

    bool CheckVisibility(Light light)
    {
        if (m_target == null || light == null)
            return false;
        var targetPoint_pos = m_target.position
           + m_target.right * m_offest.x
           + m_target.up * m_offest.y
           + m_target.forward * m_offest.z;

        var pos = targetPoint_pos - light.transform.forward * m_radius ;
        var rot = light.transform.rotation;

        transform.position = pos;
        transform.rotation = rot;
        m_customShadow.Transform(pos, rot);
        return true;
    }

    void OnDrawGizmosSelected()
    {
        if (m_target == null)
            return;
        Gizmos.color = Color.cyan;
        var pos = m_target.position
                   + m_target.right * m_offest.x
                   + m_target.up * m_offest.y
                   + m_target.forward * m_offest.z;
        Gizmos.DrawWireSphere(pos, m_radius);

        Gizmos.color = Color.gray;
        
        var localToWorldInvZM = transform.localToWorldMatrix* Matrix4x4.Scale(new Vector3(1.0f, 1.0f, -1.0f));
        Gizmos.matrix = localToWorldInvZM;

        Vector3 center = new Vector3(0, 0, -m_radius);
        Vector3 size = Vector3.one * m_radius * 2;
        center.z += -m_sceneCaptureDistance*0.5f;
        size.z += m_sceneCaptureDistance;
        Gizmos.DrawWireCube(center, size);
    }

    //private void OnDrawGizmos()
    //{
    //            if (m_target == null)
    //        return;
    //    Gizmos.color = Color.cyan;
    //    var pos = m_target.position
    //               + m_target.right * m_offest.x
    //               + m_target.up * m_offest.y
    //               + m_target.forward * m_offest.z;
    //    Gizmos.DrawWireSphere(pos, m_radius);
    //
    //    Gizmos.color = Color.gray;
    //    
    //    var localToWorldInvZM = transform.localToWorldMatrix* Matrix4x4.Scale(new Vector3(1.0f, 1.0f, -1.0f));
    //    Gizmos.matrix = localToWorldInvZM;
    //
    //    Vector3 center = new Vector3(0, 0, -m_radius);
    //    Vector3 size = Vector3.one * m_radius * 2;
    //    center.z += -m_sceneCaptureDistance*0.5f;
    //    size.z += m_sceneCaptureDistance;
    //    Gizmos.DrawWireCube(center, size);
    //}

    //[ContextMenu("My Func")]
    //void Print()
    //{
    //
    //
    //    var targetPoint_pos = m_target.position
    //        + m_target.right * m_offest.x
    //        + m_target.up * m_offest.y
    //        + m_target.forward * m_offest.z;
    //
    //    var pos = transform.position;//targetPoint_pos - m_light.transform.forward * (m_radius + m_sceneCaptureDistance);
    //    var rot = transform.rotation;//m_light.transform.rotation;
    //    print("视口矩阵"+Matrix4x4.TRS(pos, rot, Vector3.one).inverse);
    //
    //    float nearClipPlane = 0f;
    //    float farClipPlane = m_radius * 2f + m_sceneCaptureDistance;
    //    print("投影矩阵"+Matrix4x4.Ortho(-m_radius, m_radius, -m_radius, m_radius, nearClipPlane, farClipPlane));
    //}

    void SetShadowBias()
    {
        Shader.SetGlobalFloat("_CustomShadowBias", m_depthBias);
        Shader.SetGlobalFloat("_CustomShadowFilterScale", m_filterScale);
        
    }
}
