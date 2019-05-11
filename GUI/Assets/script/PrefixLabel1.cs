using UnityEditor;
using UnityEngine;

public class PrefixLabel1 : EditorWindow
{
    static int ammo = 0;


/// 现在对于创建菜单部分已经解决完成操作 
    [MenuItem("ss/PrefixLabel ")]
    static void Init()
    {
        PrefixLabel1 window = (PrefixLabel1)GetWindow(typeof(PrefixLabel1));
        window.Show();
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        // 这个就是label‘ 
        // EditorGUILayout.LabelField("ceshi") ;
        EditorGUILayout.PrefixLabel("Ammo"); /// 按照之类说 系统是这种 建议使用这个 
        ammo = EditorGUILayout.IntField(ammo);
        EditorGUILayout.EndHorizontal();
    }
}