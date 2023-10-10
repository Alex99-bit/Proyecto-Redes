using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using TMPro;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System;
using Unity.VisualScripting;

#if UNITY_EDITOR
using ParrelSync;
#endif

public class LobbiesManager : MonoBehaviour
{
    public string playerId, playerName;
    private Lobby _connectedLobby;

    public static LobbiesManager instance;

    public Action OnLobbyJoin;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    async void Start()
    {
        await Autenticar(); 
        // Aqui hay mas codigo, NECESARIO PRIMERO AUTENTICAR
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task Autenticar()
    {
        var options = new InitializationOptions();

        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Signed in as " + AuthenticationService.Instance.PlayerId);

        playerId = AuthenticationService.Instance.PlayerId;
        playerName = "UCSLP" + UnityEngine.Random.Range(1,1000);
    }

    async Task<Lobby> CreateLobby()
    {
        try
        {
            string lobbyName = "SALA " + UnityEngine.Random.Range(1, 1000);
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 4); // TO DO: Hacer Opciones
            Debug.Log("Se creo lobby: " + lobby.Name);

            OnLobbyJoin?.Invoke();

            return lobby;
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    async Task<Lobby> QuickLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Debug.Log("Se unio a lobby: " + lobby.Name);

            OnLobbyJoin?.Invoke();
            return lobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void CreateorJoin()
    {
        _connectedLobby = await QuickLobby() ?? await CreateLobby();
    }
}
