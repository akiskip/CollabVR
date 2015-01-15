using UnityEngine;
using System.Collections;

public class ImportModelScript : MonoBehaviour {

	public GameObject model;
	public GameObject cam;
	private GameObject instance;

	// Use this for initialization
	void Start () {
		instance=Instantiate (model,Vector3.zero,Quaternion.identity) as GameObject;
		cam=GameObject.Find ("CameraLeft");
	}
	
	// Update is called once per frame
	void Update () {
		instance.transform.position=cam.GetComponent<RaycastScript>().target.point;
		if (Input.GetKey (KeyCode.Return)) Destroy (gameObject);
	}
}
