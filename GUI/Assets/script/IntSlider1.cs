using UnityEditor;
using UnityEngine;

// Simple editor script that lets you clone your object in a grid

public class IntSlider1 : EditorWindow
{
    int cloneTimesX = 1;
    int cloneTimesY = 1;
    int cloneTimesZ = 1;
    int spacing = 2;

    [MenuItem("ss/IntSlider1")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(IntSlider1));
        window.Show();
    }

    void OnGUI()
    {

        // 直接复制
        cloneTimesX = EditorGUILayout.IntSlider(cloneTimesX, 1, 10);
        cloneTimesY = EditorGUILayout.IntSlider(cloneTimesY, 1, 10);
        cloneTimesZ = EditorGUILayout.IntSlider(cloneTimesZ, 1, 10);

        if (GUILayout.Button("Duplicate object"))
            CloneSelected();
    }

    void CloneSelected()
    {
        if (!Selection.activeGameObject)
        {
            Debug.LogError("Select a GameObject first");
            return;
        }


        /// 这里是实现克隆 课哦那个后位置和选项， 还有父子关系

        for (int i = 0; i < cloneTimesX; i++)
            for (int j = 0; j < cloneTimesY; j++)
                for (int k = 0; k < cloneTimesZ; k++)
                    Instantiate(Selection.activeGameObject, new Vector3(i, j, k) * spacing, Selection.activeGameObject.transform.rotation);
    }
}
