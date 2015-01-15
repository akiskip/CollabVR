using UnityEngine;
using System.Collections;

public class LabelScript : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
        target = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt (new Vector3(target.position.x, transform.position.y, target.position.z));
        transform.Rotate (0, 180, 0);
	}
}
