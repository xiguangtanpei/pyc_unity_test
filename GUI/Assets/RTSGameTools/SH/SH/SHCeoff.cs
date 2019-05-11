using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SHCeoff : EditorWindow
{
    #region Greate Window
    //添加标题
    SHCeoff()
    {
        this.titleContent = new GUIContent("SHCeoff");
    }
    //添加到工具栏
    [MenuItem("RTSGameTools/SHCeoff")]
    //创建窗口
    static void showWindow()
    {
        GetWindow(typeof(SHCeoff));
    }

    #endregion
    SH shdata=new SH();
    Texture2D EnvHDR;
    void OnGUI()
    {
       
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Terrain Tools");
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Timi-RSTGame-XiguangYang");
        GUILayout.Space(10);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        EnvHDR = EditorGUILayout.ObjectField(EnvHDR, typeof(Texture2D), true) as Texture2D;

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("求参数"))
        {
            shdata.EnvHDR = EnvHDR;
            shdata.PrintCeoff();

        }

        GUILayout.EndVertical();
    }

}
