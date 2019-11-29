using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;

public class HLTClient : MonoSingleton<HLTClient>
{
    public delegate void ClientHandler();
    public ClientHandler OnClientConnect;
    public ClientHandler OnClientDisConnect;
   
    public ClientHandler OnClientError;
    public ClientHandler OnClientMsgSend;

    
    public MyStringEvent OnServerMsgReceive= new MyStringEvent();

    private Socket _client;

    private Thread _acceptServerMsg;

    [Header("由输入获取")]
    private string ip = "192.168.0.123";
    public string Ip
    {
        get { return ip; }
    }
    public int port = 10012;

    public string receiveMsg
    {
        get;private set;
    }

    #region UNITYFUN
    private void Start()
    {
      //  StartUp();
    }
    #endregion
    public void StartUpWithIp(string ipaddress)
    {
        ip = ipaddress;
        StartUp();
    }


    public void StartUp()
    {
        try
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client.Connect(ip, port);
            _acceptServerMsg = new Thread(AcceptServerMsg);
            _acceptServerMsg.Start();
            if (OnClientConnect != null) OnClientConnect.Invoke();
            Scenemanager.Instance.console.text += "\n client" + ip+"connected" ;
        }
        catch (System.Exception e )
        {

            Debug.LogError(e.Message);
        }
    }
    /// <summary>
    /// 检查是否是host
    /// </summary>
    /// <returns></returns>
    public bool CheckIfClientAlsoSever()
    {

        if (_client == null) return false; 
        IPEndPoint remoteend = _client.RemoteEndPoint as IPEndPoint;
        IPEndPoint localend = _client.LocalEndPoint as IPEndPoint;
        Debug.Log( "remoteip "+remoteend.Address.ToString()+"; localip "+ localend.Address.ToString());
        if (remoteend.Address.ToString() == localend.Address.ToString()) {
            return true;
        } else
        {
            return false;
        }
          
        
        
       
       
    }

    public void AcceptServerMsg()
    {
        byte[] buffer = new byte[1024*64];
        while (true)
        {
            try
            {
                int len = _client.Receive(buffer);
                string str =Encoding .UTF8.GetString(buffer,0,len);
               
                //if (!Scenemanager.Instance.isServer)
                //{
                   // Debug.LogFormat("REceive Msg from server {0}", str);
                     string s = string.Format("REceive Msg from server {0}", str);
                     Debug.Log(s);
                     Scenemanager.Instance.ShowRecivedMsg(s);
                    if (OnServerMsgReceive != null) OnServerMsgReceive.Invoke(str);
               // }
               
            }
            catch (System.Exception e )
            {
                Debug.LogError(e.Message);
                if (OnClientError != null) OnClientError.Invoke();
            }
        }


    }


    public void Send(string str )
    {
        try
        {
            byte[] msg = Encoding.UTF8.GetBytes(str);
            _client.Send(msg);
            if (OnClientMsgSend != null) OnClientMsgSend.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            if (OnClientError != null) OnClientError.Invoke();
        }
    }

    public void Close()
    {
        if (_client == null) return;
        if (_client.Connected)
        {
            _client.Close();

        }
        _acceptServerMsg.Abort();
        
        if (OnClientDisConnect != null) OnClientDisConnect.Invoke();
    }

    protected override void OnDestroy()
    {
        Close();
    }
}
