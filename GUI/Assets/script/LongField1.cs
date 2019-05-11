using UnityEditor;
using UnityEngine;

// Simple editor script that lets you clone your object in a grid

public class LongField1 : EditorWindow
{
    int cloneTimesX = 1;
    int cloneTimesY = 1;
    int cloneTimesZ = 1;
    long spacing = 2;

    [MenuItem("ss/LongField1 ")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(LongField1));
        window.Show();
    }

    void OnGUI()
    {

        /// 测试数据是实时显示 
        spacing =  EditorGUILayout.LongField(spacing) ;
        Debug.Log(spacing );
    }
}