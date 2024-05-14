
  using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using System.Net.Sockets;

public class PushIndexIntCmdToWebsocketMono : MonoBehaviour
{
    public string m_websocketServer = "ws://localhost:7065";
    [Header("Debug")]
    public bool m_connectedToServer = false;
    WebSocket websocket;
 
    async void Start()
    {
        websocket = new WebSocket(m_websocketServer);

        websocket.OnOpen += () =>
        {
            m_connectedToServer = true;
        };

        websocket.OnError += (e) =>
        {
            m_connectedToServer = false;
        };

        websocket.OnClose += (e) =>
        {
            m_connectedToServer = false;
        };

        websocket.OnMessage += (bytes) =>
        {
            m_connectedToServer = true;
        };
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(websocket!=null)
        websocket.DispatchMessageQueue();
#endif
    }



    public void SendIndexIntCmd(I_IndexIntCmdGet command)
    {
        SendIndexIntCmd(command.GetIndexInt(), command.GetCommandInt());
    }
        public void SendIndexIntCmd(int index, int command)
    {

        if (this.enabled == false || gameObject.activeInHierarchy==false)
            return;

        byte[] bytes1 = BitConverter.GetBytes(index);
        byte[] bytes2 = BitConverter.GetBytes(command);

        byte[] data = new byte[bytes1.Length + bytes2.Length];
        Buffer.BlockCopy(bytes1, 0, data, 0, bytes1.Length);
        Buffer.BlockCopy(bytes2, 0, data, bytes1.Length, bytes2.Length);
        SendWebSocketMessage(data);
    }

  

    async void SendWebSocketMessage(byte[] bytes)
    {

        if (this.enabled == false || gameObject.activeInHierarchy == false)
            return;

        if (websocket.State == WebSocketState.Open)
        {
            await websocket.Send(bytes);
        }
    }

    private async void OnApplicationQuit()
    {
        if(websocket!=null)
        await websocket.Close();
    }

}
