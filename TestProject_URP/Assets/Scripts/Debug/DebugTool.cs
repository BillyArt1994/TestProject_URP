using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    [ContextMenu("Debug Bone Index")]
    void BoneIndex()
    {
        SkinnedMeshRenderer[] skinMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var item in skinMeshRenderer)
        {
            var name = item.sharedMesh.name;
            Transform[] boneList = item.bones;
            string boneName="-----------//";
            for (int i = 0; i < boneList.Length; i++) 
            {
                boneName += boneList[i].name+"/index:"+i+"\r\n";
            }
            Debug.Log(name + "ÒýÓÃ¹Ç÷À" + boneName);
        }
    }
}
