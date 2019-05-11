using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHvalueSetter : MonoBehaviour {
    public Texture2D EnvHDR;
    float[] SHf = { 0.2821f, 0.4886f, 0.4886f, 0.4886f, 1.0925f, 1.0925f, 0.3979f, 1.0925f, 0.54625f };
    float[] SHc = new float[28];
    Vector4[] SH = new Vector4[7];
    string[] SHname = { "SHxr", "SHxg", "SHxb", "SHyr", "SHyg", "SHyb", "SHz" };
    public bool update = true;
    Material Sky;
    // Use this for initialization
    void Start ()
    {

        Sky = RenderSettings.skybox;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (update)
        {
            Set();
        }	
	}


    void Set()
    {
        SH shdata = new SH();
        shdata.EnvHDR = EnvHDR;
       // Sky.SetTexture("_CubeMap",EnvHDR);
        SHc = shdata.PrintCeoff();

        for (int i = 0; i < 6; i++)
        {
            int r = i % 3;
            int m = (int)(i / 3);

            SH[i] = new Vector4(SHc[12 * m + r] * SHf[4 * m], SHc[12 * m + r + 3] * SHf[4 * m + 1], SHc[12 * m + r + 6] * SHf[4 * m + 2], SHc[12 * m + r + 9] * SHf[4 * m + 3]);
        }
        SH[6] = new Vector4(SHc[24] * SHf[8], SHc[25] * SHf[8], SHc[26] * SHf[8], 0);
        string ss = "";
        for (int i = 0; i < SH.Length; i++)
        {
            ss += "float4 "+ SHname[i] + " = float4 (" +SH[i].x + "," + SH[i].y + "," + SH[i].z + "," + SH[i].w + ");    ";
        }
        print(ss);


        for (int i = 0; i < 7; i++)
        {
            Shader.SetGlobalVector(SHname[i], SH[i]);
        }
        update = false;
    

    }
}
