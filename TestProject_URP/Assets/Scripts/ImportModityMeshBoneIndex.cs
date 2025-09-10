using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class ImportModityMeshBoneIndex : AssetPostprocessor
{ /*
    public void OnPostprocessModel(GameObject gameObject)
    {
        string[] lodSuffixes = { "LOD1", "LOD2" , "LOD3" };
        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        string fileExtension = Path.GetExtension(assetPath);

        string matchedSuffix = lodSuffixes.FirstOrDefault(suffix => fileName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase));
        if (matchedSuffix == null) return;

        string newFileName = fileName.Substring(0, fileName.Length - matchedSuffix.Length) + "LOD0";
        string newFilePath = Path.Combine(Path.GetDirectoryName(assetPath), newFileName + fileExtension);
        GameObject targetObj = EditorGUIUtility.Load(newFilePath) as GameObject;
        if (targetObj == null)
        {
            Debug.Log("当前模型资源:" + fileName + "       并未找到对应LOD0模型");
            return;
        }
        ReorderBoneIndexAndBoneWeightIndex(gameObject, targetObj);
        Debug.Log("当前模型资源:" + fileName + "     已成功刷新骨骼索引至目标LOD模型:" + newFileName);
    }


    void ReorderBoneIndexAndBoneWeightIndex(GameObject scr, GameObject target)
    {
        SkinnedMeshRenderer[] scrSMR = scr.GetComponentsInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer[] targetSMR = target.GetComponentsInChildren<SkinnedMeshRenderer>();


        for (int i = 0; i < scrSMR.Length; i++)
        {
            for (int j = 0; j < targetSMR.Length; j++)
            {
                // body_LOD1    body  //body_LOD0
                //name.substring(0,name.LastIndexOf('_')
                //var splitIndex = targetSMR[j].name.LastIndexOf("_");
                //var splitName = targetSMR[j].name.Substring(0,splitIndex < 0 ? targetSMR[j].name.Length: splitIndex);

                //var namef = targetSMR[j].name.Substring(0, targetSMR[j].name.LastIndexOf("_") == -1 ?  );
                var targetMeshName = targetSMR[j].name;
                targetMeshName = targetMeshName.Substring(0, targetMeshName.LastIndexOf('_') >= 0 ? targetMeshName.LastIndexOf('_') : targetMeshName.Length);
                if (scrSMR[i].sharedMesh.name.Contains(targetMeshName))//scrSMR[i].sharedMesh.name.Contains(targetSMR[j].name)
                {
                    List<Transform> nList = new List<Transform>(targetSMR[j].bones);
                    Transform[] nBoneList = new Transform[scrSMR[i].bones.Length];

                    Dictionary<int, int> remap = new Dictionary<int, int>();
                    for (int k = 0; k < scrSMR[i].bones.Length; k++)
                    {
                        remap[k] = nList.IndexOf(nList.Find(x => x.name == scrSMR[i].bones[k].name));
                        if (remap[k] == -1) continue; // 如果源蒙皮骨骼数多于目标骨骼数
                        nBoneList[remap[k]] = scrSMR[i].bones[k];
                    }

                    BoneWeight[] bw = scrSMR[i].sharedMesh.boneWeights;
                    for (int k = 0; k < bw.Length; k++)
                    {
                        bw[k].boneIndex0 = remap[bw[k].boneIndex0] == -1 ? 0 : remap[bw[k].boneIndex0];
                        bw[k].boneIndex1 = remap[bw[k].boneIndex1] == -1 ? 0 : remap[bw[k].boneIndex1];
                        bw[k].boneIndex2 = remap[bw[k].boneIndex2] == -1 ? 0 : remap[bw[k].boneIndex2];
                        bw[k].boneIndex3 = remap[bw[k].boneIndex3] == -1 ? 0 : remap[bw[k].boneIndex3];
                    }

                    Matrix4x4[] bp = new Matrix4x4[scrSMR[i].sharedMesh.bindposes.Length];
                    for (int k = 0; k < bp.Length; k++)
                    {
                        int index = remap[k] == -1 ? 0 : remap[k];

                        bp[index] = scrSMR[i].sharedMesh.bindposes[k];
                    }

                    scrSMR[i].bones = nBoneList;
                    scrSMR[i].sharedMesh.boneWeights = bw;
                    scrSMR[i].sharedMesh.bindposes = bp;
                }
            }
        }
    }
    */
}
