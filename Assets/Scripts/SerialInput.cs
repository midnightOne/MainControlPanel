using UnityEngine;
using System.Collections;
using System;

public class SerialInput : MonoBehaviour
{

	public static int charsNumber = 20;
	public static int transmittedCharsNumber = 3;
	public GameObject textureChangeObject;
	public static bool[] keyStates;
	static bool[] prevKeyStates;
	private SerialComm serialClass;

	private mainProgram main;
	
	/*private string[] audioIDs = {"c4","d4","e4","f4","g4","a4","b4","c5","d5","e5",
		"f5","g5","a5","b5","c6","","","","","","", //11-20
		"hw_Insert","hw_Remove","","","","","","","","",""}; //21-30*/

	// Use this for initialization
	void Start ()
	{
		keyStates = new bool[charsNumber];
		serialClass = gameObject.GetComponent<SerialComm> ();
		main = gameObject.GetComponent<mainProgram>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//int data=0;
		string tempString = serialClass.ReadFromArduino ();
		print (tempString);

		if (tempString != null) {
			if (tempString.StartsWith ("k") && tempString.Length >= transmittedCharsNumber) { //key
				//print (tempString);
				//data = (int)tempString[1] + 256*(int)tempString[2]; 
				tempString = hexToBin (tempString.Substring (1));
				//tempString = Convert.ToString(data, 2);
				//print (tempString);
				for (int i =0; i<charsNumber; i++) {
					
					if (tempString.Length > i) {
						keyStates [i] = (tempString [tempString.Length - 1 - i] == '1' ? true : false);
					} else {
						keyStates [i] = false;
					}
					
					
				}
			} else if (tempString.StartsWith ("s")) { //sound

				//AudioController.Play(audioIDs[int.Parse(tempString.Substring(1), System.Globalization.NumberStyles.HexNumber)]);
			} else if (tempString.StartsWith ("m")) { //show slave monitor
				print ("--------------------- SLAVE ON");
				GameObject.Find ("Camera-Slices").GetComponent<Camera> ().enabled = true;
			} else if (tempString.StartsWith ("M")) { //show MAIN monitor
				print ("--------------------- SLAVE OFF");
				GameObject.Find ("Camera-Slices").GetComponent<Camera> ().enabled = false;
			} else if(tempString.StartsWith ("U")){ // USB connected
				if(tempString[1] == 1){ // FIRST USB
					main.usbDriveConnected = true;
				} else {

				}
			} else if(tempString.StartsWith ("u")){ // USB DISconnected
				if(tempString[1] == 1){ // FIRST USB
					main.usbDriveConnected = false;
				} else {
					
				}
				
				
			} else if(tempString.StartsWith ("o")){ // "OTHER" - ONE CHAR COMMANDS
				//for(int i=1;i<tempString.Length;i++){
				if(tempString[1] == 'D'){ // Door opened
					main.doorOpen = true;
				} else if(tempString[1] == 'C'){ // Battery charged
					main.batteryCharged = true;
				} else if(tempString[1] == 'c'){ // Battery DIScharged
					main.batteryCharged = false;
				} else if(tempString[1] == 'B'){ // Battery installed
					main.batteryInstalled = true;
				} else if(tempString[1] == 'b'){ // Battery UNinstalled
					main.batteryInstalled = false;
				}
				//}

			}

			if (tempString.StartsWith ("f")) { // 6 PUZZLE FRAGMENTS
				print ("--------------------- " + tempString);


				textureChangeObject.GetComponent<TextureChange> ().changeTexture ((int)Char.GetNumericValue (tempString [1]) - 1, (int)Char.GetNumericValue (tempString [2]));

			}
		}

		/*for(int i = 0;i<keyNumber;i++){

			if(keyStates[i] != prevKeyStates[i]){

			}

		}*/
	}

	string hexToBin (string hex)
	{
		return System.Convert.ToString (int.Parse (hex, System.Globalization.NumberStyles.HexNumber), 2);
	}

}
