using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UObj = UnityEngine.Object;

public class CleanUpMissingScript : Editor
{
    private static bool isRemove = false;

    [MenuItem("Assets/Script/CleanUp Missing Script")]
    static void Init()
    {
        var selectObj = Selection.GetFiltered(typeof(UObj), SelectionMode.DeepAssets);
        foreach (Object obj in selectObj)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (path.EndsWith(".prefab"))
            {
                var prefab = obj as GameObject;
                var components = prefab.GetComponentsInChildren<Component>();
                var isBreak = components.Any(x => x == null);
                if (isBreak)
                {
                    components = null;
                    Debug.LogError(path);
                    CleanUp(prefab);
                    EditorUtility.SetDirty(prefab);
                    AssetDatabase.SaveAssets();
                }
                prefab = null;
            }
        }
        AssetDatabase.Refresh();
    }
    static void CleanUp(GameObject go)
    {
        var components = go.GetComponents<Component>();
        var serializedObject = new SerializedObject(go);
        var prop = serializedObject.FindProperty("m_Component");
        int r = 0;
        for (int j = 0; j < components.Length; j++)
        {
            if (components[j] == null)
            {
                prop.DeleteArrayElementAtIndex(j - r);
                r++;
            }
        }
        serializedObject.ApplyModifiedProperties();
        if (go.transform.childCount > 0)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                var child = go.transform.GetChild(i).gameObject;
                var childComponents = child.GetComponentsInChildren<Component>();
                if (childComponents.Any(x => x == null))
                {
                    childComponents = null;
                    CleanUp(child);
                }
                child = null;
            }
        }
    }
}
