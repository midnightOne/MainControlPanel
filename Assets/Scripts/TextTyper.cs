using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextTyper : MonoBehaviour {
	
	public float letterPause = 0.2f;
	public bool useExistingText=true;
	public bool autoStart=true;
	public bool coroutineBased = true;
	//public AudioClip typeSound1;
	//public AudioClip typeSound2;

	private float startTime;
	private bool running=false;
	private char[] charArray;
	private int charIndex = 0;
	private int charsToRemove = 0;
	private int charsRemoved = 0;


	private bool eraseMode = false;

	public string message;
	Text textComp;
	
	// Use this for initialization
	void Start () {
		textComp = GetComponent<Text>();
		if(useExistingText)
			message = textComp.text;

		if (autoStart) {
			typeNewText();
		}
	}


	public void typeNewText(string text="",bool animateEraseFirst=false){
		clear (animateEraseFirst);
		if(text != ""){
			message = text;
		}
		running = true;

		if (coroutineBased) {
			StartCoroutine (TypeText ());
		} else {
			startTime = Time.time;
			charArray = message.ToCharArray();
			charIndex = 0;
		}
	}

	public void clear(bool animate=false){
		if(animate){
			eraseMode = true;
			charsToRemove = textComp.text.Length;
			charsRemoved = 0;
		} else {
 			textComp.text = "";
		}
	}

	void Update(){
		if(!coroutineBased && running){
			float dt = Time.time - startTime;

			if(eraseMode){
				while(charsRemoved < dt/(letterPause/2f)){
					textComp.text = textComp.text.Remove(textComp.text.Length-1);
					charsRemoved++;

					if(textComp.text.Length == 0){
						startTime = Time.time;
						eraseMode = false;
						break;
					}
				}
			} else {
				while(charIndex < dt/letterPause){
					textComp.text += charArray[charIndex];
					charIndex++;
					if(charIndex >= charArray.Length){
						running = false;
						break;
					}
				}
			}

		}
	}


	IEnumerator TypeText () {
		if (eraseMode) {
			while(textComp.text.Length > 0){
				textComp.text = textComp.text.Remove(textComp.text.Length-1);
				yield return 0;
				yield return new WaitForSeconds (letterPause/2f);
			}
		}

		foreach (char letter in message.ToCharArray()) {
			textComp.text += letter;
			//if (typeSound1 && typeSound2)
			//	SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}
		running = false;
		yield break;
	}
}