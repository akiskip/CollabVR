using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public float moveSpeed=0.25f;
	public GameObject HUDMenu;
	public Texture crosshair;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast (transform.position,transform.TransformDirection (Vector3.forward),out hit))
		{
			if (hit.collider.tag=="HUD")
			{
				Debug.Log ("Hit");
				hit.transform.GetComponent<HUDscript>().isHighlighted=true;
			}
		}
	if (Input.GetKey (KeyCode.W))
		{
			transform.Translate (Vector3.forward*moveSpeed);
		}
	else if (Input.GetKey (KeyCode.S))
		{
			transform.Translate (-Vector3.forward*moveSpeed);
		}

	if (Input.GetKey (KeyCode.A))
		{
			transform.Translate (Vector3.left*moveSpeed);
		}
	else if (Input.GetKey (KeyCode.D))
		{
			transform.Translate (-Vector3.left*moveSpeed);
		}
	}

	void OnGUI ()
	{
		GUI.DrawTexture (new Rect(Screen.width/2-10,Screen.height/2-10,20,20),crosshair);
	}
}
