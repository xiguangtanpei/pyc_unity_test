// Shows a label in the editor with the seconds since the editor started
using UnityEditor;
using UnityEngine;
using System.IO ;
using System.Xml ; 



public class Space1 : EditorWindow
{
    [MenuItem("ss/Space1")]
    static void Init()
    {
        Space1 window = (Space1)EditorWindow.GetWindow(typeof(Space1), true, "My Empty Window");
        window.Show();
    }
    public float c  = 0.2f  ;

    void OnGUI()
    {

  
       if (GUILayout.Button ( "dddd") )
       {

       }
       EditorGUILayout.Space();

         GUILayout.Button ( "dddd")   ;


        this.Repaint();
    }
}