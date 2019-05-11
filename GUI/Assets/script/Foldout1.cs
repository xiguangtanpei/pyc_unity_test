// Create a foldable menu that hides/shows the selected transform position.
// If no Transform is selected, the Foldout item will be folded until
// a transform is selected.

using UnityEditor;
using UnityEngine;

public class Foldout1 : EditorWindow
{
    bool showPosition = true;
    string status = "Select a GameObject";

    [MenuItem("ss/Foldout1")]
    static void Init()
    {
        Foldout1 window = (Foldout1)GetWindow(typeof(Foldout1));
        window.Show();
    }

public  Vector3 cici ; 
    public void OnGUI()
    {
        showPosition = EditorGUILayout.Foldout(showPosition, status);
        if (showPosition)
        {
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position =
                    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                status = Selection.activeTransform.name;
            }
            /// 数值 关系 通过上下测试知道 这部分 一定是要处理这部分的操作， 通过关联数据解决 

            EditorGUILayout.Vector3Field("Pos",cici );

        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showPosition = false;
        }
      }
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}