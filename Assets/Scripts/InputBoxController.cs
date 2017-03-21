using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputBoxController : MonoBehaviour {

	public TextTyper titleTyper;
	public Text inputText;
	public bool isOn=false;
	private bool carpetOn = true;
	public string inputString = "";
	public int maxChars = 9;

	public float timeStamp;


	private string[] keys = {"1","2","3","4","5","6","7","8","9","0","a","s","d","f","j","k","l","v","m"}; 
	private string backSpace = "b";

	// Use this for initialization
	void Start () {
		//createInputBox ("input test");
		//StartCoroutine ("updateCarpet");
	}

	public void createInputBox(string title){
		if (titleTyper != null) {
			titleTyper.typeNewText (title);
		}
		
		isOn = true;
		StartCoroutine ("updateCarpet");
	}

	public void clear (){
		inputString = "";
	}

	public void changeTitle(string text,bool animateEraseFirst=false){
		if (titleTyper != null) {
			titleTyper.typeNewText (text, animateEraseFirst);
		}
	}

	private IEnumerator updateCarpet(){

		while (true) {
			carpetOn = !carpetOn;
			yield return new WaitForSeconds(1f);
		}

		yield break;
	}

	void addChar(string c){
		if(inputString.Length < maxChars){
			inputString += c;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isOn){
			if(Input.GetKeyDown(backSpace) && inputString.Length > 0){
				inputString = inputString.Remove(inputString.Length-1);
			}

			for(int i=0;i< keys.Length;i++){
				if(Input.GetKeyDown(keys[i])){
					addChar(keys[i].ToUpper());
				}
			}

			inputText.text = inputString;
			if(carpetOn){
				inputText.text += "_";
			}
		}
	}
}
