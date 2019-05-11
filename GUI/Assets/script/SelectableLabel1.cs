using UnityEditor;
using UnityEngine;

public class SelectableLabel1 : EditorWindow
{
    static Rect pos;

    [MenuItem("ss/SelectableLabel1")]
    static void run()
    {
        SelectableLabel1 window =EditorWindow.GetWindowWithRect<SelectableLabel1>(new Rect(0, 0, 250, 100));
        window.Show();
    }

    void OnGUI()
    {

         // 可选择标签就是可以处理
//          string bi = "迭代路径git返回丢失问题 迭代动画导出导出位置位置\n
//  157 同步中加入了max 同步方式，算是终结方式，以后文件批量修改都可以利用该功能完成\n
//  158  pbrshader工具迭代支持拷贝材质解决优化问题\n
//  159 小兵的渲染支持 dx材质 处理渲染\n
//  160  同步方式换成脚本模式，对于大文件复杂处理更快  \n
//  161 建立bat目录主要是为了批处理 调用一些函数\n
//  162 工具开发之后界面的会变少，过程中功能处理，都弄成bat 模式 利用max同步进行处理 \n
//  163 导出动画前自动设置动画范围关键帧， 关键帧动画 自动化， 不可以手工设置---要确保动画 完整 \n
//  164 迭代模型导出 链接git bug  --- 目录每创建会有问题\n
//  165 通过tietu 打开资源设置功能迭代 ，另外多了一个maketes_index 用来查找有用路径\n
//  166 修改bat 模式位明码模式\n
//  167 修改打开文件bug \n
//  168 修改导出资源同步到unity 目录中， 同时工具操作进行优化，在模型 动画导出 做到ui干净简单\n
//  169 迭代动画输出不总是fbx bug\n
//  170 迭代导出包围盒大小 人马两个模型问题 之前 00_bb  修改成 chief_bb  同时修改真多lod距离bug 算法迭代"
        EditorGUILayout.SelectableLabel("cssdf") ;



    }
}