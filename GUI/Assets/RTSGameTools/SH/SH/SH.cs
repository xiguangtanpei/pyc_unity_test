
using System.Collections.Generic;
using UnityEngine;



public  class SH
{
    public Texture2D EnvHDR;
    Vector3[] ceoff;
    public float Exp = 0.0f;
    public SH()
    {
        EnvHDR = null;
        ceoff = new Vector3[9];
       
    }

    // Use this for initialization
    float[] GetBasis(Vector3 n)
    {
        float[] h = new float[9];
        h[0] = Mathf.Sqrt(1 /(4 * Mathf.PI));
        h[1] = Mathf.Sqrt(3 / (4 * Mathf.PI)) * n.y;
        h[2] = Mathf.Sqrt(3 / (4 * Mathf.PI)) * n.z;
        h[3] = Mathf.Sqrt(3 / (4 * Mathf.PI)) * n.x;
        h[4] = 0.5f * Mathf.Sqrt(15 / Mathf.PI) * n.x * n.y;
        h[5] = 0.5f * Mathf.Sqrt(15 / Mathf.PI) * n.y * n.z;
        h[6] = 0.25f * Mathf.Sqrt(5 / Mathf.PI) * (2 * n.z * n.z - n.x * n.x - n.y * n.y);
        h[7] = 0.5f * Mathf.Sqrt(15 / Mathf.PI) * n.x * n.z;
        h[8] = 0.25f * Mathf.Sqrt(15 / Mathf.PI) * (n.x * n.x - n.y * n.y);
        return h;
    }

    //经纬度转换为三维坐标
    Vector3 UV2XYZ(Vector2 pos_uv)
    {
        Vector3 norPos = new Vector3(-Mathf.Sin(pos_uv.x), Mathf.Cos(pos_uv.x), Mathf.Sin(pos_uv.y));
        return norPos.normalized;
    }

