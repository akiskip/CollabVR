using System;
using UnityEngine;
using System.Collections;

namespace UnitySocketClient
{
    public class KodematData : MonoBehaviour
    {

        public int id;
        public AsynchronousClient connection;
        public Boolean marking = false;
        public string label = "";

        GameObject labelObject = null;


        public void SendDelete ()
        {
            connection.SendDelete (id);
        }
        
        public void SendTranslation (Vector3 pos)
        {
            connection.SendTranslation (id, pos);
        }
        
        public void SendRotation (Vector3 pos)
        {
            connection.SendRotation (id, pos);
        }
 
        public void SendMarking(Boolean mval, string lval){
            connection.SendMarking (id, mval, lval);
        }

        public void SetMarking(int value){
            if (value < 1) {
                marking = false;
            } else {
                marking = true;
            }
        }

        public void SetLabel(string value){

            Debug.Log ("Set Label " + value + " for id " + id);
            label = value;

            if (labelObject != null) {
                if(string.IsNullOrEmpty(value)){
                    GameObject.Destroy(labelObject);
                    labelObject = null;
                } else {
                    labelObject.GetComponent<TextMesh>().text = value;
                }
            } else {
                if(!string.IsNullOrEmpty(value)){
                    Debug.Log ("Create Label for " + gameObject.name);
                    labelObject = GameObject.Instantiate(Resources.Load("Label"),new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
                    labelObject.GetComponent<TextMesh>().text = value;
                    labelObject.transform.parent = transform;
                    labelObject.transform.localPosition = new Vector3(0,2,0);
                }
            }


        }
    }
}
