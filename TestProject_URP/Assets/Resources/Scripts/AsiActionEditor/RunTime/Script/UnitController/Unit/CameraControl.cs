using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace AsiActionEngine.RunTime
{
    /// <summary>
    /// 针对Cinemachine相机搭建的基类，不会包含任何相机效果
    /// </summary>
    public abstract class CameraControl : MonoBehaviour
    {
        [HideInInspector] public Transform lookTarget;
        
        [HideInInspector] public Transform defaultLookTarget;
        public ActionStateMachine stateMachine;
        public Behaviour[] allCinemachine = new Behaviour[0];//相机组

        private int lastCamID = -1;
        public virtual void OnInit(Transform _lookTarget, ActionStateMachine _stateMachine, int _defaulCam)
        {
            OnReset();
            lookTarget = _lookTarget;
            stateMachine = _stateMachine;

            foreach (var _behaviour in allCinemachine)
            {
                if (_behaviour is CinemachineVirtualCameraBase _cinemachine)
                {
                    _cinemachine.Follow = _lookTarget;
                    _cinemachine.LookAt = _lookTarget;
                }
            }
            allCinemachine[_defaulCam].gameObject.SetActive(false);
            allCinemachine[_defaulCam].gameObject.SetActive(true);

            lastCamID = _defaulCam;
        }

        public virtual void OnReset()
        {
            foreach (var _behaviour in allCinemachine)
            {
                if (_behaviour is CinemachineVirtualCameraBase _cinemachine)
                {
                    _cinemachine.gameObject.SetActive(false);
                }
            }
        }
        
        public virtual void ChangeCam(int _id, Transform _camPoint)
        {
            if (lastCamID > -1)
            {
                if (lastCamID != _id)
                {
                    if (allCinemachine[_id] is CinemachineVirtualCameraBase _cinemachine)
                    {
                        _cinemachine.Follow = _camPoint;
                        _cinemachine.LookAt = _camPoint;
                    }
                    allCinemachine[_id].gameObject.SetActive(true);
                    allCinemachine[lastCamID].gameObject.SetActive(false);
                    
                    lastCamID = _id;
                }
                else
                {
                    if (allCinemachine[_id] is CinemachineVirtualCameraBase _cinemachine)
                    {
                        _cinemachine.Follow = _camPoint;
                        _cinemachine.LookAt = _camPoint;
                    }
                    allCinemachine[_id].gameObject.SetActive(false);
                    allCinemachine[_id].gameObject.SetActive(true);
                }
            }
        }
        
        public abstract void OnUpdate(float _deltaTime);
    }
}