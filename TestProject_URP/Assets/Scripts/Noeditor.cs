using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noeditor : MonoBehaviour
{
    bool flag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Change State")]
    void ChangeState()
    {
        if (flag)
        {
            this.gameObject.hideFlags = HideFlags.None;
        }
        else
        {
            this.gameObject.hideFlags = HideFlags.HideInInspector;
            this.gameObject.hideFlags = HideFlags.NotEditable;
        }
        flag = !flag;
    }
}
