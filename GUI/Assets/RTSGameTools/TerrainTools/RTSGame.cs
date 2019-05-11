using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;


namespace RTSGame
{
    public class ExportMeshToObj
    {
        //模型结构体
        struct Model
        {
            public Mesh mesh;
            public Material[] materials;
            public Transform transform;
        }
        //材质结构体
        struct ModelMaterial
        {
            public string name;
            public float cutoff;
            public string texture;
        }

        static float unitsScale=1;
        static bool exportMaterial=false;

        //导出的细节
        /// <summary>
        /// 检查是否有重复材质
        /// </summary>
        class MaterialCompare : EqualityComparer<ModelMaterial>
        {
            public override bool Equals(ModelMaterial mtlx, ModelMaterial mtly)
            {
                return string.Compare(mtlx.name, mtly.name) == 0;
            }

            public override int GetHashCode(ModelMaterial mtl)
            {
                return mtl.name.GetHashCode();
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        public static void Export()
        {
            Transform[] objectList =
                Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
            if (objectList.Length <= 0)
            {
                EditorUtility.DisplayDialog("注意", "没选要导出的物体", "好。。。");
                return;
            }

            List<Model> modelList = new List<Model>();

            for (int i = 0; i < objectList.Length; ++i)
            {
                MeshFilter[] meshList =
                    objectList[i].GetComponentsInChildren<MeshFilter>();

                for (int j = 0; j < meshList.Length; ++j)
                {
                    Model model = new Model();
                    MeshFilter _meshFilter = meshList[j];

                    model.mesh = _meshFilter.sharedMesh;
                    Renderer meshrenderer = _meshFilter.GetComponent<Renderer>();
                    if (meshrenderer)
                        model.materials = meshrenderer.sharedMaterials;
                    model.transform = _meshFilter.transform;

                    modelList.Add(model);
                }

                SkinnedMeshRenderer[] skinList =
                    objectList[i].GetComponentsInChildren<SkinnedMeshRenderer>();

                for (int j = 0; j < skinList.Length; ++j)
                {
                    Model model = new Model();
                    SkinnedMeshRenderer _skinRender = skinList[j];

                    model.mesh = _skinRender.sharedMesh;
                    model.materials = _skinRender.sharedMaterials;
                    model.transform = _skinRender.transform;

                    modelList.Add(model);
                }
            }

            if (modelList.Count <= 0)
            {
                EditorUtility.DisplayDialog("我草", "你选中的没有模型啊", "哈哈，逗你玩呢");
                return;
            }

            string modelPath = EditorUtility.SaveFilePanel("选择导出路径", EditorPrefs.GetString("sceneExportPath"), Path.GetFileNameWithoutExtension(SceneManager.GetActiveScene().name), "obj");
            if (string.IsNullOrEmpty(modelPath))
            {
                EditorUtility.DisplayDialog("出错了", "没导出成功啊", "Oh,Shit");
                return;
            }

            string path = Path.GetDirectoryName(modelPath);
            string name = Path.GetFileNameWithoutExtension(modelPath);
            EditorPrefs.SetString("ExportPath", path);

            string materialPath = string.Format("{0}/{1}.material", path, name);
            if (!Directory.Exists(materialPath))
                Directory.CreateDirectory(materialPath);

            float modelPret =
                exportMaterial ? 0.9f : 1f;
            float materialPret = 1f - modelPret;

            HashSet<ModelMaterial> materialSet =
                new HashSet<ModelMaterial>(new MaterialCompare());

            using (StreamWriter writer = new StreamWriter(modelPath))
            {
                writer.WriteLine(
                    string.Format("mtllib ./{0}.material/material.mtl", name));
                writer.WriteLine();

                int offset = 0;
                for (int i = 0; i < modelList.Count; ++i)
                {
                    ExportMesh(writer, modelList[i], materialSet, ref offset);
                    EditorUtility.DisplayProgressBar(
                        "SceneExport",
                        string.Format(
                            "Export Model : {0}", modelList[i].transform.name),
                        modelPret * (float)i / (float)modelList.Count);
                }

                writer.Close();
            }

            if (exportMaterial)
                using (StreamWriter writer =
                    new StreamWriter(string.Format("{0}/material.mtl", materialPath)))
                {
                    int p = 0;
                    int count = materialSet.Count;

                    foreach (ModelMaterial material in materialSet)
                    {
                        ExportMaterial(writer, material, materialPath);
                        EditorUtility.DisplayProgressBar(
                            "SceneExport",
                            string.Format(
                                "Export Material : {0}", material.name),
                            materialPret * (float)p / (float)count);
                        ++p;
                    }
                    writer.Close();
                }
            EditorUtility.ClearProgressBar();
            EditorUtility.RevealInFinder(modelPath);
        }
        /// <summary>
        /// 导出mesh
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="model"></param>
        /// <param name="materalSet"></param>
        /// <param name="offset"></param>
        static void ExportMesh(TextWriter writer, Model model, HashSet<ModelMaterial> materalSet, ref int offset)
        {
            int vertexCount = model.mesh.vertexCount;
            writer.WriteLine(
                string.Format("g {0}", model.transform.name));

            for (int i = 0; i < vertexCount; ++i)
            {
                Vector3 v =
                    model.transform.TransformPoint(model.mesh.vertices[i]);
                v *= unitsScale;
                writer.WriteLine(
                    string.Format("v {0} {1} {2}", -v.x, v.y, v.z));
            }
            writer.WriteLine();

            for (int i = 0; i < vertexCount; ++i)
            {
                Vector3 n =
                    model.transform.TransformDirection(model.mesh.normals[i]);
                writer.WriteLine(
                    string.Format("vn {0} {1} {2}", -n.x, n.y, n.z));
            }
            writer.WriteLine();

            for (int i = 0; i < vertexCount; ++i)
            {
                Vector2 t = model.mesh.uv[i];
                writer.WriteLine(
                    string.Format("vt {0} {1}", t.x, t.y));
            }
            writer.WriteLine();

            for (int i = 0; i < model.mesh.subMeshCount; ++i)
            {
                Material mtl = model.materials[i];
                int[] triangles = model.mesh.GetTriangles(i);

                if (exportMaterial)
                {
                    ModelMaterial modelMtl = new ModelMaterial();
                    modelMtl.name = mtl.name;
                    modelMtl.cutoff = 1f;

                    for (int t = 0; t < 100; ++t)
                    {
                        if (!materalSet.Contains(modelMtl))
                            break;
                        modelMtl.name = string.Format("{0}_{1:d2}", mtl.name, t);
                    }

                    writer.WriteLine(
                            string.Format("usemtl {0}", modelMtl.name));
                    writer.WriteLine(
                        string.Format("usemap {0}", modelMtl.name));

                    if (mtl.mainTexture)
                        modelMtl.texture = AssetDatabase.GetAssetPath(mtl.mainTexture);
                    else
                        modelMtl.texture = null;

                    if (mtl.HasProperty("_Cutoff"))
                        modelMtl.cutoff = mtl.GetFloat("_Cutoff");


                    materalSet.Add(modelMtl);
                }

                for (int j = 0; j < triangles.Length / 3; ++j)
                    writer.WriteLine(
                        string.Format(
                            "f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}",
                            triangles[j * 3 + 0] + 1 + offset,
                            triangles[j * 3 + 1] + 1 + offset,
                            triangles[j * 3 + 2] + 1 + offset));
            }
            writer.WriteLine();

            offset += vertexCount;
        }
        /// <summary>
        /// 导出材质
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="material"></param>
        /// <param name="path"></param>
        static void ExportMaterial(TextWriter writer, ModelMaterial material, string path)
        {
            writer.WriteLine("newmtl {0}", material.name);
            writer.WriteLine("Ka  1.0 1.0 1.0");
            writer.WriteLine("Kd  1.0 1.0 1.0");
            writer.WriteLine("Ks  0.0 0.0 0.0");
            writer.WriteLine("d  1.0");
            writer.WriteLine("illum 1");

            if (!string.IsNullOrEmpty(material.texture))
            {
                string texture =
                    Path.GetFileName(material.texture);
                try
                {
                    File.Copy(material.texture, path + "/" + texture);
                }
                catch (System.Exception)
                {
                }
                writer.WriteLine("map_Kd {0}", texture);
                if (material.cutoff < 1f)
                    writer.WriteLine("map_d {0}", texture);
            }
            writer.WriteLine();
        }
    }



