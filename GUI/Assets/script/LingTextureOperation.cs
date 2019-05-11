using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LingTextureOperation : EditorWindow
{


    //  string ones = "";

    //public object one; 


    #region Greate Window
    //添加标题
    LingTextureOperation()
    {
        this.titleContent = new GUIContent("name- Operation");
    }
    //添加到工具栏
    [MenuItem("RTSGameTools/TextureOperation ")]
    //创建窗口
    static void showWindow()
    {
        GetWindow(typeof(LingTextureOperation));
    }


  
    void OnDestroy()
    {

    }
    #endregion

    string path = null;
    

    private void OnGUI()
    {
       // GUI.color = new Color(255, 0, 0);
        GUILayout.BeginVertical();
        GUILayout.Space(10);
            
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        

        GUILayout.Label("name Opertation");
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Timi-J2-TA");
        GUILayout.Space(10);

        GUILayout.Label("工程中先选中要改的文件");
        if (GUILayout.Button("改名(大写该小写-名称分两段,空格利用-代替)"))
        {
            Opration();
        }

        GUILayout.Space(10);
    }


    string WinPathToUnityPath(string path)
    {
        path = path.Substring(path.IndexOf("Assets"));
        path = path.Replace('\\', '/');
        return path;
    }
    string UnityPathToWinPath(string path)
    {
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        path = files[0].FullName.ToString();
        return path;
    }

    void Opration()
    {
        Object[] m_objects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);//选择的所以对象



        foreach (Object item in m_objects)
        {
            // if (item.GetType() != typeof(Texture2D))
          //  Debug.Log(item.GetType());

            {
           // 这里的操作不在限制类型
                string m_name = item.name;

               // string aa = Path.GetExtension(AssetDatabase.GetAssetPath(item) );
                //Debug.Log( aa );

                if (Path.GetExtension(AssetDatabase.GetAssetPath(item)) != ".cs")//判断路径是否为空
                {
                    // Debug.Log("行不行");
                    string path = AssetDatabase.GetAssetPath(item);

                   
                    string aname = ReplaceLetter(item.name);
                   // Debug.Log(aname);
                    aname = GetSmallLetter(aname);


                    // Debug.Log(path);
                    //Debug.Log(aname);
                    AssetDatabase.RenameAsset(path, aname);

                }
            }

        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


    }

    string GetSmallLetter(string s)
    {
        return s.ToLower();
    }

    string ReplaceLetter(string s)
    {
        /// 替换逻辑放到这里 
        /// 
        string  ones = "";
        char[] newchar = new char[] { '_',' '};
         string[]  newstrings =   s.Split(newchar);
        // Debug.Log(newstrings.Length);
        if (newstrings.Length > 1) {
            

            for (int i =0; i< newstrings.Length; i++ ) {
                /// 这里实现好 
                /// 
                if (i == newstrings.Length - 2) {
                    ones += newstrings[i]+"_";

                }
                else if (i == newstrings.Length - 1)  {
                    ones += newstrings[i] ;
                }
                else {
                    ones += newstrings[i] + "-";
                }



            }
        }
        else {
            ones = s; 
        }
        //Debug.Log(ones);
        return ones ;
    }



}
