using UnityEngine;
using System.Collections;

public class PotRotator : MonoBehaviour {

	public float value = 0;
	public float lowerValue = 0;
	public float higherValue = 1;
	public float lowerAngle = -140;
	public float higherAngle = 140;
	private float prevValue = 0;
	 
	private Vector3 referenceRotation;
	private Vector3 tempVec3;

	// Use this for initialization
	void Start () {
		value = lowerValue;
		tempVec3 = new Vector3 (0,0,0);
		referenceRotation = gameObject.transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if(value == prevValue){
			return;
		}

		//tempVec3 = gameObject.transform.rotation.eulerAngles;
		tempVec3.z = lowerAngle + (higherAngle-lowerAngle)*((value-lowerValue)/(higherValue-lowerValue));
		gameObject.transform.rotation = Quaternion.Euler(tempVec3 + referenceRotation);
		prevValue = value; 
	}

	/*float mapInterp(){

	}*/
}
