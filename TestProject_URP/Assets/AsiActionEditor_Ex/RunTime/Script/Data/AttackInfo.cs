using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiActionEditor_Ex.RunTime
{
    [System.Serializable]
    public class AttackInfo : IAttackInfo
    {
        [SerializeField] private float mHitSpeed = 0.1f;
        [SerializeField] private float mHitduration = 0.2f;

        #region Property
        [EditorProperty("命中僵直: ", EditorPropertyType.EEPT_Float)]
        public float HitSpeed
        {
            get { return mHitSpeed; }
            set { mHitSpeed = value; }
        }
        
        [EditorProperty("僵直持续时间(s): ", EditorPropertyType.EEPT_Float)]
        public float Hitduration
        {
            get { return mHitduration; }
            set { mHitduration = value; }
        }
        #endregion
        public IAttackInfo Clone()
        {
            AttackInfo _attackInfo = new AttackInfo();

            _attackInfo.HitSpeed = mHitSpeed;
            _attackInfo.Hitduration = mHitduration;

            return _attackInfo;
        }

        //受击参数，受击者的行为状态机，攻击者，受击点
        public void BeHit(IAttackInfo _IattackInfo, ActionStateMachine _stateMachine, Unit _attacker, Vector3 _hitPoint)
        {
            AttackInfo _attackInfo = (AttackInfo)_IattackInfo;
            // EngineDebug.Log
            // (
            //     $"受击对象: {_stateMachine.CurUnit.name}" + "\n" +
            //     $"攻击来源: {_attacker.name}" + "\n" +
            //     $"接受受击(HitSpeed): {_attackInfo.HitSpeed}" + "\n" +
            //     $"接受受击(mHitduration): {_attackInfo.mHitduration}" + "\n" +
            //     ""
            // );
            
            //将数值用于攻击者顿帧
            _attacker.ActionStateMachine.SetSpeed(_attackInfo.HitSpeed,_attackInfo.mHitduration);
        }
    }
}