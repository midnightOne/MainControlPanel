using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UnlockObject : MonoBehaviour {

	public GameObject[] buttons;
	public GameObject[] lights;

	private bool[] prevButtonStates = {false, false, false, false, false, false};
	private int currentButton = -1;

	private bool[] lightLit = {false, false, false, false, false, false, false, false, false, false};
	private bool[] correctCode = {true, true, true, false, false, true, false, false, true, true};
	/*      
	 *     CORRECT IMAGE
	 * 
	 * 		    /|\
	 *        /  |  \
	 *      /    |    \
	 *    /		 |      \
	 *    \			    /
	 * 	   \	       /
	 * 		\_________/
	 * */

	private int[][] connectedButtons = new int[6][];


	/*{
		{ -1, 0, 1, 2, -1, -1},
		{0, -1, 3, -1, 5, -1},
		{1, 3, -1, 4, 6, 7},
		{2, -1, 4, -1, -1, 8},
		{-1, 5, 6, -1, -1, 9},
		{-1, -1, 7, 8, 9, -1}
	};*/

	public void reset(){
		currentButton = -1;

		for(int i=0;i<prevButtonStates.Length;i++){
			prevButtonStates[i] = false;
		}
		for(int i=0;i<lightLit.Length;i++){
			//lightLit[i] = false;
			switchLight(i,false);
		}

		for(int i=0;i<buttons.Length;i++){
			animateTween (buttons[i]);
		}


	}

	// Use this for initialization
	void Start () {
		connectedButtons[0] = new int[6]{-1, 0, 1, 2, -1, -1};
		connectedButtons[1] = new int[6]{0, -1, 3, -1, 5, -1};
		connectedButtons[2] = new int[6]{1, 3, -1, 4, 6, 7};
		connectedButtons[3] = new int[6]{2, -1, 4, -1, -1, 8};
		connectedButtons[4] = new int[6]{-1, 5, 6, -1, -1, 9};
		connectedButtons[5] = new int[6]{-1, -1, 7, 8, 9, -1};
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateButtons(bool[] keyStates){
		for (int i=0;i<6;i++){
			if(keyStates[i]){
					//Serial.println(i);
				if(!prevButtonStates[i]){
					prevButtonStates[i] = true;
					//button press event
					processButtonPress(i);
				}
			} else {
				if(prevButtonStates[i]){

					if( i != currentButton){
						prevButtonStates[i] = false;
						//button release event
						processButtonRelease(i);
					}
				}
			}

		}
	}

	public bool checkComplete(){
		for(int i=0;i<10;i++){
			if(lightLit[i] != correctCode[i]){
				return false;
			}
		}
		return true;
	}

	void processButtonPress(int buttonIndex){
		int temp;
		
		if (currentButton == -1) {
			currentButton = buttonIndex;
			animateTween (buttons[buttonIndex], false);
			return;
		}
		
		temp = connectedButtons[currentButton][buttonIndex];
		if (temp >-1) {
			toggleLight(temp);
			//lightLit[temp] = !lightLit[temp];
			currentButton = buttonIndex;
		}

		animateTween (buttons[buttonIndex], false);
	}

	void processButtonRelease(int buttonIndex){

		animateTween (buttons[buttonIndex]);
	}

	void toggleLight(int lightIndex){
		switchLight(lightIndex, !lightLit[lightIndex]);
	}

	void switchLight(int lightIndex, bool on = true){
		if(lightLit[lightIndex] == on){
			return;
		}

		animateTween(lights[lightIndex], !on);

		lightLit[lightIndex] = on;
	}

	void animateTween(GameObject gameObj, bool forward=true, float setDuration=0.4f){
		DOTweenAnimation[] tweens = gameObj.GetComponents<DOTweenAnimation>();

		if(setDuration>=0){
			foreach (DOTweenAnimation tween in tweens) {
				tween.duration = setDuration;
			}
		}


		if(forward){
			foreach (DOTweenAnimation tween in tweens) {
				tween.DOPlayForward();
			}
		} else {
			foreach (DOTweenAnimation tween in tweens) {
				tween.DOPlayBackwards();
			}
		}
	}

}
