using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [System.Serializable]
    public class Event_WeaponPointChange : IActionEventData
    {
        [SerializeField] private bool mIsRightWeapon = true;
        [SerializeField] private int mLimbPointType = 0;
        [SerializeField] private bool mAlignToPoint = true;


        #region Property
        [EditorProperty("右手武器", EditorPropertyType.EEPT_Bool)]
        public bool IsRightWeapon
        {
            get { return mIsRightWeapon; }
            set { mIsRightWeapon = value; }
        }
        [EditorProperty("目标挂点", EditorPropertyType.EEPT_CharacteLimbType)]
        public int LimbPointType
        {
            get { return mLimbPointType; }
            set { mLimbPointType = value; }
        }
        [EditorProperty("对齐至对应挂点", EditorPropertyType.EEPT_Bool)]
        public bool AlignToPoint
        {
            get { return mAlignToPoint; }
            set { mAlignToPoint = value; }
        }
        #endregion
        public int GetEvenType() => (int)EEvenType.EET_WeaponPointChange;

        private ECharacteLimbType _limbType;
        public void Enter(ActionStatePart _actionState, bool _isSingle)
        {
            _limbType = (ECharacteLimbType)mLimbPointType;
            if (_isSingle || !mAlignToPoint)
            {
                if (_actionState.ActionStateMachine.TryGetComponent(out CharacterConfig _config))
                {
                    Transform _weapon = mIsRightWeapon ? _config.WeaponR : _config.WeaponL;
                    if (_weapon is null)
                    {
                        return;
                    }
                    
                    if (_config.HelpPointDic.TryGetValue(_limbType, out Transform _target))
                    {
                        _weapon.SetParent(_target);
                        if (mAlignToPoint)
                        {
                            _weapon.localPosition = Vector3.zero;
                            _weapon.rotation = _target.rotation;
                        }
                    }
                }
            }
        }

        public IActionEventData Clone(IActionEventData _eventData)
        {
            Event_WeaponPointChange _event = new Event_WeaponPointChange();

            _event.IsRightWeapon = mIsRightWeapon;
            _event.LimbPointType = mLimbPointType;
            _event.AlignToPoint = mAlignToPoint;

            return _event;
        }
    }
}