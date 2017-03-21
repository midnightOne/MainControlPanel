using UnityEngine;
using System.Collections;

public class ScrollingUVs : MonoBehaviour 
{
	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );
	public string textureName = "_MainTex";
	public Renderer rend;
	
	Vector2 uvOffset = Vector2.zero;

	void Start() {
		rend = GetComponent<Renderer>();
	}

	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( rend.enabled )
		{
			rend.materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}
	}
}