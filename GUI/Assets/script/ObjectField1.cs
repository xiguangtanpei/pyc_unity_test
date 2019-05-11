// EditorScript that quickly searches for a help page
// about the selected Object.
//
// If no such page is found in the Manual it opens the Unity forum.

using UnityEditor;
using UnityEngine;
using System.Collections;

public class ObjectField1 : EditorWindow
{


    ///资源建立方式 都是通过object 的方式进行访问的，
    public Object source;

    [MenuItem("ss/ObjectField1")]
    static void Init()
    {


        /// 类型处理
        var window = GetWindowWithRect<ObjectField1>(new Rect(0, 0, 165, 100));



        // ObjectField1 window = (ObjectField1)GetWindow(typeof(ObjectField1));
        window.Show();
        // window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        source = EditorGUILayout.ObjectField(source, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Search!"))
        {
            if (source == null)
                ShowNotification(new GUIContent("No object selected for searching"));
            else if (Help.HasHelpForObject(source))
                Help.ShowHelpForObject(source);
            else
                Help.BrowseURL("http://forum.unity3d.com/search.php");
        }
    }
}