using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoreManager : MonoBehaviour {

	public GameObject redGraphic;
	public Repeater repeater;
	public bool isOn = true;
	public float powerLevel = 0f;
	public float pulsationFreq = 0.1f;
	public float pulsationFreqMultiplier = 3f;
	public float pulsationAmplitude = 0.2f;
	public float minCoreScale = 1f;
	public float maxCoreScale = 10f;

	public GameObject loadRed;
	public Text loadLabel;
	public Text powerLabel;
	public float redInitialY = 0f;
	public float loadLevel = 0f;



	private int counter = 0;
	private float onMultiplier = 1f;

	public randomRotate[] rotators;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		counter++;
		if(!isOn && onMultiplier > 0f){
			onMultiplier -= 0.01f;
		} else if(isOn && onMultiplier < 1f){
			onMultiplier += 0.01f;
		}

		for(int i =0;i<rotators.Length;i++){
			rotators[i].enabled = isOn;
		}

		float temp = (minCoreScale+(maxCoreScale-minCoreScale)*powerLevel) * (1+ Mathf.Sin(counter*pulsationFreq*powerLevel*pulsationFreqMultiplier)*pulsationAmplitude) * onMultiplier;

		repeater.progressValue = powerLevel*onMultiplier;
		redGraphic.transform.localScale = new Vector3 (temp,temp,temp);
		powerLabel.text = (int)(onMultiplier*powerLevel*100) + " %";

		temp = loadLevel * (1 + Mathf.Sin (counter * pulsationFreq * powerLevel * pulsationFreqMultiplier * 0.3f) * 0.05f);
		loadLabel.text = (int)(temp*100) + "%";
		loadRed.transform.rotation =  Quaternion.Euler(new Vector3 (0,redInitialY,Mathf.Clamp(-180+temp*180,-180f,0f)));
	}
}
