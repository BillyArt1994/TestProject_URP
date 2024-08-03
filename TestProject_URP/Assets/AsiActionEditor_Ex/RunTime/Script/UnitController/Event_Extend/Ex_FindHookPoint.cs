using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public class Ex_FindHookPoint : StaticActionLogics
    {
        public bool IsFindPoint;
        public Vector3 FindPos;
        public Vector3 FindDir;
        public Vector3 mFindPos;
        public Vector3 mFindDir;
        public override void OnUpdate(ActionStateMachine _actionState)
        {
            IsFindPoint = false;
            float _maxHeight = 10;
            if (_actionState.TryGetComponent(out CharacterConfig _config))
            {
                if (_config.HelpPointDic.TryGetValue(ECharacteLimbType.Cam_Main, out Transform _transform))
                {
                    if (Physics.Raycast(_transform.position, _transform.forward, out RaycastHit _hit, 100))
                    {
                        Vector3 _startPos = _hit.point;
                        _startPos += Quaternion.Euler(0,_transform.eulerAngles.y,0) * Vector3.forward * 0.02f;
                        _startPos += Vector3.up * _maxHeight;

                        if (Physics.Raycast(_startPos, Vector3.down, out RaycastHit _hit2, _maxHeight - 0.2f))
                        {
                            mFindPos = _hit2.point;
                            mFindDir = _hit.normal;
                            IsFindPoint = true;
                            // return true;
                        }
                        else
                        {
                            // Debug.DrawRay(_hit.point, _hit.normal, Color.blue, 5);
                        }
                    }
                }
            }
            // throw new System.NotImplementedException();
        }

        public void SetFinishPoint()
        {
            FindPos = mFindPos;
            FindDir = mFindDir;
        }
    }
}