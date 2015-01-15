using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour {

	public float rotationSpeed=4;
	public float moveSpeed=0.2f;
    public Transform skybox;
    private CharacterController characterController;
	// Use this for initialization
	void Start () {
		SpaceNavigator.SetTranslationSensitivity(moveSpeed);
		SpaceNavigator.SetRotationSensitivity(rotationSpeed);
        characterController = transform.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up,SpaceNavigator.Rotation.Yaw(),Space.Self);
		//transform.Translate(SpaceNavigator.Translation,Space.Self);
        characterController.Move (transform.TransformDirection (SpaceNavigator.Translation));
        skybox.transform.position = new Vector3 (transform.position.x, transform.position.y-100, transform.position.z);
	}
}
