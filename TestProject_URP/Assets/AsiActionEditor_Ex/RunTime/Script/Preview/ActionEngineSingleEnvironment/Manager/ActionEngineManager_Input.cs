﻿using System;
using System.Collections.Generic;
using System.IO;
using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    public partial class ActionEngineManager_Input
    {
        #region Instance
        private static ActionEngineManager_Input _instance;
        public static ActionEngineManager_Input Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionEngineManager_Input();
                }

                return _instance;
            }
        }

        private ActionEngineManager m_GameManager = null;
        public ActionEngineManager CreactGameManager()
        {
            if (m_GameManager == null)
            {
                m_GameManager = GameObject.FindObjectOfType<ActionEngineManager>();
                if (m_GameManager == null)
                {
                    GameObject _manager = new GameObject();
                    _manager.name = "GameManager";
                    m_GameManager = _manager.AddComponent<ActionEngineManager>();
                }
            }
            return m_GameManager;
        }
        #endregion

        #region 结构体
        //连击元素
        private class ComboKeyElement
        {
            public int keyID;
            public float keyTime;

            public ComboKeyElement(int _keyID,float _keyTime)
            {
                keyID = _keyID;
                keyTime = _keyTime;
            }
        }
        

        #endregion
        
        #region 连击表有效按键
        private readonly KeyCode[] KeyMap = new[]
        {
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.I,
            KeyCode.O,
            KeyCode.P,
            KeyCode.U,
            KeyCode.Space,
        };

        private readonly int[] MoustButtonMap = new[]
        {
            0,
            1
        };
        #endregion

        private const float ComboTime = 0.5f;//连击按键最大等待时间
        
        private int selectID;
        private int comboMaxLenth;
        private bool mouseDis = true;
        private float mCamRotX = 0f;
        private float mCamRotY = 0f;
        private float mCamRotX_L = 0f;
        private float mCamRotY_L = 0f;
        private float mRotSpeed = 1;
        private readonly float mRotSpeed_L = 18;
        private Vector3 mInputDir;
        private InputModuleInfo mInputModuleInfo;
        
        private Dictionary<KeyCode, string> ActionCheckKey = new Dictionary<KeyCode, string>();
        private Dictionary<int, string> ActionCheckMouseKey = new Dictionary<int, string>();
        private List<KeyCombinations> ActionCombo = new List<KeyCombinations>();
        private List<ComboKeyElement> ActionComboElementList = new List<ComboKeyElement>();
        
        public CameraControl CurCamera;
        public Vector3 CamOffsetPos;
        public Vector3 CamGlobalOffsetPos;

        public Unit Player { get; private set; }
        public Camera PlayerCam { get; private set; }

        public void ChangePlayer(Unit _unit)
        {
            if (Player is not null)
            {
                if (Player.TryGetComponent(out UnitEditorPreview _lasteditorPreview))
                {
                    _lasteditorPreview.mIsPlayer = false;
                }
                CamGlobalOffsetPos = Player.transform.position - _unit.transform.position;
            }

            if (_unit.TryGetComponent(out UnitEditorPreview _editorPreview))
            {
                _editorPreview.mIsPlayer = true;
            }

            ChangePlayerCamera(Player, _unit);
            Player = _unit;
        }

        public bool IsPlayer(Unit _unit)
        {
            return Player == _unit;
        }

        public void SetCamRotSpeed(float _speed)
        {
            mRotSpeed = _speed;
        }
        
        public void Init()
        {
            //注册相机
            PlayerCam = Camera.main;
            
            //加载配置表
            LoadInputInfo();
            
            //注册按键检测
            SwitchInputInfo(selectID);
        }
        public void Update(float _deltaTime)
        {
            if (Player is null)
            {
                return;
            }
            
            CheckKey();
            if (CurCamera is not null)
                CurCamera.OnUpdate(_deltaTime);
            // SetCam(_deltaTime);
            Player.ActionStateMachine.SetCamRot(Quaternion.Euler(mCamRotY,mCamRotX,0));//playerCam.transform.rotation
            
            if (Input.GetMouseButtonDown(1))
            {
                Time.timeScale = 0.1f;
            }

            if (Input.GetMouseButtonUp(1))
            {
                Time.timeScale = 1;
            }
        }


        private void CheckKey()
        {
            //鼠标输入
            if (Input.GetMouseButtonDown(2))
            {
                SetMoseDisPlay(!mouseDis);
            }

            if (!mouseDis)
            {
                mCamRotX += Input.GetAxis("Mouse X") * mRotSpeed;
                mCamRotY -= Input.GetAxis("Mouse Y") * mRotSpeed;
                mCamRotY = Mathf.Clamp(mCamRotY, -80, 60);
            }
            else
            {
                return;
            }
            
            //位置输入
            mInputDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) mInputDir.z++;
            if (Input.GetKey(KeyCode.S)) mInputDir.z--;
            if (Input.GetKey(KeyCode.A)) mInputDir.x--;
            if (Input.GetKey(KeyCode.D)) mInputDir.x++;
            if (mInputDir != Vector3.zero)
            {
                Player.ActionStateMachine.SetMoveInput(mInputDir);
            }
            else
            {
                Player.ActionStateMachine.SetMoveInputStop();
            }

            //连击输入判断
            if (!CheckComboKey())
            {
                //常规键鼠输入
                foreach (var _KeyValue in ActionCheckKey)
                {
                    if (Input.GetKeyDown(_KeyValue.Key)) Player.ActionStateMachine.SetKeyDown(_KeyValue.Value);
                    if (Input.GetKeyUp(_KeyValue.Key)) Player.ActionStateMachine.SetKeyUp(_KeyValue.Value);
                }

                foreach (var _KeyValue in ActionCheckMouseKey)
                {
                    if (Input.GetMouseButtonDown(_KeyValue.Key)) Player.ActionStateMachine.SetKeyDown(_KeyValue.Value);
                    if (Input.GetMouseButtonUp(_KeyValue.Key)) Player.ActionStateMachine.SetKeyUp(_KeyValue.Value);
                }
            }
        }

        public void SetMoseDisPlay(bool _state)
        {
            mouseDis = _state;
            if (mouseDis)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
        private void SetCam(float _deltatime)
        {
            if (!Player)
            {
                return;
            }

            mCamRotX_L = Mathf.Lerp(mCamRotX_L, mCamRotX, mRotSpeed_L * _deltatime);
            mCamRotY_L = Mathf.Lerp(mCamRotY_L, mCamRotY, mRotSpeed_L * _deltatime);

            Transform _camTrans = PlayerCam.transform;
            _camTrans.rotation = Quaternion.Euler(mCamRotY_L, mCamRotX_L, 0);
            _camTrans.position = Player.transform.TransformPoint(0, CamOffsetPos.y, 0) + CamGlobalOffsetPos;
            _camTrans.Translate(CamOffsetPos.x, 0, CamOffsetPos.z);
        }

        private bool CheckComboKey()
        {
            float _nowTime = Time.time;

            foreach (var _keyCode in KeyMap)
            {
                if (Input.GetKeyDown(_keyCode))
                {
                    // EngineDebug.Log($"按下了: {_keyCode}");
                    AddComboElement((int)_keyCode, _nowTime);
                    break;
                }
            }//检查键盘按键

            foreach (var _mouseButton in MoustButtonMap)
            {
                if (Input.GetMouseButtonDown(_mouseButton))
                {
                    // EngineDebug.Log($"按下了: {(_mouseButton == 0 ? "鼠标左键" : "鼠标右键")}");
                    AddComboElement(-(_mouseButton + 1), _nowTime);
                    break;
                }
            }//检查鼠标按键
            
            for (int i = 0; i < ActionComboElementList.Count; i++)
            {
                ComboKeyElement _ActionComboElement = ActionComboElementList[i];
                float _deltaTime = _nowTime - _ActionComboElement.keyTime;
                if (_deltaTime > ComboTime)
                {
                    ActionComboElementList.RemoveAt(0);
                    i--;
                }
                else
                {
                    break;
                }
            }//清理过时连击按键
            
            bool _isComboKey = false;//是完整的连击列表！
            foreach (var _keyCombo in ActionCombo)
            {
                int _findFastKey = -1;//找到了连击列表第一个按键
                foreach (var _key in _keyCombo.keyGroup)
                {
                    if (_findFastKey < 0)
                    {
                        for (int i = 0; i < ActionComboElementList.Count; i++)
                        {
                            if (_key == ActionComboElementList[i].keyID)
                            {
                                _findFastKey = i;
                                break;
                            }
                        }
                    }//寻找连击列表中的第一个按键
                    else
                    {
                        if (ActionComboElementList.Count < _findFastKey + _keyCombo.keyGroup.Count)
                        {
                            // EngineDebug.Log("找到了第一个按键，但是长度超出");
                            break;
                        }//检查已有的连击列表数量是否足以支撑当前完整列表

                        _isComboKey = true;
                        for (int i = 1; i < _keyCombo.keyGroup.Count; i++)
                        {
                            if (_keyCombo.keyGroup[i] != ActionComboElementList[_findFastKey + i].keyID)
                            {
                                // string keyGroup = "KeyID\n";
                                // foreach (var VARIABLE in _keyCombo.keyGroup)
                                // {
                                //     keyGroup += $"ID: {VARIABLE}\n";
                                // }
                                // keyGroup += "ElementID\n";
                                // foreach (var VARIABLE in ActionComboElementList)
                                // {
                                //     keyGroup += $"ID: {VARIABLE.keyID}\n";
                                // }
                                //
                                // keyGroup += $"\nfindFastKey: {_findFastKey}\n";
                                // keyGroup += $"\nIndex: {i}\n";
                                // EngineDebug.Log(keyGroup);
                                
                                _isComboKey = false;
                                break;
                            }
                        }

                        if (_isComboKey)
                        {
                            //连击列表成立！！ 发送技能请求
                            // EngineDebug.Log($"连击成立: {_keyCombo.mAction}");
                            Player.ActionStateMachine.SendKeyDown(_keyCombo.mAction);
                            ActionComboElementList.Clear();
                            return true;
                        }
                    }//找到了第一个按键，从第二个按钮开始核对剩余列表
                }//组里面的每个按键

            }//所有的连击组

            return false;
        }

        private void AddComboElement(int _keyID, float _time)
        {
            if (ActionComboElementList.Count > comboMaxLenth)
            {
                ActionComboElementList.RemoveAt(0);
            }//超出连击列表最大上限，从第一个元素开始清理
            ActionComboElementList.Add(new ComboKeyElement(_keyID,_time));
        }

        private void LoadInputInfo()
        {
            string _path = ActionEngineRuntimePath.Instance.InputModulePath();

            string _str = Resources.Load<TextAsset>(_path).text;
            mInputModuleInfo = new InputModuleInfo(null, 0);
            JsonUtility.FromJsonOverwrite(_str, mInputModuleInfo);
            if (mInputModuleInfo == null) mInputModuleInfo = new InputModuleInfo(new List<InputModulePart>(), 0);
            selectID = mInputModuleInfo.InputSystemID;
        }

        private void SwitchInputInfo(int _id)
        {
            ActionCheckKey.Clear();
            ActionCheckMouseKey.Clear();
            InputModulePart _modulePart = mInputModuleInfo.InputModulePart[_id];
            foreach (var _action in _modulePart.keyActions)
            {
                ActionCheckKey.TryAdd(_action.KeyCode,_action.mAction);
            }
            foreach (var _action in _modulePart.mouseActions)
            {
                ActionCheckMouseKey.TryAdd(_action.mouseButton,_action.mAction);
            }

            ActionCombo = _modulePart.ComboInputActions;
            ActionComboElementList.Clear();
            
            comboMaxLenth = 0;
            foreach (var _key in ActionCombo)
            {
                if (_key.keyGroup.Count > comboMaxLenth)
                {
                    comboMaxLenth = _key.keyGroup.Count;
                }
            }//寻找当前输入组合里最长的连击按键数量
        }

        private void ChangePlayerCamera(Unit _lastPlayer, Unit _newPlayer)
        {
            ActionEngineManager_Unit.Instance.CreactCamera(_newPlayer.CameraObject, _newPlayer, (CameraControl _came) =>
            {
                if (_came is null)
                {
                    EngineDebug.LogError("相机切换失败！！");
                    return;
                }
            
                if (_lastPlayer is not null)
                {
                    ActionEngineManager_Unit.Instance.DestoryCam(_lastPlayer.CameraObject, CurCamera);
                }

                CurCamera = _came;
            });
        }
    }
}