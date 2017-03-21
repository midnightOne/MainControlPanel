using UnityEngine;
using System.Collections;

public class SatSearchBarController : MonoBehaviour {

	public GameObject[] cylinders = new GameObject[20];
	private float cylinderBaseScale;
	public bool locked = false;
	public GameObject target;
	public float radarRadius = 200f;
	//public bool searchEnabled = false;

	public float maxX = 1024;
	public float minX = 0;
	public float maxY = 768;
	public float minY = 0;


	public Vector3 velocity = new Vector3(0,0,0);

	[Range(0,1f)]
	public float progress = 0f;
	[Range(0,0.5f)]
	public float dispersion = 0.2f;

	// Use this for initialization
	void Start () {
		for(int i = 0; i<20; i++){
			cylinders[i] = GameObject.Find("Cylinder " + i);
			if(i==0){
				cylinderBaseScale = cylinders [0].transform.localScale.x;
			}
			cylinders[i].transform.localScale = new Vector3(0,0,0);
		}

	}
	
	// Update is called once per frame
	void Update () {
		float temp;
		Vector3 vecTemp;
		if (!locked) {
			gameObject.transform.Translate(velocity);
			vecTemp = gameObject.transform.position;

			if(vecTemp.x > maxX || vecTemp.x < minX || vecTemp.y > maxY || vecTemp.y < minY)
			{
				vecTemp.x = Mathf.Clamp(vecTemp.x,minX,maxX);
				vecTemp.y = Mathf.Clamp(vecTemp.y,minY,maxY);
				gameObject.transform.position = vecTemp;
			}

			vecTemp = target.transform.position - gameObject.transform.position;
			vecTemp.z = 0;			

			progress = 1 - Mathf.Clamp01(vecTemp.magnitude / radarRadius);

			for (int i = 0; i<20; i++) {
				temp = i / 19f;
				if (temp < progress - dispersion / 2) {
					temp = 1;
				} else {
					temp = Mathf.Clamp01 (1 - (temp - (progress - dispersion / 2)) / dispersion);
				
				}

				cylinders [i].transform.localScale = new Vector3 (cylinderBaseScale * temp, 0, cylinderBaseScale * temp);
			}

		} else {
			vecTemp = target.transform.position;
			vecTemp.z = gameObject.transform.position.z;
			gameObject.transform.position = vecTemp;

			for (int i = 0; i<20; i++) {
				temp = Random.value;
				cylinders [i].transform.localScale = new Vector3 (cylinderBaseScale * temp, 0, cylinderBaseScale * temp);
			}
		}
	}
}
