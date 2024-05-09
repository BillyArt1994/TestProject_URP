using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomShadowLightHelper : MonoBehaviour
{
    [Range(0.0f,100.0f)]
    public float m_lightSize;
    public bool m_PCSS_Enable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            CustomShadow.m_Instance.SetFocus();
            transform.hasChanged = false;
        }

    }

    void OnValidate()
    {
        Shader.SetGlobalVector("_CustomShadowCasterParams", new Vector4(m_lightSize, 0.0f, 0.0f, 0.0f));
    }
}
