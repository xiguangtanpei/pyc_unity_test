// Create a Vertical Compound Button

using UnityEngine;
using UnityEditor;

public class BoundsIntField1 : EditorWindow
{
    [MenuItem("ss/BoundsIntField1")]
    static void Init()
    {
        BoundsIntField1 window = (BoundsIntField1)
            EditorWindow.GetWindow(typeof(BoundsIntField1), true, "My Empty Window");
        window.Show();
    }


/// 这些都是基本点击操作等处理方式
// public Transform[]   bi  = Selection.transforms ;


    void OnGUI()
    {

        // var d= new Bounds() ;
        // d.center = new Vector3(0,0,0) ;
        // // d.min   = new Vector3(10,10,10 ) ;
        // // d.max   = new Vector3(20,20,20);
        // // d.size = new Vector3(10,10,10 ) ;
        // d.extents = new Vector3(10,10,10 );

        

    //    var _bound =  EditorGUILayout.boundsinField ("ss", new boun)
       /// 这里不太清除， 好像有很多问题 
   


       


    }
}