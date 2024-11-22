using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using System.Runtime.Remoting.Messaging;
using Unity.Collections;

public class PlayerObj : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private CinemachineVirtualCamera cam;
    [Header("Settings")]
    [SerializeField]
    private int OwnerPriority;
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientID(OwnerClientId);
            Debug.Log($"UserData.UserName: {userData.UserName}");
            PlayerName.Value = userData.UserName;

        }
        if (IsOwner)
        {
            cam.Priority = OwnerPriority;
        }
    }
}