    public class EportTerrainToObj
    {



        public void Export()
        {
            GameObject TerrainGO = new GameObject();
             string  terrainName = TerrainGO.name;
            string FinalExpName= terrainName;

            int tCount;
            int counter;
            int totalCount;
            float progressUpdateInterval = 10000;

            /*建立路径
            if (!System.IO.Directory.Exists(T4MPrefabFolder + "Terrains/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder + "Terrains/");
            }
            if (!System.IO.Directory.Exists(T4MPrefabFolder + "Terrains/Material/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder + "Terrains/Material/");
            }
            if (!System.IO.Directory.Exists(T4MPrefabFolder + "Terrains/Texture/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder + "Terrains/Texture/");
            }
            if (!System.IO.Directory.Exists(T4MPrefabFolder + "Terrains/Meshes/"))
            {
                System.IO.Directory.CreateDirectory(T4MPrefabFolder + "Terrains/Meshes/");
            }*/
            AssetDatabase.Refresh();
            TerrainData terrain = TerrainGO.GetComponent<Terrain>().terrainData;



            int w = terrain.heightmapWidth;
            int h = terrain.heightmapHeight;
            float tRes = w / 256;
            Vector3 meshScale = terrain.size;
            meshScale = new Vector3(meshScale.x / (h - 1) * tRes, meshScale.y, meshScale.z / (w - 1) * tRes);
            Vector2 uvScale = new Vector2((float)(1.0 / (w - 1)), (float)(1.0 / (h - 1)));

            float[,] tData = terrain.GetHeights(0, 0, w, h);
            w = (int)((w - 1) / tRes + 1);
            h = (int)((h - 1) / tRes + 1);
            Vector3[] tVertices = new Vector3[w * h];
            Vector2[] tUV = new Vector2[w * h];
            int[] tPolys = new int[(w - 1) * (h - 1) * 6];
            int y = 0;
            int x = 0;
            for (y = 0; y < h; y++)
            {
                for (x = 0; x < w; x++)
                {
                    //tVertices[y*w + x] = Vector3.Scale(meshScale, new Vector3(x, tData[(int)(x*tRes),(int)(y*tRes)], y));
                    tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(-y, tData[(int)(x * tRes), (int)(y * tRes)], x)); //Thank Cid Newman
                    tUV[y * w + x] = Vector2.Scale(new Vector2(y * tRes, x * tRes), uvScale);
                }
            }

