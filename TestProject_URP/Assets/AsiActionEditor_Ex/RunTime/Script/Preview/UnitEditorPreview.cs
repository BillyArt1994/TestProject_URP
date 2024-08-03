using System;
using System.Diagnostics;
using AsiActionEngine.RunTime;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace AsiTimeLine.RunTime
{
    /// <summary>
    /// 此脚本仅用于编辑器运行角色操作的单机环境
    /// 但是就算Runtime环境挂载这个脚本也不会出现任何问题
    /// </summary>
    public class UnitEditorPreview : ActionPreviewMark
    {
        private float mCamChangeTime;

        public bool mIsPlayer
        {
            // get { return IsPlayer; }
            set
            {
                if (value)
                {
                    mCamChangeTime = 1;//相机过渡时间
                }
                IsPlayer = value;
            }
        }
        
        [HideInInspector]
        public Unit mUnit;

        private void Start()
        {
            ActionEngineManager_Input.Instance.CreactGameManager();

            if (string.IsNullOrEmpty(ActionName))
            {
                return;
            }

            mUnit = GetComponent<Unit>();
            mUnit.CameraObject = DefaltCamera;

            ActionEngineManager_Unit.Instance.GetActionList(ActionName, list =>
            {

                ActionStateMachine statePart = 
                    new ActionStateMachine(mUnit, GetComponent<Animator>(), list);
                mUnit.SetActionStateMachine(statePart);
                mUnit.Init();
                
                ActionEngineManager_Unit.Instance.AddUnit(mUnit);
                
                if (IsPlayer)
                {
                    ActionEngineManager_Input.Instance.ChangePlayer(mUnit); //注册输入系统的操作对象
                }

            });
        }
        
        private void Update()
        {
            if (!IsPlayer)
            {
                return;
            }

            float _deltatime = Time.deltaTime;

            ActionEngineManager_Input _input = ActionEngineManager_Input.Instance;
            if (mCamChangeTime > 0)
            {
                mCamChangeTime -= _deltatime;

                _input.CamOffsetPos = Vector3.Lerp(_input.CamOffsetPos, CamOffsetPos, 7 * _deltatime);
                _input.CamGlobalOffsetPos = Vector3.Lerp(_input.CamGlobalOffsetPos, Vector3.zero, 7 * _deltatime);

                if (mCamChangeTime <= 0)
                {
                    _input.CamGlobalOffsetPos = Vector3.zero;
                }
            }
            else
            {
                _input.CamOffsetPos = CamOffsetPos;
                _input.SetCamRotSpeed(CamRotSpeed);
            }
        }
    }
}