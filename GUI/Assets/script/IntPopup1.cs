// Simple Editor Script that lets you rescale the current selected GameObject.
using UnityEditor;
using UnityEngine;

public class IntPopup1 : EditorWindow
{
    int   selectedSize = 2;
    string[] names = new string[] {"Normal", "Double", "Quadruple"};
    int[] sizes = {1, 2, 4};

    [MenuItem("ss/IntPopup1")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(IntPopup1));
        window.Show();
    }

    void OnGUI()
    {
        selectedSize = EditorGUILayout.IntPopup("Resize Scale: ", selectedSize, names, sizes);
        if (GUILayout.Button("Scale"))
            ReScale();
    }

    void ReScale()
    {
        /// 模型自身缩放
        // Selection.activeTransform.localScale 
        if (Selection.activeTransform)
            Selection.activeTransform.localScale =
                new Vector3(selectedSize, selectedSize, selectedSize);
        else
            Debug.LogError("No Object selected, please select an object to scale.");
    }
}