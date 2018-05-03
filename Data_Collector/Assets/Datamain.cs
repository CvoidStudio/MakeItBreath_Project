using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datamain : MonoBehaviour {

    // Use this for initialization
    internal RenderSys DC = new RenderSys();
    static internal PointSys Points = new PointSys();

    internal GameObject Inter_Ref, Inter_As;
    Vector3[] ver;
    string ToAS;
    void Start()
    {
        Inter_Ref = new GameObject("Tex_Reference");
        Inter_As = new GameObject("Tex_Assignment");

        DC.Initialize(Inter_Ref, Inter_As);

        // Get Position
        ToAS = "1";
        DC.SetTexture_Ref("src");
        DC.SetTexture_ToAssign(ToAS);
        Vector3[] ref_Rect = DC.GenRect
            (0.25f, -2.75f, 7.5f, 7.5f * RenderSys.Tex_Ref_H / RenderSys.Tex_Ref_W);

        Vector3[] As_Rect = null;
        if (DC.Tex_As_H < DC.Tex_As_W){
            As_Rect = DC.GenRect
            (-7.75f, -2.75f, 7.5f, 7.5f * DC.Tex_As_H / DC.Tex_As_W);
            DC.Tex_As_H = (int)(450f * DC.Tex_As_H / DC.Tex_As_W);
            DC.Tex_As_W = 450;
        }
        else{
            As_Rect = DC.GenRect
            (-7.75f, -2.75f, 7.5f * DC.Tex_As_W / DC.Tex_As_H, 7.5f);
            DC.Tex_As_W = (int)(450f * DC.Tex_As_W / DC.Tex_As_H);
            DC.Tex_As_H = 450;
        }


        DC.GenerateMesh(ref_Rect, Inter_Ref);
        DC.GenerateMesh(As_Rect, Inter_As);

        // points system
        Points.Initialize(68, "Ball");
        Points.getPointsFromFile("PTS_src");
    }

    // Update is called once per frame
    void Update()
    {
        DC.Drag_As_Pic();
        if (Input.GetKeyDown(KeyCode.O))
        {
            Points.OutputResults("D://Work//Rhythm//Research//ML_Try2//Bank1", ToAS + ".pmap");
        }
    }

    private float time_clock;

    void OnGUI()
    {

    }
}
