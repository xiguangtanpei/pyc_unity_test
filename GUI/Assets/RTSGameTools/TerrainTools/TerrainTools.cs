using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using XMLdata;
using RTSGame;
enum Size
{
    _2x2 = 2,
    _4x4 = 4,
    _8x8 = 8,
    _16x16 = 16,
    _32x32 = 32,
    _64x64 = 64
}
public static class StatPara
{
    public static int BattleFieldSize;
    public static int CitySize;
    public static Vector3 Size = new Vector3();
    public static bool ShowHelper = false;
    public static int Height = 0;
    public static void ResetPara()
    {
        BattleFieldSize = 0;
        CitySize = 0;
        Size = new Vector3();
        ShowHelper = false;
        Height = 0;
    }
}


public class TerrainTools : EditorWindow
{
    #region Greate Window
    //添加标题
    TerrainTools()
    {
        this.titleContent = new GUIContent("Terrain Tools");
    }
    //添加到工具栏
    [MenuItem("RTSGameTools/TerrainTools ")]
    //创建窗口
    static void showWindow()
    {
        GetWindow(typeof(TerrainTools));
    }


    void OnEnable()
    {

        minSize = new Vector2(660, 370);

        if (Application.isPlaying)
            isError = true;
        else
            isError = false;

        if (!isError)
        {
            /*
            if (!PlayerPrefs.HasKey("File Path"))
            {
                PlayerPrefs.SetString("File Path", "Assets/res/terrain/terraindata");
                fileName = "Assets/res/terrain/terraindata";
            }
            else
                fileName = PlayerPrefs.GetString("File Path");
            */
            fileName = "Assets/res/terrain/terraindata";
            selection = Selection.gameObjects;

            if (selection.Length == 1)
                if (selection[0].GetComponent<Terrain>() != null)
                {
                    PublicData.BaseTerrain = selection[0].GetComponent<Terrain>();
                    if (PublicData.EditTerrain)
                    {
                        if (!PublicData.BaseTerrain.gameObject.GetComponent<TerrainPaintHelper>())
                        {
                            PublicData.EditTerrain = false;
                        }
                    }
                    else
                    {
                        if (PublicData.BaseTerrain.gameObject.GetComponent<TerrainPaintHelper>())
                        {
                            PublicData.EditTerrain = true;
                        }
                    }
                }
                    
                else
                    Debug.Log("错误 - 不能获取所选物体 : 所选物体不是地形!");
            else if (selection.Length > 1)
                Debug.Log("错误 - 不能获取所选物体 : 只能选择一个物体!");

            //Create the tooltips
            label1 = new GUIContent("编辑中的地形", "");
            label2 = new GUIContent("地形精度", "必须与原始地形一致，参数名称：detail Resolution per patch");
            label3 = new GUIContent("分块数量", "2x2表示切割成4块");
            label4 = new GUIContent("分块后保存路径", "");
            label5 = new GUIContent("原SplatMap", "Terrain原始地形SplatMap");
            label6 = new GUIContent("新SplatMap", "WorldMachine生成的新SplatMap");
            label7 = new GUIContent("覆盖原始地形数据", "切割后的地形名称会与原地形一样会覆盖掉原始地形数据");
            label8 = new GUIContent("边缘混合", "将 alphamap 边缘与临近Tile数值上保持一致 " );
            label9 = new GUIContent("复制树木", "");
            label10 = new GUIContent("复制花草", "");

            label11 = new GUIContent("位移范围", "每颗树的随机偏移最大距离");
            label12 = new GUIContent("旋转范围", "每颗树的随机旋转最大角度");
            label13 = new GUIContent("缩放范围", "每颗树的随机缩放最大尺度");

        }
    }
    void OnDestroy()
    {
        StatPara.ResetPara();
        if (helper)
        {
            DestroyImmediate(helper);
            helper= null;
        }
       
    }

    #endregion

    #region GUI Parameter


    private TerrainData baseData;
    static GameObject helper=null;
    private Texture2D Splatmap;
    private Texture2D New;
    bool FlipVertical;

    int toolbarInt = 0;
    string[] toolbarStrings = { "替换SplatMap","辅助线","导出树木" ,"切割地形" ,"导出地形"};

    bool ParameterCheck = false;    
    string ButtonName = "显示线框";

    int xoffset = 0;
    int yoffset = 0;
    #endregion

    #region SlicerParameter

    private GameObject[] terrainGameObjects;
    private Terrain[] terrains;
    private TerrainData[] data;
    private Vector3 oripos;

    static Size enumValue = Size._2x2;
    static int resolutionPerPatch = 8;


    private string fileName;

    private int size;
    private int terrainsWide;
    private int terrainsLong;


    private int x;
    private int y;
    private int i;
    private int z;

    private float oldWidth;
    private float oldHeight;
    private float oldLength;
    private float newWidth;
    private float newLength;


    private float xPos;
    private float yPos;
    private float zPos;

    private int newHeightMapResolution;
    private int newEvenHeightMapResolution;


    private int newDetailResolution;
    private int newAlphaMapResolution;
    private int newBaseMapResolution;

