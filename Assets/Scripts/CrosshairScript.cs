using UnityEngine;
using System.Collections;

public class CrosshairScript : MonoBehaviour {

	public Transform camera;
	public Transform crosshair;

	// Use this for initialization
	void Start () {
		if (camera==null) camera=Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction;
		direction=camera.TransformDirection (Vector3.forward);
		crosshair.position=camera.TransformDirection (Vector3.forward*100);

		RaycastHit hit;
		if (Physics.Raycast (camera.position,direction,out hit,100))
		{
			crosshair.position=hit.point;
			Vector3 vec;
			crosshair.rotation=Quaternion.LookRotation (hit.point-camera.position,Vector3.up);
		}
	}
}
