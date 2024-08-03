using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public class Ex_BarrierData : StaticActionLogics
    {
        public Vector3 mInteractPoint = Vector3.zero;//交互点位置
        public Quaternion mInteractRot = Quaternion.identity;//交互点方向

        public float mGetHeight { get; private set; }
        public float mGetDistance { get; private set; }

        private float mStartHeight = 0.5f;
        private float mMaxHeight = 3;
        private float mInteractDis = 2;

        private Vector3 mInteractPoint_p = Vector3.zero;
        private Quaternion mInteractRot_p = Quaternion.identity;

        public override void OnUpdate(ActionStateMachine _actionState)
        {
            Transform _transform = _actionState.CurUnit.transform;

            mGetDistance = int.MaxValue;
            mGetHeight = int.MinValue;
            Vector3 _startPos = _transform.TransformPoint(Vector3.up * mStartHeight);
            if (Physics.Raycast(_startPos, _transform.forward, out RaycastHit _hit, mInteractDis))
            {
                Debug.DrawLine(_startPos,_hit.point,Color.red);
                
                if(Vector3.Angle(_hit.normal,Vector3.up) < 80)return;
                
                mGetDistance = _hit.distance;

                _startPos = _hit.point + _transform.forward * 0.02f;
                _startPos.y = mMaxHeight + _transform.position.y;
                if (Physics.Raycast(_startPos, Vector3.down, out RaycastHit _hit2, mMaxHeight))
                {
                    Debug.DrawLine(_startPos,_hit2.point, Color.red);
                    mGetHeight = mMaxHeight - _hit2.distance;
                    mInteractPoint_p = _hit2.point;
                    mInteractRot_p = Quaternion.LookRotation(_hit2.normal, Vector3.up) * Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    Debug.DrawRay(_startPos,Vector3.down * mInteractDis,Color.green);
                }
            }
            else
            {
                Debug.DrawRay(_startPos,_transform.forward * mInteractDis,Color.green);
            }
        }

        public void SetInteractPoint()
        {
            mInteractPoint = mInteractPoint_p;
            mInteractRot = mInteractRot_p;
        }
    }
}