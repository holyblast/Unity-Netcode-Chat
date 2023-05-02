using Unity.Netcode;
using UnityEngine;

public class RpcTest : NetworkBehaviour
{
    public bool IsClientOwner { get => !IsServer && IsOwner; }
    public override void OnNetworkSpawn()
    {
        if (IsClientOwner)
        {
            TestServerRpc(0, NetworkObjectId);
        }
    }

    [ClientRpc]
    private void TestClientRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        if (IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            TestServerRpc(value + 1, sourceNetworkObjectId);
        }
    }

    [ServerRpc]
    void TestServerRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        TestClientRpc(value, sourceNetworkObjectId);
    }
}
