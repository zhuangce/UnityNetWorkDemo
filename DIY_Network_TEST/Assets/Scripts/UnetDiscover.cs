using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnetDiscover : NetworkDiscovery
{
   
        //[SerializeField]

        //是否收到广播  
        public bool receivedBroadcast { get; private set; }
        //控制广播发送的频率(毫秒)  
        private int BroadcastInterval = 1000;
        //发送广播设备的IP地址  
        public string ServerIp { get; private set; }

        //公开log给外部
        public string logtext;

        private void Start()
        {
            //初始化NetworkDiscovery。  
            Initialize();
            if (NetworkManager.singleton == null)
            {
                Debug.Log("需要 NetworkManager组件");
                Destroy(this);
                return;
            }
            broadcastInterval = BroadcastInterval;
            //作为客户端监听广播  
            StartAsClient();
            //作为服务端发送广播（第一个启动的设备）  
            float InvokeWaitTime = 1 + Random.value;
            Invoke("TobeServer", InvokeWaitTime);


        }
        private void TobeServer()
        {

            //如果接收到了广播，说明已经有设备先广播了并作为了服务器，就接收广播，并连接  
            if (receivedBroadcast)
            {

                //  IOSMainUIManager.Instance.InitUserType(false);
                return;
            }
            Debug.Log("本设备作为服务器发送广播...");
            //GameManager.Instance.DebugText.text += "\n本设备作为服务器发送广播...";
            //如果没有接收到了广播，说明还没有设备创建服务器  
            StopBroadcast();//停止接听/发送广播  

            //开启本机服务器（也可以作为客户端）  
            NetworkManager.singleton.StartHost();
            //开始发送广播  
            StartAsServer();
            //IOSMainUIManager.Instance.InitUserType(true);
            //GlobleData.Instance.IsServer = true;
        }
        //开始监听广播  
        public override void OnReceivedBroadcast(string fromAddress, string data)
        {


            //if (GameManager.Instance == null)
            //{
            //    return;
            //}
            //if (GameManager.Instance.GetIsNet() == false)
            //{
            //    return;
            //}

            //从广播中接收ip数据，如果已经接收过了就不需要接收了  
            if (receivedBroadcast)
            {
                return;
            }

            receivedBroadcast = true;
            //停止接听/发送广播  
            StopBroadcast();
            //把广播设备的IP保存  
            ServerIp = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1);

            Debug.Log("从服务器 " + ServerIp + ":接收到广播,连接作为客户端...");
            //logtext = "从服务器 " + ServerIp + ":接收到广播,连接作为客户端...";
          
            //GameManager.Instance.DebugText.text += "\n从服务器 "+ ServerIp + ":接收到广播,连接作为客户端...";
            //设置NetworkManager的IP  
            NetworkManager.singleton.networkAddress = ServerIp;
            //自己作为一个客户端连接上服务器IP  
            NetworkManager.singleton.StartClient();
            //TransmitAnchorData.Instance.SetServerIP(ServerIp);
        }

        public void CmdSendArCoreLocation()
        {
            string method = "SendArCoreLocation";
            GameObject go = new GameObject();

        }
    }

