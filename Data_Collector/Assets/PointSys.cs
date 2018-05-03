using UnityEngine;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;
using System.IO;


public class PointSys
{
    public Vector3[] PointsPos;
    public Vector3[] OutPos;
    static internal int Num_Ver;
    static internal GameObject[] Balls;
    private Sprite Src_Sprite;

    private GameObject myCanvas;

    private LineRenderer Lines;


    public PointSys() {
    }
    public void Initialize(int num_Points, string src_image) {
        Num_Ver = num_Points;
        PointsPos = new Vector3[num_Points];
        OutPos = new Vector3[num_Points];

        myCanvas = GameObject.Find("Canvas");
        Src_Sprite = Resources.Load<Sprite>(src_image);
        Balls = new GameObject[Num_Ver];
        for (int i = 0; i < Num_Ver; ++i)
        {
            Balls[i] = new GameObject(string.Format("PTC-{0}",i));
            Balls[i].AddComponent<Image>();
            Balls[i].GetComponent<Image>().sprite = Src_Sprite;
            Balls[i].AddComponent<Control>();
        }
    }

    // For loading heatmap & meshgrid from external file
    public bool getPointsFromFile(string FileName, float Z = 1f)
    {
        TextAsset DataText = Resources.Load(FileName) as TextAsset;
        string RawContent = DataText.text;
        Regex ReturnLine = new Regex("\n");
        if (RawContent != null)
        {
            Regex Delimiter = new Regex(",");

            string[] MyContent = ReturnLine.Split(RawContent);
            // ------------------------------------------------- //
            // Get Vertices Info
            int V_ind;
            for (V_ind = 0; V_ind < Num_Ver; ++V_ind)
            {
                string[] Info = Delimiter.Split(MyContent[V_ind]);
                // x,y,(z),u,v,r,g,(b)
                PointsPos[V_ind].x = Convert.ToSingle(Info[1])*450f + 15f;
                PointsPos[V_ind].y = -Convert.ToSingle(Info[2])*450f + 285f;
                PointsPos[V_ind].z = Z;
                OutPos[V_ind].z = Z + 1f;


                Balls[V_ind].transform.position = PointsPos[V_ind];

                Balls[V_ind].transform.localScale = new Vector3(0.08f, 0.08f, 1f);
                Balls[V_ind].transform.SetParent(myCanvas.transform, false);

                // Connections
                LineRenderer Lines = Balls[V_ind].AddComponent<LineRenderer>();
                Lines.material = new Material(Resources.Load<Shader>("LineShader"));
                Lines.positionCount = 2;
                Lines.startColor = Color.red;
                Lines.endColor = Color.red;
                Lines.startWidth = 0.03f;
                Lines.endWidth = 0.03f;
                
                Lines.SetPosition(0, new Vector3(PointsPos[V_ind].x/59.5f, PointsPos[V_ind].y/59.5f, Z + 1f));
                Lines.SetPosition(1, new Vector3(PointsPos[V_ind].x/59.5f, PointsPos[V_ind].y/59.5f, Z + 1f));
                Lines.transform.SetParent(myCanvas.transform, false);
            }

        }
        else
        {
            Debug.Log(FileName + ".json was not found");
            return false;
        }
        return true;
    }

    // ---------------- Point Class ----------------- //
    internal void OutputResults(string file_path, string file_name)
    {
        StreamWriter sw = new StreamWriter(file_path + "//" + file_name, false);

        float Adjust_x = 450f, Adjust_y = 450f;
        if (RenderSys.Tex_Ref_W > RenderSys.Tex_Ref_H)
            Adjust_y *= RenderSys.Tex_Ref_H / RenderSys.Tex_Ref_W;
        else
            Adjust_x *= RenderSys.Tex_Ref_W / RenderSys.Tex_Ref_H;

        for (int i = 0; i < Num_Ver; ++i){
            string line = string.Format(
                "{0},{1:F3},{2:F3}", i, 
                ((OutPos[i].x - RenderSys.trans_x) / RenderSys.zoom * 59.5f + 455f) / Adjust_x,
                ((OutPos[i].y - RenderSys.trans_y) / RenderSys.zoom * 59.5f + 165f) / Adjust_y);
            sw.WriteLine(line);
        }
        sw.Close();
        sw.Dispose();
        
    }

}

