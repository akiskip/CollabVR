using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnitySocketClient
{

    public class ComponentManager : MonoBehaviour
    {

        public Transform testObject;

        public Queue<string> messageQueue = new Queue<string>();

        public Dictionary<int, VisuComponentParser> componentsDict = new Dictionary<int, VisuComponentParser>();
        public Dictionary<string, GameObject> resopurceCache = new Dictionary<string, GameObject>();
        public AsynchronousClient connection;

        void Start(){
            connection = GetComponent<AsynchronousClient> ();
        }


        public void handleNetworkEvent(String eventMsg)
        {
            messageQueue.Enqueue(eventMsg);
        }

        public void handleMessage(String eventMsg)
        {
            string[] subStringArray = eventMsg.Split(' ');
            int id = int.Parse(subStringArray[1]);

            try
            {
                if (componentsDict.ContainsKey(id))
                {
                    VisuComponentParser comp = componentsDict[id];
                    comp.processMsg(eventMsg);
                }
                else
                {
                    //    Debug.Log("Before create " + id);
                    componentsDict[id] = new VisuComponentParser();
                    //  Debug.Log("Before get " + id);
                    VisuComponentParser comp = componentsDict[id];
                    comp.componentManager = this;
                    comp.testObject = this.testObject;
                    comp.processMsg(eventMsg);
                    // Debug.Log("After get " + id);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }

 

        // Update is called once per frame
        void Update()
        {
            if (messageQueue.Count > 0)
            {
                string msg = messageQueue.Dequeue();
                handleMessage(msg);
            }
        }
    }

}
