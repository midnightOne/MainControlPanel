using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Vectrosity;



public class CircularSineWaveVis : MonoBehaviour {

	public static bool enabled = true;

	public CircularSineWave data;
	private int totalSegments = 0;
	
	public float thickness = 1f;
	public bool live = true;
	private VectorLine spline_;
	public Camera renderCamera;
	
	public Material lineMaterial = null;




	// Use this for initialization
	void Start () {
		totalSegments = data.totalSegments;
		spline_ = new VectorLine("Spline", new Vector3[totalSegments+1],lineMaterial, thickness, LineType.Continuous);
		spline_.drawTransform = gameObject.transform;
		VectorLine.SetCamera (renderCamera);
		//spline_.Draw3DAuto();
	}
	
	// Update is called once per frame
	void Update () {
		if(!CircularSineWaveVis.enabled){
			return;
		}
		data.UpdateData ();
		spline_. points3 = data.points;
		spline_.Draw3D ();
	}
}
