
// selected GameObjects.

using UnityEngine;
using UnityEditor;

public class LayerField1 : EditorWindow
{
    static int selectedLayer = 0;

    [MenuItem("ss/LayerField1")]
    static void Init()
    {
        LayerField1 window = (LayerField1)GetWindow(typeof(LayerField1));
        window.Show();
    }

    // Disable menu if we dont have at least 1 gameobject selected
    [MenuItem("ss/LayerField1", true)]
    static bool ValidateSelection()
    {
        return Selection.activeGameObject != null;
    }
    /// 这种就是条件菜单了， 当 第二相 设置成true的时候， 这个时候 终于以 会用到小下面这函数来判断是否可用 菜单命名 



    void OnGUI()
    {
        selectedLayer = EditorGUILayout.LayerField("Layer for Objects:", selectedLayer);
        if (GUILayout.Button("Set Layer!"))
            SetLayer();
    }

    static void SetLayer()
    {

        /// 设置每个物体的层属性等操作 
        foreach (var go in Selection.gameObjects)
            go.layer = selectedLayer;
    }
}