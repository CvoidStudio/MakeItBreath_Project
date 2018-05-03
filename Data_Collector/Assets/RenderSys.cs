using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.UI;


public class RenderSys
{
    public RenderSys() {}

    // Direct Render from Material without camera
    private RenderTexture rt1, rt2;  // ref / to assign
    private Texture Tex1, Tex2;  // ref / to assign
    private Texture2D RenderTo;

    public GameObject Reference;
    public GameObject ToAssign;

    internal MeshRenderer Tar1, Tar2;

    internal float Assign_x, Assign_y, Assign_zoom;

    public void Initialize(GameObject obj1, GameObject obj2)
    {
        Reference = obj1;
        Reference.AddComponent<MeshFilter>();
        Reference.AddComponent<MeshRenderer>();

        ToAssign = obj2;
        ToAssign.AddComponent<MeshFilter>();
        ToAssign.AddComponent<MeshRenderer>();

       
    }


    static internal int Tex_Ref_W, Tex_Ref_H;
    public void SetTexture_Ref(string file_name)
    {
        Shader Disort_shader = Resources.Load<Shader>("Ref_TexShader");
        Material mat = new Material(Disort_shader);
        Tex1 = Resources.Load<Texture>(file_name);
        mat.SetTexture("_MainTex", Tex1);
        Tar1 = Reference.GetComponent<MeshRenderer>();
        Tar1.material = mat;

        // store the info
        Tex_Ref_W = Tex1.width;
        Tex_Ref_H = Tex1.height;

        rt1 = new RenderTexture(Tex1.width, Tex1.height, 0);

        // Save Efficiency
        Tar1.receiveShadows = false;
        Tar1.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    internal int Tex_As_W, Tex_As_H;
    public void SetTexture_ToAssign(string file_name)
    {
        Shader Disort_shader = Resources.Load<Shader>("TexShader");
        Material mat = new Material(Disort_shader);
        Tex2 = Resources.Load<Texture>(file_name);
        mat.SetTexture("_MainTex", Tex2);
        Tar2 = ToAssign.GetComponent<MeshRenderer>();
        Tar2.material = mat;

        // store the info
        Tex_As_W = Tex2.width;
        Tex_As_H = Tex2.height;


        rt2 = new RenderTexture(Tex1.width, Tex1.height, 0);

        // Save Efficiency
        Tar2.receiveShadows = false;
        Tar2.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // Frame Material
        GameObject.Find("Frame").GetComponent<Image>().material =
            new Material(Resources.Load<Shader>("BackShader"));

        Adjust_Assign_Zoom(1f);
    }


    internal Vector3[] GenRect(float x, float y, float width, float height)
    {
        Vector3[] ver_coord =
        {
            new Vector3(x,y,0),
            new Vector3(x+width,y,0),
            new Vector3(x+width,y+height,0),
            new Vector3(x,y+height,0),
        };
        return ver_coord;
    }

    public void GenerateMesh(Vector3[] Rect, GameObject Target)
    {
        // Get Vertices Info
        Vector2[] ver_uv ={
            new Vector2(0f,0f),
            new Vector2(1f,0f),
            new Vector2(1f,1f),
            new Vector2(0f,1f),
        };

        int[] Tri = {
            0,1,3,
            3,1,2
        };

        Target.GetComponent<MeshFilter>().mesh.vertices = Rect;
        Target.GetComponent<MeshFilter>().mesh.uv = ver_uv;
        Target.GetComponent<MeshFilter>().mesh.triangles = Tri;
    }


    // For Adjusting the pos of Assign-Tex
    internal void Adjust_Assign_Pos(float x, float y)
    {
        Tar2.material.SetFloat("trans_x", x);
        Tar2.material.SetFloat("trans_y", y);
    }
    internal void Adjust_Assign_Zoom(float zoom)
    {
        Tar2.material.SetFloat("zoom", zoom);
    }


    private bool in_Rect(float mx, float my, float x, float y, float w, float h)
    {
        float dx = mx - x;
        float dy = my - y;
        return (dx > 0) && (dx < w) && (dy > 0) && (dy < h);
    }
    private Vector3 mousePos;
    static internal float trans_x = 0f, trans_y = 0f, zoom = 1f;
    bool Move = false;
    float MouseWheelSensitivity = 0.5f;
    internal void Drag_As_Pic()
    {
        if (in_Rect(Input.mousePosition.x, Input.mousePosition.y, 25f, 135f, 450f, 450f))
        {
            // Drag
            if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift)){
                mousePos = Input.mousePosition;
                Move = true;
            }

            // Scroll
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                zoom += Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
                Adjust_Assign_Zoom(zoom);

                for (int i = 0; i < PointSys.Num_Ver; ++i)
                {
                    Vector3 newV = Datamain.Points.OutPos[i] * zoom + new Vector3(trans_x, trans_y, 0f);
                    PointSys.Balls[i].GetComponent<LineRenderer>().SetPosition(1, newV);
                    PointSys.Balls[i].GetComponent<Image>().rectTransform.position = newV;
                }
                    
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            Move = false;
        }


        if (Move)
        {
            trans_x += (Input.mousePosition.x - mousePos.x) / 200f;
            trans_y += (Input.mousePosition.y - mousePos.y) / 200f;
            Adjust_Assign_Pos(trans_x, trans_y);
            mousePos = Input.mousePosition;

            for (int i = 0; i < PointSys.Num_Ver; ++i) 
            {
                Vector3 newV = Datamain.Points.OutPos[i] * zoom + new Vector3(trans_x, trans_y, 0f);
                PointSys.Balls[i].GetComponent<LineRenderer>().SetPosition(1, newV);
                PointSys.Balls[i].GetComponent<Image>().rectTransform.position = newV;


            }
        }

        /*
        Datamain.Points.OutPos[0].x = (450f + 25f - 480f) / 59.5f * zoom + trans_x;
        Datamain.Points.OutPos[0].y = (450f + 135f - 300f) / 59.5f * zoom + trans_y;
        Datamain.Points.Balls[0].GetComponent<LineRenderer>().SetPosition
                (1, Datamain.Points.OutPos[0]);
                */
    }
}
