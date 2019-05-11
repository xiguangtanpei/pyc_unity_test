// Simple script that lets you visualize your scripts in an editor window
// This can be expanded to save your scripts also in the editor window.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// 这个会用到很多, 有问题的打印出阿里, 还是说现在在lsit中, 点击搜索到场景中的资源 

public class TextArea1 : EditorWindow
{
    string text = "Nothing Opened...";
    TextAsset txtAsset;
    Vector2 scroll;

    [MenuItem("ss/TextArea1")]
    static void Init()
    {
        TextArea1 window = (TextArea1)GetWindow(typeof(TextArea1), true, "EditorGUILayout.TextArea");
        window.Show();
    }

    Object source;


/// 用来显示一些错误信息, 还是 但是反饭就不够厚想用list
//./ 或者说是显示出来的list 

    void OnGUI()
    {
        source = EditorGUILayout.ObjectField(source, typeof(Object), true);
        TextAsset newTxtAsset = (TextAsset)source;

        if (newTxtAsset != txtAsset)
            ReadTextAsset(newTxtAsset);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        text = EditorGUILayout.TextArea(text, GUILayout.Height(position.height - 30));
        EditorGUILayout.EndScrollView();
    }

    void ReadTextAsset(TextAsset txt)
    {
        text = txt.text;
        txtAsset = txt;
    }
}
