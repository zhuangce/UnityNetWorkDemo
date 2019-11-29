using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UnityEngine.Events;
using System.Threading;
using System.Net.NetworkInformation;

public class HLTServer :MonoSingleton<HLTServer>
{

    public delegate void ServerHandler();
    public delegate void ServerClientHandler();
    public delegate void ReceiveMsgHandler();

    public ServerHandler OnNewMessageReceived;
    public ServerHandler OnNewMessageSendToAllClient;
    public ServerClientHandler OnNewMessageSendToClient;
    public ServerHandler OnServerClose;
    public MyStringEvent  OnServerStartReady = new MyStringEvent();
    public ServerClientHandler OnClientConnet;
    public ServerClientHandler OnClientDisConnet;
    public ServerHandler OnServerError;

    public string receiveMsg {  get; private set; }

    public string Ip
    {
        get { return ip; }
    }
    private string ip = "192.168.0.132";


    private int port = 10012;

    private Socket _server;

    private Thread _acceptClientConnectThread;

    private List<Socket> _clientList = new List<Socket>();


    #region UNITYFUN
    private void Start()
    {
       // ip = "";
        ip = GetIP();
        SetUp();
    }


    private void Update()
    {
        
    }
    #endregion

    public void SetUp(string ipadd="")
    {
        if (!string.IsNullOrEmpty(ipadd)) ip = ipadd;
        try
        {
            
            _server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            Scenemanager.Instance.console.text += "\n getip:" + ip;
            EndPoint ep = new IPEndPoint(IPAddress.Parse(ip),port);
            Scenemanager.Instance.console.text += "\n  endpointdone";
            _server.Bind(ep);
            
            _server.Listen(50);
            //单线程会阻塞，引入多线程
            //_server.Accept();
            _acceptClientConnectThread = new Thread(AcceptClientConnect);

            _acceptClientConnectThread.Start();
            string log =   string.Format("服务器初始完成;ip :{0} ; port:{1} ; ",ip,port);
            Debug.Log(log);
            
            if (OnServerStartReady != null) OnServerStartReady.Invoke(log);

        }
        catch (System.Exception e)
        {
            Scenemanager.Instance.console.text += "\n"+ e.Message;
            Debug.LogError(e.Message);
            OnServerError.Invoke();

        }
    }

    public void AcceptClientConnect()
    {
        while (true)
        {
            try
            {
                Socket clientsocket = _server.Accept();
                if (clientsocket != null)
                    _clientList.Add(clientsocket);
                IPEndPoint clientendpoint =clientsocket.RemoteEndPoint as IPEndPoint;

                string log = string.Format("客户端信息 ip :{0} ;  port : {1}", clientendpoint.Address.ToString(), clientendpoint.Port);
                //Debug.LogFormat("客户端信息 ip :{0} ;  port : {1}",clientendpoint.Address.ToString(),clientendpoint.Port);
                

                if (OnClientConnet != null) OnClientConnet.Invoke();
                Thread acceptClientMSg = new Thread(AcceptMsg);
                acceptClientMSg.Start(clientsocket);
               
            }
            catch (System.Exception e)  
            {
                Debug.LogError(e.Message);
                if (OnServerError != null) OnServerError();
            }
        }
    }

    public void AcceptMsg(object obj)
    {
        Socket client = obj as Socket;

        byte[] buffer = new byte[client.ReceiveBufferSize];

        IPEndPoint clientendpoint = client.RemoteEndPoint as IPEndPoint;

        try
        {
            while (true)
            {
                int len = client.Receive(buffer);
                string str = Encoding.UTF8.GetString(buffer,0,len);

                string s = string.Format("Receive:ip: {0} ; port:{1} ; MSG: {2}", clientendpoint.Address.ToString(), port, str);
                //Debug.LogFormat("Receive:ip: {0} ; port:{1} ; MSG: {2}",clientendpoint.Address.ToString(),port,str);
               
                if (!string.IsNullOrEmpty(str))
                {
                    Debug.Log(s);
                    Scenemanager.Instance.ShowRecivedMsg(s);
                    SendAll(str);
                    //if (OnNewMessageReceived != null) OnNewMessageReceived.Invoke();
                }
               

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            _clientList.Remove(client);
            if (OnClientDisConnet != null) OnClientDisConnet();
        }
    }


    public void Send(string str ,Socket client)
    {
        try
        {
            byte[] strbyte = Encoding.UTF8.GetBytes(str);
            client.Send(strbyte);
            if (OnNewMessageSendToClient != null) OnNewMessageSendToClient();

        }
        catch (System.Exception e)
        {

            Debug.LogError(e.Message);
            if (OnServerError != null) OnServerError();
        }
    }

    public void SendAll(string str)
    {
       
        for (int i = 0; i < _clientList.Count; i++)
        {
           
            Send(str,_clientList[i]);
           
        }
        if (OnNewMessageSendToAllClient != null) OnNewMessageSendToAllClient();
    }

    public void Close()
    {
        if (_clientList.Count>0)
        {
            for (int i = 0; i < _clientList.Count; i++)
            {
                _clientList[i].Close();
            }
        }

        _clientList.Clear();
        if(_server!=null)
           _server.Close();
         _acceptClientConnectThread.Abort();
         if (OnServerClose != null) OnServerClose();
       
    }

    protected override void OnDestroy()
    {
        Close();
    }

    public string GetIP()
    {
//#if WINDOWS_UWP
        

//#elif UNITY_EDITOR
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adater in adapters)
        {
           
           // foreach (UnicastIPAddressInformation item in adater.GetIPProperties().UnicastAddresses)
           // {
           //    Debug.Log( item.Address.ToString());
           // }
            if (adater.Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection UniCast = adater.GetIPProperties().UnicastAddresses;
                if (UniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in UniCast)
                    {
                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                           // Debug.Log(uni.Address.ToString());
                            return uni.Address.ToString();
                        }
                    }
                }
            }
        }
        return null;

//#endif
    }

    public  string GetLocalIP()
    {
        try
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress item in IpEntry.AddressList)
            {
                //AddressFamily.InterNetwork  ipv4
                //AddressFamily.InterNetworkV6 ipv6
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    return item.ToString();
                }
            }
            return "";
        }
        catch { return ""; }
    }
}
