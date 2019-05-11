// Creates a new menu in the Editor called "Toggle2s" with a new menu item called "ToggleLeft Toggle2"

using UnityEngine;
using UnityEditor;

public class Toggle2 : EditorWindow
{
    bool showBtn = true;

    [MenuItem("ss/Toggle2")]
    static void Init()
    {
        Toggle2 window = (Toggle2)EditorWindow.GetWindow(typeof(Toggle2), true, "ToggleLeft Toggle2");
        window.Show();
    }

    void OnGUI()
    {
        showBtn = EditorGUILayout.ToggleLeft("Show Button", showBtn);
        if (showBtn)
        {
            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
        }
    }
}