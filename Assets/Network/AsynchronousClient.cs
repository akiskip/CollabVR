using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

// State object for receiving data from remote device.

namespace UnitySocketClient
{
    public class StateObject
    {


        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder ();


    }

    public class AsynchronousClient : MonoBehaviour
    {
        public ComponentManager componentManager;
        public  bool firstRead;
        private string ipAddress = "127.0.0.1";
        // The port number for the remote device.
        private int port = 6104;
        public string userName = "Oculus";

        // ManualResetEvent instances signal completion.
        private  ManualResetEvent connectDone =
            new ManualResetEvent (false);
        private  ManualResetEvent sendDone =
            new ManualResetEvent (false);
        private  ManualResetEvent receiveDone =
            new ManualResetEvent (false);

        // The response from the remote device.
        private  String response = String.Empty;
        private  Socket client;
        private string textfieldcontent = "ID";

        private int old_marking_id = -1;


        void Start ()
        {
            componentManager = GetComponent ("ComponentManager") as ComponentManager;
            readPropertiesFile ();
            Thread newThread = new Thread (new ThreadStart (StartConnection));
            newThread.Start ();
        }
    
        void OnApplicationQuit ()
        {
            shutdown ();
        }

        //Start
        void OnGUI ()
        {
      
            textfieldcontent = GUI.TextField (new Rect (10, 50, 150, 30), textfieldcontent);
      
            //Start
            if (GUI.Button (new Rect (10, 10, 200, 30), "Send Delete")) {
                Debug.Log ("tetxtfield" + textfieldcontent);
                SendMarking (int.Parse (textfieldcontent), true, "OCULUS");
            }
            //Close

        }

        public void StartConnection ()
        {
            StartClient ();
        }

