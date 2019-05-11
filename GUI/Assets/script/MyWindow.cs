using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor ; 



/// 总胡来说很多问题使用国很多游戏胡GUi 问题比较大 



public class MyWindow : EditorWindow {

	//定义变量
	string  myString =  "Hello world";
	bool groupEnabled ;
	bool  myBool = true ;
	/// 这里要加f 
	public float myFloat = 1.232f ;
	public Texture tex  ; 
	public Texture texa ; 



	// 加入到工具菜单中要处理的东西 
/// 下面接显示wiondow窗口 
	[MenuItem ("ss/my woindow")]
	static void Init ()
	{
		MyWindow window = (MyWindow) GetWindow(typeof(MyWindow)) ;
		window.Show();

		//  GetWindow(typeof(MyWindow)); /// 好像不加show 也可以 
	}
	/// 创建窗口成功过 


	/// 成功了以后在GUI中创建各种控件 

  public Vector2 sc ; 
	/// 控件适合工具 适合游戏 
	 private void OnGUI() {
		 // 学习一下labe 控件 
		 //https://docs.unity3d.com/2017.4/Documentation/ScriptReference/EditorWindow.html 
		 // 我发现很多种lable 模式
		GUI.skin.label.fontSize = 12; 
		 GUILayout .Label("base setring ") ; 
		 // 按照 操作 学习一下 GUI 各种 操作  
		GUILayout.Label("测试", GUILayout.Width(23)) ;

		// 添加图片  
		 GUILayout.Label(tex, GUILayout.Width (30) ,GUILayout.Height(30)) ;  // 测试这样放不进去 
		 // 实际测试支持， 需要选择脚本 在脚本地方加入 

		 

		/// 字体大小设置成紧跟模式  
		/// 根据类型采用拦截的方式实现 并处理 
		GUI.skin.label.fontSize = 33; 
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label("test "   );

		GUI.skin.label.fontSize = 12;

		// 根据GUi 的内部操作 大概有20多 依次在这里说明  
		// begin area
		/// 区域测试有效， 基本都是一个矩形面积， 加纹理， 或者加入
		GUILayout.BeginArea(new Rect (200,200,40,40) ,texa   ) ; 
		GUILayout.Button("测试位置") ;
		GUILayout.EndArea() ;
		


		// 创建水平控件的布阵方式 

		// 测试优先级去很高 关于 控件布置一定要早 
		GUILayout.BeginHorizontal("box") ; 
		GUILayout.Button ("一定要有我");
			GUILayout.Button ("一定要有我");
				GUILayout.Button ("一定要有我");
				GUILayout.Button ("一定要有我");
				GUILayout.Button ("一定要有我");
					
	    GUILayout.EndHorizontal();

		

	    
		sc = GUILayout.BeginScrollView(sc ,GUILayout.Width(100),GUILayout.Height(100)) ; 

	GUILayout.Button ("一定要有我");
			GUILayout.Button ("一定要有我");
				GUILayout.Button ("一定要有我");
				GUILayout.Space(20); /// 这种基本shader中也会用到的 
	GUILayout.Button ("一定要有我");
			GUILayout.Button ("一定要有我");
				GUILayout.Button ("一定要有我");

		// 实验成功， 身下的就是 是否开进度条 ，设置进度条风格之类的 
		GUILayout.EndScrollView () ;


    /// 开始做一个竖直方向处理 
	GUILayout.BeginVertical("我是竖直方向") ;

	GUILayout.Button ("一定要有我");
			GUILayout.Button ("一定要有我");
				GUILayout.FlexibleSpace() ; /// 控件左不好  /// 原来这是拉伸使用的 
				GUILayout.Button ("一定要有我");
	GUILayout.Button ("一定要有我");
			GUILayout.Button ("一定要有我");

	GUILayout.EndVertical ();

	/// 针对说 面积 横向 纵向，只要有 end 一切都搞定 
      

 /// 简单在 Gui box 控件  主要创建纹理和 图片 

	  GUILayout.Box("我是box" ) ; 

      GUILayout.Box(tex ,GUILayout.Width (50) , GUILayout.Height(50) ) ; 
     
	 // button 使用很多了 这里设置一个 
	 /// 主要处理图拍你肯纹理 
	   var bb = GUILayout.Button( tex , GUILayout.Width(40) ,GUILayout.Height(40) ) ; 
	    if (bb) {
			Debug.Log("我才是事件重点") ;
		}

		/// 扩展高度 和宽度 现在先不处理， 
		///https://docs.unity3d.com/2017.4/Documentation/ScriptReference/GUILayout.FlexibleSpace.html 
		// 综合使用 横向肯纵向排版 
		/// 空间处理椒放到 布局器里面的 

		/// 横向滚动数值 
	// GUILayout.HorizontalScrollbar(myFloat ,0.03f,-4.0f ,4.0f) ;
	/// 滚动不太会使用 

	/// 滑块数值更多是我想要的 
	 var hk = GUILayout.HorizontalSlider(myFloat,0.0f,1.0f);
	 Debug.Log(hk); 
	 // 先放一下 
	 /// 密码写入 

	   var mi = GUILayout.PasswordField("","*"[0],24,GUILayout.Width(200)) ; 
		 Debug.Log(mi);


	   var bb1 = GUILayout.Button( "我是依次按钮" ) ; 
	    if (bb1) {
			Debug.Log("我才是事件重点") ;
		}

		/// 这里有些问题 
		    var bb2 = GUILayout.RepeatButton("我是重复按钮");
	    if (bb2) {
			Debug.Log("我是重复按钮") ;
		}

		// 如果不是展示 只是工具显示，多半不使用  seelection grid ， 如果显示一些缩略图还是很有用 
		 var  imtx = new Texture [] {tex ,texa} ;
		//  var st = new string[]{} ;
		/// 查找所错误显示出来还是很有用的 
		GUILayout.SelectionGrid(0 ,imtx ,2, GUILayout.Width(300),GUILayout.Height(30))  ;

		/// 写入一坨文字 
		var ss = GUILayout.TextArea("些文字") ;
		Debug.Log(ss);

     EditorGUILayout.TextArea("");

		 GUILayout.TextField("ceshi---测试确实在游戏视角下进行胡") ;
		 EditorGUILayout.TextField("我不是在游戏视角下进行胡");

		 GUILayout.Toggle(true,"ceshi");
		 EditorGUILayout.Toggle("看来椒真的",true);







		
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	    void OnDestroy()
    {

    }
}
