using UnityEngine;
using System.Collections;

public class RaycastScript : MonoBehaviour {

	public GameObject testBlock;
	public GameObject cam1;
	public GameObject cam2;
	private float diff;
	public RaycastHit target;
	// Use this for initialization
	void Start () {
		diff=cam1.transform.position.x-cam2.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Vector3 fwd = this.transform.TransformDirection(Vector3.forward);
		if (Physics.Raycast (new Vector3(transform.position.x+diff,transform.position.y,transform.position.z),fwd,out hit))
		{
			target=hit;
			if (hit.transform.tag=="menu")
				hit.transform.GetComponent<MenuButtonScript>().hitByRay=true;

		}
	}
}
