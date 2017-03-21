using UnityEngine;
using System.Collections;

public class Archivecontroller : MonoBehaviour {

	public mainProgram main;
	public InputBoxController input;
	public bool fileReceived = false;

	// Use this for initialization
	void Start () {
	
	}

	
	// Update is called once per frame
	void Update () {
		if(input.inputString.Length == 5){
			if(input.inputString == "F4L79" && !fileReceived){
				main.dropCapsule();
				input.inputString = "";
				main.showMessageBox("Info", "Please receive the file capsule through the mechanized archieve tube. \n \n Пожалуйста, получите капсулу с файлом через трубу механизованного архива.", 8f);
				fileReceived = true;
			} else {
				input.inputString = "";
				main.showMessageBox("Error", "There was an error while retrieving the selected file. \n \n Во время получения файла возникла ошибка.", 8f);
			}
		}
	}

	public bool isOn
	{
		get { return input.isOn; }
		set
		{
				input.isOn = value;
		}
	}
}
