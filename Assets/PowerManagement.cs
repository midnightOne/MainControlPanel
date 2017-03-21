using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerManagement : MonoBehaviour {

	public CoreManager[] cores;
	public BarManager[] bars;
	public Text ionText;
	[Range(0,1)]
	public float ionSpeed = 0f;
	public CanvasGroup ionBlockMessage;
	public CanvasGroup ionPurgeMessage;
	public CanvasGroup ionRefillMessage;

	public bool ionsPurged = false;

	public bool ionBlock = true;
	private float ionDisplaySpeed = 0f;
	private float ionIncrements = 0.1f;

	public float currentCapacity = 0f;

	private float coreIncrements = 0.06666f;

	// Use this for initialization
	void Start () {
		setIons(ionSpeed);
		ionPurgeMessage.alpha = 0.3f;
		ionRefillMessage.alpha = 0.3f;
	}

	public void ionSpeed_inc(){
		setIons(ionSpeed + ionIncrements);
	}
	public void ionSpeed_dec(){
		setIons(ionSpeed - ionIncrements);
	}

	public void ionRefill(){
		ionPurgeMessage.alpha = 0.3f;
		ionRefillMessage.alpha = 1f;
		setIons(1f);
		ionsPurged = false;
	}

	public void ionPurge(){
		ionPurgeMessage.alpha = 1f;
		setIons(0f);
		ionBlock = false;
		ionsPurged = true;
	}


	void setIons(float value){
		ionSpeed = Mathf.Clamp01 (value);

		bars[0].value = ionSpeed;
		if (!ionBlock) {
			bars [1].value = ionSpeed;
		} else {
			bars [1].value = 0f;
		}
	}

	public void core_1_inc(){
		cores [0].powerLevel = Mathf.Clamp01 (cores [0].powerLevel+coreIncrements);
	}

	public void core_1_dec(){
		cores [0].powerLevel = Mathf.Clamp01 (cores [0].powerLevel-coreIncrements);
	}

	public void core_2_inc(){
		cores [1].powerLevel = Mathf.Clamp01 (cores [1].powerLevel+coreIncrements);
	}
	
	public void core_2_dec(){
		cores [1].powerLevel = Mathf.Clamp01 (cores [1].powerLevel-coreIncrements);
	}


	// Update is called once per frame
	void Update () {
		if(ionDisplaySpeed < ionSpeed){
			ionDisplaySpeed = Mathf.Clamp(ionDisplaySpeed + 0.01f, 0f, ionSpeed);
		} else if(ionDisplaySpeed > ionSpeed){
			ionDisplaySpeed = Mathf.Clamp(ionDisplaySpeed - 0.01f, ionSpeed, 1f);
		}

		ionText.text = (int)(ionDisplaySpeed*100) + " %";

		if (ionBlock) {
			ionBlockMessage.alpha = (Mathf.Sin (Time.time * 3.14f * 2f) * 0.5f + 0.5f) * 0.5f + 0.5f;
		} else if(!ionBlock && ionBlockMessage.alpha > 0f) {
			ionBlockMessage.alpha = 0f;
		}

		currentCapacity = cores [1].powerLevel * cores [1].isOn.GetHashCode() + cores [0].powerLevel * cores [0].isOn.GetHashCode();


	}
}
