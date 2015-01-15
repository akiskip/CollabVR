using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	if (Input.GetKeyDown (KeyCode.Escape))
        {
            Application.Quit ();
        }
	}
}
