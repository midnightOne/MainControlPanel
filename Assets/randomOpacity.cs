using UnityEngine;
using System.Collections;

public class randomOpacity : MonoBehaviour {


	public int stepLengthFrames = 10;
	public bool randomStep = true;
	public bool randomShift = true;

	private int frameCounter = 0;

	// Use this for initialization
	void Start () {
		if (randomStep){
			stepLengthFrames = Random.Range (2, stepLengthFrames);
		}
		if (randomShift) {
			frameCounter = Random.Range (0, stepLengthFrames);
		}


	}
	
	// Update is called once per frame
	void Update () {
		Color tempColor;

		if(frameCounter == 0){
			tempColor = gameObject.GetComponent<SpriteRenderer>().color;
			tempColor.a = Random.value;
			gameObject.GetComponent<SpriteRenderer>().color = tempColor;
		}

		frameCounter = (frameCounter+1)%stepLengthFrames;
	}

}
