// Create a Vertical Compound Button

using UnityEngine;
using UnityEditor;

public class DelayedDoubleField1 : EditorWindow
{

    public double ling_d;

    [MenuItem("ss/DelayedDoubleField1")]
    static void Init()
    {
        DelayedDoubleField1 window = (DelayedDoubleField1)
            EditorWindow.GetWindow(typeof(DelayedDoubleField1), true, "My Empty Window");
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


        ling_d =  EditorGUILayout.DelayedDoubleField(ling_d);
        /// dsf
        Debug.Log(ling_d);
        if (GUILayout.Button(""))
        {
            
        }


    }

    
    ///  原来是要机械能重画操作 
    /// 美哦10个单位进行调用
    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}