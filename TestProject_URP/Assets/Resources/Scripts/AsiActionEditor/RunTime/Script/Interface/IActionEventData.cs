using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsiActionEngine.RunTime
{
    public interface IActionEventData
    {
        void Enter(ActionStatePart _actionState, bool _isSingle)
        {
        }
        void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)//deltaTime(ms)
        {
        }
        void Exit(ActionStatePart _actionState, bool _interruot)//interruot 是否因为打断轨退出
        {
        }
        int GetEvenType();
        IActionEventData Clone(IActionEventData _eventData);
    }

    public struct ActionMachineTime
    {
        /// <summary>
        /// 每帧间隔时间
        /// </summary>
        public readonly float Deltatime;
        /// <summary>
        /// 当前状态机在当前状态下已经度过的时间
        /// </summary>
        public readonly float CurrentTime;
        /// <summary>
        /// 当前事件的触发时间
        /// </summary>
        public readonly int TriggerTime;
        /// <summary>
        /// 当前事件的持续时间
        /// </summary>
        public readonly int Duration;

        public ActionMachineTime(float _deltatime,float _currentTime,int _triggerTime,int _duration)
        {
            Deltatime = _deltatime;
            CurrentTime = _currentTime;
            TriggerTime = _triggerTime;
            Duration = _duration;
        }
    }
}