    void SHPreCompute()
    {
        int width = EnvHDR.width;
        int height = EnvHDR.height;

        
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
               Vector4 c = EnvHDR.GetPixel(i,j);
               // c.x = Mathf.Min(1, Mathf.Max(0, c.x * Mathf.Pow(2, Exp)));
               // c.x = Mathf.Min(1, Mathf.Max(0, c.x * Mathf.Pow(2, Exp)));
               // c.x = Mathf.Min(1, Mathf.Max(0, c.x * Mathf.Pow(2, Exp)));
                 //Debug.Log(c);

                Vector2 uv = new Vector2((float)(i+0.5f) / (width+1), (float)(j+0.5f) / (height+1));
                uv.y = uv.y * 2 - 1;
                uv.x *=  2*Mathf.PI;
                uv.y *=  (Mathf.PI / 2);
                Vector3 normal = UV2XYZ(uv);
                float temp = normal.y;
                normal.y = normal.z;
                normal.z = temp;
                float[] basis_temp = GetBasis(normal);   //////xyz!!!!!!!!!!!!!!!!!!!!!

                for (int k = 0; k < 9; k++)
                { 
                    ceoff[k] += new Vector3(c.x,c.y,c.z) * basis_temp[k];
                }             
            }
        }



        for (int k = 0; k < 9; k++)
        {
            ceoff[k] *= 4 * Mathf.PI / ((width+1) * (height+1));
        }    
    }

    public float[] PrintCeoff()
    {
        ceoff = new Vector3[9];

        if (EnvHDR)
        {

            SHPreCompute();

            List<float> newceoff = new List<float>();
            for (int i = 0; i < 9; i++)
            {
                Vector3 c = new Vector3(ceoff[i].x, ceoff[i].y, ceoff[i].z);

                newceoff.Add(c.x);
                newceoff.Add(c.y);
                newceoff.Add(c.z);
            }
            
            string ss = "";
            for (int j = 0; j < newceoff.Count; j++)
            {
                ss += newceoff[j] + "f , ";
            }
            Debug.Log(ss);
            return newceoff.ToArray();
        }
        return null;
      
    }



    /*
    int factorial(int n)
    {
        int r = 1;
        for (int i = 0; i < n; i++)
        {
            r *= (i + 1);
        }
        return r;
    }


    float K(int l, int m)
    {
        // renormalisation constant for SH function
        float temp = ((2.0f * l + 1.0f) * factorial(l - m)) / (4.0f * PI * factorial(l + m));
        return Mathf.Sqrt(temp);
    }

    float P(int l, int m, float x)
    {
        // evaluate an Associated Legendre Polynomial P(l,m,x) at x
        float pmm = 1.0f;
        if (m > 0)
        {
            float somx2 =Mathf.Sqrt((1.0f - x) * (1.0f + x));
 
            float fact = 1.0f;
            for (int i = 1; i <= m; i++)
            {
                pmm *= (-fact) * somx2;
                fact += 2.0f;
            }
        }
        if (l == m) return pmm;
        float pmmp1 = x * (2.0f * m + 1.0f) * pmm;
        if (l == m + 1) return pmmp1;
        float pll = 0.0f;
        for (int ll = m + 2; ll <= l; ++ll)
        {
            pll = ((2.0f * ll - 1.0f) * x * pmmp1 - (ll + m - 1.0f) * pmm) / (ll - m);
            pmm = pmmp1;
            pmmp1 = pll;
        }
        return pll;
    }

    float SH(int l, int m, float theta, float phi)
    {
        // return a point sample of a Spherical Harmonic basis function
        // l is the band, range [0..N]
        // m in the range [-l..l]
        // theta in the range [0..Pi]
        // phi in the range [0..2*Pi]
        float sqrt2 = Mathf.Sqrt(2.0f);
        if (m == 0) return K(l, 0) * P(l, m,Mathf.Cos(theta));
        else if (m > 0) return sqrt2 * K(l, m) * Mathf.Cos(m * phi) * P(l, m, Mathf.Cos(theta));
        else return sqrt2 * K(l, -m) * Mathf.Sin(-m * phi) * P(l, -m, Mathf.Cos(theta));
    }




    struct SHSample
    {
        public Vector3 sph;
        public Vector3 vec;
        public float[] coeff;
    };

    void SH_setup_spherical_samples(SHSample[] samples, int sqrt_n_samples,int n_bands)
    {
        // fill an N*N*2 array with uniformly distributed
        // samples across the sphere using jittered stratification
        int i = 0; // array index
        float oneoverN = 1.0f / sqrt_n_samples;
        for (int a = 0; a < sqrt_n_samples; a++)
        {
            for (int b = 0; b < sqrt_n_samples; b++)
            {
                // generate unbiased distribution of spherical coords
                float x = (a + Random.value) * oneoverN; // do not reuse results
                float y = (b + Random.value) * oneoverN; // each sample must be random
                float theta = 2.0f * Mathf.Acos(Mathf.Sqrt(1.0f - x));
                float phi = 2.0f * PI * y;
                samples[i].sph =new Vector3 (theta, phi, 1.0f);
                // convert spherical coords to unit vector
                Vector3 vec=new Vector3( Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(theta));
                samples[i].vec = vec;
                // precompute all SH coefficients for this sample
                for (int l = 0; l < n_bands; ++l)
                {
                    for (int m = -l; m <= l; ++m)
                    {
                        int index = l * (l + 1) + m;
                        samples[i].coeff[index] = SH(l, m, theta, phi);
                    }
                }
                ++i;
            }
        }
    }
    */
    /*

    public static short[][] GetPixs(Bitmap bitmap)
    {
        int height = bitmap.Height;
        int width = bitmap.Width;
        byte tempB, tempG, tempR;
        short[][] spOriginData = new short[height][];
        for (int i = 0; i < height; i++)
        {
            spOriginData[i] = new short[width];
        }
        BitmapData dataOut = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        int offset = dataOut.Stride - dataOut.Width * 3;
        try
        {
            unsafe
            {
                byte* pOut = (byte*)(dataOut.Scan0.ToPointer());
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        tempB = pOut[0];
                        tempG = pOut[1];
                        tempR = pOut[2];
                        double data = 0.31 * tempR + 0.59 * tempG + 0.11 * tempB;
                        if (data > 255)
                            spOriginData[y][x] = 255;
                        else
                        if (data < 0)
                            spOriginData[y][x] = 0;
                        else
                            spOriginData[y][x] = (short)data;
                        pOut += 3;
                    }
                    pOut += offset;
                }
                bitmap.UnlockBits(dataOut);
            }
        }
        catch
        {
        }
        return spOriginData;
    }

    */

}
