using UnityEditor;
using UnityEngine;

public class RectField1 : EditorWindow
{
    static Rect pos;

    [MenuItem("ss/RectField1")]
    static void run()
    {
        RectField1 window =EditorWindow.GetWindowWithRect<RectField1>(new Rect(0, 0, 250, 100));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        pos =  EditorGUILayout.RectField("Internal input:", pos);
        /// 这种是定义4个数值,来解决这部分的问题 \
        
        /// 下面的类型是处理int类型 

        /// int 类型默认高版本才有 
        // var ci = EditorGUILayout.RectIntField("dd" ,pos ) ;
            

        Debug.Log(pos.y);
        // EditorGUILayout.BeginHorizontal();
        // GUILayout.FlexibleSpace();
        // GUILayout.Label("x: " + (pos.x).ToString());
        // GUILayout.FlexibleSpace();
        // GUILayout.Label("y: " + (pos.y).ToString());
        // GUILayout.FlexibleSpace();
        // GUILayout.Label("w: " + (pos.width).ToString());
        // GUILayout.FlexibleSpace();
        // GUILayout.Label("h: " + (pos.height).ToString());
        // GUILayout.FlexibleSpace();
        // EditorGUILayout.EndHorizontal();
        // EditorGUILayout.EndVertical();

        if (GUILayout.Button("Close"))
        {
            this.Close();
        }
    }
}