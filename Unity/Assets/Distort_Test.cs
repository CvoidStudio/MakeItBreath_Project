using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System;
//using MIB_Image;

public class Distort_Test : MonoBehaviour {

    // Use this for initialization
    internal IMAK_Distortion distortion_map1 = new IMAK_Distortion();

	public GameObject Inter;
	Vector3[] ver;
    void Start () {
        // = GameObject.Find("Distort1");
        distortion_map1.Initialize(Inter);
        distortion_map1.getMeshFromFile("HM_73", 10f);
        //distortion_map1.SetMaterial("Distort_Mat");
        distortion_map1.SetTexture("73");

    }

	// Update is called once per frame
	void Update () {
        time_clock = Time.time;
		//distortion_map1.Distort_Anime.GetComponent<MeshFilter> ().mesh.vertices[5].x += 1f;
		distortion_map1.Breath(time_clock, 0.1f, 1);
		float blink_time = time_clock - time_clock%(2.0f);
		distortion_map1.blink (time_clock, blink_time, 0.8f, 1f);
		distortion_map1.Send_GPU_Command();

    }

    private float time_clock;

    void OnGUI()
    {
		
    }
}

