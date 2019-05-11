using UnityEngine;
using UnityEditor;

public class Toggle1 : UnityEditor.EditorWindow
{
    bool showBtn = true;

    [MenuItem("ss/Toggle1")]
    static void Init()
    {
        Toggle1 window = (Toggle1)EditorWindow.GetWindow(typeof(Toggle1), true, "My Empty Window");
        window.Show();
    }

    void OnGUI()
    {
        showBtn = EditorGUILayout.Toggle("Show Button", showBtn);
        if (showBtn)
            if (GUILayout.Button("Close"))
                this.Close();
    }
}
