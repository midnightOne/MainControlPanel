using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Repeater : MonoBehaviour {

	public GameObject referenceObject;
	private Vector3 referencePosition;
	private Quaternion referenceRotation;

	[Range(0,1)]
	public float progressValue = 1f;
	private float currentCopies;
	public int numberOfCopies;
	public bool positionShift=true;
	public Vector3 shift;

	public bool rotationShift = false;
	[Range(0,1)]
	public float startOfRotation=0;
	[Range(0,1)]
	public float endOfRotation=1;
	public float circleRadius;
	//public Vector3 rotShift;

	public GameObject center;
	public bool fade = false;

	private float prevProgress;
	private Vector3 prevShift;
	private Vector3 prevRotShift;
	private float prevStartOfRotation;
	private float prevEndOfRotation;
	private float prevCircleRadius;


	public GameObject[] copyAarray; 

	// Use this for initialization
	void Start () {
		copyAarray = new GameObject[numberOfCopies];
		Quaternion tempQuaternion = new Quaternion();
		float tempRotation;
		Vector3 tempPosition = new Vector3(0,0,0);
		Color tempColor;

		if(!center){
			center = referenceObject;
		}
		for(int i=0;i<numberOfCopies;i++){
			if(positionShift){
				tempPosition = referenceObject.transform.localPosition + shift*i;
			} else {
				tempPosition = new Vector3(0,0,0);
			}

			if(rotationShift){
				tempRotation = startOfRotation + (i/(float)numberOfCopies)*(endOfRotation-startOfRotation); // 0 to 1
				tempQuaternion.eulerAngles = new Vector3(0,0,tempRotation*360);
				tempPosition += placeOnCircle(center.transform.localPosition, circleRadius, tempRotation);
			} else {
				tempQuaternion = referenceObject.transform.rotation;
			}




			copyAarray[i] = (GameObject)Object.Instantiate(referenceObject, tempPosition, tempQuaternion);
			copyAarray[i].transform.localPosition = tempPosition;
			//copyAarray[i].transform.parent = referenceObject.transform.parent;
			copyAarray[i].transform.SetParent(gameObject.transform, false);
			copyAarray[i].transform.localScale = referenceObject.transform.localScale;
			if(i < currentCopies){
				copyAarray[i].SetActive(true);
			} else {
				copyAarray[i].SetActive(false);
			}
		}
		referencePosition = referenceObject.transform.localPosition;
		referenceRotation = referenceObject.transform.rotation;
		//Destroy (referenceObject);
		referenceObject.SetActive (false);
		Update ();
		/*prevShift = shift;
		prevCopies = copies;
		prevStartOfRotation = startOfRotation;
		prevEndOfRotation = endOfRotation;
		prevCircleRadius = circleRadius;*/
	}

	Vector3 placeOnCircle (Vector3 center ,   float radius , float circlePosition){
		float ang = circlePosition * 360;
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.z = center.z;
		return pos;
	}

	// Update is called once per frame
	void Update__ () {
		currentCopies = Mathf.Clamp(currentCopies,0,numberOfCopies);
		Color tempColor;

		if(shift != prevShift){
			for(int i=0;i<numberOfCopies;i++){
				copyAarray[i].transform.localPosition = new Vector3(0,0,0) + referencePosition + shift*i;
			}
			prevShift = shift;
		}

		if (progressValue != prevProgress) {
			for(int i=0;i<numberOfCopies;i++){

				if(i <= Mathf.CeilToInt(currentCopies)){
					copyAarray[i].SetActive(true);
					if(fade){
						if(i == Mathf.CeilToInt(currentCopies)){
							tempColor = copyAarray[i].GetComponent<Renderer>().material.color;
							tempColor.a = currentCopies-(i-1);

							copyAarray[i].GetComponent<Renderer>().material.color = tempColor;
						} else {
							tempColor = copyAarray[i].GetComponent<Renderer>().material.color;
							tempColor.a = 1;
							
							copyAarray[i].GetComponent<Renderer>().material.color = tempColor;
						}
					}
				} else {
					copyAarray[i].SetActive(false);
				}
			}
			prevProgress = progressValue;
		}
		//Object.Instantiate(referenceObject, 
		//referenceObject.
	}

	void Update(){
		currentCopies = numberOfCopies * progressValue;//Mathf.Clamp(currentCopies,0,numberOfCopies);
		Color tempColor;

		Quaternion tempQuaternion = new Quaternion();
		float tempRotation;
		Vector3 tempPosition;

		if (shift != prevShift || prevStartOfRotation != startOfRotation || prevEndOfRotation != endOfRotation || prevCircleRadius != circleRadius || progressValue != prevProgress) {
			for (int i=0; i<numberOfCopies; i++) {
				if (positionShift) {
					tempPosition = referencePosition + shift * i;
					prevShift = shift;
				} else {
					tempPosition = new Vector3 (0, 0, 0);
				}

				if (rotationShift) {
					tempRotation = startOfRotation + (i / (float)numberOfCopies) * (endOfRotation - startOfRotation); // 0 to 1
					tempQuaternion.eulerAngles = new Vector3 (0, 0, -tempRotation * 360);
					tempPosition += placeOnCircle (center.transform.localPosition, circleRadius, tempRotation);

					prevStartOfRotation = startOfRotation;
					prevEndOfRotation = endOfRotation;
					prevCircleRadius = circleRadius;
				} else {
					tempQuaternion = referenceRotation;
				}


				//copyAarray[i] = (GameObject)Object.Instantiate(referenceObject, tempPosition, tempQuaternion);

				copyAarray [i].transform.localPosition = tempPosition;
				copyAarray [i].transform.rotation = tempQuaternion;
				//copyAarray[i].transform.SetParent(referencetransform.parent, true);
				//copyAarray[i].transform.localScale = referencetransform.localScale;

				//if (copies != prevCopies) {
					if (i <= Mathf.CeilToInt (currentCopies)) {
						copyAarray [i].SetActive (true);
						if (fade) {
							if (i == Mathf.CeilToInt (currentCopies)) {
								tempColor = copyAarray [i].GetComponent<Renderer> ().material.color;
								tempColor.a = currentCopies - (i - 1);
							
								copyAarray [i].GetComponent<Renderer> ().material.color = tempColor;
							} else {
								tempColor = copyAarray [i].GetComponent<Renderer> ().material.color;
								tempColor.a = 1;
							
								copyAarray [i].GetComponent<Renderer> ().material.color = tempColor;
							}
						}
					} else {
						copyAarray [i].SetActive (false);
					}
				prevProgress = progressValue;
				//}
			}
		}

		//prevCopies = copies;
	}
}
