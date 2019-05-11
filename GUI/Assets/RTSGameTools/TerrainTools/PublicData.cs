using UnityEngine;
using System.Collections.Generic;

//树木地格结构
public struct TreeData
{
    int treeType;
    int treeID;

    public void Reset()
    {
        treeType = -1;
        treeID = -1;
    }

    public void SetData(int type,int index)
    {
        treeType = type;
        treeID = index;
    }

    public int GetTreeID()
    {
        return treeID;
    }
    public bool Check()
    {
        return treeType >=0;
    }
    public bool Check(int curType)
    {
        return treeType == curType;
    } 
    
}

//工具公用的数据和方法
public static class PublicData
{
    public static bool EditTerrain = false;//工具开关

    public static Terrain BaseTerrain; //要编辑的地形
    public static float offsetRange = 5.0f;//单体树木位置最大偏移距离
    public static float rotateRange = 180.0f;//单体数目的最大旋转角度
    public static float scaleRange  = 1.0f;//单体树木的最大缩放尺度
    public static int typeCount = 0;
    public static List<Texture> TreeViews = new List<Texture>();//GUI使用的树木PreView
    public static List<GameObject> PrefabTrees = new List<GameObject>();//各种树木的预制件

    public static TreeData[,] Data;//树木数据
    public static List<Transform> Trees;//场景中的树木实例
    public static Transform treeRoot;
    //计算当前坐标所在Tile的ID
    public static Vector2 GetRowAndColumn(Vector3 Point)
    {
        Vector2 offset = new Vector2(PublicData.BaseTerrain.transform.position.x, PublicData.BaseTerrain.transform.position.z);
        float tSize = PublicData.BaseTerrain.terrainData.size.x;
        float pRow = (Point.x - offset.x);
        float pColumn = (Point.z - offset.y);
        int tileSize =(int) (tSize / offsetRange);
        Vector2 rAndc = new Vector2((int)(pRow / tileSize), (int)(pColumn / tileSize));
        Debug.Log(pRow + " " + pColumn);
        Debug.Log(pRow + " size" + pColumn);
        Debug.Log("tileSize"+tileSize);
        Debug.Log(rAndc.x + "randc" + rAndc.y);
        return rAndc;
    }
    public static Vector2 GetTileCenterPos(Vector2 index)
    {
        float tSize = PublicData.BaseTerrain.terrainData.size.x;
        float tileSize = tSize / offsetRange;
        return new Vector2(tileSize*index.x+tileSize*0.5f, tileSize * index.y+ tileSize * 0.5f);
    }

    //初始化树木数据
    public static void InitData()
    {
        int wide =(int)BaseTerrain.terrainData.size.x;
        int r = (int)(wide / offsetRange);
        Data = new TreeData[r, r];
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < r; j++)
            {
                Data[j, i].Reset();
            }
        }//(Data.GetUpperBound(0)+1)
        Trees = new List<Transform>();

    }
    //增加树木
    public static void AddTrees(Vector3 editPoint,int treetype)
    {
        Vector2 DataIndex = GetRowAndColumn(editPoint);
        Debug.Log(Data.GetLength(0) + " " + Data.GetLength(1));
        Debug.Log((int)DataIndex.x + " 2 " + (int)DataIndex.y);
        if (Data[(int)DataIndex.x, (int)DataIndex.y].Check(treetype)) return;
        else
        {
            Vector2 centerPos = GetTileCenterPos(DataIndex);
            //Debug.Log("centerpos:"+centerPos);
            Vector3 pos = new Vector3(Random.Range(-offsetRange, offsetRange) + centerPos.x,0, Random.Range(-offsetRange, offsetRange) + centerPos.y)+BaseTerrain.transform.position;
            float Angle = Random.Range(-rotateRange, rotateRange);
            Vector3 scale = new Vector3(1+Random.Range(1-scaleRange, scaleRange-1),1+ Random.Range(1-scaleRange, scaleRange-1),1+ Random.Range(1-scaleRange, scaleRange-1));
            //Debug.Log(PrefabTrees.Count);
            GameObject tree = GameObject.Instantiate(PrefabTrees[treetype], pos, Quaternion.Euler(0, Angle, 0)) as GameObject;
            Transform temptree = tree.transform;
            temptree.localScale = scale;
            temptree.parent = treeRoot;
            TreeData tempData = new TreeData();
          
            if (Data[(int)DataIndex.x, (int)DataIndex.y].Check())
            {
                GameObject.DestroyImmediate(Trees[Data[(int)DataIndex.x, (int)DataIndex.y].GetTreeID()]);
                Trees[Data[(int)DataIndex.x, (int)DataIndex.y].GetTreeID()] = temptree;
                tempData.SetData(treetype, Data[(int)DataIndex.x, (int)DataIndex.y].GetTreeID());

            }
            else
            {
                Trees.Add(temptree);
                tempData.SetData(treetype, Trees.Count);
            }                       
            Data[(int)DataIndex.x, (int)DataIndex.y] = tempData;

        }

        
        
            
       
    }


}
