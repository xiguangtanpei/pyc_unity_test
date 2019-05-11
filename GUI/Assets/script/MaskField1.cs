using UnityEngine;
using UnityEditor;

public class MaskField1 : EditorWindow
{
    static int flags = 0;
    static string[] options = new string[] {"CanJump", "CanShoot", "CanSwim" ,"show"};

    [MenuItem("ss/MaskField1")]
    static void Init()
    {
        MaskField1 window = (MaskField1)GetWindow(typeof(MaskField1));
        window.Show();
    }

    void OnGUI()
    {

        //// intpopup 这个类型实现部分 

        /// 这部分要处理 
        /// 独立实现是 0  1 2  4 8 的处理方式， 可以通过选择不同的类型，实习那 0 -8 的不同选择 
        flags = EditorGUILayout.MaskField("Player Flags", flags, options);

        EditorGUILayout.MaskField("Player Flags", 0, options);
        Debug.Log(flags) ; 
    }
}