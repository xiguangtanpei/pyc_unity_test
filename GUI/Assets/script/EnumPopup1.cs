using UnityEditor;
using UnityEngine;
using System.Collections;

// Creates an instance of a primitive depending on the option selected by the user.
/// 枚举方式 多半要用这种， 
/// 注意枚举独立成类 
public enum OPTIONS
{
    CUBE = 0,
    SPHERE = 1,
    PLANE = 2
}

public class EnumPopup1 : EditorWindow
{
    public OPTIONS op;
    [MenuItem("ss/EnumPopup1")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(EnumPopup1));
        window.Show();
    }

    void OnGUI()
    {
        op = (OPTIONS)EditorGUILayout.EnumPopup("Primitive to create:", op);
        if (GUILayout.Button("Create"))
            InstantiatePrimitive(op);
    }

    void InstantiatePrimitive(OPTIONS op)
    {
        switch (op)
        {
            /// 创建资源 
            case OPTIONS.CUBE:
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = Vector3.zero;
                break;
            case OPTIONS.SPHERE:
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = Vector3.zero;
                break;
            case OPTIONS.PLANE:
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.position = Vector3.zero;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }
}