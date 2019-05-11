// Editor Script that creates a password field and lets you
// visualize what have you typed in a label.

using UnityEditor;
using UnityEngine;

public class PasswordField1 : EditorWindow
{
    string text  = "Some text here";

    [MenuItem("ss/PasswordField1")]
    static void Init()
    {
        PasswordField1 window = (PasswordField1)GetWindow(typeof(PasswordField1));
        window.Show();
    }

    void OnGUI()
    {
        /// 密码的写入过程 
        /// 制作过程需要表现的东西 
        text = EditorGUILayout.PasswordField("Type Something:", text);
        EditorGUILayout.LabelField("Written Text:", text);
    }
}