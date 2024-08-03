using System.Collections.Generic;
using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_ActionDebug : IActionEventData
    {
        [SerializeField] private string mDebugName = "";

        #region property
        [EditorProperty("Debug标题", EditorPropertyType.EEPT_String)]
        public string DebugName
        {
            get { return mDebugName; }
            set { mDebugName = value; }
        }
        #endregion
        
        public int GetEvenType() => (int)EEvenType.EET_ActionDebug;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            #if UNITY_EDITOR
            string _eventDebug = "";
            for (int i = 0; i < _actionState.CurrentActionEvents.Count; i++)
            {
                ActionEvent _actionEvent = _actionState.CurrentActionEvents[i];
                _eventDebug += $"  {i}、 {_actionEvent.EventData.GetType().Name}  D:{_actionEvent.Duration}\n";
            }

            string _interruptDebug = "";
            for (int i = 0; i < _actionState.CurActionInterrupt.Count; i++)
            {
                ActionInterrupt _actionInterrupt = _actionState.CurActionInterrupt[i];
                _interruptDebug += $"  {i}、 {_actionInterrupt.ActionName}\n";
            }

            string _actionLable = "";
            List<int> _ActionLableList = _actionState.ActionStateMachine.GetActionLableList;
            for (int i = 0; i < _ActionLableList.Count; i++)
            {
                _actionLable += $"{i}: {_actionState.ActionStateMachine.GetActionLable(_ActionLableList[i])}\n";
            }

            EngineDebug.Log(
                "ActionDebug" +
                $"<color=#FFCC00>{mDebugName}</color>\n" +
                $"Debug信息来自 {_actionState.mCurActionLayer} 层级\n" +
                $"Action状态层为 {_actionState.GetActionType()} \n" +
                $"当前层级时间 {_actionState.ElapsedTime} ms\n" +
                $"事件数量: {_actionState.CurrentActionEvents.Count} \n" + 
                _eventDebug + "\n" + 
                $"打断轨数量: {_actionState.CurActionInterrupt.Count}\n" + 
                _interruptDebug + "\n" + 
                $"当前状态标签数量：{_ActionLableList.Count}\n" + 
                _actionLable + "\n\n"
            );
            #endif
        }


        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_ActionDebug actionDebug = new Event_ActionDebug();
            actionDebug.DebugName = mDebugName;
            return actionDebug;
        }
    }
}