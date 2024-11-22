using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
public class LobbyItem : MonoBehaviour
{
    [SerializeField] TMP_Text LobbyNameText;
    [SerializeField] TMP_Text LobbyPlayersText;
    private LobbiesList LobbiesList;
    private Lobby Lobby;
    public void Initialise(LobbiesList LobbyList, Lobby lobby)
    {
        this.LobbiesList = LobbyList;
        this.Lobby = lobby;
        LobbyNameText.text = lobby.Name;
        LobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
       
    }
    public void Join()
    {
        LobbiesList.joinAsync(Lobby);
    }
}
