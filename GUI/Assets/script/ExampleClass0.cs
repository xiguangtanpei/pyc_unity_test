using UnityEngine;
using System.Collections;

public class ExampleClass0 : MonoBehaviour {
    public string stringToEdit = "Hello World";
    void OnGUI() {
        stringToEdit = GUILayout.TextField(stringToEdit, 25);
    }
}