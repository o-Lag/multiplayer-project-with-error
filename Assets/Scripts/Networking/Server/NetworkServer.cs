using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager;
    private Dictionary<ulong, string> ClientIdToAuth = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> AuthIdToUserData = new Dictionary<string, UserData>();
  public NetworkServer(NetworkManager networkManager)
    {
        this._networkManager = networkManager;
        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
        _networkManager.OnServerStarted += OnNetworkReady;
    }

  

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)

    {

        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);

        UserData userData = JsonUtility.FromJson<UserData>(payload);

        if (userData != null)

        {
            ClientIdToAuth[request.ClientNetworkId] = userData.UserAuthID;
            AuthIdToUserData[userData.UserAuthID] = userData;

        }



        response.Approved = true;

        response.CreatePlayerObject = true;

    }

    private void OnNetworkReady()
    {
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
      
    }
    
    private void OnClientDisconnect(ulong clientId)
    {
      if(ClientIdToAuth.TryGetValue(clientId, out string authID))
        {
            ClientIdToAuth.Remove(clientId);
            AuthIdToUserData.Remove(authID);
        }
    }
    public UserData GetUserDataByClientID(ulong clientID)
    {
        if (ClientIdToAuth.TryGetValue(clientID, out string authid))
        {
            if (AuthIdToUserData.TryGetValue(authid, out UserData data))
            {
                return data;
            }
            return null;
        }
        return null;
    }
    public void Dispose()
    {
        if(_networkManager != null)
        {
            _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
            _networkManager.OnServerStarted -= OnNetworkReady;
            _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
            if (_networkManager.IsListening)
            {
                _networkManager.Shutdown();
            }
        }
        
    }
}
