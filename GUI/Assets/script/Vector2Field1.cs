using UnityEditor;
using UnityEngine;

public class Vector2Field1 : UnityEditor.EditorWindow
{
    float distance = 0f;
    Vector3 p1;
    Vector3 p2;

    [MenuItem("ss/Vector2Field1")]
    static void Init()
    {
        Vector2Field1 window = (Vector2Field1)EditorWindow.GetWindow(typeof(Vector2Field1), true, "My Empty Window");
        window.Show();
    }

    void OnGUI()
    {
        p1 = EditorGUILayout.Vector2Field("Point 1:", p1);
        p2 = EditorGUILayout.Vector2Field("Point 2:", p2);
        EditorGUILayout.LabelField("Distance:", distance.ToString());
        if (GUILayout.Button("Close"))
            this.Close();
    }

    void OnInspectorUpdate()
    {
        distance = Vector2.Distance(p1, p2);
        this.Repaint();
    }
}