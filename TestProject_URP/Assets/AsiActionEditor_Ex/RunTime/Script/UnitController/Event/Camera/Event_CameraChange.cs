using AsiActionEngine.RunTime;
using AsiTimeLine.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace AsiActionEditor_Ex.RunTime
{
    [System.Serializable]
    public class Event_CameraChange : IActionEventData
    {
        [SerializeField] private int mEnterCam = 0;
        [SerializeField] private int mEnterPoint = (int)ECharacteLimbType.Cam_Main;
        [SerializeField] private int mExitCam = 0;
        [SerializeField] private int mExitCamPoint = (int)ECharacteLimbType.Cam_Main;
        [SerializeField] private int mActionLable = 0;
        [SerializeField] private bool mCheckLable = false;
        [SerializeField] private bool mIsContain = true;

        #region Property

        [EditorProperty("进入时触发相机： ", EditorPropertyType.EEPT_Camera)]
        public int EnterCam
        {
            get { return mEnterCam; }
            set { mEnterCam = value; }
        }
        [EditorProperty("进入相机绑定点位： ", EditorPropertyType.EEPT_CharacteLimbType)]
        public int EnterPoint
        {
            get { return mEnterPoint; }
            set { mEnterPoint = value; }
        }
        
        [EditorProperty("退出时触发相机： ", EditorPropertyType.EEPT_Camera)]
        public int ExitCam
        {
            get { return mExitCam; }
            set { mExitCam = value; }
        }
        [EditorProperty("退出相机绑定点位： ", EditorPropertyType.EEPT_CharacteLimbType)]
        public int ExitCamPoint
        {
            get { return mExitCamPoint; }
            set { mExitCamPoint = value; }
        }
        [EditorProperty("状态标签: ", EditorPropertyType.EEPT_ActionLable)]
        public int ActionLable
        {
            get { return mActionLable; }
            set { mActionLable = value; }
        }
        [EditorProperty("检查标签： ", EditorPropertyType.EEPT_Bool)]
        public bool CheckLable
        {
            get { return mCheckLable; }
            set { mCheckLable = value; }
        }

        [EditorProperty("包含： ", EditorPropertyType.EEPT_Bool)]
        public bool IsContain
        {
            get { return mIsContain; }
            set { mIsContain = value; }
        }
        

        #endregion
        
        public int GetEvenType() => (int)EEvenType.EET_CameraChange;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            if (!ActionEngineManager_Input.Instance.IsPlayer(_actionState.ActionStateMachine.CurUnit))
            {
                return;
            }

            if (_actionState.ActionStateMachine.TryGetComponent(out CharacterConfig _config))
            {
                if (_config.HelpPointDic.TryGetValue((ECharacteLimbType)mEnterPoint, out Transform _camePoint))
                {
                    if (!mCheckLable)
                    {
                        ActionEngineManager_Input.Instance.CurCamera.ChangeCam(mEnterCam, _camePoint);
                    }
                    else
                    {
                        if (_actionState.ActionStateMachine.CheckActionLable(mActionLable) == mIsContain)
                            ActionEngineManager_Input.Instance.CurCamera.ChangeCam(mEnterCam, _camePoint);
                    }
                }
            }
        }

        public void Exit(ActionStatePart _actionState, bool _interruot)
        {
            if (!ActionEngineManager_Input.Instance.IsPlayer(_actionState.ActionStateMachine.CurUnit))
            {
                return;
            }

            if (_actionState.ActionStateMachine.TryGetComponent(out CharacterConfig _config))
            {
                if (_config.HelpPointDic.TryGetValue((ECharacteLimbType)mEnterPoint, out Transform _camePoint))
                {
                    if (!mCheckLable)
                    {
                        ActionEngineManager_Input.Instance.CurCamera.ChangeCam(mExitCam, _camePoint);
                    }
                    else
                    {
                        if (_actionState.ActionStateMachine.CheckActionLable(mActionLable) == mIsContain)
                            ActionEngineManager_Input.Instance.CurCamera.ChangeCam(mExitCam, _camePoint);
                    }
                }
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_CameraChange _event = new Event_CameraChange();

            _event.EnterCam = mEnterCam;
            _event.EnterPoint = mEnterPoint;
            _event.ExitCam = mExitCam;
            _event.ExitCamPoint = mExitCamPoint;
            _event.ActionLable = mActionLable;
            _event.CheckLable = mCheckLable;
            _event.IsContain = mIsContain;

            return _event;
        }
    }
}