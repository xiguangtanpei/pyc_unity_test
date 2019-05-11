using UnityEngine;
using UnityEditor;

// Editor Script that multiplies the scale of the current selected GameObject

/// 要用这种方式来处理 
public class DropdownButton1 : EditorWindow
{
    float sizeMultiplier = 1.0f;
    public Texture  dt ; 

    [MenuItem("ss/DropdownButton1")]
    static void Init()
    {
        var window = (DropdownButton1)GetWindow(typeof(DropdownButton1));
        window.Show();
    }



    void OnGUI()
    {   

        var ci = new GUIContent() ;
        ci.image = dt ;
        ci.text = "ceshi" ; 
        ci.tooltip ="显示过程" ;
        var dit =  EditorGUILayout.DropdownButton(ci, new FocusType()) ;
        if (dit )
        {
            Debug.Log("444444");
        }
 


    }
}