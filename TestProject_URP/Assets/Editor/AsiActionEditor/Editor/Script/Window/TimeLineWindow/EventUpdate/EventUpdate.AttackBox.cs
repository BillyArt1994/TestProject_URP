using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiActionEngine.Editor
{
    public partial class EventUpdate
    {
        private void Update_EventAttackBox(int _time, Event_AttackBox _eventPlayAnim, EditorActionEvent _actionEvent)
        {
            AttackBoxInfo mAttackBoxInfo = _eventPlayAnim.AttackBoxInfo;
            if (ResourcesWindow.Instance.GetRole().TryGetComponent(out CharacterConfig _config))
            {
                // Debug.Log("绘制攻击盒22: " + _eventPlayAnim.AttackInfo.ReferPoint);
                AttackBoxInfo attackBoxInfo = _eventPlayAnim.AttackBoxInfo;
                int _BoxDrawType = ActionEventDescripted.Instance.BoxDrawType;
                if (_BoxDrawType == 2 || _BoxDrawType == 0)
                {
                    DrawDefaultBox(attackBoxInfo, _config, _actionEvent, _time);
                }
                if (_BoxDrawType == 2 || _BoxDrawType == 1)
                {
                    DrawBekaBox(attackBoxInfo, _actionEvent.TriggerTime, _time);
                    // DrawDefaultBox(_attackInfo, _config, _actionEvent, _time);
                }
                
            }
        }

        private void DrawDefaultBox(AttackBoxInfo attackBoxInfo,CharacterConfig _config,EditorActionEvent _actionEvent,int _time)
        {
            if (_config.HelpPointDic.TryGetValue(attackBoxInfo.ReferPoint, out Transform _target))
            {
                // Debug.Log("绘制攻击盒33");
                Vector3 _startPos = _target.TransformPoint(attackBoxInfo.OffsetPos);
                Quaternion _dir = (_target.rotation * Quaternion.Euler(attackBoxInfo.OffsetRot) );
                Vector3 _endPos = _startPos + (_dir * Vector3.forward * attackBoxInfo.Scale.x);

                //常规绘制
                bool _isDraw = _time >= _actionEvent.TriggerTime;
                if (_isDraw) _isDraw = _time <= _actionEvent.TriggerTime + _actionEvent.Duration;
                if (_isDraw)
                {
                    DrawBox(_startPos, _endPos, _dir * Vector3.up, attackBoxInfo);
                }
            }
        }

        private void DrawBekaBox(AttackBoxInfo attackBoxInfo, int _triggerTime, int _time)
        {
            int _curTrigerTime = _time - _triggerTime;

            if (_curTrigerTime >= 0)
            {
                for (int i = 0; i < attackBoxInfo.Box.Length; i++)
                {
                    AttackBoxPart _boxPart = attackBoxInfo.Box[i];
                    bool _isDraw = _curTrigerTime >= _boxPart.TriggerTime;
                    if (_isDraw)
                    {
                        int _life = ActionEventDescripted.Instance.BakerBoxLife;
                        if (i == attackBoxInfo.Box.Length - 1)
                        {
                            _isDraw = _curTrigerTime <= _boxPart.TriggerTime + _life;
                        }//末端
                        else
                        {
                            _isDraw = _curTrigerTime < attackBoxInfo.Box[i + 1].TriggerTime + _life;
                        }//常规

                        if (_isDraw)
                        {
                            Transform _transform = ResourcesWindow.Instance.GetRole().transform;
                            Vector3 _startPos = _transform.TransformPoint(_boxPart.StartPos);
                            Vector3 _endPos = _startPos + _transform.TransformDirection(_boxPart.Dir) 
                                * attackBoxInfo.Scale.x;
                            DrawBox(_startPos, _endPos, Vector3.up, attackBoxInfo);
                        }
                    }
                }
            }
        }

        private void DrawBox(Vector3 _startPos, Vector3 _endPos, Vector3 _AxisY, AttackBoxInfo attackBoxInfo)
        {
            if (attackBoxInfo.AttackBoxType == 0)
            {
                ScenceDraw.Instance.CreactDrawBox(new DrawCapsule(_startPos, _endPos, _AxisY, attackBoxInfo.Scale.y));
            } //胶囊
            else if (attackBoxInfo.AttackBoxType == 1)
            {
                ScenceDraw.Instance.CreactDrawBox(new DrawLine(_startPos, _endPos));
            } //射线
            else if (attackBoxInfo.AttackBoxType == 2)
            {
                ScenceDraw.Instance.CreactDrawBox(new DrawLine(_startPos, _endPos));
            } //方块
            else if (attackBoxInfo.AttackBoxType == 3)
            {
                ScenceDraw.Instance.CreactDrawBox(new DrawLine(_startPos, _endPos));
            } //球
        }
    }
}