    private SplatPrototype[] splatProtos;
    private DetailPrototype[] detailProtos;
    private TreePrototype[] treeProtos;
    private TreeInstance[] treeInst;

    private float grassStrength;
    private float grassAmount;
    private float grassSpeed;
    private Color grassTint;

    private int[] layers;
    private int arrayPos;


    private float progress;
    private float progressScale;

    private bool createPressed = false;

    private GameObject[] selection;

    private float treeDistance;
    private float treeBillboardDistance;
    private float treeCrossFadeLength;
    private float treeMaximumFullLODCount;
    private float detailObjectDistance;
    private float detailObjectDensity;
    private float heightmapPixelError;
    private float heightmapMaximumLOD;
    private float basemapDistance;
    private int lightmapIndex;
    private bool castShadows;
    private Material materialTemplate;
    private TerrainRenderFlags editorRenderFlags;

    private bool isError;

    static bool blend = true;

    static bool copyAllTrees;
    static bool copyAllDetails;

   

    #endregion

    #region PaiterParameter


    #endregion

    #region tip
    //提示
    private GUIContent label1;
    private GUIContent label2;
    private GUIContent label3;
    private GUIContent label4;
    private GUIContent label5;
    private GUIContent label6;
    private GUIContent label7;
    private GUIContent label8;
    private GUIContent label9;
    private GUIContent label10;

    private GUIContent label11;
    private GUIContent label12;
    private GUIContent label13;
    #endregion
    void OnGUI()
    {
        if (Application.isPlaying)
            isError = true;
        GUILayout.BeginVertical();
        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Terrain Tools");
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Timi-RSTGame-TA");
        GUILayout.Space(10);

        PublicData.BaseTerrain = EditorGUILayout.ObjectField(label1, PublicData.BaseTerrain, typeof(Terrain), true) as Terrain;

        if (!isError)
        {
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
            switch (toolbarInt)
            {
                case 0:
                    GUIToolbar0();
                    break;
                 case 1:
                    GUIToolbar1();
                    break;
                case 2:
                    GUIToolbar2();
                    break;
                case 3:
                    GUIToolbar3();
                    break;
                case 4:
                    GUIToolbar4();
                    break;
            }
        }
        else
            EditorGUILayout.LabelField("该工具不能再运行模式下工作. 退出运行模式重新启动该工具");
        GUILayout.EndVertical();


    }

