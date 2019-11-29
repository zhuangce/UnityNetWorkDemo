using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStateTracker : MonoBehaviour
{
    public delegate void OnConnectStateChange();

    public OnConnectStateChange OnConnect;
    public OnConnectStateChange OnDisConnect;
    public OnConnectStateChange OnReConnect;
    public OnConnectStateChange OnError;




}
