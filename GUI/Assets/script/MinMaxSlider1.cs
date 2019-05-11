// Place the selected object randomly between the interval of the Min Max Slider
// in the X,Y,Z coords

using UnityEditor;
using UnityEngine;

public class MinMaxSlider1 : EditorWindow
{
    float minVal   = -10;
    float minLimit = -20;
    float maxVal   =  10;
    float maxLimit =  20;

    [MenuItem("ss/MinMaxSlider1")]
    static void Init()
    {
        MinMaxSlider1 window = (MinMaxSlider1)GetWindow(typeof(MinMaxSlider1));
        window.Show();
    }

    void OnGUI()
    {


        /// 这个控件是动态更改，这个是同时返回两个数据， 一个最大 一个最小 
        EditorGUILayout.LabelField("Min Val:", minVal.ToString());
        EditorGUILayout.LabelField("Max Val:", maxVal.ToString());
        EditorGUILayout.MinMaxSlider(ref minVal, ref maxVal, minLimit, maxLimit);
        if (GUILayout.Button("Move!"))
            PlaceRandomly();
    }

    void PlaceRandomly()
    {
        if (Selection.activeTransform)
            Selection.activeTransform.position =
                new Vector3(Random.Range(minVal, maxVal),
                    Random.Range(minVal, maxVal),
                    Random.Range(minVal, maxVal));
        else
            Debug.LogError("Select a GameObject to randomize its position.");
    }
}