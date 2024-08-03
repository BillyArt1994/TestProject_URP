using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_FindPoint : IActionEventData
    {
        [SerializeField] protected EInteraetPointType mPointType;
        
        #region property

        [EditorProperty("点位类型", EditorPropertyType.EEPT_Enum)]
        public EInteraetPointType PointType
        {
            get { return mPointType; }
            set { mPointType = value; }
        }
        

        #endregion
        public int GetEvenType() => (int)EEvenType.EET_FindPoint;

        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
        }

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            TryFindPoint(_actionState.ActionStateMachine,out Vector3 _pos,out Vector3 _dir);
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_FindPoint _event = new Event_FindPoint();

            _event.PointType = mPointType;
            
            return _event;
        }

        private bool TryFindPoint(ActionStateMachine _stateMachine,out Vector3 _pos,out Vector3 _normal)
        {
            if (mPointType == EInteraetPointType.HookLock)
            {
                if(_stateMachine.TryGetStaticLogic(out Ex_FindHookPoint _exFindPoint))
                {
                    if (_exFindPoint.IsFindPoint)
                    {
                        Debug.DrawRay(_exFindPoint.mFindPos, Vector3.up * 3, Color.red);
                    }
                }
            }

            _pos = Vector3.zero;
            _normal = Vector3.up;
            return false;
        }
        
        // public struct PosAndRot
        // {
        //     public Vector3 Pos;
        //     public Quaternion Rot;
        //
        //     public PosAndRot(Vector3 _pos, Quaternion _rot)
        //     {
        //         Pos = _pos;
        //         Rot = _rot;
        //     }
        // }
    }
}