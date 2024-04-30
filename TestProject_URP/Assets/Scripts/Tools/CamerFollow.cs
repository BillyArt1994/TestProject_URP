using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    public Transform m_Transform;
    Vector3 m_dir;
    // Start is called before the first frame update
    void Start()
    {
        m_dir = m_Transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_Transform.position - m_dir;
    }
}
