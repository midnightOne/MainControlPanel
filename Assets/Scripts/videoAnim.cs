using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(AudioSource))]

public class videoAnim : MonoBehaviour {

	//public int loopFrom = 0; 
	public MovieTexture introMovie;
	public MovieTexture loopMovie;
	public bool autoStart = true;
	private bool isPlaying;

	private MovieTexture currentTexture;
	MovieTexture movie;



	// Use this for initialization
	void Start () {
		isPlaying = autoStart;

		if (!introMovie && !loopMovie) {
			movie = GetComponent<Renderer> ().material.mainTexture as MovieTexture;
			currentTexture = movie;

			movie.loop = true;
			if(autoStart){
				movie.Play ();
			}
		} else if (introMovie) {
			switchToIntro();
			if(autoStart){
				movie.Play ();
			} 
			if(loopMovie && autoStart){
				PlayThenDoSomething(switchToLoop);
			}
		} else {
			switchToLoop();
			if(autoStart){
				movie.Play ();
			}
		}



		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void start(){
		movie.Play ();
		isPlaying = true;

		if(!autoStart && currentTexture == introMovie){
			PlayThenDoSomething(switchToLoop);
		}
	}

	public void stop(){
		movie.Stop();
		isPlaying = false;
	}
	public void pause(){
		movie.Pause();
		isPlaying = false;
	}
	public void unpause(){
		movie.Play();
		isPlaying = true;
	}

	void switchToIntro(){
		movie = currentTexture = introMovie;
		movie.loop = false;
		GetComponent<Renderer> ().material.mainTexture = movie;
		movie.Stop ();
		if(isPlaying){
			movie.Play ();
		}
	}

	void switchToLoop(){
		movie = currentTexture = loopMovie;
		movie.loop = true;
		GetComponent<Renderer> ().material.mainTexture = movie;
		if(isPlaying){
			movie.Play ();
		}

		StopCoroutine ("FindEnd");
	}

	public void PlayThenDoSomething(Action callback)
	{
		movie.Stop();
		movie.Play ();

		StartCoroutine(FindEnd(callback));
	}
	
	private IEnumerator FindEnd(Action callback)
	{
		while(movie.isPlaying)
		{
			yield return 0;
		}
		
		callback();
		yield break;
	}
}
