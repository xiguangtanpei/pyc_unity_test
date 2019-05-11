using UnityEditor;
using UnityEngine;

public class Vector2intField2 : UnityEditor.EditorWindow
{
    float distance = 0f;
    Vector4 p1;
    Vector4 p2;



/// 该类型 测试不支持 
    [MenuItem("ss/Vector2intField2")]
    static void Init()
    {
        Vector2intField2 window = (Vector2intField2)EditorWindow.GetWindow(typeof(Vector2intField2), true, "My Empty Window");
        window.Show();
    }

    void OnGUI()
    {
        p1 = EditorGUILayout.Vector4Field("Point 1:", p1);
        p2 = EditorGUILayout.Vector4Field("Point 2:", p2);
        // EditorGUILayout.LabelField("Distance:", distance.ToString());
    //     if (GUILayout.Button("Close"))
    //         this.Close();
    }

    void OnInspectorUpdate()
    {
        // distance = Vector2.Distance(p1, p2);
        this.Repaint();
    }
}