using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;


public class ClientManager : MonoBehaviour
{
    private static ClientManager instance;
    public static ClientManager Instance
    {

        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ClientManager>();
            }
            return instance;
        }
    }

    public int port = 20084;
    public string serverIP = "192.168.1.112";

    private Socket udpclient;
    private IPAddress iP;
    private EndPoint ep;
    private byte[] data = new byte[2048];
    private int length = 0;
    private string message;
    private string allMessage;

   // public string inputMessage;
   // public string outMessage;



    private void Start()
    {
        StartClient();
    }
    public void StartClient()
    {
        udpclient = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);

        iP = IPAddress.Parse(serverIP);
        ep = new IPEndPoint(iP,port);
    }

    public bool SendMessageToServer(Vector3 pos) {

        float[] posArr = new float[] { pos.x, pos.y, pos.z};
        message = string.Join( "|",posArr);
        Debug.Log(message);
        data = Encoding.UTF8.GetBytes(message);
        udpclient.SendTo(data,ep);
        return true;
    }

}
