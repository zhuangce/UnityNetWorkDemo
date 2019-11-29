using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

public class TCPServer : MonoBehaviour
{
    private static TCPServer instance;
    public static TCPServer Instance
    {

        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TCPServer>();
            }
            return instance;
        }
    }
    public int port = 20084;
    public string Message
    {
        get { return message; }
    }

    public string serverIP = "192.168.1.112";
    private string message;

    private int length;

    private EndPoint endp;
    private Socket tcpServer;
    private void Start()
    {
       // serverIP =
      //  Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault<IPAddress>(a => a.AddressFamily.ToString().Equals("InterNetwork")).ToString();
        //Debug.Log("localip" + serverIP);
        StartServer();
    }
    private void StartServer()
    {
        tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse(serverIP);
        EndPoint ep = new IPEndPoint(ip, port);
        tcpServer.Bind(ep);
        //接受数据
        tcpServer.Listen(100);

        Socket client = tcpServer.Accept();
        //endp = new IPEndPoint(IPAddress.Any, port);
        //发消息
        message = "这是服务器";
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.Send(data);

        while (true)
        {
            length = client.Receive(data);
            message = Encoding.UTF8.GetString(data, 0, length);
            Debug.Log("客户端" + message);
        }

    }



}
