using UnityEngine;
using System.Collections;

public class HUDControllerScript : MonoBehaviour {
	public GameObject importModel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetKeyDown (KeyCode.Space))
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive (true);
			}
		}
	else if (Input.GetKeyUp (KeyCode.Space))
		{
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive (false);
				//Instantiate (importModel,Vector3.zero,Quaternion.identity);
			}
		}
	}
}
