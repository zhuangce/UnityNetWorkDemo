using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class MyStringEvent: UnityEvent<string>
{

}

public class KEyBoard : MonoSingleton<KEyBoard>
{

    public Text output ;
    public Image indicator;
    public Button editDoneBtn;
    public Button clearBtn;
    public Button deleteBtn;
    [HideInInspector]
    public MyStringEvent OnEditDoneEvnet = new MyStringEvent();

    

    void Start()
    {
        
        editDoneBtn.onClick.AddListener(OnEditDone);
        clearBtn.onClick.AddListener(OnClearBtnDone);
        deleteBtn.onClick.AddListener(OnDeleteLastChar);
        DontDestroyOnLoad(gameObject);
        
        OnClose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputString(string str)
    {
        output.text += str;
    }

    public void OnTriggerIndiCator()
    {
        OnEditDoneEvnet.RemoveAllListeners();
        OnEditDoneEvnet.AddListener(LogOut);
        indicator.color = Color.green;
        //OnClose();
        OnClearBtnDone();
        //Scenemanager.Instance.SendMSG();

    }

    public void OnClearBtnDone()
    {
        output.text = "";
    }

    public void OnEditDone()
    {
        string outtext = output.text;
        output.text =outtext;
        OnEditDoneEvnet.Invoke(outtext);

    }
    public void LogOut(string n)
    {
        string str = output.text;
        OnClearBtnDone();
        Scenemanager.Instance.SendMSG(str);
    }

    public void OnDeleteLastChar()
    {
        if (output.text != "")
        {
            output.text = output.text.Remove(output.text.Length-1,1);
        }
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
    public void OnOpen()
    {
        gameObject.SetActive(true);
    }
}
