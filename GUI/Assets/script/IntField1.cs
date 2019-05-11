// Editor Script that clones the selected GameObject a number of times.

using UnityEditor;
using UnityEngine;

public class IntField1 : EditorWindow
{
    int clones = 1;
    static int sizeMultiplier;


/// 窗口模式基本固定 这个步骤要处理 
    [MenuItem("ss/IntField1")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(IntField1));
        window.Show();
    }

    void OnGUI()
    {
        sizeMultiplier = EditorGUILayout.IntField("Number of clones:", clones);

        if (GUILayout.Button("Clone!"))
        {
            if (!Selection.activeGameObject)
            {
                Debug.Log("Select a GameObject first");
                return;
            }
            // 实现克隆物体
            for (var i = 0; i < sizeMultiplier; i++)
                Instantiate(Selection.activeGameObject, Vector3.zero, Quaternion.identity);
        }
    }
}