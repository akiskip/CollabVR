using UnityEngine;
using System.Collections;
using System.Net;
using UnitySocketClient;

public class M_Edit : MonoBehaviour
{

    public Transform target;
    public Transform camera;
    public Transform c_view_objects;
    public Transform c_move;
    public Texture t_move;
    public Texture t_rotate;
    public Texture t_delete;
    public Texture t_move_hl;
    public Texture t_rotate_hl;
    public Texture t_delete_hl;
    private Transform buttonMove;
    private Transform buttonRotate;
    private Transform buttonDelete;
    private int viewIndex = 0;
    private int layer = 1 << 9;

    // Use this for initialization
    void Awake ()
    {
        if (camera == null)
            camera = Camera.main.transform;
        transform.position = camera.position + camera.TransformDirection (Vector3.forward) * 6;
        transform.LookAt (camera.position);
        transform.Rotate (0, 180, 0);
    
        buttonMove = transform.FindChild ("Orientation/Move");
        buttonRotate = transform.FindChild ("Orientation/Rotate");
        buttonDelete = transform.FindChild ("Orientation/Delete");
    }
  
    // Update is called once per frame
    void Update ()
    {
        buttonMove.renderer.material.mainTexture = t_move;
        buttonRotate.renderer.material.mainTexture = t_rotate;
        buttonDelete.renderer.material.mainTexture = t_delete;
        viewIndex = 0;
    
        Vector3 direction = camera.TransformDirection (Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast (camera.position, direction, out hit, 10, layer)) {
            if (hit.transform.tag == "menu") {
                if (hit.transform == buttonMove) {
                    buttonMove.renderer.material.mainTexture = t_move_hl;
                    viewIndex = 1;
                } else if (hit.transform == buttonRotate) {
                    buttonRotate.renderer.material.mainTexture = t_rotate_hl;
                    viewIndex = 2;
                } else if (hit.transform == buttonDelete) {
                    buttonDelete.renderer.material.mainTexture = t_delete_hl;
                    viewIndex = 3;
                }
            }
        }
    
        //Close Menu
        if (Input.GetKeyDown (KeyCode.Return)) {

            GameObject current = target.gameObject;
            KodematData kd = null;
            while (current != null) {
                kd = current.GetComponent<KodematData> ();
                if (kd != null) {
                    break;
                } else {
                    current = current.transform.parent.gameObject;
                }
            }

            switch (viewIndex) {
            case 0:   //Cancel
                Instantiate (c_view_objects, Vector3.zero, Quaternion.identity);
                break;

            case 1:   //Move
                if (kd != null) {
                    kd.SendMarking(true, "Edited by " + kd.connection.userName);
                    Transform controller;
                    Transform ghost;
                    ghost = Instantiate (current.transform, current.transform.position, current.transform.rotation) as Transform;
                    //ghost.GetComponent<MeshCollider> ().enabled = false;
                    ghost.gameObject.layer = 2;

                    foreach (Transform child in ghost.GetComponentsInChildren<Transform>()) {

                        child.gameObject.layer = 2;
                    }

                    controller = Instantiate (c_move, Vector3.zero, Quaternion.identity) as Transform;
                    controller.GetComponent<C_move> ().target = current.transform;
                    controller.GetComponent<C_move> ().ghost = ghost;
                    controller.GetComponent<C_move> ().kodematData = kd;
                }
                break;

            case 2:
                Instantiate (c_view_objects, Vector3.zero, Quaternion.identity);

                if(kd != null){
                    kd.SendMarking(true, "Edited by "  + kd.connection.userName);
                }

                break;

            //Delete
            case 3:
                Instantiate (c_view_objects, Vector3.zero, Quaternion.identity);
        


                if (kd != null) {
                    kd.SendDelete ();
                }


        //target.SendMessage ("Unhighlight",SendMessageOptions.DontRequireReceiver);
        //Destroy (target.gameObject);
                break;
            }

            Destroy (gameObject);
        }
    }
}
