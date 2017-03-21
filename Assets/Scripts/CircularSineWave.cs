using UnityEngine;
using System.Collections.Generic;

public class CircularSineWave : MonoBehaviour {

	//public int segments = 250;
	public int totalSegments = 250;
	//public bool loop = true;
	//public bool usePoints = false;

	public float frequency = 0.25f;
	public float amplitude = 1f;
	public float baseRadius = 20f;

	public float deltaT = 0.1f;
	private float totalDelta = 0;

	public float delay = 0f;
	public float r = 1f;

	public Vector3[] points;
	public float[] waveValues;

	private bool updatedThisFrame = false;

	// Use this for initialization
	protected void Start () {
		points = new Vector3[totalSegments+1];
		waveValues = new float[totalSegments + 1];
	}
	
	// Update is called once per frame
	void Update () {
		updatedThisFrame = false;
	}



	public virtual void UpdateData () {
		if(updatedThisFrame){
			return;
		}

		float deltaRotation = 360f / totalSegments;

		for(int i=0;i<totalSegments; i++){
			waveValues[i] = baseRadius+Mathf.Sin(i*frequency+totalDelta+delay)*amplitude;
			points[i] = PolarToCartesian(new Vector2(waveValues[i],i*deltaRotation))/* + gameObject.transform.position*/;
		}
		points [totalSegments] = points [0];


		totalDelta += deltaT;
		updatedThisFrame = true;
	}

	protected Vector3 PolarToCartesian (Vector2 polar)
	{
		
		//an origin vector, representing lat,lon of 0,0. 
		
		Vector3 origin= new Vector3(0,0,r);
		//build a quaternion using euler angles for lat,lon
		Quaternion rotation = Quaternion.Euler(polar.x,polar.y,0);
		//transform our reference vector by the rotation. Easy-peasy!
		Vector3 point=rotation*origin;
		
		return point;
	}

}




/*
public class blah : MonoBehaviour {
	
	public int segments = 250;
	public bool loop = true;
	public bool usePoints = false;
	public bool live = true;
	
	private VectorLine spline_;
	
	// Use this for initialization
	void Start () {
		List<Vector3> splinePoints = new List<Vector3>();
		int i = 1;
		GameObject obj = GameObject.Find("Sphere"+(i++));
		while (obj != null) {
			splinePoints.Add(obj.transform.position);
			obj = GameObject.Find("Sphere"+(i++));
		}
		
		if (usePoints) {
			VectorPoints dotLine = new VectorPoints("Spline", new Vector3[segments+1], null, 2.0f);
			dotLine.MakeSpline (splinePoints.ToArray(), segments, loop);
			dotLine.Draw();
		}
		else {
			VectorLine spline = spline_ = new VectorLine("Spline", new Vector3[segments+1], null, 2.0f, LineType.Continuous);
			spline.MakeSpline (splinePoints.ToArray(), segments, loop);
			
			
			spline.Draw3D();
			
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(live && spline_ != null){
			
			List<Vector3> splinePoints = new List<Vector3>();
			int i = 1;
			GameObject obj = GameObject.Find("Sphere"+(i++));
			while (obj != null) {
				splinePoints.Add(obj.transform.position);
				obj = GameObject.Find("Sphere"+(i++));
			}
			
			spline_.MakeSpline (splinePoints.ToArray(), segments, loop);
			
			
			spline_.Draw3D();
			
		}
	}
}*/

