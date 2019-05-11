using UnityEngine;
using UnityEditor;

// Editor Script that multiplies the scale of the current selected GameObject

/// 要用这种方式来处理 
public class FloatField1 : EditorWindow
{
    float sizeMultiplier = 1.0f;

    [MenuItem("ss/FloatField1")]
    static void Init()
    {
        var window = (FloatField1)GetWindow(typeof(FloatField1));
        window.Show();
    }

    void OnGUI()
    {
        sizeMultiplier = EditorGUILayout.FloatField("Increase scale by:", sizeMultiplier);

        if (GUILayout.Button("Scale"))
        {
            /// 这种 确实h是可以使用一个 
            if (!Selection.activeGameObject)
            {
                Debug.Log("Select a GameObject first");
                return;
            }

            Selection.activeTransform.localScale =
                Selection.activeTransform.localScale * sizeMultiplier;
        }
    }
}