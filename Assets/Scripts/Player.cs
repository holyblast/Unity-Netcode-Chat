using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    public static Action<string> SendMessageEvent;

    private void OnEnable()
    {
        SendMessageEvent += Send;
    }

    private void OnDisable()
    {
        SendMessageEvent -= Send;
    }

    private void Send(string message)
    {
        ServerRpcParams paramas = new ServerRpcParams();
        SendMessageServerRpc(message, paramas);
    }

    [ServerRpc]
    private void SendMessageServerRpc(string message, ServerRpcParams serverRpcParams)
    {
        if (!NetworkManager.IsServer) return;

        ulong clientID = serverRpcParams.Receive.SenderClientId;
        RecieveMessageClientRpc(clientID, message);
    }

    [ClientRpc]
    private void RecieveMessageClientRpc(ulong clientID, string message)
    {
        MenuManager.ReceiveMessageEvent($"{clientID}",message);
    }
}
