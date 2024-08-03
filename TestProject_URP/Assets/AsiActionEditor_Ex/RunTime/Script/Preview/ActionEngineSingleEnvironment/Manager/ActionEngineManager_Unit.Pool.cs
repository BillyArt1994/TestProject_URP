using System;
using System.Collections.Generic;
using System.IO;
using AsiActionEngine.RunTime;
// using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace AsiTimeLine.RunTime
{
    public partial class ActionEngineManager_Unit
    {
        //各单位对象池
        private Dictionary<int, ObjectPool<CameraControl>> mCameraPool = new Dictionary<int, ObjectPool<CameraControl>>();
        private Dictionary<int, ObjectPool<Unit>> mUnitPool = new Dictionary<int, ObjectPool<Unit>>();
        private Dictionary<string, CameraWarp> mCameraWarps = new Dictionary<string, CameraWarp>();

        public void CreactCamera(string _name, Unit _player, Action<CameraControl> _callback) =>
            OnCreactCamera(_name, _player, _callback);
        public void CreactUnit(string _unitWarp, Action<Unit> _callback) => OnCreactUnit(_unitWarp, _callback);

        public void DestoryCam(string _name, CameraControl _gameObject) => OnDestoryCam(_name, _gameObject);
        
        private void OnCreactCamera(string _name, Unit _player, Action<CameraControl> _callback)
        {
            CameraWarp _cameraWarp = null;
            if (string.IsNullOrEmpty(_name))
            {
                if (mCameraWarps.Count > 0)
                {
                    EngineDebug.LogWarning("未配置相机, 将加载第一个相机组");
                    //读取json
                    _cameraWarp = OnGetCameraWarp(_name);
                }
                else
                {
                    EngineDebug.LogError("未配置相机, 也从未加载过相机");
                    return;
                }
            }
            else
            {
                //读取json
                _cameraWarp = OnGetCameraWarp(_name);
            }
        
            if (mCameraPool.ContainsKey(_cameraWarp.ID))
            {
                CameraControl _camera = mCameraPool[_cameraWarp.ID].Get();
                CamControllerInit(_camera, _player, _cameraWarp);
                _callback(_camera);
            }
            else
            {
                //创建对象池
                ActionEngineResources.Instance.LoadAsync(_cameraWarp.ModelPath, (Object _obj) =>
                {
                    if (_obj is null)
                    {
                        EngineDebug.LogError($"相机加载路径错误\n<color=#FFCC00>{_cameraWarp.ModelPath}</color>");
                        return;
                    }
                    GameObject _gameObject = (GameObject)_obj;
                    ObjectPool<CameraControl> _monsterPool = new ObjectPool<CameraControl>(
                        //新建对象
                        () => {
                            return Object.Instantiate(_gameObject).GetComponent<CameraControl>();
                        },
                        //取出对象
                        (CameraControl _object) =>
                        {
                            _object.gameObject.SetActive(true);
                        },
                        //存入对象
                        (CameraControl _object) =>
                        {
                            _object.gameObject.SetActive(false);
                        },
                        //销毁对象
                        (CameraControl _object) =>
                        {
                            Object.Destroy(_object.gameObject);
                        },
                        true,
                        1,
                        5
                    );
                    mCameraPool.Add(_cameraWarp.ID, _monsterPool);
                    CameraControl _camera = _monsterPool.Get();
                    CamControllerInit(_camera, _player, _cameraWarp);
                    _callback(_camera);
                });

            }
        }

        private void CamControllerInit(CameraControl _cameraControl, Unit _unit, CameraWarp _cameraWarp)
        {
            if (_unit.TryGetComponent(out CharacterConfig _config))
            {
                if (_config.HelpPointDic.TryGetValue(ECharacteLimbType.Cam_Main, out Transform _transform))
                {
                    _cameraControl.OnInit(_transform, _unit.ActionStateMachine, _cameraWarp.DefaultCamID);
                }
                else
                {
                    EngineDebug.LogWarning("未配置相机挂点");
            
                }
            }
            else
            {
                EngineDebug.LogWarning("未挂载Config组件");
            }
        }
        
        private void OnDestoryCam(string _name, CameraControl _cam)
        {
            CameraWarp _cameraWarp = null;
            if (string.IsNullOrEmpty(_name))
            {
                if (mCameraWarps.Count > 0)
                {
                    EngineDebug.LogWarning("未配置相机, 将加载第一个相机组");
                    //读取json
                    _cameraWarp = OnGetCameraWarp(_name);
                }
                else
                {
                    EngineDebug.LogError("未配置相机, 也从未加载过相机");
                    return ;
                }
            }
            else
            {
                //读取json
                _cameraWarp = OnGetCameraWarp(_name);
            }
        
            // if (_cam.TryGetComponent(out CameraControl _cameraControl))
            // {
            //     _cameraControl.OnReset();
            // }
        
            mCameraPool[_cameraWarp.ID].Release(_cam);
        }
        
        //角色异步加载
        private void OnCreactUnit(string _name, Action<Unit> _loadCallback)
        {
            OnGetUnitWarp(_name, (UnitWarp _unitWarp) =>
            {
                if (mUnitPool.ContainsKey(_unitWarp.ID))
                {
                    UnitInit(mUnitPool[_unitWarp.ID].Get(), _unitWarp, _loadCallback);
                    // _loadCallback(mUnitPool[_unitWarp.ID].Get());
                }
                else
                {
                    ActionEngineResources.Instance.LoadAsync(_unitWarp.ModelPath, (Object _obj) =>
                    {
                        if (_obj is null)
                        {
                            EngineDebug.LogError($"单位加载路径错误\n<color=#FFCC00>{_unitWarp.ModelPath}</color>");
                            return;
                        }
                        GameObject _gameObject = (GameObject)_obj;
                        ObjectPool<Unit> _monsterPool = new ObjectPool<Unit>(
                            //新建对象
                            () =>
                            {
                                return Object.Instantiate(_gameObject).GetComponent<Unit>();
                            },
                            //取出对象
                            (Unit _object) =>
                            {
                                _object.gameObject.SetActive(true);
                            },
                            //存入对象
                            (Unit _object) =>
                            {
                                _object.gameObject.SetActive(false);
                            },
                            //销毁对象
                            (Unit _object) =>
                            {
                                Object.Destroy(_object.gameObject);
                            },
                            true,
                            1,
                            20
                        );
                        mUnitPool.Add(_unitWarp.ID, _monsterPool);
                        UnitInit(_monsterPool.Get(), _unitWarp, _loadCallback);
                    });
                }
            });
        }

        private void UnitInit(Unit _unit, UnitWarp _unitWarp, Action<Unit> _loadCallback)
        {
            ActionEngineManager_Unit.Instance.GetActionList(_unitWarp.Action, list =>
            {
                ActionStateMachine statePart = 
                    new ActionStateMachine(_unit, _unit.GetComponent<Animator>(), list);
                _unit.SetActionStateMachine(statePart);
                _unit.Init();
                
                ActionEngineManager_Unit.Instance.AddUnit(_unit);
                _loadCallback(_unit);
            });
        }
        
        private CameraWarp OnGetCameraWarp(string _name)
        {
            if (mCameraWarps.ContainsKey(_name))
            {
                return mCameraWarps[_name];
            }
            else
            {
                string _info = Resources.Load<TextAsset>(ActionEngineRuntimePath.Instance.CameraPath(_name)).text;

                // string _info = File.ReadAllText(ActionEngineRuntimePath.Instance.CameraPath(_name));
                CameraWarp _cameraWarp = new CameraWarp();
                JsonUtility.FromJsonOverwrite(_info,_cameraWarp);
                mCameraWarps.Add(_name, _cameraWarp);
                return _cameraWarp;
            }
        }
    }
}