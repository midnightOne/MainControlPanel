using UnityEngine;
using System.Collections;

public class SpaceRotation : MonoBehaviour {

	public float rx = 0.5f;
	public float ry = 1.6f;
	public float rz = 0.3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate (rx,ry,rx);
	}
}
