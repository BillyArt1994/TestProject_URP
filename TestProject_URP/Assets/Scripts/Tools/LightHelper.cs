using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class LightHelper: MonoBehaviour
{
    private Light m_light;
    private float m_distance;
    private float m_radiance = 1.0f;
    public Transform m_targetObj;
    public Color m_lineColor = Color.red;
    public bool m_drawGizmos = true;
    public bool m_showDisInfo = true;
    public bool m_alwayShow = true;

    // Start is called before the first frame update
    void Start()
    {
        m_light = GetComponent<Light>();
    }

    void Awake()
    {
        m_light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Lookat();
      //  AutoExposureBalance();
    }

    public void OnDrawGizmos()
    {
        if (Selection.activeGameObject != gameObject && m_alwayShow == false) return; 
        if (m_targetObj == null || m_drawGizmos == false) return;
        Gizmos.color = m_lineColor;
        Gizmos.DrawLine(m_targetObj.position,transform.position);
        if (m_showDisInfo == false) return;
        m_distance = Vector3.Distance(m_targetObj.position, transform.position);
        var disInfo = m_distance.ToString() + "m";
        Handles.color = m_lineColor;
        Handles.Label(m_targetObj.position + ((transform.position - m_targetObj.position)*0.9f), disInfo);
    }

    void Lookat()
    {
        if (m_targetObj == null) return;
        transform.LookAt(m_targetObj);
    }

   // void AutoExposureBalance()
   // {
   //     m_light.intensity = m_radiance * (m_distance * m_distance);
   // }
   //
   // public void LockRadiance() {
   //
   //     m_radiance = m_light.intensity / (m_distance * m_distance);
   // }
}