    void GUIToolbar0()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        Splatmap = EditorGUILayout.ObjectField(label5, Splatmap, typeof(Texture2D), true) as Texture2D;
        New = EditorGUILayout.ObjectField(label6, New, typeof(Texture2D), true) as Texture2D;
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("替换纹理"))
        {
            Replace();
        }
    }

    void GUIToolbar1()
    {
        if (ParameterCheck)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("注意：");
            GUILayout.Label("点击按钮后，请点击一下地形来刷新Scene窗口");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(ButtonName))
            {
                StatPara.ShowHelper = !StatPara.ShowHelper;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("地形名称：");
            GUILayout.Label(PublicData.BaseTerrain.gameObject.name);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("战场中心大小：");
            GUILayout.Label(StatPara.BattleFieldSize.ToString());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("战场城池大小：");
            GUILayout.Label(StatPara.CitySize.ToString());
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("参考基线高度：");
            StatPara.Height = EditorGUILayout.IntSlider(StatPara.Height, 0, 50);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("中心横向位移：");
            xoffset = EditorGUILayout.IntSlider(xoffset, -512, 512);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("中心纵向位移：");
            yoffset = EditorGUILayout.IntSlider(yoffset, -512, 512);
            EditorGUILayout.EndHorizontal();
            StatPara.Size = new Vector3(PublicData.BaseTerrain.terrainData.size.x+xoffset*2, 0, PublicData.BaseTerrain.terrainData.size.z+yoffset*2);
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.Space(10);
            //战场中心尺寸
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("战场中心大小：");
            StatPara.BattleFieldSize = EditorGUILayout.IntField(StatPara.BattleFieldSize);
            EditorGUILayout.EndHorizontal();
            //战场城池尺寸
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("战场城池大小：");
            StatPara.CitySize = EditorGUILayout.IntField(StatPara.CitySize);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();         
            if (GUILayout.Button("参数检查"))
            {
                ParameterCheck = false;
                if (DrawHelperCheck())
                {
                    ParameterCheck = true;
                    if (!helper)
                    {
                        helper = new GameObject();
                        helper.AddComponent<DrawHelper>();                      
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        if (StatPara.ShowHelper)
        {
            ButtonName = "点击关闭线框";
        }
        else
        {
            ButtonName = "点击显示线框";
        }
    }

    void GUIToolbar2()
    {
        GUILayout.Space(10);
        if (GUILayout.Button("输出XML数据"))
        {
            ExportTreeXML XMLdata = new ExportTreeXML();
            XMLdata.CreateXML(PublicData.BaseTerrain);
        }

    }

    void GUIToolbar3()
    {
            EditorGUILayout.LabelField(""); // Used for Spacing only

            enumValue = (Size)EditorGUILayout.EnumPopup(label3, enumValue);
            EditorGUILayout.LabelField("");// Used for Spacing only

            fileName = EditorGUILayout.TextField(label4, fileName);

            EditorGUILayout.LabelField(""); // Used for Spacing only
            blend = EditorGUILayout.Toggle(label8, blend);

            copyAllTrees = EditorGUILayout.Toggle(label9, copyAllTrees);
            copyAllDetails = EditorGUILayout.Toggle(label10, copyAllDetails);

            EditorGUILayout.LabelField(""); // Used for Spacing only

            if (GUILayout.Button("Create Terrain"))
            {
                if (PublicData.BaseTerrain != null)
                {
                    createPressed = true;

                    StoreData();

                    if (CheckForErrors())
                    {
                        CreateTerrainData();
                        CopyTerrainData();
                        //Optional step
                        if (blend)
                            BlendEdges();
                        SetNeighbors();
                        this.Close();
                    }
                    else
                        createPressed = false;
                }
                else
                {
                    this.ShowNotification(new GUIContent("没有选择需要编辑的地形"));
                    GUIUtility.keyboardControl = 0; // Added to shift focus to original window rather than the notification
                }
            }
       
    }

    void GUIToolbar4()
    {
        GUILayout.Space(10);
        if (GUILayout.Button("导出OBJ文件"))
        {
            ExportMeshToObj.Export();
        }
        GUI.skin.label.fontSize = 10;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("注意：选中要导出的物体\n 确定是否导出材质球 \n 确定单位比例 \n 点击“这是导出按钮”\n 再选择导出路径 \n 导出名称默认是场景名字 \n by: xiguang yang");
    }

    void Replace()
    {
        if ((Splatmap == null) || (New == null))
        {
            EditorUtility.DisplayDialog("纹理为空", " 请将splatmap选择就位", "返回");
            return;
        }
        if (New.format != TextureFormat.RGBA32 && New.format != TextureFormat.ARGB32 && New.format != TextureFormat.RGB24)
        {
            EditorUtility.DisplayDialog("格式错误", " 纹理格式需要时 RGBA 32", "返回");
            return;
        }

        var w = New.width;
        if (Mathf.ClosestPowerOfTwo(w) != w)
        {
            EditorUtility.DisplayDialog("尺寸错误", "纹理长宽的必须是2的密次倍", "返回");
            return;
        }

        try
        {
            var pixels = New.GetPixels();
            if (FlipVertical)
            {
                var h = w; // always square in unity
                for (var y = 0; y < h / 2; y++)
                {
                    var otherY = h - y - 1;
                    for (var x = 0; x < w; x++)
                    {
                        var swapval = pixels[y * w + x];
                        pixels[y * w + x] = pixels[otherY * w + x];
                        pixels[otherY * w + x] = swapval;
                    }
                }
            }
            Splatmap.Resize(New.width, New.height, New.format, true);
            Splatmap.SetPixels(pixels);
            Splatmap.Apply();
        }
        catch (IOException)
        {
            EditorUtility.DisplayDialog("纹理不能读取", "新的SplatMap纹理导入设置中要勾选Read/Write选项", "返回");
            return;
        }
    }
    bool DrawHelperCheck()
    {
        bool returnValue = true;
        if (PublicData.BaseTerrain == null)
        {
            EditorUtility.DisplayDialog("参数错误", "地形未赋值，请拖拽需要编辑的地形到工具中参数“编辑中的地形”上", "确认");
            return false;
        }
        StatPara.Size = PublicData.BaseTerrain.terrainData.size; ;
        if (StatPara.Size.x != StatPara.Size.z)
        {
            EditorUtility.DisplayDialog("资源错误", "地形不是正方形，请确认地形长和宽数据", "确认");
            return false;
        }
        if (StatPara.Size.x <= StatPara.BattleFieldSize || StatPara.Size.x <= StatPara.CitySize || StatPara.BattleFieldSize <= StatPara.CitySize)
        {
            EditorUtility.DisplayDialog("参数错误", "请按照以下大小来设置参数：\n 地形大小>战场中心大小>战场城池大小", "确认");
            return false;
        }             
        return returnValue;
    }


    //Retrieve and store all the data from the old terrain into our variables
    void StoreData()
    {
        size = (int)enumValue;
        terrainsLong = size;
        terrainsWide = size;

        baseData = PublicData.BaseTerrain.terrainData;

        oldWidth = baseData.size.x;
        oldHeight = baseData.size.y;
        oldLength = baseData.size.z;

        newWidth = oldWidth / terrainsWide;
        newLength = oldLength / terrainsLong;

        xPos = PublicData.BaseTerrain.GetPosition().x;
        yPos = PublicData.BaseTerrain.GetPosition().y;
        zPos = PublicData.BaseTerrain.GetPosition().z;

        newHeightMapResolution = ((baseData.heightmapResolution - 1) / size) + 1;
        newEvenHeightMapResolution = newHeightMapResolution - 1;

        newDetailResolution = baseData.detailResolution / size;
        newAlphaMapResolution = baseData.alphamapResolution / size;
        newBaseMapResolution = baseData.baseMapResolution / size;

        treeDistance = PublicData.BaseTerrain.treeDistance;
        treeBillboardDistance = PublicData.BaseTerrain.treeBillboardDistance;
        treeCrossFadeLength = PublicData.BaseTerrain.treeCrossFadeLength;
        treeMaximumFullLODCount = PublicData.BaseTerrain.treeMaximumFullLODCount;
        detailObjectDistance = PublicData.BaseTerrain.detailObjectDistance;
        detailObjectDensity = PublicData.BaseTerrain.detailObjectDensity;
        heightmapPixelError = PublicData.BaseTerrain.heightmapPixelError;
        heightmapMaximumLOD = PublicData.BaseTerrain.heightmapMaximumLOD;
        basemapDistance = PublicData.BaseTerrain.basemapDistance;
        lightmapIndex = PublicData.BaseTerrain.lightmapIndex;
        castShadows = PublicData.BaseTerrain.castShadows;
        editorRenderFlags = PublicData.BaseTerrain.editorRenderFlags;

        splatProtos = baseData.splatPrototypes;
        detailProtos = baseData.detailPrototypes;
        treeProtos = baseData.treePrototypes;
        treeInst = baseData.treeInstances;

        grassStrength = baseData.wavingGrassStrength;
        grassAmount = baseData.wavingGrassAmount;
        grassSpeed = baseData.wavingGrassSpeed;
        grassTint = baseData.wavingGrassTint;
    }
    //Check for any errors with User Input
    bool CheckForErrors()
    {
        if (newHeightMapResolution < 33)
        {
            this.ShowNotification(new GUIContent("Error with Heightmap Resolution - See Console for More Information"));
            GUIUtility.keyboardControl = 0;
            Debug.Log("The Heightmap Resolution for the new terrains must be 33 or larger. Currently it is " + newHeightMapResolution.ToString() + ".\nThe new Heightmap Resolution is calculated as"
            + "follows: New Resolution = ((Old Resolution - 1) / New Dimension Width) + 1 -- For example, a 4x4 grid has a New Dimension Width of 4.\n You can rectify this problem by"
            + "either increasing the heightmap resolution of the base terrain, or reducing the number of new terrains to be created.");
            return false;
        }
        else if (newAlphaMapResolution < 16)
        {
            this.ShowNotification(new GUIContent("Error with AlphaMap Resolution - See Console for More Information"));
            GUIUtility.keyboardControl = 0;
            Debug.Log("The Alpha Map Resolution of the new terrains is too small. Value must be 16 or greater. Current value is " + newAlphaMapResolution.ToString()
            + ".\nPlease increase the Base Terrains alpha map resolution or reduce the number of new terrains to be created.");
            return false;
        }
        else if (newBaseMapResolution < 16)
        {
            this.ShowNotification(new GUIContent("Error with BaseMap Resolution - See Console for More Information"));
            GUIUtility.keyboardControl = 0;
            Debug.Log("The Base Map Resolution of the new terrains is too small. Value must be 16 or greater. Current value is " + newBaseMapResolution.ToString()
            + ".\nPlease increase the Base Terrains base map resolution or reduce the number of new terrains to be created.");
            return false;
        }
        else if (baseData.detailResolution % size != 0)
        {
            this.ShowNotification(new GUIContent("Error with Detail Resolution - See Console for More Information"));
            GUIUtility.keyboardControl = 0;
            Debug.Log("The Base Terrains detail resolution does not divide perfectly. Please change the detail resolution or number of terrains to be created to rectify this issue.");
            return false;
        }
        else if (AssetDatabase.LoadAssetAtPath(fileName + "/" + PublicData.BaseTerrain.name + "_Data_" + 1 + "_" + 1 + ".asset", typeof(TerrainData)) != null)
        {

            this.ShowNotification(new GUIContent("保存路径下有重名文件"));
            GUIUtility.keyboardControl = 0;
            return false;
        }
        else
            return true;
    }
    //Create the terrain data (including
    void CreateTerrainData()
    {
        progress = 0.0f;
        EditorUtility.DisplayProgressBar("Progress", "Generating Terrains", progress);

        if (!Mathf.IsPowerOfTwo(newDetailResolution))
            Debug.Log("Detail Resolution of new terrains is not a power of 2. Accurate results are not guaranteed.");

        if (newDetailResolution % resolutionPerPatch != 0)
            Debug.Log("Detail Resolution of new terrains does not divide resolution per patch value evenly. Unity will\n" +
            " automatically downgrade resolution to a value that does divide evenly, however, accurate results are not guaranteed.");

        terrainGameObjects = new GameObject[terrainsLong * terrainsWide];

        terrains = new Terrain[terrainsLong * terrainsWide];

        data = new TerrainData[terrainsLong * terrainsWide];

        progressScale = .9f / (terrainsLong * terrainsWide);

        for (y = 0; y < terrainsLong; y++)
        {
            for (x = 0; x < terrainsWide; x++)
            {
                AssetDatabase.CreateAsset(new TerrainData(), fileName + "/" + PublicData.BaseTerrain.name + "_Data_" + (y + 1) + "_" + (x + 1) + ".asset");
                progress += progressScale;
                EditorUtility.DisplayProgressBar("Progress", "Generating Terrains", progress);
            }
        }
    }
    void CopyTerrainData()
    {
        progressScale = .2f / (terrainsLong * terrainsWide);
        arrayPos = 0;

        for (y = 0; y < terrainsLong; y++)
        {
            for (x = 0; x < terrainsWide; x++)
            {
                terrainGameObjects[arrayPos] = Terrain.CreateTerrainGameObject((TerrainData)AssetDatabase.LoadAssetAtPath(fileName + "/" + PublicData.BaseTerrain.name + "_Data_" + (y + 1) + "_" + (x + 1) + ".asset", typeof(TerrainData)));

                terrainGameObjects[arrayPos].name = PublicData.BaseTerrain.name + "_Slice_" + (y + 1) + "_" + (x + 1);

                terrains[arrayPos] = terrainGameObjects[arrayPos].GetComponent<Terrain>();

                SetRenderLightmapParameters(terrains[arrayPos], CreateLightmapParameterFile(fileName, terrainGameObjects[arrayPos].name, (y*terrainsLong+x*terrainsWide+1)));//给每个terrain sliced tile 赋予LightmapParameter

                data[arrayPos] = terrains[arrayPos].terrainData;

                data[arrayPos].heightmapResolution = newEvenHeightMapResolution;

                data[arrayPos].alphamapResolution = newAlphaMapResolution;

                data[arrayPos].baseMapResolution = newBaseMapResolution;

                data[arrayPos].SetDetailResolution(newDetailResolution, resolutionPerPatch);

                data[arrayPos].size = new Vector3(newWidth, oldHeight, newLength);

                //Splat prototypes
                SplatPrototype[] tempSplats = new SplatPrototype[splatProtos.Length];

                for (i = 0; i < splatProtos.Length; i++)
                {
                    tempSplats[i] = new SplatPrototype();
                    tempSplats[i].texture = splatProtos[i].texture;

                    //******Line to uncomment below if Using 4.x.x********//
                    //****************************************************//
                    //****************************************************//
                    //tempSplats[i].normalMap = splatProtos[i].normalMap;
                    //****************************************************//
                    //****************************************************//
                    //************Line to uncomment above*****************//

                    tempSplats[i].tileSize = new Vector2(splatProtos[i].tileSize.x, splatProtos[i].tileSize.y);


                    tempSplats[i].tileOffset = new Vector2((newWidth * x) % splatProtos[i].tileSize.x + splatProtos[i].tileOffset.x, (newLength * y) % splatProtos[i].tileSize.y + splatProtos[i].tileOffset.y);
                }
                data[arrayPos].splatPrototypes = tempSplats;





                layers = baseData.GetSupportedLayers(x * data[arrayPos].detailWidth - 1, y * data[arrayPos].detailHeight - 1, data[arrayPos].detailWidth, data[arrayPos].detailHeight);
                int layerLength = layers.Length;

                if (copyAllDetails)
                    data[arrayPos].detailPrototypes = detailProtos;
                else
                {
                    DetailPrototype[] tempDetailProtos = new DetailPrototype[layerLength];
                    for (i = 0; i < layerLength; i++)
                        tempDetailProtos[i] = detailProtos[layers[i]];
                    data[arrayPos].detailPrototypes = tempDetailProtos;
                }

                for (i = 0; i < layerLength; i++)
                    data[arrayPos].SetDetailLayer(0, 0, i, baseData.GetDetailLayer(x * data[arrayPos].detailWidth, y * data[arrayPos].detailHeight, data[arrayPos].detailWidth, data[arrayPos].detailHeight, layers[i]));

                System.Array.Clear(layers, 0, layers.Length);

                //if copy all trees is checked, we can just set each terrains tree prototypes to the base terrain. We'll skip this step if it's unchecked, and execute
                //a more complicated algorithm below instead.
                if (copyAllTrees)
                    data[arrayPos].treePrototypes = treeProtos;



                data[arrayPos].wavingGrassStrength = grassStrength;
                data[arrayPos].wavingGrassAmount = grassAmount;
                data[arrayPos].wavingGrassSpeed = grassSpeed;
                data[arrayPos].wavingGrassTint = grassTint;



                data[arrayPos].SetHeights(0, 0, baseData.GetHeights(x * (data[arrayPos].heightmapWidth - 1), y * (data[arrayPos].heightmapHeight - 1), data[arrayPos].heightmapWidth, data[arrayPos].heightmapHeight));
                float[,,] map = new float[newAlphaMapResolution, newAlphaMapResolution, splatProtos.Length];
                map = baseData.GetAlphamaps(x * data[arrayPos].alphamapWidth, y * data[arrayPos].alphamapHeight, data[arrayPos].alphamapWidth, data[arrayPos].alphamapHeight);
                data[arrayPos].SetAlphamaps(0, 0, map);

                terrainGameObjects[arrayPos].GetComponent<TerrainCollider>().terrainData = data[arrayPos];

                terrainGameObjects[arrayPos].transform.position = new Vector3(x * newWidth + xPos, yPos, y * newLength + zPos);



                arrayPos++;

                progress += progressScale;

                EditorUtility.DisplayProgressBar("Progress", "Generating Terrains", progress);

            }//End the x for loop
        }//End the y for loop

        for (y = 0; y < terrains.Length; y++)
        {
            terrains[y].treeDistance = treeDistance;
            terrains[y].treeBillboardDistance = treeBillboardDistance;
            terrains[y].treeCrossFadeLength = treeCrossFadeLength;
            terrains[y].treeMaximumFullLODCount = (int)treeMaximumFullLODCount;
            terrains[y].detailObjectDistance = detailObjectDistance;
            terrains[y].detailObjectDensity = detailObjectDensity;
            terrains[y].heightmapPixelError = heightmapPixelError;
            terrains[y].heightmapMaximumLOD = (int)heightmapMaximumLOD;
            terrains[y].basemapDistance = basemapDistance;
            terrains[y].lightmapIndex = lightmapIndex;
            terrains[y].castShadows = castShadows;
            terrains[y].editorRenderFlags = editorRenderFlags;

            //******Line to uncomment below if Using 4.x.x********//
            //****************************************************//
            //****************************************************//
            //terrains[y].materialTemplate = materialTemplate;
            //****************************************************//
            //****************************************************//
            //************Line to uncomment above*****************//
        }
        //Only execute these lines of code if copyAllTrees is false
        int[,] treeTypes = new int[0, 0];
        if (!copyAllTrees)
        {
            treeTypes = new int[terrainsWide * terrainsLong, treeProtos.Length];
            //Loop through every single tree
            for (i = 0; i < treeInst.Length; i++)
            {
                Vector3 origPos2 = Vector3.Scale(new Vector3(oldWidth, 1, oldLength), new Vector3(treeInst[i].position.x, treeInst[i].position.y, treeInst[i].position.z));

                int column2 = Mathf.FloorToInt(origPos2.x / newWidth);
                int row2 = Mathf.FloorToInt(origPos2.z / newLength);

                treeTypes[(row2 * terrainsWide) + column2, treeInst[i].prototypeIndex] = 1;
            }

            for (i = 0; i < terrainsWide * terrainsLong; i++)
            {
                int numOfPrototypes = 0;
                for (y = 0; y < treeProtos.Length; y++)
                    if (treeTypes[i, y] == 1)
                        numOfPrototypes++;
                //else --not necessary I think
                //treeTypes[i,y] = treeProtos.Length; //replace the 0 at this spot with the length of the treeProtos array. Later, if we find this spot has this value,
                //we'll know that this prototype is not found on this terrain. We will need to know this.
                TreePrototype[] tempPrototypes = new TreePrototype[numOfPrototypes];
                int tempIndex = 0;
                for (y = 0; y < treeProtos.Length; y++)
                    if (treeTypes[i, y] == 1)
                    {
                        tempPrototypes[tempIndex] = treeProtos[y];
                        //In addition, replace the value at tempTypes[i,y] with the index of where that prototype is stored for that terrain, like this
                        treeTypes[i, y] = tempIndex;
                        tempIndex++;
                    }

                data[i].treePrototypes = tempPrototypes;
            }
        }

        for (i = 0; i < treeInst.Length; i++)
        {
            Vector3 origPos = Vector3.Scale(new Vector3(oldWidth, 1, oldLength), new Vector3(treeInst[i].position.x, treeInst[i].position.y, treeInst[i].position.z));

            int column = Mathf.FloorToInt(origPos.x / newWidth);
            int row = Mathf.FloorToInt(origPos.z / newLength);

            Vector3 tempVect = new Vector3((origPos.x - (newWidth * column)) / newWidth, origPos.y, (origPos.z - (newLength * row)) / newLength);
            TreeInstance tempTree = new TreeInstance();

            tempTree.position = tempVect;
            tempTree.widthScale = treeInst[i].widthScale;
            tempTree.heightScale = treeInst[i].heightScale;
            tempTree.color = treeInst[i].color;
            tempTree.lightmapColor = treeInst[i].lightmapColor;

            if (copyAllTrees)
                tempTree.prototypeIndex = treeInst[i].prototypeIndex;
            else
                tempTree.prototypeIndex = treeTypes[(row * terrainsWide) + column, treeInst[i].prototypeIndex];

            terrains[(row * terrainsWide) + column].AddTreeInstance(tempTree);



        }
        //refresh prototypes
        for (i = 0; i < terrainsWide * terrainsLong; i++)
            data[i].RefreshPrototypes();
    }
    // Blend function
    void BlendEdges()
    {
        int alphaWidth = data[0].alphamapWidth;
        int alphaHeight = data[0].alphamapHeight;
        int numOfSplats = data[0].splatPrototypes.Length;
        float avg;

        if (terrainsWide > 1 && terrainsLong == 1)
        {
            for (x = 0; x < terrainsWide - 1; x++)
            {
                float[,,] mapLeft = data[x].GetAlphamaps(0, 0, alphaWidth, alphaHeight);
                float[,,] mapRight = data[x + 1].GetAlphamaps(0, 0, alphaWidth, alphaHeight);

                for (i = 0; i < alphaHeight; i++)
                    for (y = 0; y < numOfSplats; y++)
                    {
                        avg = (mapLeft[i, alphaWidth - 1, y] + mapRight[i, 0, y]) / 2f;
                        mapLeft[i, alphaWidth - 1, y] = avg;
                        mapRight[i, 0, y] = avg;
                    }

                data[x].SetAlphamaps(0, 0, mapLeft);
                data[x + 1].SetAlphamaps(0, 0, mapRight);
            }
        }


        //Single column / multiiple rows
        else if (terrainsLong > 1 && terrainsWide == 1)
        {


            for (x = 0; x < terrainsLong - 1; x++)
            {
                float[,,] mapBottom = data[x].GetAlphamaps(0, 0, alphaWidth, alphaHeight);
                float[,,] mapTop = data[x + 1].GetAlphamaps(0, 0, alphaWidth, alphaHeight);

                for (i = 0; i < alphaWidth; i++)
                    for (y = 0; y < numOfSplats; y++)
                    {
                        avg = (mapBottom[alphaHeight - 1, i, y] + mapTop[0, i, y]) / 2f;
                        mapBottom[alphaHeight - 1, i, y] = avg;
                        mapTop[0, i, y] = avg;
                    }

                data[x].SetAlphamaps(0, 0, mapBottom);
                data[x + 1].SetAlphamaps(0, 0, mapTop);
            }
        }

        //multiple row/ columns
        else if (terrainsWide > 1 && terrainsLong > 1)
        {
            //set arrayPos to -2 so it will be at 0 for the first terrain.		
            arrayPos = -2;
            for (z = 0; z < terrainsLong - 1; z++)
            {
                arrayPos++;
                for (x = 0; x < terrainsWide - 1; x++)
                {
                    arrayPos++;
                    float[,,] mapBLeft = data[arrayPos].GetAlphamaps(0, 0, alphaWidth, alphaHeight);
                    float[,,] mapBRight = data[arrayPos + 1].GetAlphamaps(0, 0, alphaWidth, alphaHeight);
                    float[,,] mapTLeft = data[arrayPos + terrainsWide].GetAlphamaps(0, 0, alphaWidth, alphaHeight);
                    float[,,] mapTRight = data[arrayPos + terrainsWide + 1].GetAlphamaps(0, 0, alphaWidth, alphaHeight);

                    //Always do these two things, no matter where the terrain lies in the group

                    //Set the edge between the BRight and TRight
                    for (i = 1; i < alphaWidth - 1; i++)
                        for (y = 0; y < numOfSplats; y++)
                        {
                            avg = (mapBRight[alphaHeight - 1, i, y] + mapTRight[0, i, y]) / 2f;
                            mapBRight[alphaHeight - 1, i, y] = avg;
                            mapTRight[0, i, y] = avg;
                        }

                    //Set the edge between the top left and top right terrains
                    for (i = 1; i < alphaHeight - 1; i++)
                        for (y = 0; y < numOfSplats; y++)
                        {
                            avg = (mapTLeft[i, alphaWidth - 1, y] + mapTRight[i, 0, y]) / 2f;
                            mapTLeft[i, alphaWidth - 1, y] = avg;
                            mapTRight[i, 0, y] = avg;
                        }

                    //Set the corner between the four terrains
                    for (y = 0; y < numOfSplats; y++)
                    {
                        avg = (mapBLeft[alphaHeight - 1, alphaWidth - 1, y] + mapBRight[alphaHeight - 1, 0, y] + mapTLeft[0, alphaWidth - 1, y] + mapTRight[0, 0, y]) / 4f;
                        mapBLeft[alphaHeight - 1, alphaWidth - 1, y] = avg;
                        mapBRight[alphaHeight - 1, 0, y] = avg;
                        mapTLeft[0, alphaWidth - 1, y] = avg;
                        mapTRight[0, 0, y] = avg;
                    }

                    //If the terrain is on the bottom row
                    if (z == 0)
                    {
                        //Set the edge between the bottom left and bottom right terrains
                        for (i = 1; i < alphaHeight - 1; i++)
                            for (y = 0; y < numOfSplats; y++)
                            {
                                avg = (mapBLeft[i, alphaWidth - 1, y] + mapBRight[i, 0, y]) / 2f;
                                mapBLeft[i, alphaWidth - 1, y] = avg;
                                mapBRight[i, 0, y] = avg;
                            }

                        //Set the bottom most spot point between BLeft and BRight
                        for (y = 0; y < numOfSplats; y++)
                        {
                            avg = (mapBLeft[0, alphaWidth - 1, y] + mapBRight[0, 0, y]) / 2f;
                            mapBLeft[0, alphaWidth - 1, y] = avg;
                            mapBRight[0, 0, y] = avg;
                        }
                    }


                    //If the terrain is also in the first column
                    if (x == 0)
                    {
                        //Set the edge between the BLeft and TLeft
                        for (i = 1; i < alphaWidth - 1; i++)
                            for (y = 0; y < numOfSplats; y++)
                            {
                                avg = (mapBLeft[alphaHeight - 1, i, y] + mapTLeft[0, i, y]) / 2f;
                                mapBLeft[alphaHeight - 1, i, y] = avg;
                                mapTLeft[0, i, y] = avg;
                            }


                        //Set the left most point between BLeft and TLeft
                        for (y = 0; y < numOfSplats; y++)
                        {
                            avg = (mapBLeft[alphaHeight - 1, 0, y] + mapTLeft[0, 0, y]) / 2f;
                            mapBLeft[alphaHeight - 1, 0, y] = avg;
                            mapTLeft[0, 0, y] = avg;
                        }


                    }

                    //if this is the second to last terrain in the row
                    if (x == terrainsWide - 2)
                        for (y = 0; y < numOfSplats; y++)
                        {
                            //Set the right most point between the Bright map and Tright map
                            avg = (mapBRight[alphaHeight - 1, alphaWidth - 1, y] + mapTRight[0, alphaWidth - 1, y]) / 2f;
                            mapBRight[alphaHeight - 1, alphaWidth - 1, y] = avg;
                            mapTRight[0, alphaWidth - 1, y] = avg;
                        }
                    //if this is the second to last terrain in the column
                    if (z == terrainsLong - 2)
                        for (y = 0; y < numOfSplats; y++)
                        {
                            //Set the right most point between the TLeft map and TRight map
                            avg = (mapTLeft[alphaHeight - 1, alphaWidth - 1, y] + mapTRight[alphaHeight - 1, 0, y]) / 2f;
                            mapTLeft[alphaHeight - 1, alphaWidth - 1, y] = avg;
                            mapTRight[alphaHeight - 1, 0, y] = avg;
                        }

                    data[arrayPos].SetAlphamaps(0, 0, mapBLeft);
                    data[arrayPos + 1].SetAlphamaps(0, 0, mapBRight);
                    data[arrayPos + terrainsWide].SetAlphamaps(0, 0, mapTLeft);
                    data[arrayPos + terrainsWide + 1].SetAlphamaps(0, 0, mapTRight);
                }//End of x loop
            }//End of z loop
        }//End of else if
    }//End of Blend function
    //SetNeighbors
    void SetNeighbors()
    {
        arrayPos = 0;

        for (y = 0; y < terrainsLong; y++)
        {
            for (x = 0; x < terrainsWide; x++)
            {
                if (y == 0)
                {
                    if (x == 0)
                        terrains[arrayPos].SetNeighbors(null, terrains[arrayPos + terrainsWide], terrains[arrayPos + 1], null);
                    else if (x == terrainsWide - 1)
                        terrains[arrayPos].SetNeighbors(terrains[arrayPos - 1], terrains[arrayPos + terrainsWide], null, null);
                    else
                        terrains[arrayPos].SetNeighbors(terrains[arrayPos - 1], terrains[arrayPos + terrainsWide], terrains[arrayPos + 1], null);
                }
                else if (y == terrainsLong - 1)
                {
                    if (x == 0)
                        terrains[arrayPos].SetNeighbors(null, null, terrains[arrayPos + 1], terrains[arrayPos - terrainsWide]);
                    else if (x == terrainsWide - 1)
                        terrains[arrayPos].SetNeighbors(terrains[arrayPos - 1], null, null, terrains[arrayPos - terrainsWide]);
                    else
                        terrains[arrayPos].SetNeighbors(terrains[arrayPos - 1], null, terrains[arrayPos + 1], terrains[arrayPos - terrainsWide]);
                }
                else
                {
                    if (x == 0)
                        terrains[arrayPos].SetNeighbors(null, terrains[arrayPos + terrainsWide], terrains[arrayPos + 1], terrains[arrayPos - terrainsWide]);
                    else if (x == terrainsWide - 1)
                        terrains[arrayPos].SetNeighbors(terrains[arrayPos - 1], terrains[arrayPos + terrainsWide], null, terrains[arrayPos - terrainsWide]);
                    else
                        terrains[arrayPos].SetNeighbors(terrains[arrayPos - 1], terrains[arrayPos + terrainsWide], terrains[arrayPos + 1], terrains[arrayPos - terrainsWide]);
                }

                //Increment arrayPos
                arrayPos++;
            }//End the x for loop	
        }//End the y for loop

        for (i = 0; i < terrainsWide * terrainsLong; i++)
            terrains[i].Flush();

        EditorUtility.ClearProgressBar();

    }//End the button press if statement
    LightmapParameters CreateLightmapParameterFile(string path, string name, int bakeTag)//创建lightmapparameter 
    {
        LightmapParameters lp = new LightmapParameters();
        lp.bakedLightmapTag = bakeTag;
        path = path + "/"+ name + ".asset";
        AssetDatabase.CreateAsset(lp, path);
        AssetDatabase.ImportAsset(path);
        return (LightmapParameters)AssetDatabase.LoadAssetAtPath(path, typeof(LightmapParameters));
    }
    void SetRenderLightmapParameters(Terrain terrain, LightmapParameters param)//将lightmapparameter赋值给Terrain  //Renderer renderer,
    {
        //SerializedObject so = new SerializedObject(renderer);
        SerializedObject so = new SerializedObject(terrain);
        var sp = so.FindProperty("m_LightmapParameters");
        var ss = so.FindProperty("m_ScaleInLightmap");

        sp.objectReferenceValue = param;
        ss.floatValue = 1;
        so.ApplyModifiedProperties();
        
    }









}



