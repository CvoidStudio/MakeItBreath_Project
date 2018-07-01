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
        Inter = new GameObject();
        // = GameObject.Find("Distort1");
        distortion_map1.Initialize(Inter);
        distortion_map1.getMeshFromFile("3_ZHM", 10f);
        //distortion_map1.SetMaterial("Distort_Mat");
        distortion_map1.SetTexture("3");

    }

	// Update is called once per frame
	void Update () {
        time_clock = Time.time;

    }

    private float time_clock;

    void OnGUI()
    {
        distortion_map1.FollowYourMouse();
    }
}

