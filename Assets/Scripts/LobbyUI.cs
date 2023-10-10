using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public GameObject panelLobby;
    public TMP_Text playerNameUnoText;

    private object lobbyname;

    private void Start()
    {
        LobbiesManager.instance.OnLobbyJoin += OnLobbyJoin;
    }
    
    void OnLobbyJoin()
    {
        panelLobby.SetActive(true);
        playerNameUnoText.text = LobbiesManager.instance.playerName;
    }

    void OnDestroy()
    {
        LobbiesManager.instance.OnLobbyJoin -= OnLobbyJoin;
    }
}
