using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public class Ex_2D_Ground : StaticActionLogics
    {
        public bool IsGround { get; private set; } = false;

        public override void OnUpdate(ActionStateMachine _actionState)
        {
            Vector2 _pos = ActionEngineManager_Input.Instance.Player.transform.position;
            float _offset = 10;
            IsGround = Physics2D.Raycast(_pos + Vector2.up * _offset, Vector2.down, _offset + 10);
        }
    }
}