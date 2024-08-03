using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_CharacterAddForce:IActionEventData
    {
        [SerializeField] private Vector3 mForceDir = Vector3.up;

        #region Property
        [EditorProperty("力度施加向量: ", EditorPropertyType.EEPT_Vector3)]
        public Vector3 ForceDir
        {
            get { return mForceDir; }
            set { mForceDir = value; }
        }
        #endregion
        
        public int GetEvenType() => (int)EEvenType.EET_CharacterAddForce;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            if (_stateMachine.TryGetLogic(out Ex_Update_CharacterControl _characterControl))
            {
                _characterControl.PosY = mForceDir.y;
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_CharacterAddForce _event = new Event_CharacterAddForce();

            _event.ForceDir = mForceDir;
            
            return _event;
            // throw new System.NotImplementedException();
        }
    }
}