using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsiActionEngine.RunTime
{
    public partial class ActionStateMachine
    {
        public readonly float HoldKeyIntervalTime = 0.2f;//长按判定时间

        public bool IsMoveInput { get; private set; }//是否持续输入位移
        public Vector3 PlayerInputMoveDir { get; private set; }//玩家输入的方向
        public Quaternion CamRot { get; private set; }//注视的朝向(一般为相机的 Quaternion )
        public Animator CurAnimator { get; private set; }//当前更新的动画
        public Unit CurUnit { get; private set; }//当前绑定单位
        public Unit OnHitUnit { get; set; }//命中的单位
        public GameObject OnHitObject { get; set; }//命中的对象
        public ActionStateInfo ActionStateInfo => mActionStateInfo;
        public List<ActionStatePart> AllActionStatePart => mAllActionStatePart;//所有层级
        public Dictionary<int, ActionState> ActionStates => mActionStates;
        public Dictionary<string, int> ActionStateID => mActionStateID;
        public void UnitBehit(IAttackInfo _attackInfo, Unit _attacker, Vector3 _hitPoint) 
            => OnBeHit(_attackInfo,_attacker, _hitPoint);

        public void UnitOnHit(GameObject _BeHit, Unit _BeHitUnit, Vector3 _hitPoint)
            => OnOnHit(_BeHit, _BeHitUnit, _hitPoint);
        
        public void OnUpdate(float _deltaTime) => OnUpdateState(_deltaTime);
        public void SetKeyDown(string _keyName) => OnSetKeyDown(_keyName);
        public void SetKeyUp(string _keyName) => OnSetKeyUp(_keyName);
        public void SendKeyDown(string _keyName) => OnSendKeyDown(_keyName);
        
        //Animator
        public void SetAnimatorFloat(string _name, float _value) => OnSetAnimatorFloat(_name, _value);
        public void SetAnimatorInt(string _name, int _value) => OnSetAnimatorInt(_name, _value);


        public void ChangeAction(string _name, int _mixTime, int _setTime) => OnChangeAction(_name, _mixTime, _setTime);
        
        /// <summary>
        /// 设置状态机运行速度(例如放慢或者加快角色速度等)
        /// </summary>
        /// <param name="_speed"></param>
        /// <param name="_duration"></param>
        public void SetSpeed(float _speed, float _duration = -1f) => OnSetSpeed(_speed, _duration);
        
        /// <summary>
        /// 注册方向输入
        /// </summary>
        /// <param name="_dir">输入的向量</param>
        public void SetMoveInput(Vector3 _dir) => OnSetMoveInput(_dir);
        
        /// <summary>
        /// 注册方向停止输入
        /// </summary>
        public void SetMoveInputStop() => OnSetMoveInputStop();
        
        /// <summary>
        /// 注册朝向（一般情况请传入相机的 Quaternion ）
        /// </summary>
        /// <param name="_rot">朝向</param>
        public void SetCamRot(Quaternion _rot) => OnSetHeadRot(_rot);
        public Quaternion GetCamRot() => CamRot;

    }
}