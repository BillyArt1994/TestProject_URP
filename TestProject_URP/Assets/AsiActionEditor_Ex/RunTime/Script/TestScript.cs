using System;
using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public class TestScript : MonoBehaviour
    {
        public string InputName;
        public float intervalTime = 2;

        private float _curIntervalTime;
        private Unit _player;
        private void Start()
        {
            _player = GetComponent<Unit>();
        }

        private void Update()
        {
            float _deltaTime = Time.deltaTime;

            _curIntervalTime += _deltaTime;
            if (_curIntervalTime > intervalTime)
            {
                _player.ActionStateMachine.SendKeyDown(InputName);
                _curIntervalTime = 0;
            }
        }
    }
}