using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode][RequireComponent(typeof(Camera))]
public class LookDevFreeCamera : MonoBehaviour
{
    
    public bool m_isActive { get; set; }
    SceneView m_sceneView;
    Camera m_gameCam;
    public float m_moveSpeed = 1f;
    public float m_mouseSpeed = 1f;
    private float m_deltX;
    private float m_deltY;

    private void Start()
    {
        Init();
    }

    void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            CamerFreeMove();
            if (Input.GetMouseButton(1))
            {
                CursorVisible(true);
                FollowRotation();
            }
            else
            {
                CursorVisible(false);
            }
        }

    }

    private void CamerFreeMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontal *= 3; vertical *= 3;
        }
        m_gameCam.transform.Translate(Vector3.forward * vertical * m_moveSpeed * Time.deltaTime);
        m_gameCam.transform.Translate(Vector3.right * horizontal * m_moveSpeed * Time.deltaTime);
    }

    void EditorUpdate()
    {
        if (m_sceneView != null && m_isActive && !EditorApplication.isPlaying)
        {
            SyncCameras();
        }
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }



    void SyncCameras()
    {
        m_gameCam.nearClipPlane = m_sceneView.cameraSettings.nearClip;
        m_gameCam.fieldOfView = m_sceneView.cameraSettings.fieldOfView;
        m_gameCam.transform.position =  Vector3.LerpUnclamped(m_gameCam.transform.position, m_sceneView.camera.transform.position, Time.deltaTime*100.0f) ;//m_gameCam.transform.forward  * m_sceneView.cameraDistance;
        m_gameCam.transform.rotation = Quaternion.Lerp(m_gameCam.transform.rotation, m_sceneView.rotation, Time.deltaTime * 100.0f);
    }

    void Init()
    {
        m_sceneView = SceneView.lastActiveSceneView;
        if (m_sceneView != null)
        {
            m_gameCam = Camera.main;
        }
    }

    private void FollowRotation()
    {
        m_deltX += Input.GetAxis("Mouse X") * m_mouseSpeed;
        m_deltY -= Input.GetAxis("Mouse Y") * m_mouseSpeed;
        m_deltX = ClampAngle(m_deltX, -360, 360);
        m_deltY = ClampAngle(m_deltY, -70, 70);
        m_gameCam.transform.rotation = Quaternion.Euler(m_deltY, m_deltX, 0);
    }

    private void CursorVisible(bool b)
    {
        //Cursor.lockState = b ? CursorLockMode.Locked : Cursor.lockState = CursorLockMode.None;
        Cursor.visible = b ? false : true;
    }

    float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;
        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }

}
