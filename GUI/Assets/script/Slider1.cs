// Shows a label in the editor with the seconds since the editor started
using UnityEditor;
using UnityEngine;
using System.IO ;
using System.Xml ; 



public class Slider1 : EditorWindow
{
    [MenuItem("ss/Slider1")]
    static void Init()
    {
        Slider1 window = (Slider1)EditorWindow.GetWindow(typeof(Slider1), true, "My Empty Window");
        window.Show();
    }
    public float c  = 0.2f  ;

    void OnGUI()
    {

        /// 其实变化数值一定要是变量,不然就会划不动哈 
        

        // 时间以及该更新 
       c =   EditorGUILayout.Slider(c ,0.0f ,10.0f ) ;
      Debug.Log(c);
  
            


        this.Repaint();
    }
}