            y = 0;
            x = 0;
            int index = 0;
            for (y = 0; y < h - 1; y++)
            {
                for (x = 0; x < w - 1; x++)
                {
                    tPolys[index++] = (y * w) + x;
                    tPolys[index++] = ((y + 1) * w) + x;
                    tPolys[index++] = (y * w) + x + 1;

                    tPolys[index++] = ((y + 1) * w) + x;
                    tPolys[index++] = ((y + 1) * w) + x + 1;
                    tPolys[index++] = (y * w) + x + 1;
                }
            }

            bool ExportNameSuccess = false;
            int num = 1;
            string Next;
            do
            {
                Next = terrainName + num;

                if (!System.IO.File.Exists("Assets/RTSGameTools/TerrainData" + terrainName + ".prefab"))
                {
                    FinalExpName = terrainName;
                    ExportNameSuccess = true;
                }
                else if (!System.IO.File.Exists("Assets/RTSGameTools/TerrainData" + Next + ".prefab"))
                {
                    FinalExpName = Next;
                    ExportNameSuccess = true;
                }
                num++;
            } while (!ExportNameSuccess);

            //StreamWriter  sw = new StreamWriter(T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj");
            StreamWriter sw = new StreamWriter(FinalExpName + ".obj");
            try
            {

                sw.WriteLine("# T4M File");
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                counter = tCount = 0;
                totalCount = (int)((tVertices.Length * 2 + (tPolys.Length / 3)) / progressUpdateInterval);
                for (int i = 0; i < tVertices.Length; i++)
                {
                   // UpdateProgress();
                    StringBuilder sb = new StringBuilder("v ", 20);
                    sb.Append(tVertices[i].x.ToString()).Append(" ").
                       Append(tVertices[i].y.ToString()).Append(" ").
                       Append(tVertices[i].z.ToString());
                    sw.WriteLine(sb);
                }

                for (int i = 0; i < tUV.Length; i++)
                {
                   // UpdateProgress();
                    StringBuilder sb = new StringBuilder("vt ", 22);
                    sb.Append(tUV[i].x.ToString()).Append(" ").
                       Append(tUV[i].y.ToString());
                    sw.WriteLine(sb);
                }
                for (int i = 0; i < tPolys.Length; i += 3)
                {
                   // UpdateProgress();
                    StringBuilder sb = new StringBuilder("f ", 43);
                    sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").
                       Append(tPolys[i + 1] + 1).Append("/").Append(tPolys[i + 1] + 1).Append(" ").
                       Append(tPolys[i + 2] + 1).Append("/").Append(tPolys[i + 2] + 1);
                    sw.WriteLine(sb);
                }
            }
            catch (Exception err)
            {
                Debug.Log("Error saving file: " + err.Message);
            }
            sw.Close();
            AssetDatabase.SaveAssets();

            

