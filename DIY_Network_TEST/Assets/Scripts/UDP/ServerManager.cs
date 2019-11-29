using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

public  class ServerManager :MonoBehaviour
{
    private static ServerManager instance;
    public static ServerManager Instance {

        get
        {
            if (instance == null)
            {
                instance =FindObjectOfType<ServerManager>();
            }
            return instance;
        }
    }

    public int port = 20084;
    public string serverIP = "192.168.0.132";
    private string message;
    public string Message
    {
        get { return message; }
    }
    private int length;
    private byte[] data = new byte[1024];
    private EndPoint endp;
    private Socket udpServer;
    private void Start()
    {
        //serverIP =
       // Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault<IPAddress>(a => a.AddressFamily.ToString().Equals("InterNetwork")).ToString();
      //  Debug.Log("localip" + serverIP);
        StartServer();
    }
    public void StartServer()
    {
        udpServer = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
        IPAddress ip = IPAddress.Parse(serverIP);
        EndPoint ep = new IPEndPoint(ip,port);
        udpServer.Bind(ep);
        //接受数据
         endp = new IPEndPoint(IPAddress.Any,port);
        
       
        length = 0;

        //while (true)
        //{
        //    length = udpServer.ReceiveFrom(data,ref endp);
        //    message = Encoding.UTF8.GetString(data,0,length);
        //    Debug.Log("从IP："+(endp as IPEndPoint).Address+"取得消息"+message);
        //}
    }
    private void Update()
    {
        UpdateMessage();
    }

    public void UpdateMessage()
    {
        length = udpServer.ReceiveFrom(data, ref endp);
        message = Encoding.UTF8.GetString(data, 0, length);
        Debug.Log("从IP：" + (endp as IPEndPoint).Address + "取得消息" + message);
    }

    public Vector3 ReceiveMSG()
    {
        if (message == "") return Vector3.zero;
        string[] str = message.Split('|');
        Vector3 vec = new Vector3(int.Parse( str[0]), int.Parse(str[1]), int.Parse(str[2]));
        return vec;
    }
  

}
