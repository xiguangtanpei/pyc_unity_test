using UnityEngine;
using UnityEditor;

// Change the color of the selected GameObjects.

public class ColorField1 : EditorWindow
{
    Color matColor = Color.white;

    [MenuItem("ss/ColorField")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(ColorField1));
        window.Show();
    }

    void OnGUI()
    {

        // 利用按钮取值颜色 
        matColor = EditorGUILayout.ColorField("New Color", matColor);

        if (GUILayout.Button("Change!"))
            ChangeColors();
    }

    private void ChangeColors()
    {
        ///利用激活对象显示
        if (Selection.activeGameObject)
        // 在选择的 游戏对象中进行循环处理， 实现逻辑部分 
            foreach (GameObject t in Selection.gameObjects)
            {
                Renderer rend = t.GetComponent<Renderer>();

                if (rend != null)
                    rend.sharedMaterial.color = matColor;
            }
    }
}