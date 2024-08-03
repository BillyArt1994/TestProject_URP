using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Cinemachine;

namespace AsiActionEngine.RunTime
{
    #if UNITY_EDITOR
    //Editor专用的序列化字典
    [Serializable]public class InterrupOffset: SerializedDictionary<string, int> { }
    #endif
    
    //Config用到的序列化字典
    [Serializable] public class CharacterLimbDic : SerializedDictionary<ECharacteLimbType,Transform> { }

    [Serializable] public class CinemachineDic : SerializedDictionary<string, Behaviour[]> { }
}