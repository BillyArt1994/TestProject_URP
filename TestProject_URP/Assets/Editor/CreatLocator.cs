using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class CreatLocator : EditorWindow
{
    [MenuItem("GameObject/Creat Helpers/Locator",priority = 0)]
    static void CreatLocatorG()
    {
        GameObject existingObject = GameObject.Find("Locator");
        int suffix = 0;
        string name = "Locator";
        while (existingObject != null)
        {
            // ������ڣ���Ӻ�׺��Ų����¼��
            suffix++;
            name = "Locator" + "_" + suffix;
            existingObject = GameObject.Find(name);
        }

        GameObject locatorObject = new GameObject(name);
        locatorObject.transform.position = new Vector3(0f, 0f, 0f);
        locatorObject.transform.rotation = Quaternion.identity;
        locatorObject.AddComponent<LocatorGizmo>();
    }
}
