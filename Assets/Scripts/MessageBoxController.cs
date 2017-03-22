using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MessageBoxController : MonoBehaviour {

	public TextAutoTyper titleTyper;
	public TextAutoTyper mainTextTyper;
	public bool on=false;

	// Use this for initialization
	void Start () {
	
	}

	public void createMessageBox(string title, string message){
		titleTyper.typeNewText (title);
		mainTextTyper.typeNewText (message);
		on = true;
	}

	public void showMessageBox (string title, string text, float stayFor=2f)
	{
		if (on) {
			StopCoroutine ("delayedhideMessageBox");
			createMessageBox (title, text);
		} else {

			gameObject.SetActive (true);
			gameObject.GetComponent<DOTweenAnimation> ().DOPlayBackwards ();
			createMessageBox (title, text);
		}
		
		StartCoroutine ("delayedhideMessageBox", stayFor);
		
	}
	
	IEnumerator delayedhideMessageBox (float delay)
	{
		yield return new WaitForSeconds (delay);  
		hideMessageBox ();
		yield break;
		
	}
	
	void hideMessageBox ()
	{
		gameObject.GetComponent<DOTweenAnimation> ().DOPlayForward ();
		StartCoroutine ("delayeddisableMessageBox", 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
