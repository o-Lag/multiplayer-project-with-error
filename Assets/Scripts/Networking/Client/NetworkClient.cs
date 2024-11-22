using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient : IDisposable
{
    private NetworkManager _networkManager;
    private const string SceneName = "Menu";
    public NetworkClient(NetworkManager networkManager)
    {
        this._networkManager = networkManager;
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (clientId != 0 && clientId != _networkManager.LocalClientId) { return; }
        if (SceneManager.GetActiveScene().name != SceneName) 
        {
            SceneManager.LoadScene(SceneName);
        
        }
        if (_networkManager.IsConnectedClient)
        {
            _networkManager.Shutdown();
        }
    }
    public void Dispose()
    {
        if (_networkManager != null) {

            _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        }
      
    }
}
