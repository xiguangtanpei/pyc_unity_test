// Automatically change the name of the selected object via a text field
using UnityEditor;
using UnityEngine;

public class TextField1 : EditorWindow
{
    [MenuItem("ss/TextField1")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(TextField1));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Select an object in the hierarchy view");
        if (Selection.activeGameObject)
            Selection.activeGameObject.name =
                EditorGUILayout.TextField("Object Name: ", Selection.activeGameObject.name);
        this.Repaint();
    }
}