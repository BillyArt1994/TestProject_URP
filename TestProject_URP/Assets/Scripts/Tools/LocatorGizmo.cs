#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LocatorGizmo : MonoBehaviour
{
    public Color m_color = Color.red;
    [Range(0f, 1.0f)]
    public float m_size = 0.3f;
    public bool m_showGizemo = true;

    public void OnDrawGizmos()
    {
        if (m_showGizemo == false) return;
        Gizmos.color = m_color;
        Gizmos.DrawLine(transform.position- new Vector3(m_size,0.0f,0.0f), transform.position + new Vector3(m_size, 0.0f, 0.0f));
        Gizmos.DrawLine(transform.position- new Vector3(0.0f, m_size, 0.0f), transform.position + new Vector3(0.0f, m_size, 0.0f));
        Gizmos.DrawLine(transform.position- new Vector3(0.0f, 0.0f, m_size), transform.position + new Vector3(0.0f, 0.0f, m_size));
    }
}
#endif