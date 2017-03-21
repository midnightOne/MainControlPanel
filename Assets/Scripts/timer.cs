using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

public class timer : MonoBehaviour {

	public bool paused = false;

	private Text textComp;
	private float startTime;
	private string tempString;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		textComp = gameObject.GetComponent<Text> ();
	}

	public void setStartTime(){
		startTime = Time.time;
	}

	public float timeLeft(){
		return 3600 - (Time.time - startTime);
	}
	
	// Update is called once per frame
	void Update () {

		//Time.time

		float deltaTime = 3600 - (Time.time - startTime);

		int hours = (int)deltaTime / 3600;
		deltaTime -= hours * 3600;
		int minutes = (int)deltaTime / 60;
		deltaTime -= minutes * 60;
		int seconds = (int)deltaTime;
		deltaTime -= seconds;
		int millis = (int)(deltaTime * 1000);

		if (!paused) {
			tempString = minutes.ToString ("00") + ":" + seconds.ToString ("00") + ":" + millis.ToString ("000");
			textComp.text = tempString;
		} else {
			if(seconds%2 == 0){
				textComp.text = tempString;
			} else {
				textComp.text = "";
			}
		}
	}
}
