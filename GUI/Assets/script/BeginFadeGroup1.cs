using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class BeginFadeGroup1 : EditorWindow
{
    AnimBool m_ShowExtraFields;
    string m_String;
    Color m_Color = Color.white;
    // 颜色的定义 
    int m_Number = 0;

// 表周模式 
    [MenuItem("ss/BeginFadeGroup1")]
    static void Init()
    {
        BeginFadeGroup1 window = (BeginFadeGroup1)EditorWindow.GetWindow(typeof(BeginFadeGroup1));
    }

    void OnEnable()
    {
        m_ShowExtraFields = new AnimBool(true);
        m_ShowExtraFields.valueChanged.AddListener(Repaint);
    }

    void OnGUI()
    {
        // 这工空间很有用 

        m_ShowExtraFields.target = EditorGUILayout.ToggleLeft("Show extra fields", m_ShowExtraFields.target);

        

        //Extra block that can be toggled on and off.
        if (EditorGUILayout.BeginFadeGroup(m_ShowExtraFields.faded))
        {
            /// 这里加入像是一个子集处理模式 
           EditorGUI.indentLevel++;
            EditorGUILayout.PrefixLabel("Color");
            m_Color = EditorGUILayout.ColorField(m_Color);
            EditorGUILayout.PrefixLabel("Text");
            m_String = EditorGUILayout.TextField(m_String);
            EditorGUILayout.PrefixLabel("Number");
            m_Number = EditorGUILayout.IntSlider(m_Number, 0, 10);
           EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFadeGroup();
    }
}