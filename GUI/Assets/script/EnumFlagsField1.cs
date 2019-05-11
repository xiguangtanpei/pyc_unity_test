using UnityEditor;
using UnityEngine;


   public  enum ExampleFlagsEnum
    {
        None = 0, // Custom name for "Nothing" option
        A = 1 << 0,
        B = 1 << 1,
        AB = A | B, // Combination of two flags
        C = 1 << 2,
        All = ~0, // Custom name for "Everything" option
    }
class EnumFlagsField1 : EditorWindow
{


    ExampleFlagsEnum m_Flags;

    [MenuItem("ss/EnumFlagsField1")]
    static void OpenWindow()
    {
        GetWindow<EnumFlagsField1>().Show();
    }

    void OnGUI()
    {
        ///m_Flags = (ExampleFlagsEnum)EditorGUILayout.EnumFlagsField(m_Flags);
        /// 这个版本好像不能使用 有很大问题 
        // EditorGUILayout.EnumFlagsField(m_Flags);


    }
}