
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;

namespace XMLdata
{
    public class XMLComFunction
    {
        public string GetXMLPath(Terrain terrain)
        {
            string path = Application.dataPath + "/" + terrain.name + ".xml";
            return path;
        }
    }
  
    public class ExportTreeXML
    {
       

        public void CreateXML(Terrain terrain)
        {
            XMLComFunction cf = new XMLComFunction();
            string path = cf.GetXMLPath(terrain);
            TerrainData terrainData = terrain.terrainData;
            TreePrototype[] treePrototypes = terrainData.treePrototypes;
            int treeInstCount = terrainData.treeInstanceCount;

            if (!File.Exists(path))
            {
                // 创建xml文档实例
                XmlDocument xmlDoc = new XmlDocument();
                // 创建根节点
                XmlElement root = xmlDoc.CreateElement("trees");
                root.SetAttribute("name", path);


                for (int i = 0; i < treeInstCount; ++i)
                {
                    TreeInstance treeInst = terrainData.GetTreeInstance(i);
                    int protoIndex = treeInst.prototypeIndex;

                    TreePrototype treePt = treePrototypes[protoIndex];
                    GameObject treeSpm = treePt.prefab;
                    //Prefab type
                    string treepath = AssetDatabase.GetAssetPath(treeSpm);

                    //Prefab transform
                    float scale = treeSpm.transform.localScale.x;
                    scale *= treeInst.heightScale;
                    Vector3 pos = Vector3.Scale(treeInst.position, terrainData.size);
                    Vector3 rot = (Quaternion.AngleAxis(treeInst.rotation * Mathf.Rad2Deg, Vector3.up)).eulerAngles;
                    Vector3 sca = new Vector3(scale, scale, scale);

                    // 创建transform子节点
                    XmlElement transNode = xmlDoc.CreateElement("tree");
                    transNode.SetAttribute("path", treepath);

                    XmlElement posElement = CreateTransformElement("position", pos, xmlDoc);
                    XmlElement rotElement = CreateTransformElement("rotation", rot, xmlDoc);
                    XmlElement scaElement = CreateTransformElement("localscale", sca, xmlDoc);
                    transNode.AppendChild(posElement);
                    transNode.AppendChild(rotElement);
                    transNode.AppendChild(scaElement);

                    root.AppendChild(transNode);
                }
                xmlDoc.AppendChild(root);
                xmlDoc.Save(path);
            }

        }

        public XmlElement CreateTransformElement(string name, Vector3 value, XmlDocument xmlDoc)
        {
            XmlElement element = xmlDoc.CreateElement(name);
            XmlElement x = xmlDoc.CreateElement("x");
            x.InnerText = value.x.ToString();
            XmlElement y = xmlDoc.CreateElement("y");
            y.InnerText = value.y.ToString();
            XmlElement z = xmlDoc.CreateElement("z");
            z.InnerText = value.z.ToString();
            element.AppendChild(x);
            element.AppendChild(y);
            element.AppendChild(z);
            return element;
        }

    }

    public class RestoreTreeXML
    {
        public void ReadXML(Terrain terrain)
        {
            XMLComFunction cf = new XMLComFunction();
            string path = cf.GetXMLPath(terrain);
            // 判断文件不存在返回
            while(!File.Exists(path))
            {
                bool checkfile = EditorUtility.DisplayDialog("没有找到对应XML数据",
                "是否需要自己查找XML数据文件按", "查找", "放弃");
                if (checkfile)
                {
                    path = EditorUtility.OpenFilePanel("查找XML文件", "", "xml");
                }
                else
                    return;
            }

            // 新建 XML 实例
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            // 获取一级节点  user
            XmlNode root = xmlDoc.SelectSingleNode("Root");

            // 获取一级节点下的所有二级节点
            XmlNodeList tranList = root.ChildNodes;

            // 遍历所有的二级节点
            for (int i = 0; i < tranList.Count; ++i)
            {
                XmlNode _node = tranList[i];
                TreeInstance treeIn = new TreeInstance();

                treeIn.position = GetTranValue("position", _node);
                //treeIn.rotation = Quaternion.Euler( GetTranValue("rotation", _node));

                if (_node.Name == "rotation")
                {

                }
                if (_node.Name == "localscale")
                {

                }
            }            
        }

        Vector3 GetTranValue(string valueName, XmlNode node)
        {
            Vector3 tranV = new Vector3();
            if (node.Name == valueName)
            {
                XmlNodeList valueList = node.ChildNodes;
                float x=0.0f, y=0.0f, z = 0.0f;
                for (int i = 0; i < valueList.Count; i++)
                {
                    XmlNode value = valueList[i];                   
                    if (value.Name == "x")
                    {
                        x = float.Parse(value.InnerText);
                    }
                    if (value.Name == "y")
                    {
                        y = float.Parse(value.InnerText);
                    }
                    if (value.Name == "z")
                    {
                        z = float.Parse(value.InnerText);
                    }
                }
                tranV = new Vector3(x, y, z);           
            }
            return tranV;
        }

    }

}
