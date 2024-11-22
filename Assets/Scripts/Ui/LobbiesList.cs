using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbiesList : MonoBehaviour
{
    [SerializeField] private Transform LobbyItemParent;
    [SerializeField] private LobbyItem LobbyItemPrefab;

    private Lobby Lobby;
    private bool isJoining = false;
    private bool isRefreshing = false;
    private void OnEnable()
    {
        refreshList();
    }

    public async void refreshList()
    {
        if (isRefreshing) { return; }
        isRefreshing = true;
            try
            {
                QueryLobbiesOptions options = new QueryLobbiesOptions();
                options.Count = 25;
                options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),

                 new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0")

            };
           QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            foreach(Transform child in LobbyItemParent)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbies.Results)
            {
               LobbyItem lobbyItem =   Instantiate(LobbyItemPrefab, LobbyItemParent);
                lobbyItem.Initialise(this, lobby);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
        


        isRefreshing = false;
    }
    public async void joinAsync(Lobby lobby)
    {
        if (isJoining){ return; }
        isJoining = true;
        try
        {
            Lobby JoiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = JoiningLobby.Data["JoinCode"].Value;
            await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogException(e);
        }
        isJoining=false;
    }
}
