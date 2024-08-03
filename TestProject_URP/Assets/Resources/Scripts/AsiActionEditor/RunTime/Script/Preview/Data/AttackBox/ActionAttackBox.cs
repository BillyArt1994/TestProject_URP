using UnityEngine;
using UnityEngine.Serialization;

namespace AsiActionEngine.RunTime
{
    
    [System.Serializable]
    public class AttackBoxInfo
    {
        public AttackBoxPart[] Box = new AttackBoxPart[0];
        public int AttackBoxType;

        public ECharacteLimbType ReferPoint;
        public Vector3 OffsetPos;
        public Vector3 OffsetRot;
        public Vector3 Scale = new Vector3(1, 0.2f, 0);//X长度,Y半径

        public AttackBoxInfo Clone()
        {
            AttackBoxInfo _attackBoxInfo = new AttackBoxInfo();

            _attackBoxInfo.Box = Box;
            _attackBoxInfo.AttackBoxType = AttackBoxType;

            _attackBoxInfo.ReferPoint = ReferPoint;
            _attackBoxInfo.OffsetPos = OffsetPos;
            _attackBoxInfo.OffsetRot = OffsetRot;
            _attackBoxInfo.Scale = Scale;

            return _attackBoxInfo;
        }
    }
    
    [System.Serializable]
    public struct AttackBoxPart
    {
        public Vector3 StartPos;
        public Vector3 Dir;
        public int TriggerTime;

        public AttackBoxPart(Vector3 _StartPos, Vector3 _dir, int _TriggerTime)
        {
            StartPos = _StartPos;
            Dir = _dir;
            TriggerTime = _TriggerTime;
        }
    }
}