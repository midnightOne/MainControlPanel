using UnityEngine;
using System.Collections;

public class BarManager : MonoBehaviour {

	public RandomScale[] bars;
	[Range(0f,1f)]
	public float value = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0;i< bars.Length;i++){
			bars[i].amplitude = value;
		}
	}
}
