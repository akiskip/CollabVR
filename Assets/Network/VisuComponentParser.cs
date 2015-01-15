using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnitySocketClient
{
    public class VisuComponentParser
    {

        public ComponentManager componentManager;
        public Transform testObject;
        private int id = -1;
        private String translationMsg = String.Empty;
        private String rotationMsg = String.Empty;
        private String scaleMsg = String.Empty;
        private String parentMsg = String.Empty;
        private String typeMsg = String.Empty;
        private String createMsg = String.Empty;
        private float[] translation = new float[3];
        private float[] rotation = new float[3];
        private float[] scale = new float[3];
        private string modelPath = String.Empty;
        private String type = String.Empty;
        private String name = String.Empty;
        private int marking = -1;
        private string label = String.Empty;
        private int parentId = -1;
        private GameObject instance = null;
        //public static Dictionary<int, String> attributesMap = new Dictionary<int,string>();
        private Boolean isComplete = false;
        private string[] subStringArray;
        public Boolean deleted = false;
        private KodematData kodematData = null;

        void Start ()
        {
            
        }

        public void processMsg (String msg)
        {
            if (deleted)
                return;

            subStringArray = msg.Split (' ');
            if (id < 0) {
                id = int.Parse (subStringArray [1]);
            }

            string changeType = subStringArray [2];

            switch (changeType) {
            case "TRANSLATION":
                {
                    translationMsg = msg;
                    //attributesMap[0] = translationMsg ;
                    translation [0] = float.Parse (subStringArray [4]);
                    translation [1] = float.Parse (subStringArray [6]);
                    translation [2] = float.Parse (subStringArray [8]);
                    //Console.WriteLine("translation is x" + translationMsg[0] + " y" + translationMsg[1] + " z" + translationMsg[2]);
                    //Debug.Log("ID " + id + "translation is x " + translation[0] + " y " + translation[1] + " z " + translation[2]);
                    if (instance != null) {
                        instance.transform.position = new Vector3 (-translation [0], translation [1], translation [2]);
                    }
                    break;
                }
            case "ROTATION":
                {
                    rotationMsg = msg;
                    //attributesMap[1] = rotationMsg;
                    rotation [0] = float.Parse (subStringArray [4]);
                    rotation [1] = float.Parse (subStringArray [6]);
                    rotation [2] = float.Parse (subStringArray [8]);

                    if (instance != null) {
                        instance.transform.eulerAngles = new Vector3 (rotation [0] - 90, -rotation [1], rotation [2]);
                    }
                    break;
                }
            case "SCALE":
                {
                    scaleMsg = msg;
                    //attributesMap[2] = scaleMsg;
                    scale [0] = float.Parse (subStringArray [4]);
                    scale [1] = float.Parse (subStringArray [6]);
                    scale [2] = float.Parse (subStringArray [8]);
//          if (instance != null) {
//            instance.transform.localScale = new Vector3 (scale [0], scale [1], scale [2]);
//          }

                    break;
                }
            case "TYPE":
                {
                    try {
                        typeMsg = msg;
                        //attributesMap[3] = modelMsg;

                        modelPath = subStringArray [6];
                        modelPath = modelPath.Substring (0, modelPath.Length - 4);

                        type = subStringArray [4];

                
                        Debug.Log ("ID " + id + " type '" + type + "' model " + modelPath);
                    } catch (Exception e) {
                        Debug.Log ("{0} Exception caught " + e);
                    }
                    break;
                }
            case "PARENT":
                {
                    //not necessary to create
                    parentMsg = msg;

                    parentId = int.Parse (subStringArray [4]);
                    break;
                }
            case "CREATE":
                {
                    try {
                        // Debug.Log("show msg " + msg + " name" + subStringArray[4]);
                        createMsg = msg;
                        //attributesMap[4] = modelMsg;
                        name = subStringArray [4];
                    } catch (Exception e) {
                        Debug.Log ("{0} Exception caught " + e);
                    }

                    break;
                }
            case "DELETE":
                {
                    try {
                        this.Dispose ();

                    } catch (Exception e) {
                        Debug.Log ("{0} Exception caught " + e);
                    }
                
                    break;
                }
            case "MARKING":
                {
                    try {
                        if (kodematData != null) {
                            kodematData.SetMarking (int.Parse (subStringArray [3]));
                            string tmp = "";
                            for (int i = 5; i < subStringArray.Length; i++) {
                                tmp += " " + subStringArray [i];
  
                            }
                            tmp = tmp.Trim ();
                            kodematData.SetLabel (tmp);
                        } else {
                            this.marking = int.Parse (subStringArray [3]);
                            string tmp = "";
                            for (int i = 5; i < subStringArray.Length; i++) {
                                tmp += " " + subStringArray [i];
                            
                            }
                            tmp = tmp.Trim ();
                            this.label = tmp;
                        }
                    
                    } catch (Exception e) {
                        Debug.Log ("{0} Exception caught " + e);
                    }
                
                    break;
                }
            //default : {} break;


            }

            
            isComplete = checkComplete ();
            if (isComplete && instance == null) {            //also test if there is already an object with that id or name in scenegraph
                //create new component
                Vector3 v = new Vector3 (-translation [0], translation [1], translation [2]);

                //Debug.Log("new object with id " + id + " of type " + type + " will be created with " + v);
                CreateObject (id, name, v);
            }

        }

        public Boolean checkComplete ()
        {
            if (String.IsNullOrEmpty (translationMsg)) {
                return false;
            }
            //if (String.IsNullOrEmpty(rotationMsg))
            //{
            //    return false;
            //}
            //if (String.IsNullOrEmpty(scaleMsg))
            //{
            //    return false;
            //}
            if (String.IsNullOrEmpty (typeMsg)) {
                return false;
            }
            if (String.IsNullOrEmpty (createMsg)) {
                return false;
            }
            return true;
        }

        public void CreateObject (int id, string name, Vector3 pos)
        {





            if (!String.IsNullOrEmpty (modelPath)) {

                GameObject resource = null;
                componentManager.resopurceCache.TryGetValue (modelPath, out resource);
                if (resource == null) {
                    resource = Resources.Load (modelPath, typeof(GameObject)) as GameObject;
                    componentManager.resopurceCache.Add (modelPath, resource);
                } else {
                    Debug.Log ("GameObject found in cache");
                }

                Quaternion rot = Quaternion.identity;
                //rot.eulerAngles = new Vector3 (-90, 0, 90);
                rot.eulerAngles = new Vector3 (rotation [0] - 90, rotation [1], rotation [2]);
                Debug.Log ("Trying to load model: " + modelPath);
                instance = GameObject.Instantiate (resource, pos, rot) as GameObject;
            } else {
                Debug.Log ("modelPath is null or empty");
            }
            Transform t = instance.transform;//GameObject.Instantiate(testObject, pos, Quaternion.identity) as Transform;


            //Alle Objekte unsichtbar machen
            //instance.SetActive (false);


            t.parent = GameObject.Find ("/SceneObjects").transform;


            foreach (Transform child in t.GetComponentsInChildren<Transform>()) {
                if (!modelPath.Contains ("boden")) {
                    child.gameObject.tag = "object";
                }
                child.gameObject.layer = 8;
                if (child.name.Contains ("stapler")) {
                    MeshRenderer mr = child.gameObject.GetComponent<MeshRenderer> ();
                    if (mr != null) {
                        child.gameObject.AddComponent<BoxCollider> ();
                    }
                }

            }
                
            if (!modelPath.Contains ("boden")) {
                instance.tag = "object"; 
            }
            instance.layer = 8;
            if (!String.IsNullOrEmpty (name)) {
                t.name = name;
            }

            //instance.AddComponent<Fade> ();
            kodematData = instance.AddComponent <KodematData> ();
            kodematData.id = id;
            kodematData.connection = componentManager.connection;
            if (marking >= 0) {
                kodematData.SetMarking (marking);
            }
            if (!String.IsNullOrEmpty (label)) {
                kodematData.SetLabel (label);
            }
        }
    
        public void Dispose ()
        {

            deleted = true;
            if (instance != null) {
                GameObject.Destroy (instance);
            }
    
        }
    }
}