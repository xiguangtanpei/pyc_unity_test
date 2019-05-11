using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(TerrainPaintHelper))]
[CanEditMultipleObjects]
public class Painter : Editor
{
    bool isPaint;
    float brushSize = 16f;
    float brushStronger = 0.5f;
    int ResOption=-1;
    int TreeType = 0;
    List<Texture> TexOption;

    


    void OnEnable()
    {
        TexOption = new List<Texture>();
        TexOption.Add((Texture)AssetDatabase.LoadAssetAtPath("Assets/RTSGameTools/TerrainTools/Res/Add.jpg", typeof(Texture)));
        TexOption.Add((Texture)AssetDatabase.LoadAssetAtPath("Assets/RTSGameTools/TerrainTools/Res/Sub.jpg", typeof(Texture)));
        ResOption = -1;
    }

    void OnSceneGUI()
    {

        if (isPaint)
        {
            Paint();
        }
            
    

    }
    public override void OnInspectorGUI()

    {
        GUIStyle boolBtnOn = new GUIStyle(GUI.skin.GetStyle("Button"));//得到Button样式
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        isPaint = GUILayout.Toggle(isPaint, EditorGUIUtility.IconContent("EditCollider"), boolBtnOn, GUILayout.Width(35), GUILayout.Height(25));//编辑模式开关
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        brushSize = (int)EditorGUILayout.Slider("Brush Size", brushSize, 1, 36);//笔刷大小
        brushStronger = EditorGUILayout.Slider("Brush Stronger", brushStronger, 0, 1f);//笔刷强度
        ResOption = GUILayout.SelectionGrid(ResOption, TexOption.ToArray(), TexOption.Count, GUILayout.Width(32 * (TexOption.Count)), GUILayout.Height(32));
        GUILayout.BeginHorizontal();
        TreeType = GUILayout.SelectionGrid(TreeType, PublicData.TreeViews.ToArray(), PublicData.TreeViews.Count, GUILayout.Width(64 * (PublicData.TreeViews.Count)), GUILayout.Height(64));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < PublicData.PrefabTrees.Count; i++)
        {
            GUILayout.Label(PublicData.PrefabTrees[i].name, GUILayout.Width(64));
        }
        GUILayout.EndHorizontal();

        if (ResOption == 0)
        {
            string path = EditorUtility.OpenFilePanel("Add Tree", "Asset/res/", "prefab");
            if (path.Length != 0)
            {
                path = GetUnityPath(path);
                GameObject temptree = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                if (temptree)
                {
                    if (!PublicData.PrefabTrees.Contains(temptree))
                    {
                        PublicData.PrefabTrees.Add(temptree);
                        PublicData.TreeViews.Add(AssetPreview.GetAssetPreview(temptree));
                    }
                }
                else
                {
                    Debug.Log("没有");
                }
            }
            else
            {
                Debug.Log("no path");
            }
            ResOption = -1;
        }
        if (ResOption == 1)
        {
            if (PublicData.PrefabTrees.Count > TreeType)
            {
                PublicData.PrefabTrees.RemoveAt(TreeType);
                PublicData.TreeViews.RemoveAt(TreeType);
            }
            ResOption = -1;
        }
    }
    string GetUnityPath(string path)
    {
        path = path.Substring(path.IndexOf("Assets"));
        path = path.Replace('\\', '/');
        return path;
    }

    string GetWindowPath(string path)//require System.IO
    {
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        path = files[0].FullName.ToString();
        return path;
    }

    void Paint()
    {     
        Transform CurrentSelect = Selection.activeTransform;
        //MeshFilter temp = CurrentSelect.GetComponent<MeshFilter>();//获取当前模型的MeshFilter
        Terrain temp = CurrentSelect.GetComponent<Terrain>();
        float orthographicSize = (brushSize * CurrentSelect.localScale.x) * (temp.terrainData.size.x / 200);//笔刷在模型上的正交大小        
        bool ToggleF = false;
        Event e = Event.current;//检测输入
        HandleUtility.AddDefaultControl(0);
        RaycastHit raycastHit = new RaycastHit();
        Ray terrain = HandleUtility.GUIPointToWorldRay(e.mousePosition);//从鼠标位置发射一条射线
        if (Physics.Raycast(terrain, out raycastHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("terrain")))//射线检测名为"terrain"的层
        {
            Handles.color = new Color(0.3f,1f, 0.1f, 0.02f);//颜色
            Handles.DrawSolidDisc(raycastHit.point, raycastHit.normal, orthographicSize);
            //鼠标点击或按下并拖动进行绘制
            if ((e.type == EventType.mouseDrag && e.alt == false && e.control == false && e.shift == false && e.button == 0) || (e.type == EventType.MouseDown && e.shift == false && e.alt == false && e.control == false && e.button == 0 && ToggleF == false))
            {
                Vector2 RowColum = PublicData.GetRowAndColumn(raycastHit.point);
                PublicData.AddTrees(raycastHit.point, TreeType);
                //Undo.RegisterCompleteObjectUndo(MaskTex, "meshPaint");//保存历史记录以便撤销
                ToggleF = true;
            }
            else if (e.type == EventType.mouseUp && e.alt == false && e.button == 0 && ToggleF == true)
            {
                ToggleF = false;
            }           
        }
    }

    
}
