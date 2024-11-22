using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Text;
using Unity.Services.Authentication;

public class ClientGameManager : IDisposable
{
    private JoinAllocation JoinAllocation;
    private NetworkClient networkClient;

 

    public async Task<bool> InitAsync()
    {
      await UnityServices.InitializeAsync();
      networkClient = new NetworkClient(NetworkManager.Singleton);
       
       AuthState authstate = await AuthenticationWrapper.DoAuth();
        if (authstate == AuthState.Authenticated)
        {
            return true;
        }
        return false;
    }
 
    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            JoinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch(Exception e)  
        {
                Debug.LogException(e);
                return;
        }
       UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
       RelayServerData relayServerData = new RelayServerData(JoinAllocation, "dtls");
       transport.SetRelayServerData(relayServerData);

        UserData userData = new UserData()
        {
            UserName = PlayerPrefs.GetString(NameSelector.PlayerName,"Missing Name"),
            UserAuthID = AuthenticationService.Instance.PlayerId
        };
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartClient();
    }
    public void Dispose()
    {
       networkClient?.Dispose();
    }
}
