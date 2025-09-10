using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceShader : MonoBehaviour
{
    public Shader m_shader;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    [ContextMenu("ReplaceShader")]
    void ReplaceShaderFn()
    {
        GetComponent<Camera>().SetReplacementShader(m_shader,"RenderType");
    }

    [ContextMenu("ResetReplaceShader")]
    void ResetReplaceShaderFn()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
