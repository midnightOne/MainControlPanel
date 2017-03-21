using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RandomScale : MonoBehaviour {

	//public GameObject[] bars;
	public float amplitude = 1f;
	public float time = 2f;
	public float delayMutiplier = 0f;
	//public Vector3 axes = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
		StartCoroutine("scale");
	}
		
	IEnumerator scale(){
		float tempTime;
		float tempScale;

		while(true){
			while(!this.enabled){
				yield return new WaitForSeconds(0.5f);
			}

			tempScale = Random.Range (0, amplitude);

			tempTime = Random.Range(0.1f,time);
			gameObject.transform.DOScale (new Vector3(1,tempScale,1), tempTime).SetEase(Ease.InOutSine);
			yield return new WaitForSeconds(tempTime+ tempTime*delayMutiplier);
		}
		
	}


	// Update is called once per frame
	void Update () {
	
	}
}
