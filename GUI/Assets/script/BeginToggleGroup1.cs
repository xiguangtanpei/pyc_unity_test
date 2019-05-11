// Simple script that lets you align GameObjects
// position/rotation/scale wise with the selected active transform

using UnityEngine;
using UnityEditor;

public class BeginToggleGroup1 : EditorWindow
{
   // 在多元素实现处理中，多元素的处理非常有用 

    bool[] pos = new bool[3] { true, true, true };
    bool[] rot = new bool[3] { true, true, true };
    bool[] scale = new bool[3] { true, true, true };

    bool posGroupEnabled = true;
    bool rotGroupEnabled = true;
    bool scaleGroupEnabled = false;

    void OnGUI()
    {
        // bool 组的开关直接影响内部的的设置开光
        posGroupEnabled = EditorGUILayout.BeginToggleGroup("Align position", posGroupEnabled);
        pos[0] = EditorGUILayout.Toggle("x", pos[0]);
        pos[1] = EditorGUILayout.Toggle("y", pos[1]);
        pos[2] = EditorGUILayout.Toggle("z", pos[2]);
        EditorGUILayout.EndToggleGroup();

        rotGroupEnabled = EditorGUILayout.BeginToggleGroup("Align rotation", rotGroupEnabled);
        rot[0] = EditorGUILayout.Toggle("x", rot[0]);
        rot[1] = EditorGUILayout.Toggle("y", rot[1]);
        rot[2] = EditorGUILayout.Toggle("z", rot[2]);
        EditorGUILayout.EndToggleGroup();

        scaleGroupEnabled = EditorGUILayout.BeginToggleGroup("Align scale", scaleGroupEnabled);
        scale[0] = EditorGUILayout.Toggle("x", scale[0]);
        scale[1] = EditorGUILayout.Toggle("y", scale[1]);
        scale[2] = EditorGUILayout.Toggle("z", scale[2]);
        EditorGUILayout.EndToggleGroup();

        GUILayout.Space(30);
        if (GUILayout.Button("Align!"))
            Align();
    }

    void Align()
    {

        ///  场景中选的的物体是利用这里来实现，并处理
        Transform[] transforms = Selection.transforms;  
        // foreach ( var  item in transforms )
        // {
        //     Debug.Log( item.name );
            
        // }

        Transform activeTransform = Selection.activeTransform;

        // foreach ( var  item in activeTransform )









        if (transforms.Length < 2)
        {
            Debug.LogWarning("BeginToggleGroup1: select at least two objects.");
            return;
        }


        /// 这里是做了简单的对其， 现在来看应该是 

        /// 激活的应该就是 最后xuan'de d那一个

        for (int i = 0; i < transforms.Length; i++)
        {
            if (posGroupEnabled)
            {
                Vector3 newPos;
                newPos.x = pos[0] ?
                    activeTransform.position.x : transforms[i].position.x;
                newPos.y = pos[1] ?
                    activeTransform.position.y : transforms[i].position.y;
                newPos.z = pos[2] ?
                    activeTransform.position.z : transforms[i].position.z;
                transforms[i].position = newPos;
            }
            if (rotGroupEnabled)
            {
                Vector3 newRot;
                newRot.x = rot[0] ?
                    activeTransform.rotation.eulerAngles.x : transforms[i].rotation.eulerAngles.x;
                newRot.y = rot[1] ?
                    activeTransform.rotation.eulerAngles.y : transforms[i].rotation.eulerAngles.y;
                newRot.z = rot[2] ?
                    activeTransform.rotation.eulerAngles.z : transforms[i].rotation.eulerAngles.z;
                transforms[i].rotation = Quaternion.Euler(newRot);
            }
            if (scaleGroupEnabled)
            {
                Vector3 newScale;
                newScale.x = scale[0] ?
                    activeTransform.localScale.x : transforms[i].localScale.x;
                newScale.y = scale[1] ?
                    activeTransform.localScale.y : transforms[i].localScale.y;
                newScale.z = scale[2] ?
                    activeTransform.localScale.z : transforms[i].localScale.z;
                transforms[i].localScale = newScale;
            }
        }
    }

    [MenuItem("ss/BeginToggleGroup1")]
    static void Init()
    {
        BeginToggleGroup1 window = (BeginToggleGroup1)EditorWindow.GetWindow(typeof(BeginToggleGroup1));
        window.Show();
    }
}