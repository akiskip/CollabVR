using UnityEngine;
using System.Collections;

public class C_ViewObjects : MonoBehaviour
{

    public Transform camera;
    public Transform m_edit;
    public Transform m_main;
    private float focusTime = 0;
    private Transform newTarget;
    private Transform oldTarget;
    private int layer;

    // Use this for initialization
    void Start ()
    {
        if (camera == null)
            camera = Camera.main.transform;
    }
  
    // Update is called once per frame
    void Update ()
    {
        Vector3 direction;
        direction = camera.TransformDirection (Vector3.forward);

//    if (Input.GetKeyDown (KeyCode.Return))
//    {
//      if (focusTime<0.5f)
//      {
//        Instantiate (m_main,Vector3.zero,Quaternion.identity);
//        Destroy (gameObject);
//      }
//    }

        RaycastHit hit;
        oldTarget = newTarget;
        newTarget = null;

        if (Physics.Raycast (camera.position, direction, out hit, 100)) {

            if (Input.GetKeyDown (KeyCode.Return)) {
                Debug.Log ("Hit " + hit.transform.gameObject.tag + "");
            }

            if (hit.transform.gameObject.tag == "object") {
                newTarget = hit.transform;
            }
        }

        if (oldTarget != null && newTarget != null) {
            if (oldTarget == newTarget) {
                //focusTime += Time.deltaTime;
                //if (focusTime > 0.5f) {
                    focusTime = 0.5f;
                    newTarget.SendMessage ("Highlight", SendMessageOptions.DontRequireReceiver);
                    //Menu
                    if (Input.GetKeyDown (KeyCode.Return)) {
                        //Menü erzeugen
                        Transform m;
                        m = Instantiate (m_edit, Vector3.zero, Quaternion.identity) as Transform;
                        m.GetComponent<M_Edit> ().target = newTarget;
                        Destroy (gameObject);
                    }
                //}
            } else {
                //focusTime = 0;
                oldTarget.SendMessage ("Unhighlight", SendMessageOptions.DontRequireReceiver);
            }
        } else if (oldTarget != null) {
            //focusTime = 0;
            oldTarget.SendMessage ("Unhighlight", SendMessageOptions.DontRequireReceiver);
        }
    }
}
