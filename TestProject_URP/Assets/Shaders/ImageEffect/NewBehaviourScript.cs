using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Material m_material;
    // Start is called before the first frame update
    void Start()
    {
        m_material = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("test")]
    void KeyWord()
    {
        if (m_material == null) 
        {
            m_material = GetComponent<MeshRenderer>().sharedMaterial;
        }
        
        m_material.EnableKeyword("_CHANNELDISPLAY_GRAY");
        
        //m_material.DisableKeyword("_CHANNELDISPLAY_RGBA");
        //m_material.DisableKeyword("_CHANNELDISPLAY_R");
        //m_material.DisableKeyword("_CHANNELDISPLAY_G");
        //m_material.DisableKeyword("_CHANNELDISPLAY_B");
        //m_material.DisableKeyword("_CHANNELDISPLAY_A");
    }
}
