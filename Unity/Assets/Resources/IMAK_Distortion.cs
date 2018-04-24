using UnityEngine;
using System.Text.RegularExpressions;
using System;


public class IMAK_Distortion
{
    public IMAK_Distortion()
    {
        Num_Ver = 0;
        Num_Tri = 0;
        for (int i = 0; i < 4; ++i)
            GPU_Comm [i] = 0f;
    }

    public void Initialize(GameObject obj)
    {
        Distort_Anime = obj;
        Distort_Anime.AddComponent<MeshFilter>();
        Distort_Anime.AddComponent<MeshRenderer>();
    }

    public GameObject Distort_Anime;
   
    // Some Basic Info
    internal int Num_Ver;
    internal int Num_Tri;

    // Draw Animation
    internal MeshRenderer Tar;
    internal float[] GPU_Comm = new float[4];
    public void Breath(float clock, float magnitude, float freq)
    {
        float shift = (magnitude * Mathf.Sin(clock*freq) - 0.8f*magnitude) / 2;
        GPU_Comm[0] = shift;
    }

    public void blink(float clock, int start_time, float freq)
    {
        float overtime = clock % start_time;
        Debug.Log(overtime.ToString());
        if (0.5f<overtime && overtime< 2.5f)
        {
            GPU_Comm[1] = 0.01f/1.5f * (Mathf.Sin(overtime*Mathf.PI)-1f);
        }
        else
        {
            GPU_Comm[1] = 0;
        }

    }

    public void Send_GPU_Command(){
        Tar.material.SetFloat("command1", GPU_Comm[0]);
        Tar.material.SetFloat("command2", GPU_Comm[1]);

        for (int i = 0; i < 4; ++i)
            GPU_Comm [i] = 0f;
    }

    // Set Texture
    public void SetMaterial(string mat_name)
    {
        Material mat = Resources.Load<Material>(mat_name);
        Tar = Distort_Anime.GetComponent<MeshRenderer>();
        Tar.material = mat;

        // Save Efficiency
        Tar.receiveShadows = false;
        Tar.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void SetTexture(string file_name)
    {
        Shader Disort_shader = Resources.Load<Shader>("DistortionShader");
        Material mat = new Material(Disort_shader);
        Tex = Resources.Load<Texture>(file_name);
        mat.SetTexture("_MainTex", Tex);
        Tar = Distort_Anime.GetComponent<MeshRenderer>();
        Tar.material = mat;


        rt = new RenderTexture (Tex.width,Tex.height,0);

        // Save Efficiency
        Tar.receiveShadows = false;
        Tar.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }


    // For loading heatmap & meshgrid from external file
    public bool getMeshFromFile(string FileName, float Z = 1f)
    {
        TextAsset DataText = Resources.Load(FileName) as TextAsset;
        string RawContent = DataText.text;
        Regex ReturnLine = new Regex("\n");
        if (RawContent != null)
        {
            Regex Delimiter = new Regex(",");

            string[] MyContent = ReturnLine.Split(RawContent);
            // Read header
            string[] Header = Delimiter.Split(MyContent[0]);
            if (Header[0] != "#")
            {
                Debug.Log(FileName + ".json was not found");
                return false;
            }

            Num_Ver = Convert.ToInt32(Header[1]);
            Num_Tri = Convert.ToInt32(Header[2]);


            // Bind Property
            Mesh Distort_map = Distort_Anime.GetComponent<MeshFilter>().mesh;
            // ------------------------------------------------- //
            // Get Vertices Info
            Vector3[] ver_coord = new Vector3[Num_Ver];
            Vector2[] ver_uv = new Vector2[Num_Ver];
            Vector3[] ver_nor = new Vector3[Num_Ver];
            Color[] ver_color = new Color[Num_Ver];

            int l_ind = 1;
            while (l_ind < 1 + Num_Ver)
            {
                string[] Info = Delimiter.Split(MyContent[l_ind]);
                // x,y,(z),u,v,r,g,(b)
                int V_ind = l_ind - 1;
                ver_coord[V_ind][0] = Convert.ToSingle(Info[0])*10f-5f;
                ver_coord[V_ind][1] = -Convert.ToSingle(Info[1])*10f+5f;
                ver_coord[V_ind][2] = Z;

                ver_uv[V_ind][0] = Convert.ToSingle(Info[2]);
                ver_uv[V_ind][1] = Convert.ToSingle(Info[3]);

                ver_nor[V_ind][2] = Convert.ToSingle(Info[4]); // Group
                ver_nor[V_ind][0] = Convert.ToSingle(Info[5]); // dir_x
                ver_nor[V_ind][1] = Convert.ToSingle(Info[6]); // dir_y

                ver_color[V_ind].r = 0;
                ver_color[V_ind].g = 0;
                ver_color[V_ind].b = 0;
                ver_color[V_ind].a = 1;

                l_ind++;
            }

            Distort_Anime.GetComponent<MeshFilter>().mesh.vertices = ver_coord;
            Distort_Anime.GetComponent<MeshFilter>().mesh.uv = ver_uv;
            Distort_Anime.GetComponent<MeshFilter>().mesh.colors = ver_color;
            Distort_Anime.GetComponent<MeshFilter>().mesh.normals = ver_nor;

            // Get Triangles Info
            int[] Tri = new int[Num_Tri * 3];
            int At = 0;
            while (l_ind < Num_Ver + 1 + Num_Tri)
            {
                string[] Info = Delimiter.Split(MyContent[l_ind]);
                
                Tri[At] = Convert.ToInt16(Info[0]);
                Tri[At + 1] = Convert.ToInt16(Info[1]);
                Tri[At + 2] = Convert.ToInt16(Info[2]);

                l_ind++;
                At += 3;
            }
            Distort_Anime.GetComponent<MeshFilter>().mesh.triangles = Tri;
            // ------------------------------------------------- //
           
        }
        else
        {
            Debug.Log(FileName + ".json was not found");
            return false;
        }
        return true;
    }


    // Direct Render from Material without camera
    private RenderTexture rt;
    private Texture Tex;
    private Texture2D RenderTo;

    public void SetRenderTarget(ref Texture2D texture){
        texture = new Texture2D (Tex.width, Tex.height);
        RenderTo = texture;
    }

    public void GetFrame(){
        Graphics.Blit (Tex, rt, Tar.material, 0);
        RenderTo.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
        RenderTo.Apply ();
    }
}
