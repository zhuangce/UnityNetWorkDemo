
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if WINDOWS_UWP
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#endif

public class NSServer : MonoBehaviour
{
#if WINDOWS_UWP
    StreamSocket socket;
    StreamSocketListener listener;
    String port;
#endif

    // Use this for initialization
    void Start()
    {
#if WINDOWS_UWP
        listener = new StreamSocketListener();
        port = "8888";
        listener.ConnectionReceived += Listener_ConnectionReceived;
        listener.Control.KeepAlive = false;

        Listener_Start();
#endif
    }

#if WINDOWS_UWP
     
        private async void Listener_Start()
    {
        Debug.Log("Listener started");
        try
        {
            await listener.BindServiceNameAsync(port);
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }

        Debug.Log("Listening");
    }

    private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
    {
        Debug.Log("Connection received");
        DataReader reader = new DataReader(args.Socket.InputStream);
        try
        {
            while (true)
            {
                // Read first 4 bytes (length of the subsequent string). 
                uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                if (sizeFieldCount != sizeof(uint))
                {
                    // The underlying socket was closed before we were able to read the whole data. 
                    return;
                }

                // Read the string. 
                uint stringLength = reader.ReadUInt32();
                uint actualStringLength = await reader.LoadAsync(stringLength);
                if (stringLength != actualStringLength)
                {
                    // The underlying socket was closed before we were able to read the whole data.
                    return;
                }

                // dump data
                Debug.Log("Received: " + reader.ReadString(actualStringLength));
            }
        }
        catch (Exception exception)
        {
            // If this is an unknown status it means that the error is fatal and retry will likely fail. 
            if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
            {
                throw;
            }

            // dump data
            Debug.Log("Read Stream failed: " + exception.Message);
        }
    }
#endif
    // Update is called once per frame
    void Update()
    {

    }
}
