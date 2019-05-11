// Create a Vertical Compound Button

using UnityEngine;
using UnityEditor;

public class DelayedFloatField1 : EditorWindow
{
    [MenuItem("ss/DelayedFloatField1")]
    static void Init()
    {
        DelayedFloatField1 window = (DelayedFloatField1)
            EditorWindow.GetWindow(typeof(DelayedFloatField1));
        window.Show();
    }


/// 这些都是基本点击操作等处理方式
   public  float flt ;

    void OnGUI()
    {
        var c = EditorGUILayout.DelayedFloatField("ceshi",flt );
       Debug.Log(c ) ;
    }

    

}