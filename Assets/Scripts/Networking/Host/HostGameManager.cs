using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Text;
using Unity.Services.Authentication;
public class HostGameManager : IDisposable
{
    private const int MaxConections = 20;
    public string JoinCode;
    private string id;
    public NetworkServer NetworkServer{get;private set;}
    private Allocation allocation;
    private const string GameSceneName = "gameplayScene";
    public async Task StartHostAsync()
    {
        try
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConections);
        }
        catch (Exception E) 
        { 
        
            Debug.LogException(E);
            return;
        }

        try
        {
           JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(JoinCode);
        }
        catch (Exception E)
        {

            Debug.LogException(E);
            return;
        }

        UnityTransport transport =  NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation,"dtls");
        transport.SetRelayServerData(relayServerData);

        try
        {
            CreateLobbyOptions LobbyOptions = new CreateLobbyOptions();
            LobbyOptions.IsPrivate = false;
            LobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: JoinCode
                    
                    )
                }
            };
          string PlayerName = PlayerPrefs.GetString(NameSelector.PlayerName, "Unknown");
          Lobby lobby =  await Lobbies.Instance.CreateLobbyAsync($"{PlayerName}'s Lobby", MaxConections, LobbyOptions );
          id = lobby.Id;
          HostSingleton.Instance.StartCoroutine(HeartBeatLobby(15));
        }
        catch (LobbyServiceException E) 
        { 
            Debug.Log(E);
            return;
        
        }
         NetworkServer = new NetworkServer(NetworkManager.Singleton);
        UserData userData = new UserData()
        {
            UserName = PlayerPrefs.GetString(NameSelector.PlayerName, "Missing Name"),
            UserAuthID = AuthenticationService.Instance.PlayerId
        };
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
       
    }
    private IEnumerator HeartBeatLobby(float waitTimeSeconds)
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true) {

            Lobbies.Instance.SendHeartbeatPingAsync(id);
            yield return delay;
            Debug.Log("pinging");
        }
    }

    public async void Dispose()
    {
        HostSingleton.Instance.StopCoroutine(nameof(HeartBeatLobby));
        if (!string.IsNullOrEmpty(id)) 
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(id);
            }
            catch (LobbyServiceException E) { 
            
                Debug.LogException(E);
            
            }
            id = string.Empty;
        }
        NetworkServer?.Dispose();
    }
}
