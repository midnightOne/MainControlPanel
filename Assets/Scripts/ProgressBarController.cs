using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBarController : MonoBehaviour {

	public Text percentageLabel;
	public Text engTitle;
	public Text rusTitle;

	[Range(0,1)]
	public float progress = 0;
	private float prevProgress;
	public ObjectRepeater repeaterObject;
	

	// Use this for initialization
	void Start () {

		prevProgress = progress;
	}

	public void setTitle(string eng, string rus){
		engTitle.text = eng;
		rusTitle.text = rus;
	}
	
	// Update is called once per frame
	void Update () {
		progress = Mathf.Clamp01 (progress);
		if(progress == prevProgress){
			return;
		}



		if(percentageLabel != null){
			percentageLabel.text = Mathf.RoundToInt(progress*100).ToString();
		}

		if(repeaterObject != null){
			repeaterObject.progressValue = progress;
		}
		prevProgress = progress;
	}
}
