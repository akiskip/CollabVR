using UnityEngine;
using System.Collections;

public class M_Main : MonoBehaviour {
	
	public Transform camera;
	public Transform c_view_objects;
	public Transform target;

	public Texture t_cursor;
	public Texture t_createbox;
	
	public Texture t_cursor_hl;
	public Texture t_createbox_hl;
	
	private Transform buttonCursor;
	private Transform buttonCreatebox;
	
	private int viewIndex=0;
	
	// Use this for initialization
	void Awake () {
		if (camera==null) camera=Camera.main.transform;
		transform.position=camera.position+camera.TransformDirection (Vector3.forward)*3;
		transform.LookAt (camera.position);
		transform.Rotate (0,180,0);
		
		buttonCursor=transform.FindChild ("Orientation/Cursor");
		buttonCreatebox=transform.FindChild ("Orientation/Createbox");
	}
	
	// Update is called once per frame
	void Update () {
		buttonCursor.renderer.material.mainTexture=t_cursor;
		buttonCreatebox.renderer.material.mainTexture=t_createbox;
		viewIndex=0;
		
		Vector3 direction=camera.TransformDirection (Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (camera.position,direction,out hit,10))
		{
			if (hit.transform.tag=="menu")
			{
				if (hit.transform==buttonCursor) {buttonCursor.renderer.material.mainTexture=t_cursor_hl; viewIndex=1;}
				else if (hit.transform==buttonCreatebox) {buttonCreatebox.renderer.material.mainTexture=t_createbox_hl; viewIndex=2;}
			}
		}
		
		//Close Menu
		if (Input.GetKeyDown (KeyCode.Return))
		{
			Instantiate (c_view_objects,Vector3.zero,Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
