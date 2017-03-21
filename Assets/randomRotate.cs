using UnityEngine;
using System.Collections;
using DG.Tweening;

public class randomRotate : MonoBehaviour {

	public float amplitude = 20f;
	public float time = 2f;

	// Use this for initialization
	void Start () {
		StartCoroutine("rotate");

	}
	IEnumerator rotate(){
		float tempTime;
		while(true){
			while(!this.enabled){
				yield return new WaitForSeconds(0.5f);
			}

			tempTime = Random.Range(0.1f,time);
			gameObject.transform.DORotate (new Vector3(0,0,Random.Range (-amplitude, amplitude)), tempTime, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine);
			yield return new WaitForSeconds(tempTime*2f);
		}

	}

	// Update is called once per frame
	void Update () {
		//gameObject.transform.DORotate (Random.Range (-amplitude, amplitude), time);
	}
}
