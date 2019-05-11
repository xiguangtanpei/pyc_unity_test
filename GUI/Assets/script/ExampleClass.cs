using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    // Add Example1 into a new menu list
    [MenuItem("ss/Example1", false,3)]
    public static void Example1()
    {
        print("Example/Example1");
    }

    // Add Example2 into the same menu list
    [MenuItem("ss/Example2", false,3)]
    public static void Example2()
    {
        print("Example/Example2");
    }
}