            //UpdateProgress();

            /*  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Deplacement de l'obj dans les repertoire mesh
            FileUtil.CopyFileOrDirectory(FinalExpName + ".obj", T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj");
            FileUtil.DeleteFileOrDirectory(FinalExpName + ".obj");



            //Force Update
            AssetDatabase.ImportAsset(T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj", ImportAssetOptions.ForceUpdate);

            UpdateProgress();

            //Instance du T4M
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj", typeof(GameObject));

            AssetDatabase.Refresh();


            GameObject forRotate = (GameObject)Instantiate(prefab, TerrainGO.transform.position, Quaternion.identity) as GameObject;
            Transform childCheck = forRotate.transform.Find("default");
            Child = childCheck.gameObject;
            forRotate.transform.DetachChildren();
            DestroyImmediate(forRotate);
            Child.name = FinalExpName;
            Child.AddComponent<T4MObjSC>();
            //Child.transform.rotation= Quaternion.Euler(0, 90, 0);

            UpdateProgress();

            //Application des Parametres sur le Script
            Child.GetComponent<T4MObjSC>().T4MMaterial = Tmaterial;
            Child.GetComponent<T4MObjSC>().ConvertType = "UT";

            //Regalges Divers
            vertexInfo = 0;
            partofT4MObj = 0;
            trisInfo = 0;
            int countchild = Child.transform.childCount;
            if (countchild > 0)
            {
                Renderer[] T4MOBJPART = Child.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < T4MOBJPART.Length; i++)
                {
                    if (!T4MOBJPART[i].gameObject.AddComponent<MeshCollider>())
                        T4MOBJPART[i].gameObject.AddComponent<MeshCollider>();
                    T4MOBJPART[i].gameObject.isStatic = true;
                    T4MOBJPART[i].material = Tmaterial;
                    T4MOBJPART[i].gameObject.layer = 30;
                    T4MOBJPART[i].gameObject.AddComponent<T4MPartSC>();
                    Child.GetComponent<T4MObjSC>().T4MMesh = T4MOBJPART[0].GetComponent<MeshFilter>();
                    partofT4MObj += 1;
                    vertexInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;
                    trisInfo += T4MOBJPART[i].gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
                }
            }
            else
            {
                Child.AddComponent<MeshCollider>();
                Child.isStatic = true;
                Child.GetComponent<Renderer>().material = Tmaterial;
                Child.layer = 30;
                vertexInfo += Child.GetComponent<MeshFilter>().sharedMesh.vertexCount;
                trisInfo += Child.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
                partofT4MObj += 1;
            }

            UpdateProgress();


            GameObject BasePrefab2 = PrefabUtility.CreatePrefab(T4MPrefabFolder + "Terrains/" + FinalExpName + ".prefab", Child);
            AssetDatabase.ImportAsset(T4MPrefabFolder + "Terrains/" + FinalExpName + ".prefab", ImportAssetOptions.ForceUpdate);
            GameObject forRotate2 = (GameObject)PrefabUtility.InstantiatePrefab(BasePrefab2) as GameObject;

            DestroyImmediate(Child.gameObject);

            Child = forRotate2.gameObject;

            TerrainGO.GetComponent<Terrain>().enabled = false;

            EditorUtility.SetSelectedWireframeHidden(Child.GetComponent<Renderer>(), true);

            UnityTerrain = TerrainGO.gameObject;

            EditorUtility.ClearProgressBar();

            AssetDatabase.DeleteAsset(T4MPrefabFolder + "Terrains/Meshes/Materials");
            terrainName = "";
            AssetDatabase.StartAssetEditing();
            //Modification des attribut du mesh avant de le pr茅fabriquer
            ModelImporter OBJI = ModelImporter.GetAtPath(T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj") as ModelImporter;
            OBJI.globalScale = 1;
            OBJI.splitTangentsAcrossSeams = true;
            OBJI.normalImportMode = ModelImporterTangentSpaceMode.Calculate;
            OBJI.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
            OBJI.generateAnimations = ModelImporterGenerateAnimations.None;
            OBJI.meshCompression = ModelImporterMeshCompression.Off;
            OBJI.normalSmoothingAngle = 180f;
            //AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
            AssetDatabase.ImportAsset(T4MPrefabFolder + "Terrains/Meshes/" + FinalExpName + ".obj", ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.StopAssetEditing();
            PrefabUtility.ResetToPrefabState(Child);


    *//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }






    }
}


    