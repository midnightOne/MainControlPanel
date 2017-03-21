using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class TextureChange : MonoBehaviour {

	public GameObject[] tiles = new GameObject[6];
	public Texture[] myTextures = new Texture[6];
	int maxTextures;
	int arrayPos = 0;
	
	void Start ()
	{
		maxTextures = myTextures.Length-1;
		for(int i =0; i<6;i++){
			changeTexture(i,0);
		}
	}
	

	void Update ()
	{
		int index = -1;

		if (Input.GetKeyDown(KeyCode.Alpha1)){
			index = 0;
		} else if (Input.GetKeyDown(KeyCode.Alpha2)){
			index = 1;
		}else if (Input.GetKeyDown(KeyCode.Alpha3)){
			index = 2;
		}else if (Input.GetKeyDown(KeyCode.Alpha4)){
			index = 3;
		}else if (Input.GetKeyDown(KeyCode.Alpha5)){
			index = 4;
		}else if (Input.GetKeyDown(KeyCode.Alpha6)){
			index = 5;
		}

		if(index > -1){
			//tiles[index].GetComponent<Renderer>().material.mainTexture = myTextures[7];
			//yield return new WaitForSeconds(0.5);
			tiles[index].GetComponent<Renderer>().material.mainTexture = myTextures[arrayPos];
			if(arrayPos == maxTextures){
				arrayPos = 0;
			}else{
				arrayPos++;
			}
		}
	}

	/// <summary>
	/// Changes the texture of fragments.
	/// </summary>
	/// <param name="obj">Fragment index. Zero-based.</param>
	/// <param name="tex">Texture index. 0-no signal</param>

	public void changeTexture(int obj, int tex){

		tiles[obj].GetComponent<Renderer>().material.mainTexture = myTextures[tex];
	}

}


