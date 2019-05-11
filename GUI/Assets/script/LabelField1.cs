// Shows a label in the editor with the seconds since the editor started
using UnityEditor;
using UnityEngine;

public class LabelField1 : EditorWindow
{
    [MenuItem("ss/LabelField1")]
    static void Init()
    {
        LabelField1 window = (LabelField1)EditorWindow.GetWindow(typeof(LabelField1), true, "My Empty Window");
        window.Show();
    }

    void OnGUI()
    {

        // 时间以及该更新 
        EditorGUILayout.LabelField("Time since start: ",
            EditorApplication.applicationContentsPath.ToString());


                    EditorGUILayout.LabelField("Time since start: ",
            EditorApplication.applicationPath.ToString());

            


        this.Repaint();
    }
}