using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float joyX;
		joyX=Input.GetAxis ("Horizontal");
		transform.Translate (joyX,0,0);
	}

	void OnGUI ()
	{
		GUI.Label (new Rect (10,10,100,100),Input.GetAxis ("Horizontal").ToString ());
	}
}
