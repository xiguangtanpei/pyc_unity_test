using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BeginScrollView1 : EditorWindow
{
    Vector2 scrollPos;
    string t = "This is a string inside a Scroll view!";

    [MenuItem("ss/BeginScrollViewExample")]
    static void Init()
    {
        BeginScrollView1 window = (BeginScrollView1)EditorWindow.GetWindow(typeof(BeginScrollView1), true, "My Empty Window");
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        scrollPos =EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.Label(t);
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Add More Text", GUILayout.Width(100), GUILayout.Height(100)))
            t += " \nAnd this is more text!";
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Clear"))
            t = "";
    }
}