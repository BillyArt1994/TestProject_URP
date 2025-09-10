#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class LightHelper : MonoBehaviour
{
    private Light m_light;
    private float m_distance;
    public Transform m_targetObj;
    [Range(0.0f, 1.0f)]
    public float m_distanceInfoPos = 0.9f;
    [Range(0.0f, 1.0f)]
    public float m_angleInfoPos = 0.1f;

    public Color m_lineColor = Color.red;
    public bool m_drawHelpGizmos = true;
    public bool m_showDistanceInfo = true;
    public bool m_showAngleInfo = true;
    public bool m_alwayShow = true;

    void Start()
    {
        m_light = GetComponent<Light>();
    }

    void Awake()
    {
        m_light = GetComponent<Light>();
    }

    void LateUpdate()
    {

        if ((transform.hasChanged || m_targetObj.transform.hasChanged)&& (m_light.type == LightType.Spot))
        {
            Lookat();
        }
    }
    public void OnDrawGizmos()
    {
        if (m_light.type == LightType.Spot) 
        {
        if (Selection.activeGameObject != gameObject && m_alwayShow == false) return;
        if (m_targetObj == null || m_drawHelpGizmos == false) return;
        Gizmos.color = m_lineColor;
        Gizmos.DrawLine(m_targetObj.position, transform.position);

        Handles.color = m_lineColor;
        var targetToScrDirUnNmz = transform.position - m_targetObj.position;
        if (m_showDistanceInfo)
        {
            m_distance = Vector3.Distance(m_targetObj.position, transform.position);
            var disLabelPos = m_targetObj.position + targetToScrDirUnNmz * m_distanceInfoPos;
            disLabelPos = transform.InverseTransformPoint(disLabelPos);
            disLabelPos = transform.TransformPoint(new Vector3(disLabelPos.x, disLabelPos.y + 0.2f, disLabelPos.z + 0.2f));
            Handles.Label(disLabelPos, transform.name + " Distance:" + m_distance.ToString("0.00") + "m");

        }

        if (m_showAngleInfo)
        {
            Vector3 scrToTarDir = (m_targetObj.position - new Vector3(transform.position.x, m_targetObj.position.y, transform.position.z)).normalized;
            float angle = Vector3.Angle(transform.forward, scrToTarDir);
            var angleLabelPos = m_targetObj.position + ((transform.position - m_targetObj.position) * m_angleInfoPos);
            angleLabelPos = transform.InverseTransformPoint(angleLabelPos);
            angleLabelPos = transform.TransformPoint(new Vector3(angleLabelPos.x, angleLabelPos.y + 0.2f, angleLabelPos.z));
            Handles.Label(angleLabelPos, transform.name + " Angle:" + angle.ToString("0.00") + "бу");

            var guidesDir = new Vector3(transform.position.x, m_targetObj.position.y, transform.position.z) - m_targetObj.position;
            var guidesPos = m_targetObj.position + guidesDir * m_angleInfoPos;
            var arcNormal = Vector3.Cross(targetToScrDirUnNmz.normalized, guidesDir.normalized);
            Handles.DrawWireArc(m_targetObj.transform.position, arcNormal, targetToScrDirUnNmz.normalized, angle, m_angleInfoPos);
            Gizmos.DrawLine(m_targetObj.position, guidesPos);
        }
        }
    }
    void Lookat()
    {
        if (m_targetObj == null) return;
        transform.LookAt(m_targetObj);
    }

}
#endif