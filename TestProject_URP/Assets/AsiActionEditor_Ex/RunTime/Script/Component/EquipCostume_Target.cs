﻿using System;
using System.Collections.Generic;
using AsiActionEngine.RunTime;
using UnityEngine;

namespace AsiTimeLine.RunTime
{
    [ExecuteAlways]
    public class EquipCostume_Target : MonoBehaviour
    {
        public Transform AnimRoot;
        public ClothBones ClothBones = new ClothBones();
    }
}