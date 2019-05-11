// Create a custom transform inspector that shows the X,Y,Z,W
// quaternion components instead of the rotation angles.

using UnityEditor;
using UnityEngine;

public class InspectorTitlebar1 : EditorWindow
{
    bool      fold = true;
    Vector4   rotationComponents;
    Transform selectedTransform;

    /// 解决问题

    [MenuItem("ss/InspectorTitlebar1")]
    static void Init()
    {
        var window = GetWindow(typeof(InspectorTitlebar1));
        window.Show();
    }

    void OnGUI()
    {
        if (Selection.activeGameObject)
        {
            selectedTransform = Selection.activeGameObject.transform; // 激活一个 

            fold = EditorGUILayout.InspectorTitlebar(fold, selectedTransform);

            EditorGUILayout.InspectorTitlebar(fold, selectedTransform);

            if (fold)
            {
                selectedTransform.position =
                    EditorGUILayout.Vector3Field("Position", selectedTransform.position);
                EditorGUILayout.Space();
                rotationComponents =
                    EditorGUILayout.Vector4Field("Detailed Rotation",
                        QuaternionToVector4(selectedTransform.localRotation));
                EditorGUILayout.Space();
                selectedTransform.localScale =
                    EditorGUILayout.Vector3Field("Scale", selectedTransform.localScale);
            }

            selectedTransform.localRotation = ConvertToQuaternion(rotationComponents);
            EditorGUILayout.Space();
        }
    }

    Quaternion ConvertToQuaternion(Vector4 v4)
    {
        return new Quaternion(v4.x, v4.y, v4.z, v4.w);
    }

    Vector4 QuaternionToVector4(Quaternion q)
    {
        return new Vector4(q.x, q.y, q.z, q.w);
    }

/// 动态调用的重要性 
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
}