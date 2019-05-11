// Simple editor script that lets you set a tag for the selected GameObjects.
using UnityEditor;
using UnityEngine;

public class TagField1 : EditorWindow
{
    static string tagStr = "";

    [MenuItem("ss/TagField1")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(TagField1));
        window.Show();
    }

    void OnGUI()
    {

        ///这个是调用系统的一个完整的控件 
        tagStr = EditorGUILayout.TagField("Tag for Objects:", tagStr);
        if (GUILayout.Button("Set Tag!"))
        {
            SetTags();
        }
    }

    static void SetTags()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            go.tag = tagStr;
        }
    }
}
