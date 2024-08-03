using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsiActionEngine.RunTime
{
    public partial class ActionStateMachine
    {
        private float mSetSpeedDuration = -1;
        private float mTimeScale = 1;//行为状态机时间膨胀
        private ActionStateInfo mActionStateInfo;
        
        private Dictionary<int, ActionState> mActionStates = new Dictionary<int, ActionState>();//当前角色的所有行为
        private Dictionary<string, int> mActionStateID = new Dictionary<string, int>();//角色行为的ID
        private List<ActionStatePart> mAllActionStatePart = new List<ActionStatePart>();//所有并行执行的Action
        
        public ActionStateMachine(
            Unit unit, 
            Animator _animtor, 
            ActionStateInfo _actionStateInfo
        )
        {
            CurUnit = unit;
            CurAnimator = _animtor;
            mActionStateInfo = _actionStateInfo;
            foreach (var VARIABLE in _actionStateInfo.mActionState)
            {
                if (!ActionStates.TryAdd(VARIABLE.ID, VARIABLE))
                {
                    EngineDebug.LogError($"Action字典初始化失败,出现同ID:{VARIABLE.ID}");
                }

                if (!ActionStateID.TryAdd(VARIABLE.Name, VARIABLE.ID))
                {
                    EngineDebug.LogError($"Action字典初始化失败,出现同名:{VARIABLE.Name}");
                }
            }

            for (int i = 0; i < 5; i++)
            {
                mAllActionStatePart.Add(new ActionStatePart(this, i));
            }

            Init_EventPool();
            Init(mActionStateInfo.mDefaultAction);
            EngineDebug.Log($"默认Action: {mActionStateInfo.mDefaultAction}\n受击Action：{mActionStateInfo.mHitAction}");
        }
        
        private void Init(string _actionName)
        {
            if (string.IsNullOrEmpty(_actionName))
            {
                EngineDebug.LogWarning($"Action初始化失败，当前UnitWarp未设置默认Action, 已默认加载列表内第一个Action");
            }
            else
            {
                if (ActionStateID.ContainsKey(_actionName))
                {
                    OnEnter(ActionStates[ActionStateID[_actionName]]);
                    return;
                }
                else
                {
                    EngineDebug.LogWarning($"Action初始化失败，当前数据不存在:<color=#FF3333>{_actionName}</color>, 已默认加载列表内第一个Action");
                }
            }

            foreach (var VARIABLE in ActionStates)
            {
                if (VARIABLE.Value.AnimaLayer == 0)
                {
                    OnEnter(VARIABLE.Value);
                    break;
                }
            }
        }
        
        private void OnEnter(ActionState _action)
        {
            mAllActionStatePart[0].OnEnter(_action);
        }

        private void OnUpdateState(float _deltatime)
        {
            SetSpeedUpdate(_deltatime);
            _deltatime *= mTimeScale;
            for (int i = mAllActionStatePart.Count - 1; i >= 0; i--)
            {
                mAllActionStatePart[i].OnUpdate(_deltatime);
            }
            Update_Extend(_deltatime);
            LateUpdate_Extend(_deltatime);
        }
        
        //设置输入
        private void OnSetMoveInput(Vector3 _move)
        {
            PlayerInputMoveDir = _move;
            IsMoveInput = true;
        }
        private void OnSetHeadRot(Quaternion _rot)
        {
            CamRot = _rot;
        }
        private void OnSetMoveInputStop()
        {
            IsMoveInput = false;
        }

        private void OnSetSpeed(float _speed, float _duration)
        {
            mSetSpeedDuration = _duration;
            mTimeScale = _speed;
            if (CurAnimator is not null)
            {
                CurAnimator.speed = _speed;
            }
        }

        private void SetSpeedUpdate(float _deltatime)
        {
            if (mSetSpeedDuration > 0)
            {
                mSetSpeedDuration -= _deltatime;
                if (mSetSpeedDuration <= 0)
                {
                    mTimeScale = 1;
                    if (CurAnimator is not null)
                    {
                        CurAnimator.speed = 1;
                    }
                }
            }
        }
        
        // 注册按键按下
        private void OnSetKeyDown(string _keyName)
        {
            foreach (var VARIABLE in mAllActionStatePart)
            {
                if (VARIABLE.ActionEnble)
                {
                    VARIABLE.SetKeyDown(_keyName);
                }
            }
        }
        // 注册按键抬起
        private void OnSetKeyUp(string _keyName)
        {
            foreach (var VARIABLE in mAllActionStatePart)
            {
                if (VARIABLE.ActionEnble)
                {
                    VARIABLE.SetKeyUp(_keyName);
                }
            }
        }

        private void OnSendKeyDown(string _keyName)
        {
            foreach (var VARIABLE in mAllActionStatePart)
            {
                if (VARIABLE.ActionEnble)
                {
                    VARIABLE.NowInputDownKey = _keyName;
                    // VARIABLE.SetKeyDown(_keyName);
                }
            }
        }

        private ActionState OnGetActionState(string _name)
        {
            if (ActionStateID.TryGetValue(_name, out int _id))
            {
                if (ActionStates.TryGetValue(_id, out ActionState _action))
                {
                    return _action;
                }
            }
            else
            {
                EngineDebug.LogError($"不存在这个Action: {_name}");
            }
            return null;
        }

        private void OnChangeAction(string _name, int _mixTime, int _offsetTime)
        {
            ActionState _actionState = OnGetActionState(_name);
            if (_actionState is null) return;
            mAllActionStatePart[_actionState.AnimaLayer].ChangeState(_name, _mixTime, _offsetTime);
        }
    }
}