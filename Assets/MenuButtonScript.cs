using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	public Texture tex1;
	public Texture tex2;
	private bool isHighlighted=false;
	public bool hitByRay=false;

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture=tex1;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (hitByRay)
		{
			if (!isHighlighted)
			{
				renderer.material.mainTexture=tex2;
				isHighlighted=true;
			}
		}
		else
		{
			if (isHighlighted)
			{
				renderer.material.mainTexture=tex1;
				isHighlighted=false;
			}
		}
		hitByRay=false;
	}
}
