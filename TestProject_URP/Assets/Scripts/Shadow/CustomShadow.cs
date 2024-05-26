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
public class CustomShadow : MonoBehaviour
{
    public TexSize m_shadowMapSize = TexSize._1024;
    public Transform m_target;
    private Transform m_targetTemp;
    public Vector3 m_offest;
    public Light m_light;
    public float m_radius = 1f;
    public float m_sceneCaptureDistance = 4f;
    [Range(0.0f, 5.0f)]
    public float m_depthBias = 0.1f;
    [Range(0.0f, 5.0f)]
    public float m_filterScale = 1.0f;
    CustomShadowCasterFeature m_customShadow;
    private bool dirty = true;
    const string CUSTOM_SHADOW_KW = "_CUSTOM_SHADOW";
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
            if (m_light.gameObject.GetComponent<CustomShadowLightHelper>() == null)
            {
                m_light.gameObject.AddComponent<CustomShadowLightHelper>();
            }
        }
        //SetFocus();
    }


    void OnEnable()
    {
        m_customShadow = CustomShadowCasterFeature.m_Instance;
        if (m_target == null)
        {
            SetKeyWord(m_target, CUSTOM_SHADOW_KW, true);
        }
        
        m_customShadow.Init((int)m_shadowMapSize, (int)m_shadowMapSize);
        SetFocus();
        Debug.Log("OnEnable");
    }

    void OnValidate()
    {
        if (!this.enabled || m_customShadow == null) return;
        SetFocus();
        m_customShadow.Init((int)m_shadowMapSize, (int)m_shadowMapSize);
        if (m_target != null && m_target != m_targetTemp)
        {
            SetKeyWord(m_target, CUSTOM_SHADOW_KW, true);
            var mats = m_target.GetComponentInChildren<MeshRenderer>().sharedMaterials;
            //foreach (var mat in mats)
            //{
            //    var kwE = mat.shaderKeywords;
            //    foreach (var kk in kwE)  Debug.Log(kk);
            //}
            SetKeyWord(m_targetTemp, CUSTOM_SHADOW_KW, false);
            m_targetTemp = m_target;
        }
    }

    public void SetFocus()
    {
        if (m_customShadow == null) return;
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

        var pos = targetPoint_pos - light.transform.forward * m_radius;
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

        var localToWorldInvZM = transform.localToWorldMatrix * Matrix4x4.Scale(new Vector3(1.0f, 1.0f, -1.0f));
        Gizmos.matrix = localToWorldInvZM;

        Vector3 center = new Vector3(0, 0, -m_radius);
        Vector3 size = Vector3.one * m_radius * 2;
        center.z += -m_sceneCaptureDistance * 0.5f;
        size.z += m_sceneCaptureDistance;
        Gizmos.DrawWireCube(center, size);
    }

    private void OnDisable()
    {
        if (m_target != null)
        {
            SetKeyWord(m_target, CUSTOM_SHADOW_KW, false);
        }
    }

    private void OnDestroy()
    {
        if (m_target != null)
        {
            SetKeyWord(m_target, CUSTOM_SHADOW_KW, false);
        }
    }

    /// <summary>
    /// Set KeyWord Form input Transform include all childer node
    /// </summary>
    /// <param name=""></param>
    void SetKeyWord(Transform transf, string keyWord, bool flag)
    {
        var mats = m_target.GetComponentInChildren<MeshRenderer>().sharedMaterials;
        foreach (var mat in mats)
        {
            if (flag)
            {
                mat.EnableKeyword(CUSTOM_SHADOW_KW);
            }
            else
            {
                mat.DisableKeyword(CUSTOM_SHADOW_KW);
            }
        }
    }
}
