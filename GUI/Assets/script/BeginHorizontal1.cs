// Create a Horizontal Compound Button

using UnityEngine;
using UnityEditor;

public class BeginHorizontal1 : EditorWindow
{
    [MenuItem("ss/Begin-End Horizontal usage")]
    static void Init()
    {
        BeginHorizontal1 window = (BeginHorizontal1)GetWindow(typeof(BeginHorizontal1));
        window.Show();
    }

    void OnGUI()
    {
        Rect r = EditorGUILayout.BeginHorizontal("Button");
        /// 
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

       // 按钮排布还是使用这里的形式 
       
        


        EditorGUILayout.EndHorizontal();
    }
}