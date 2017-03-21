using UnityEngine;
using System.Collections;
using Vectrosity;

public class graphDisplay : MonoBehaviour {

	public static bool enabled = true;

	public GameObject upperRight;
	public CircularSineWave data;
	public float thickness = 1f;
	public Camera renderCamera;
	public Material lineMaterial = null;

	private int totalSegments = 0;
	private VectorLine spline_;

	private float zeroLevel;

	// Use this for initialization
	void Start () {
		totalSegments = data.totalSegments;
		spline_ = new VectorLine("Spline", new Vector3[totalSegments],lineMaterial, thickness, LineType.Continuous);
		//spline_.drawTransform = gameObject.transform;
		VectorLine.SetCamera (renderCamera);
		SetLinePoints ();
		//upperRight.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if(!graphDisplay.enabled){
			return;
		}

		data.UpdateData ();
		for (int i = 0; i < totalSegments; i++) {
			spline_.points3[i].z = zeroLevel+data.waveValues[i];
		}
		spline_.Draw3D ();
	}

	void SetLinePoints () {
		zeroLevel = (gameObject.transform.position.z + upperRight.transform.position.z)/2.0f;
		for (int i = 0; i < totalSegments; i++) {
			float xPoint = Mathf.Lerp(gameObject.transform.position.x, upperRight.transform.position.x, (i+0.0f)/totalSegments);
			spline_.points3[i] = new Vector3(xPoint, gameObject.transform.position.y, zeroLevel);
		}

	}
}
