using UnityEditor;
using UnityEngine;

public class hierarchyWindowChanged1
{
    [MenuItem("ss/hierarchyWindowChanged1")]
    static void Example()
    {

        /// 消失 命名等操作都会得到解决 
        /// 这里是代理
        EditorApplication.hierarchyWindowChanged += ExampleCallback;
    }

    static void ExampleCallback()
    {
        Object[] all = Resources.FindObjectsOfTypeAll(typeof(Object));
        Debug.Log("There are " + all.Length + " objects at the moment.");

        //EditorApplication.Beep();
    }
}