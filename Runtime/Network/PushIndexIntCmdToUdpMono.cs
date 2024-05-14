using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class PushIndexIntCmdToUdpMono : MonoBehaviour
{


    public string m_targetIp;
    public int  m_targetPort;

    UdpClient client = new UdpClient();
    public void OnEnable()
    {
        if (client != null)
            client.Close();
        client = new UdpClient();

    }
    public void OnDisable()
    {
        if (client != null)
            client.Close();
    }

    public void SendUdpMessage(I_IndexIntCmdGet command)
    {
        if (client == null ||  command==null)
            return;
        SendUdpMessage(command.GetIndexInt(), command.GetCommandInt());
    }

    public void SendUdpMessage(int intIndex, int intCmd)
    {

        SendUdpMessage(client, intIndex, intCmd);
    }
    public void SendUdpMessageAsTwoIntSplitBySpace(string indexIntCmd)
    {
        while(indexIntCmd.IndexOf("  ")>-1)
        {
            indexIntCmd = indexIntCmd.Replace("  ", " ");
        }
        string[] tokens = indexIntCmd.Trim().Split(" ");
        if (tokens.Length == 2) {

            if (int.TryParse(tokens[0], out int intIndex) && int.TryParse(tokens[1], out int intCmd)) {
                SendUdpMessage(client, intIndex, intCmd);
            }
        }
    }
    public void SendUdpMessage(UdpClient client, int intIndex, int intCmd)
    {
        if (client == null )
            return;

        if (this.enabled == false || gameObject.activeInHierarchy == false)
            return;

        byte[] bytes1 = BitConverter.GetBytes(intIndex);
        byte[] bytes2 = BitConverter.GetBytes(intCmd);

        byte[] data = new byte[bytes1.Length + bytes2.Length];
        Buffer.BlockCopy(bytes1, 0, data, 0, bytes1.Length);
        Buffer.BlockCopy(bytes2, 0, data, bytes1.Length, bytes2.Length);
        client.Send(data, data.Length, new IPEndPoint(IPAddress.Parse(m_targetIp), m_targetPort));

    }
}
