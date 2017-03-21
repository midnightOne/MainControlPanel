using UnityEngine;
using System.Collections;

public class sineDrift : MonoBehaviour
{
	Vector3 originalPosition;
	Vector3 m_centerPosition;
	float m_degrees;

	public float m_speed = 1.0f;
	public float m_amplitude = 1.0f;
	public float m_period = 1.0f;
	public int maxTravel = 100;

	void Start()
	{
		originalPosition = m_centerPosition = transform.position;
	}
	
	void Update()
	{
		float deltaTime = Time.deltaTime;
		
		// Move center along x axis
		m_centerPosition.x += deltaTime * m_speed;

		if(m_centerPosition.x - originalPosition.x > maxTravel){
			m_centerPosition.x = originalPosition.x;
		}
		
		// Update degrees
		float degreesPerSecond = 360.0f / m_period;
		m_degrees = Mathf.Repeat(m_degrees + (deltaTime * degreesPerSecond), 360.0f);
		float radians = m_degrees * Mathf.Deg2Rad;
		
		// Offset by sin wave
		Vector3 offset = new Vector3(0.0f, m_amplitude * Mathf.Sin(radians), 0.0f);
		transform.position = m_centerPosition + offset;
		

	}
	

}


/*
public class sineDrift : MonoBehaviour {

	/*public bool xAxis = true;
	public bool yAxis = true;
	public bool zAxis = false;*/
/*
	public float amplitude = 10f;
	public float xSpeed = 10f;
	public float ySpeed = 1f;
	public int maxTravel = 100;
	public GameObject relativeTo;

	private float referenceX;
	private float startTime;
	private float dt;

	// Use this for initialization
	void Start () {
		referenceX = gameObject.transform.position.x;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		dt = Time.time - startTime;

		gameObject.transform.Translate (xSpeed, 0, Mathf.Sin (dt*ySpeed) * amplitude,relativeTo.transform);
		if (gameObject.transform.position.x - referenceX >maxTravel){
			gameObject.transform.Translate (referenceX - gameObject.transform.position.x, 0, 0,relativeTo.transform);
		}
	}
}*/
