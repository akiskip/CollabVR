using UnityEngine;
using System.Collections;

public class LightHandler : MonoBehaviour {


	GameObject	camerauser;
	private GameObject lightGameObject2;
	private Light lightComp;

	// Use this for initialization
	void Start () {
	


	
	}
	bool toggle = true;
	// Update is called once per frame
	void Update () {
	

		if (Input.GetKeyDown(KeyCode.R))
		{
			
			lightGameObject2 = GameObject.Find("lightGameObject2");
			
			lightComp = lightGameObject2.light;
			
			if(lightGameObject2 != null)
				Debug.Log("light not null");
			else
				Debug.Log("light null");


			if (lightComp.enabled == true && toggle  == true )
			{
				lightComp.enabled = false;
				toggle = false;
				Debug.Log("light is now false");
			}
			else if (lightComp.enabled == false && toggle  == false )
			{
				lightComp.enabled = true;
				Debug.Log("light is now true");
				toggle = true;
			}

	

}


	}
}
