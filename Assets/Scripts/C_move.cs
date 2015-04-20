using UnityEngine;
using System.Collections;

/*

Code for camera movement within the unity scene and the information is sent to KodematData

*/
namespace UnitySocketClient
{
    public class C_move : MonoBehaviour
    {

        public Transform camera;
        public Transform target;
        public Transform ghost;
        public Transform c_view_objects;
        //private float yScale;
        private int layer;
        public KodematData kodematData;
        // Use this for initialization
        void Start ()
        {
            if (camera == null)
                this.camera = Camera.main.transform;
            //yScale = target.collider.bounds.size.y / 2;
        }
  
        // Update is called once per frame
        void Update ()
        {
            layer = 1 << 8;
            layer = ~layer;
            Vector3 direction = camera.TransformDirection (Vector3.forward);
            RaycastHit hit;
            Vector3 targetPosition = ghost.position;
            if (Physics.Raycast (camera.position, direction, out hit, 100)) {
                targetPosition = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
            }

            ghost.position = Vector3.Slerp (ghost.position, targetPosition, 0.3f);

            if (Input.GetKeyDown (KeyCode.Return)) {
                Instantiate (c_view_objects, Vector3.zero, Quaternion.identity);
                //target.SendMessage ("Unhighlight",SendMessageOptions.DontRequireReceiver);

                //Send
                kodematData.SendTranslation (targetPosition);

                Destroy (ghost.gameObject);
                Destroy (gameObject);
            }
        }
    }
}
