using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	public GameObject menuItem;
	public GameObject camera;
	public GameObject importModel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space))
		{
			menuItem.SetActive (true);
			AlignMenu ();
		}
		else if (Input.GetKeyUp (KeyCode.Space))
		{
			menuItem.SetActive (false);
			foreach (Transform child in menuItem.transform)
			{
				if (child.GetComponent<MenuButtonScript>().hitByRay) Instantiate (importModel,Vector3.zero,Quaternion.identity);
			}

		}
	}

	void AlignMenu ()
	{
		menuItem.transform.position=camera.transform.position+camera.transform.TransformDirection(Vector3.forward);
		menuItem.transform.rotation=camera.transform.rotation;
		menuItem.transform.Rotate (-90,0,0);
		menuItem.transform.Rotate (0,180,0);
	}
}
