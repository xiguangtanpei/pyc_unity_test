using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMatrix : MonoBehaviour {
    // public Material TargetM;
    // public Camera camera;
    // public Transform target;
    //public Transform Lit;
    // public Color LightColor=Color.white;
    // public float LightIntensity=1.0f;
    public Texture2D EnvHDR;

    public Vector4[] SH=new Vector4[7];
    float[] SHf = { 0.2821f, 0.4886f, 0.4886f, 0.4886f, 1.0925f, 1.0925f, 0.3979f, 1.0925f, 0.54625f };

 /*//sky02
    public float[] SHc =
    {
        3.235152f , 2.983731f , 2.91614f ,
        0.160743f , -0.04557052f , -0.2774327f ,
        -0.09505244f , 0.076104f , 0.2465347f ,
        -0.1345672f , -0.03358774f , 0.07418646f ,
        0.05430403f , 0.0498231f , 0.033431f ,
        0.1391596f , 0.1303495f , 0.09764007f ,
        -0.4416841f , -0.3782544f , -0.3379838f ,
        -0.1262822f , -0.03304388f , 0.06366697f ,
        0.03001502f , 0.009320604f , -0.05686459f ,
    };*/

        //sky03
    public float[] SHc =
{
        2.814351f , 2.972716f , 3.32999f , -0.5623536f , -0.3225635f , 0.121599f , 0.01476434f , 0.008604963f , 0.000881402f , -0.06434114f , -0.05789223f , -0.01831096f , -0.0377995f , -0.02074883f , 0.01216985f , -0.01548949f , -0.01356842f , -0.007325687f , 0.2323035f , 0.2323058f , 0.2252145f , 0.03705175f , 0.02642546f , 0.007798085f , 0.4167466f , 0.4137461f , 0.3920018f ,
    };

 
    string[] SHname = { "SHxr", "SHxg", "SHxb", "SHyr", "SHyg", "SHyb", "SHz" };

    void Start ()
    {
        SH shdata = new SH();
        shdata.EnvHDR = EnvHDR;
        SHc=shdata.PrintCeoff();

        for (int i = 0; i < 6; i++)
        {
            int r = i % 3;
            int m =(int)( i / 3);

            SH[i] = new Vector4(  SHc[12*m+r]*SHf[4*m],   SHc[12*m+r+3]*SHf[4*m+1],   SHc[12*m+r+6]*SHf[4*m+2],   SHc[12*m+r+9] * SHf[4*m+3]  );
        }
        SH[6] = new Vector4(SHc[24] * SHf[8], SHc[25] * SHf[8], SHc[26] * SHf[8], 0);
        string ss="";
        for (int i = 0; i < SH.Length; i++)
        {
            ss += "," + SH[i].x+","+SH[i].y +","+ SH[i].z+","+ SH[i].w;
        }
        print(ss);
    }
	
	void Update ()
    {
       // Matrix4x4 WMatrix = target.localToWorldMatrix;
       // TargetM.SetMatrix("W", WMatrix);
       // Matrix4x4 OMatrix = target.worldToLocalMatrix;
       // TargetM.SetMatrix("O", OMatrix);
       // Matrix4x4 VMatrix = camera.worldToCameraMatrix;
       // TargetM.SetMatrix("V", WMatrix);
       // Matrix4x4 ViMatrix = camera.cameraToWorldMatrix;
       // TargetM.SetMatrix("Vi", ViMatrix);
      //  Matrix4x4 PMatrix = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true);//必须经过处理，否则与shader中不同
       // Matrix4x4 VPMatrix =  PMatrix* VMatrix ;
      //  TargetM.SetMatrix("VP", VPMatrix);
      //  Shader.SetGlobalVector("_LitDir", -Lit.forward);
        // TargetM.SetVector("_LitDirx", -Lit.right);
        // TargetM.SetVector("_LitDiry", -Lit.up);
      //  Shader.SetGlobalColor("_LitColor", LightColor);
     //   Shader.SetGlobalFloat("_LitIntensity", LightIntensity);
        
        for (int i = 0; i < 7; i++)
        {
            Shader.SetGlobalVector(SHname[i], SH[i]);
        }
 

    }
}
