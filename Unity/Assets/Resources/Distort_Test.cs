using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System;
//using MIB_Image;

public class Distort_Test : MonoBehaviour {

    // Use this for initialization
    internal IMAK_Distortion distortion_map1 = new IMAK_Distortion();

    public float breathFreq = 1f;
    public float blinkFreq = 1f;
    public float breathMagnitude = 0.17f;
    public string filename;

    Vector3[] ver;
    void Start () {
        // = GameObject.Find("Distort1");
        distortion_map1.Initialize(this.gameObject);
        distortion_map1.getMeshFromFile("HM_"+ filename, 10f);
        //distortion_map1.SetMaterial("Distort_Mat");
        distortion_map1.SetTexture(filename);

    }

    // Update is called once per frame
    void Update () {
        time_clock = Time.time;
        //distortion_map1.Distort_Anime.GetComponent<MeshFilter> ().mesh.vertices[5].x += 1f;
        distortion_map1.Breath(time_clock, breathMagnitude, breathFreq);
        distortion_map1.blink (time_clock, 5, blinkFreq);
        distortion_map1.Send_GPU_Command();

    }

    private float time_clock;

    void OnGUI()
    {
        
    }
}

