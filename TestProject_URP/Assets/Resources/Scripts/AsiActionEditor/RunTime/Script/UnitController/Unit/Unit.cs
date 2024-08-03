using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AsiActionEngine.RunTime
{
    [RequireComponent(typeof(Animator))]
    public class Unit : MonoBehaviour
    {
        // private string m_CameraObject;
        private ActionStateMachine m_ActionStateMachine;
        public string CameraObject { get; set; }
        public ActionStateMachine ActionStateMachine => m_ActionStateMachine;


        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            // mActionStateMachine
        }

        public virtual void OnUpdate(float _time)
        {
            m_ActionStateMachine.OnUpdate(_time);
        }

        public virtual void OnLateUpdate(float _time){}

        public void SetActionStateMachine(ActionStateMachine part)
        {
            m_ActionStateMachine = part;
        }
    }
}
