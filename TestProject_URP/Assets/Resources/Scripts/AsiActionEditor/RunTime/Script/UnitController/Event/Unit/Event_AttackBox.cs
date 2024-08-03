using System.Collections.Generic;
using AsiActionEngine.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace AsiActionEngine.RunTime
{

    [System.Serializable]
    public class Event_AttackBox : IActionEventData
    {
        [SerializeField] private AttackBoxInfo mAttackBoxInfo = new AttackBoxInfo();
        [SerializeReference] private IAttackInfo mAttackInfo;


        private List<GameObject> beHitTarget = new List<GameObject>();//已经击中的对象
        private CharacterConfig config;
        private ActionStatePart _actionStatePart;
        private float curTime = 0;
        private LayerMask layerMask;

        #region property

        public AttackBoxInfo AttackBoxInfo
        {
            get { return mAttackBoxInfo; }
            set { mAttackBoxInfo = value; }
        }
        
        public IAttackInfo AttackInfo
        {
            get { return mAttackInfo; }
            set { mAttackInfo = value; }
        }
        #endregion


        public int GetEvenType() => -(int)EEvenTypeInternal.EET_AttackBox;
        
        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            // ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            // _stateMachine.OnHitObject = null;
            // _stateMachine.OnHitUnit = null;
            
            curTime = _actionState.ElapsedTime;
            _actionStatePart = _actionState;
            if (_actionState.ActionStateMachine.TryGetComponent(out CharacterConfig _config))
            {
                config = _config;
                layerMask = _config.AttackLayer;
            }
            else
            {
                layerMask = int.MaxValue;
                config = null;
            }
            beHitTarget.Clear();
        }

        public void Update(ActionStatePart _actionState, ActionMachineTime _actionTime)
        {
            int boxNumber = mAttackBoxInfo.Box.Length;
            if (boxNumber > 0)
            {
                // bool _notFind = true;
                int _findID = 0;
                for (int i = 0; i < boxNumber; i++)
                {
                    AttackBoxPart _boxPart = mAttackBoxInfo.Box[i];
                    int _PartTriggerTime = _boxPart.TriggerTime + _actionTime.TriggerTime;
                    bool _isDraw = _PartTriggerTime >= curTime;
                    if (_isDraw)
                    {
                        _isDraw = _PartTriggerTime < _actionTime.CurrentTime;
                        if (_isDraw)
                        {
                            // _notFind = false;
                            Transform _transform = _actionState.ActionStateMachine.CurUnit.transform;
                            Vector3 _startPos = _transform.TransformPoint(_boxPart.StartPos);
                            Vector3 _endPos = 
                                _startPos + _transform.TransformDirection(_boxPart.Dir) * mAttackBoxInfo.Scale.x;
                            CheckBox(_startPos, _endPos);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // if (_notFind)
                // {
                //     AttackBoxPart _boxPart = mAttackInfo.Box[_findID];
                //     Transform _transform = _actionState.ActionStateMachine.CurUnit.transform;
                //     Vector3 _startPos = _transform.TransformPoint(_boxPart.StartPos);
                //     Vector3 _endPos = 
                //         _startPos + _transform.TransformDirection(_boxPart.Dir) * mAttackInfo.Scale.x;
                //     CheckBox(_startPos, _endPos);
                // }//如果遍历所有数据后没有找到可以触发的碰撞射线，则使用最后的碰撞射线
            }//有烘焙数据，去烘焙数据检验碰撞结果
            else
            {
                if (config is not null)
                {
                    if (config.HelpPointDic.TryGetValue(mAttackBoxInfo.ReferPoint, out Transform _transform))
                    {
                        Vector3 _startPos = _transform.TransformPoint(mAttackBoxInfo.OffsetPos);
                        Quaternion _rot = _transform.rotation * Quaternion.Euler(mAttackBoxInfo.OffsetRot);
                        Vector3 _endPos = _startPos + _rot * Vector3.forward * mAttackBoxInfo.Scale.x;
                        CheckBox(_startPos, _endPos);
                    }
                    else
                    {
                        Transform _unitTrans = _actionState.ActionStateMachine.CurUnit.transform;

                        Vector3 _startPos = _unitTrans.TransformPoint(mAttackBoxInfo.OffsetPos);
                        Quaternion _rot = _unitTrans.rotation * Quaternion.Euler(mAttackBoxInfo.OffsetRot);
                        Vector3 _endPos = _startPos + _rot * Vector3.forward * mAttackBoxInfo.Scale.x;
                        CheckBox(_startPos, _endPos);
                    }
                }
            }//无烘焙数据，绑定挂点判定
            curTime = _actionTime.CurrentTime;
            // throw new System.NotImplementedException();
        }

        public void Exit(ActionStatePart _actionState, bool _interruot)
        {
            ActionStateMachine _stateMachine = _actionState.ActionStateMachine;
            _stateMachine.OnHitObject = null;
            _stateMachine.OnHitUnit = null;
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_AttackBox _event = null;
            if (_eventData != null) _event = _eventData as Event_AttackBox;
            else _event =  new Event_AttackBox();

#if UNITY_EDITOR
            _event.AttackBoxInfo = mAttackBoxInfo.Clone();
            _event.AttackInfo = mAttackInfo.Clone();
#else
            _event.AttackBoxInfo = mAttackBoxInfo;
            _event.AttackInfo = mAttackInfo;
#endif
            // _event.HitSpeed = mHitSpeed;
            // _event.Hitduration = mHitduration;

            return _event;
        }

        #region Funtion

        private void CheckBox(Vector3 _startPos,Vector3 _endPos)
        {
#if UNITY_EDITOR
            Debug.DrawLine(_startPos, _endPos, Color.red, 0.5f);
#endif

            if (mAttackBoxInfo.AttackBoxType == 0)
            {
                Collider[] _colliders =
                    Physics.OverlapCapsule(_startPos, _endPos, mAttackBoxInfo.Scale.y, layerMask);
                foreach (var VARIABLE in _colliders)
                {
                    BeHitListAdd(VARIABLE.gameObject);
                }
            } //胶囊
            else if (mAttackBoxInfo.AttackBoxType == 1)
            {
                Vector3 _dir = _endPos - _startPos;
                RaycastHit[] _colliders = Physics.RaycastAll(_startPos, _dir, _dir.magnitude, layerMask);
                foreach (var VARIABLE in _colliders)
                {
                    BeHitListAdd(VARIABLE.collider.gameObject);
                }
            } //射线
            else if (mAttackBoxInfo.AttackBoxType == 2)
            {
            } //方块
            else if (mAttackBoxInfo.AttackBoxType == 3)
            {
            } //球
        }

        //最终伤害输出
        private void BeHitListAdd(GameObject _obj)
        {
            if (!beHitTarget.Contains(_obj))
            {
                ActionStateMachine _stateMachine = _actionStatePart.ActionStateMachine;

                Vector3 _hitPoint = Vector3.zero;//命中位置
                
                if (_obj.TryGetComponent(out Unit _unit))
                {
                    _stateMachine.UnitOnHit(_obj, _unit, _hitPoint);
                    _unit.ActionStateMachine.UnitBehit(mAttackInfo, _stateMachine.CurUnit, _hitPoint);
                }
                else
                {
                    _stateMachine.UnitOnHit(_obj, null, _hitPoint);
                }
                beHitTarget.Add(_obj);
            }
        }
        #endregion
    }
}