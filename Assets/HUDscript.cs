using UnityEngine;
using System.Collections;

public class HUDscript : MonoBehaviour {

	public bool isHighlighted=false;

	void LateUpdate ()
	{
		if (isHighlighted)
		{
			renderer.material.color=new Color(1,1,1,0.8f);
		}
		else renderer.material.color=new Color(0.5f,0.5f,0.5f,0.5f);
		isHighlighted=false;
	}
}
