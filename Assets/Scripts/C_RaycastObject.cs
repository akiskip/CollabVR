using UnityEngine;
using System.Collections;

public class C_RaycastObject : MonoBehaviour {

	private Color color;

	// Use this for initialization
	void Start () {
		color=renderer.material.color;
	}

	void Highlight ()
	{
		renderer.material.color=new Color(color.r+0.5f,color.g+0.5f,color.b+0.5f);
	}

	void Unhighlight ()
	{
		renderer.material.color=color;
	}
}
