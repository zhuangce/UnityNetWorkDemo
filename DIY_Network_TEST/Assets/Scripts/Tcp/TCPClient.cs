using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

public class TCPClient : MonoBehaviour
{
    private static TCPClient instance;
    public static TCPClient Instance
    {

        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TCPClient>();
            }
            return instance;
        }
    }

    public int port = 20084;
    public string serverIP = "192.168.1.112";

    private Socket tcpclient;
    private IPAddress iP;
    private EndPoint ep;
    private byte[] data = new byte[2048];
    private int length = 0;
    private string message;
    private string allMessage;

  
    private void Start()
    {
        StartClient();
    }
    private void StartClient()
    {
        tcpclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        iP = IPAddress.Parse(serverIP);
        ep = new IPEndPoint(iP, port);
        byte[] data = new byte[1024];
        length = 0;
        tcpclient.Connect(ep);
        length = tcpclient.Receive(data);
        message = Encoding.UTF8.GetString(data,0,length);
        Debug.Log("接受消息"+message);
       

    }

    public void SendMessage( string input, out byte[] data)
    {
        message = input;
        data = Encoding.UTF8.GetBytes(message);
        tcpclient.Send(data);
    }
}
