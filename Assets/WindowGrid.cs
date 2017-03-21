using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WindowGrid : MonoBehaviour {

	public bool[] windowsEnabled;
	public GameObject[] windows;
	private RectTransform[] rectTransforms;
	public float columnWidth = 100f;
	public float centerHeight = 700f;
	public float padding = 10f;

	public bool enabled = false;

	private List<int> activeTransforms;
	private List<int> inactiveTransforms;

	private float[] aspectRatios;
	private List<int> aspectRatioIndicesSorted; // INDICES of widths of windows in descending order
	private float averageAspectRatio;

	private Vector3[] targetPositions;
	private float[] targetScales;

	private RectTransform canvasTransform;
	private float width;
	private float height;


	// Use this for initialization
	void Start () {
		canvasTransform = (GetComponent<Canvas> ().transform as RectTransform);
		width = canvasTransform.rect.width;
		height = canvasTransform.rect.height;

		Debug.Log ("CANVAS WIDTH = " + width);

		rectTransforms = new RectTransform[windows.Length];
		targetPositions = new Vector3[windows.Length];
		targetScales = new float[windows.Length];
		aspectRatioIndicesSorted = new List<int>();
		aspectRatios = new float[windows.Length];

		activeTransforms =  new List<int>();
		inactiveTransforms =  new List<int>();

		for(int i=0;i<windows.Length;i++){
			rectTransforms[i] = windows[i].GetComponent<RectTransform>();
			Debug.Log ("OBJ WIDTH = " + rectTransforms[i].rect.width);
			 // .transform.localScale = new Vector3(testDelta,testDelta,testDelta);
			//aspectRatiosSorted[i] = i;
		}

		//sortDimensions ();
	}

	void sortDimensions(){
		averageAspectRatio = 0;
		aspectRatioIndicesSorted.Clear ();


		for(int i=0;i<windows.Length;i++){
			aspectRatios[i] = (rectTransforms[i].rect.width/rectTransforms[i].rect.height);
			if(windowsEnabled[i]){
				aspectRatioIndicesSorted.Add (i);
				averageAspectRatio += aspectRatios[i];
			}
		}
		if(aspectRatioIndicesSorted.Count == 0){
			return;
		}
		averageAspectRatio = averageAspectRatio / aspectRatioIndicesSorted.Count;

		//Array.Sort(aspectRatiosSorted,delegate(int x, int y) { return (aspectRatios[y]).CompareTo(aspectRatios[x]); }); //ARRAY
		//aspectRatiosSorted.Sort (SortByAspectRatio);
		aspectRatioIndicesSorted.Sort((x,y)=>(aspectRatios[y]).CompareTo(aspectRatios[x]));

		//Debug.Log ("");
		//Array.Sort(heightsSorted,delegate(int x, int y) { return rectTransforms[y].rect.height.CompareTo(rectTransforms[x].rect.height); });

		/*for (int i=0; i<aspectRatiosSorted.Count; i++) {
			Debug.Log(aspectRatios[aspectRatiosSorted[i]]);
		}*/
	}


	/*static int SortByAspectRatio(int x, int y)
	{
		return (aspectRatios[y]).CompareTo(aspectRatios[x]);
	}*/
	
	// Update is called once per frame
	void Update () {
		width = canvasTransform.rect.width;
		height = canvasTransform.rect.height;

		sortDimensions ();

		calculateTargetPositions ();

		for(int i=0;i<windows.Length;i++){
			rectTransforms[i].localScale = multerp(rectTransforms[i].localScale, new Vector3(targetScales[i],targetScales[i],targetScales[i]));
			rectTransforms[i].anchoredPosition = multerp(rectTransforms[i].anchoredPosition, targetPositions[i]);
			//rectTransforms[i].localScale = new Vector3(targetScales[i],targetScales[i],targetScales[i]);
			//rectTransforms[i].anchoredPosition = targetPositions[i];
		}
	}

	/**
	 * a - from
	 * b - to
	 * mult - multiplier
	 */

	Vector3 multerp(Vector3 a, Vector3 b, float mult = 0.3f){
		return new Vector3 (a.x+(b.x - a.x)*mult,a.y+(b.y - a.y)*mult,a.z+(b.z - a.z)*mult);
	}

	void calculateTargetPositions(){

		activeTransforms.Clear ();

		// FOR LEFT COLUMN
		float tempScale;
		float groupHalfHeight = 0; //(rectTransforms[i].rect.height*tempScale+padding)*windows.Length/2;
		float currentVerticalPosition = 0;
		int paddingCount = 0;
		int enabledCount = 0;

		Rect menuArea = new Rect ();

		for (int i=0; i<windows.Length; i++) {
			if(!windowsEnabled[i] || !enabled){
				tempScale = columnWidth/rectTransforms[i].rect.width;
				groupHalfHeight += rectTransforms[i].rect.height*tempScale;
				paddingCount++;
			} else {
				activeTransforms.Add(i);
				enabledCount++;
			}
		}
		groupHalfHeight += padding * (paddingCount-1);
		groupHalfHeight = groupHalfHeight/2.0f;
		
		currentVerticalPosition = height / 2 - groupHalfHeight;
		
		for(int i=0;i<windows.Length;i++){
			if(!enabled){
				tempScale = columnWidth/rectTransforms[i].rect.width;
				targetScales[i] = tempScale;
				
				targetPositions[i] = new Vector3(-columnWidth - padding, currentVerticalPosition,0);
				currentVerticalPosition += (rectTransforms[i].rect.height*tempScale+padding);
			} else if(!windowsEnabled[i]){
				tempScale = columnWidth/rectTransforms[i].rect.width;
				targetScales[i] = tempScale;
				
				targetPositions[i] = new Vector3(padding, currentVerticalPosition,0);
				currentVerticalPosition += (rectTransforms[i].rect.height*tempScale+padding);
			}
		}

		if(!enabled){
			return;
		}

		// FOR CENTER
		float tempHeight = 0;
		float maxWidth = 0;

		float centerWidth = width - (2*padding+columnWidth); //width - 2*(2*padding+columnWidth);

		menuArea.width = width - (3*padding+columnWidth);
		menuArea.height = centerHeight;
		menuArea.x = 2 * padding + columnWidth;
		menuArea.y = (height - centerHeight) / 2;


		//currentVerticalPosition = (height - centerHeight)/2;

		/*if (enabledCount == 1) {
			for(int i=0;i<windows.Length;i++){
				if(windowsEnabled[i]){
					tempScale = calculateLowerScale(rectTransforms[i].rect.width,rectTransforms[i].rect.height, centerWidth, centerHeight);// centerWidth / rectTransforms[i].rect.width;
					targetScales[i] = tempScale;
					targetPositions[i] = new Vector3(width/2 - rectTransforms[i].rect.width*tempScale/2f, height/2f-rectTransforms[i].rect.height*tempScale/2f,0);
					break;
				}
			}


		} else*/ 
		if(enabledCount < 3){
			for(int i=0;i<windows.Length;i++){
				if(windowsEnabled[i]){
					tempHeight += rectTransforms[i].rect.height;
					maxWidth = Mathf.Max(maxWidth, rectTransforms[i].rect.width);
				}
			}
			//tempHeight += padding*(enabledCount-1);
			tempScale = calculateLowerScale(maxWidth, tempHeight, centerWidth, (centerHeight-padding*(enabledCount-1))); //(centerHeight-padding*(enabledCount-1)) / tempHeight;
			currentVerticalPosition = height/2 - tempHeight*tempScale/2;


			for(int i=0;i<windows.Length;i++){
				if(windowsEnabled[i]){
					targetScales[i] = tempScale;
					targetPositions[i] = new Vector3(padding+columnWidth+ centerWidth/2 - rectTransforms[i].rect.width*tempScale/2,currentVerticalPosition,0);
					currentVerticalPosition += rectTransforms[i].rect.height*tempScale + padding;
					//tempHeight += rectTransforms[i].rect.height;
				}
			}
		} else {

			place(aspectRatioIndicesSorted, menuArea);

			/*if(enabledCount%2 == 1){ // ODD
				if(aspectRatios[aspectRatioIndicesSorted[aspectRatioIndicesSorted.Count-1]] <= 0.5){

				}

			} else {
			
			
			}*/
		}

	}

	void place(List<int> sortedAspectList, Rect area){
		float tempScale;
		float tempScale2;
		int tempIndex;
		int tempIndex2;
		//float totalHeight =0;

		while(sortedAspectList.Count > 0){
			if (area.width / area.height > 1.5) { // area wide
				tempIndex = sortedAspectList[sortedAspectList.Count-1];
				if(aspectRatios[tempIndex] <= 0.5){
					tempScale = area.height / rectTransforms[tempIndex].rect.height;
					targetPositions[tempIndex] = new Vector3(area.x, area.y,0);
					targetScales[tempIndex] = tempScale;
					area.x += rectTransforms[tempIndex].rect.width*tempScale+padding;
					area.width -= rectTransforms[tempIndex].rect.width*tempScale+padding;
					sortedAspectList.RemoveAt(sortedAspectList.Count-1);

					//place(sortedAspectList, area);
				} else {
					if(sortedAspectList.Count > 1 && aspectRatios[sortedAspectList[0]] < 3){

						tempIndex = sortedAspectList[0];
						tempIndex2 = sortedAspectList[sortedAspectList.Count-1];

						tempScale2 = rectTransforms[tempIndex].rect.height / rectTransforms[tempIndex2].rect.height;
						tempScale = (area.width-padding) / (rectTransforms[tempIndex].rect.width + rectTransforms[tempIndex2].rect.width*tempScale2);

						area.height -= rectTransforms[tempIndex].rect.height*tempScale+padding;
						targetPositions[tempIndex] = new Vector3(area.x, area.y+area.height+padding,0);
						targetPositions[tempIndex2] = new Vector3(area.x+rectTransforms[tempIndex].rect.width*tempScale+padding, area.y+area.height+padding,0);
						targetScales[tempIndex] = tempScale;
						targetScales[tempIndex2] = tempScale*tempScale2;
						//area.x += rectTransforms[tempIndex].rect.width*tempScale;
						//area.width -= rectTransforms[tempIndex].rect.width*tempScale;

						sortedAspectList.RemoveAt(sortedAspectList.Count-1);
						sortedAspectList.RemoveAt(0);
					} else {
						tempIndex = sortedAspectList[0];

						tempScale = calculateLowerScale(rectTransforms[tempIndex].rect.width, rectTransforms[tempIndex].rect.height, area.width, area.height);//area.width / rectTransforms[tempIndex].rect.width;
						
						area.height -= rectTransforms[tempIndex].rect.height*tempScale+padding;
						targetPositions[tempIndex] = new Vector3(area.x+area.width/2-rectTransforms[tempIndex].rect.width*tempScale/2, area.y+area.height+padding,0);
						targetScales[tempIndex] = tempScale;
						//area.x += rectTransforms[tempIndex].rect.width*tempScale;
						//area.width -= rectTransforms[tempIndex].rect.width*tempScale;

						sortedAspectList.RemoveAt(0);
					}
				}

			} else { // area square to narrow
				tempIndex = sortedAspectList[0];
				
				tempScale =  calculateLowerScale(rectTransforms[tempIndex].rect.width, rectTransforms[tempIndex].rect.height, area.width, area.height);//area.width / rectTransforms[tempIndex].rect.width;
				
				area.height -= rectTransforms[tempIndex].rect.height*tempScale+padding;
				targetPositions[tempIndex] = new Vector3(area.x+area.width/2-rectTransforms[tempIndex].rect.width*tempScale/2, area.y+area.height+padding,0);
				targetScales[tempIndex] = tempScale;
				//area.x += rectTransforms[tempIndex].rect.width*tempScale;
				//area.width -= rectTransforms[tempIndex].rect.width*tempScale;
				
				sortedAspectList.RemoveAt(0);
			}
		}
	}
	/*
	void place(int index_1, Rect area, int index_2=-1){

	}*/

	float calculateLowerScale(float w, float h, float W, float H){
		return Mathf.Min(W/w,H/h);
	}
}
