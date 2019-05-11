using UnityEditor;
using UnityEngine;
using System.Collections;

// Creates an instance of a primitive depending on the option selected by the user.
public class Popup1 : EditorWindow
{
    public string[] options = new string[] {"Cube", "Sphere", "Plane"};
    public int index = 0;
    [MenuItem("ss/Popup1")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(Popup1));
        window.Show();
    }

    void OnGUI()
    {
        /// 这里的逻辑 index 之际从0 开始挨着排序 
        index = EditorGUILayout.Popup(index, options);
        if (GUILayout.Button("Create"))
            InstantiatePrimitive();
    }

    void InstantiatePrimitive()
    {

        /// 关于seitch 的实现处理表示  
        switch (index)
        {
            case 0:
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = Vector3.zero;
                break;
            case 1:
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = Vector3.zero;
                break;
            case 2:
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.transform.position = Vector3.zero;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }
}