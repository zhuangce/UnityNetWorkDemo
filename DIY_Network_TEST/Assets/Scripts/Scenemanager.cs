using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Scenemanager : MonoSingleton<Scenemanager>
{
    public GameObject serverPrefab;
    public GameObject clientPrefab;

    public Button clientBtn;
    public Button serverBtn;

    public InputField  inf;

    public Text console;

    public Image connectState;
    public bool isServer
    {
        get;private set;
    }

    private void Start()
    {

        clientBtn.onClick.AddListener(TobeClient);
        serverBtn.onClick.AddListener(TobeServer);
    }

    private string threadmsg;
    private void Update()
    {
        if (!string.IsNullOrEmpty(threadmsg))
        {
            console.text += "\n" + threadmsg;
            threadmsg = "";
        }

            
    }

    public void TobeClient()
    {
        isServer = false;   
        serverPrefab.gameObject.SetActive(false);
        //clientPrefab.gameObject.SetActive(true);
        KEyBoard.Instance.OnOpen();
        KEyBoard.Instance.OnEditDoneEvnet.AddListener((t)=> {
            HLTClient.Instance.OnClientConnect += KEyBoard.Instance.OnTriggerIndiCator;
            HLTClient.Instance.StartUpWithIp(t);
        });
        StartCoroutine(InitalScene());

    }
    public void TobeServer()
    {
        isServer = true;
        serverPrefab.gameObject.SetActive(true);
        // clientPrefab.gameObject.SetActive(true);
        KEyBoard.Instance.OnOpen();
        HLTServer.Instance.OnServerError += () => {
            KEyBoard.Instance.OnEditDoneEvnet.AddListener((t) => {
                HLTServer.Instance.SetUp(t);
               // HLTClient.Instance.OnClientConnect += KEyBoard.Instance.OnTriggerIndiCator;
                HLTClient.Instance.StartUpWithIp(t);
                //KEyBoard.Instance.OnTriggerIndiCator();
            });
        };
        StartCoroutine(DelayCallBack(2,()=> {
            HLTClient.Instance.OnClientConnect += KEyBoard.Instance.OnTriggerIndiCator;
            HLTClient.Instance.StartUpWithIp(HLTServer.Instance.GetIP()); }));
        StartCoroutine( InitalScene());
    }
    


    private IEnumerator InitalScene()
    {
        yield return null;
        clientBtn.gameObject.SetActive(false);
        serverBtn.gameObject.SetActive(false);
        HLTClient.Instance.OnServerMsgReceive.AddListener((s)=> { ShowRecivedMsg(s); });
      //  inf.gameObject.SetActive(true);
       // inf.onEndEdit.AddListener(SendMSG);
        if (isServer)
        {
            HLTServer.Instance.OnNewMessageReceived += ReciveMsg;
            HLTServer.Instance.OnServerStartReady.AddListener((log)=> {
                console.text += "\n "+log;
            });
        }
          
    }
    public void ShowRecivedMsg(string str)
    {
        threadmsg = str;
    }
  
    public void SendMSG(string str)
    {
       // KEyBoard.Instance.LogOut();
        console.text += "\n" + str;
        //if (isServer)
        //{
            HLTClient.Instance.Send(str);
       // }

    }

    private void ReciveMsg()
    {
      //  if (HLTClient.Instance.CheckIfClientAlsoSever()) return;
        console.text += "\n" + HLTClient.Instance.receiveMsg;
        Debug.Log("Received");

    }
    public IEnumerator DelayCallBack(float time,Action callback)
    {
        yield return new WaitForSeconds(time);
        callback.Invoke();
    }


}