        public void StartClient ()
        {
            Debug.Log ("Start Connection to KoDeMat Server");
            // Connect to a remote device.
            try {
                // Establish the remote endpoint for the socket.
                // The name of the 
                // remote device is "host.contoso.com".
                //  IPHostEntry ipHostInfo = Dns.Resolve("host.contoso.com");
                IPAddress ipAddressObj = IPAddress.Parse (ipAddress.Trim());
                IPEndPoint remoteEP = new IPEndPoint (ipAddressObj, port);


                while (client == null || !client.Connected) {
          
                    if (client != null && !client.Connected) {
                        client.Close ();
                        Debug.Log ("Connection Failed, will retry...");
                        connectDone.WaitOne (1000);
                    }
                    
                    // Create a TCP/IP socket.
                    client = new Socket (AddressFamily.InterNetwork,
                   SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.
                    client.BeginConnect (remoteEP,
                    new AsyncCallback (ConnectCallback), client);
                    connectDone.WaitOne (2000);

                }

                // Send test data to the remote device.
                Debug.Log ("Connection successful");
                Send (client, "First message from Unity client<EOF>");
                sendDone.WaitOne ();

                // Receive the response from the remote device.
                Receive (client);
                receiveDone.WaitOne ();

                // Write the response to the console.
                Console.WriteLine ("Response received : {0}", response);
                Debug.Log (response);



            } catch (Exception e) {
                Debug.Log (e.ToString ());
                Console.WriteLine (e.ToString ());
            }
        }

        public  void shutdown ()
        {
            // Release the socket.
            client.Shutdown (SocketShutdown.Both);
            client.Close ();
        }

        public Socket getClient ()
        {
            return client;
        }

        private  void ConnectCallback (IAsyncResult ar)
        {
            try {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect (ar);

                Console.WriteLine ("Socket connected to {0}",
                    client.RemoteEndPoint.ToString ());

                // Signal that the connection has been made.
                connectDone.Set ();
            } catch (Exception e) {
                Console.WriteLine (e.ToString ());
            }
        }

        private  void Receive (Socket client)
        {
            try {
                // Create the state object.
                StateObject state = new StateObject ();
                state.workSocket = client;

                firstRead = true;

                // Begin receiving the data from the remote device.
                client.BeginReceive (state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback (ReadCallback), state);
            } catch (Exception e) {
                Console.WriteLine (e.ToString ());
            }
        }

        public  void ReadCallback (IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive (ar);

            if (bytesRead > 0) {
                // There  might be more data, so store the data received so far.
                state.sb.Append (Encoding.UTF8.GetString (
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString ();
                int eofindex = content.IndexOf ("<EOF>");
                while (eofindex > -1) {
                    // All the data has been read from the 
                    // client. Display it on the console. Add here the handler
                    Console.WriteLine ("Received Message: {0}",
                        content.Substring (0, eofindex));
                    //  Debug.Log(content.Substring(0, eofindex));

                    String eventReceived = content.Substring (0, eofindex);

                    componentManager.handleNetworkEvent (eventReceived);

                    state.sb.Remove (0, eofindex + 5);
                    content = state.sb.ToString ();
                    eofindex = content.IndexOf ("<EOF>");
                }
                state.buffer = new byte[StateObject.BufferSize];
                handler.BeginReceive (state.buffer, 0, StateObject.BufferSize, 0,
                   new AsyncCallback (ReadCallback), state);
            }
        }

        public  void Send (Socket client, String data)
        {

            // Convert the string data to byte data using ASCII encoding.
            byte[] tmpByteData = Encoding.UTF8.GetBytes (data);

            int msgSize = tmpByteData.Length;

            byte size = (byte)msgSize;

            byte[] byteData = new byte[msgSize + 1];
            byteData [0] = size;
            tmpByteData.CopyTo (byteData, 1);


            // Begin sending the data to the remote device.
            client.BeginSend (byteData, 0, byteData.Length, 0,
                new AsyncCallback (SendCallback), client);
        }

        public  void Send (String data)
        {


            // Convert the string data to byte data using ASCII encoding.
            byte[] tmpByteData = Encoding.UTF8.GetBytes (data);

            int msgSize = tmpByteData.Length;



            byte size = (byte)msgSize;

            byte[] byteData = new byte[msgSize + 1];
            byteData [0] = size;
            tmpByteData.CopyTo (byteData, 1);


            // Begin sending the data to the remote device.
            client.BeginSend (byteData, 0, byteData.Length, 0,
                new AsyncCallback (SendCallback), client);

        }

        private  void SendCallback (IAsyncResult ar)
        {
            try {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend (ar);
                Console.WriteLine ("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set ();
            } catch (Exception e) {
                Console.WriteLine (e.ToString ());
            }
        }

        public void SendDelete (int id)
        {
            Send ("ID " + id + " DELETE");
        }

        public void SendTranslation (int id, Vector3 pos)
        {
            Send ("ID " + id + " TRANSLATION X " + -pos.x + " Y " + pos.y + " Z " + pos.z);
        }

        public void SendRotation (int id, Vector3 pos)
        {
            Send ("ID " + id + " ROTATION X " + pos.x + " Y " + pos.y + " Z " + pos.z);
        }

        public void SendMarking (int id, Boolean marking, string label)
        {
            if (old_marking_id >= 0) {
                Send ("ID " + old_marking_id + " MARKING " + 0 + " LABEL EMPTY");
            }
            old_marking_id = id;
            Send ("ID " + id + " MARKING " + (marking ? 1 : 0) + " LABEL " + label);
        }
        //public static int Main(String[] args)
        //{
        //    StartClient();
        //    return 0;
        //}


        public void readPropertiesFile(){
            FileInfo theSourceFile = null;
            StreamReader reader = null; 
            string txt = null;

            Debug.Log ("Trying to read properties from " + Application.dataPath + "/properties.txt");
            
            theSourceFile = new FileInfo (Application.dataPath + "/properties.txt");
            if (theSourceFile != null && theSourceFile.Exists) {
                reader = theSourceFile.OpenText ();
            } else {
                return;
            }
            
            if ( reader == null )
            {
                textfieldcontent = "before";
                Debug.Log("properties.txt not found or not readable");
            }
            else
            {
                // Read each line from the file
                string[] propertiesArray = new string[3];
                int i = 0;
                while ( ( txt = reader.ReadLine()) != null )
                    //        GUI.Label(new Rect(10f,10f,10f,50f),"txt");
                {     
                    if(i < propertiesArray.Length){
                        
                        string[] substrings =txt.Split('=');
                        propertiesArray[i] = substrings[1];
                        textfieldcontent = propertiesArray[i];
                        i++;
                    }                
                    Debug.Log("-->" + txt);
                }
                ipAddress = propertiesArray[0];
                port = int.Parse(propertiesArray[1]);
                userName = propertiesArray[2];
            }
            
        }

    }
}