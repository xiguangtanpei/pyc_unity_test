// Create a foldable menu that hides/shows the selected transform position.
// If no Transform is selected, the Foldout item will be folded until
// a transform is selected.

using UnityEditor;
using UnityEngine;

public class GetControlRect1 : EditorWindow
{
    bool showPosition = true;
    string status = "Select a GameObject";

    [MenuItem("ss/GetControlRect1")]
    static void Init()
    {
        GetControlRect1 window = (GetControlRect1)GetWindow(typeof(GetControlRect1));
        window.Show();
    }

public  Vector3 cici ; 
    public void OnGUI()
    {
        /// 初步测试发现只有宽会使用到 
         var c = EditorGUILayout.GetControlRect() ;
         Debug.Log(c.ToString()) ; 

        var ci = MessageType.Warning ;
        
        /// 显示一些操作， 不日差惹怒
         EditorGUILayout.HelpBox("help ", ci ) ;
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}