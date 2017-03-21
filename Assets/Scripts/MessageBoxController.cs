using UnityEngine;
using System.Collections;

public class MessageBoxController : MonoBehaviour {

	public TextTyper titleTyper;
	public TextTyper mainTextTyper;
	public bool on=false;

	// Use this for initialization
	void Start () {
	
	}

	public void createMessageBox(string title, string message){
		titleTyper.typeNewText (title);
		mainTextTyper.typeNewText (message);
		on = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
