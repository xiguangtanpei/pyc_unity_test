using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CurveField1 : EditorWindow
{
    AnimationCurve curveX = AnimationCurve.Linear(0, 0, 10, 10);
    AnimationCurve curveY = AnimationCurve.Linear(0, 0, 10, 10);
    AnimationCurve curveZ = AnimationCurve.Linear(0, 0, 10, 10);

    [MenuItem("ss/CurveField1")]
    static void Init()
    {
        CurveField1 window = (CurveField1)EditorWindow.GetWindow(typeof(CurveField1));
        window.Show();
    }

    void OnGUI()
    {
        curveX = EditorGUILayout.CurveField("Animation on X", curveX);
        curveY = EditorGUILayout.CurveField("Animation on Y", curveY);
        curveZ = EditorGUILayout.CurveField("Animation on Z", curveZ);

        if (GUILayout.Button("Generate Curve"))
            AddCurveToSelectedGameObject();
    }

    void  AddCurveToSelectedGameObject()
    {
        if (Selection.activeGameObject)
        {


            var ci  =  curveX.keys  ;
            Debug.Log(ci);
        //     FollowAnimationCurve comp =Selection.activeGameObject.AddComponent<FollowAnimationCurve>();

        //     comp.SetCurves(curveX, curveY, curveZ);
        }
        else
        {
            Debug.LogError("No Game Object selected for adding an animation curve");
        }
    }
}