using UnityEngine;
using System.Collections;

public class CircularSineWaveDifference : CircularSineWave {

	//public GameObject wavesGameObject;
	public CircularSineWave wave_a;
	public CircularSineWave wave_b;

	public bool calculateEffectiveness = false; 
	public int effectiveness = 0;
	//public bool aMinusB = true;

	// Use this for initialization
	new void Start () {
		totalSegments = wave_a.totalSegments;
		base.Start ();
		//waves = wavesGameObject.GetComponents<CircularSineWave> ();
		//points = new Vector3[totalSegments+1];
	}
	
	// Update is called once per frame
	void Update () {

	}

	public override void UpdateData ()
	{
		//wave_a.UpdateData ();
		//wave_b.UpdateData ();

		float deltaRotation = 360f / totalSegments;

		for(int i=0;i<totalSegments;i++){
				waveValues[i] = wave_a.waveValues[i] + wave_b.waveValues[i];

			//effectiveness += 

			points[i] = PolarToCartesian(new Vector2(waveValues[i],i*deltaRotation)) + gameObject.transform.position;
		}
		points [totalSegments] = points [0];

		//base.UpdateData ();
	}
}
