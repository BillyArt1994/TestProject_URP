using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_MoveToHookPoint : IActionEventData
    {
        [SerializeField] protected float mMoveSpeed = 10.0f;

        #region Property

        public float MoveSpeed
        {
            get { return mMoveSpeed; }
            set { mMoveSpeed = value; }
        }

        #endregion
        public int GetEvenType() => (int)EEvenType.EET_MoveToHookPoint;

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetStaticLogic(out Ex_FindHookPoint _hookPoint))
            {
                if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
                {
                    
                }
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_MoveToHookPoint _event = new Event_MoveToHookPoint();

            _event.MoveSpeed = mMoveSpeed;

            return _event;
        }
    }
}