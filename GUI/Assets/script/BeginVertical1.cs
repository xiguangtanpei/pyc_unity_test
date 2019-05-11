// Create a Vertical Compound Button

using UnityEngine;
using UnityEditor;

public class BeginVertical1 : EditorWindow
{
    [MenuItem("ss/BeginVertical1")]
    static void Init()
    {
        BeginVertical1 window = (BeginVertical1)
            EditorWindow.GetWindow(typeof(BeginVertical1), true, "My Empty Window");
        window.Show();
    }


/// 这些都是基本点击操作等处理方式

    void OnGUI()
    {
        Rect r = (Rect)EditorGUILayout.BeginVertical("Button");
        // if (GUI.Button(r, GUIContent.none))
        //     Debug.Log("Go here");
        // GUILayout.Label("I'm inside the button");
        // GUILayout.Label("So am I");
       if   (GUILayout.Button("按钮使用这里的") )
       {
           Debug.Log("按钮");
       }
               if   (GUILayout.Button("按钮使用这里的") )
       {
           Debug.Log("按钮");
       }
        
       if   (GUILayout.Button("按钮使用这里的") )
       {
           Debug.Log("按钮");
       }
               if   (GUILayout.Button("按钮使用这里的") )
       {
           Debug.Log("按钮");
       }



        EditorGUILayout.EndVertical();
    }
}