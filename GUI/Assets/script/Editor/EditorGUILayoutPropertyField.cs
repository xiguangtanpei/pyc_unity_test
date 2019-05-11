//This next script shows how to call upon variables from the "MyGameObject" Script (the first script) to make custom fields in the Inspector for these variables.

using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(MyGameObjectScript))]
[CanEditMultipleObjects]
public class EditorGUILayoutPropertyField : Editor
{
    SerializedProperty m_IntProp;
    SerializedProperty m_VectorProp;
    SerializedProperty m_GameObjectProp;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        m_IntProp = serializedObject.FindProperty("m_MyInt");
        m_VectorProp = serializedObject.FindProperty("m_MyVector");
        m_GameObjectProp = serializedObject.FindProperty("m_MyGameObject");
    }

    public override void OnInspectorGUI()
    {
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(m_IntProp, new GUIContent("Int Field"), GUILayout.Height(20));
        EditorGUILayout.PropertyField(m_VectorProp, new GUIContent("Vector Object"));
        EditorGUILayout.PropertyField(m_GameObjectProp, new GUIContent("Game Object"));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}