using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class coreManager : NetworkBehaviour
{

    public TMP_Text txtScore1, txtScore2;

    public NetworkVariable<int> scoreN1 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> scoreN2 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public static coreManager singleton;

    public chatScript chatscrpt;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        ActualizarScoreClientRPC();
        NetworkManager.Singleton.OnClientConnectedCallback += GameStart;
    }

    public void GameStart(ulong playerId)
    {
        Debug.Log("Player conectado: " + playerId);
        if (playerId >= 1)
        {
            StartCoroutine(NetworkManager.Singleton.ConnectedClients[0].PlayerObject.gameObject.GetComponent<MovPlayer>().gameRain());
        }
    }


    private void Awake()
    {
        singleton = this;
    }

    [ServerRpc (RequireOwnership = false)]
    public void SumarScoresServerRPC(ulong OwnerClientId)
    {
        Debug.Log("Servidor _ Anoto jugador: " + OwnerClientId);

        if (OwnerClientId == 0)
        {
            scoreN1.Value++;
            //txtScore1.text = "Score = " + scoreN1.Value.ToString();
            ActualizarScoreClientRPC();
        }
        else if (OwnerClientId == 1)
        {
            scoreN2.Value++;
            //txtScore2.text = "Score = " + scoreN2.Value.ToString();
            ActualizarScoreClientRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SumarScoresServerRPC(int player,bool spike) // 1 rojo, 0 azul
    {
        Debug.Log("Servidor _ Anoto jugador: " + player);

        if (player == 0 && !spike)
        {
            //azul
            scoreN1.Value++;
            //txtScore1.text = "Score = " + scoreN1.Value.ToString();
            ActualizarScoreClientRPC();
        }
        else if (player == 0 && spike)
        {
            // azul pierde puntos
            scoreN1.Value--;
            ActualizarScoreClientRPC();
        } 
        else if (player == 1 && !spike)
        {
            //rojo
            scoreN2.Value++;
            //txtScore2.text = "Score = " + scoreN2.Value.ToString();
            ActualizarScoreClientRPC();
        }
        else if (player == 1 && spike)
        {
            // rojo pierde puntos
            scoreN2.Value--;
            ActualizarScoreClientRPC();
        }
    }

    [ClientRpc]
    void ActualizarScoreClientRPC()
    {
        txtScore1.text = "Score = " + scoreN1.Value.ToString();
        txtScore2.text = "Score = " + scoreN2.Value.ToString();
    }

    [ServerRpc (RequireOwnership = false)]
    public void MandarTextoChatServerRpc(string text,ulong player)
    {
        MandarTextoChatClientRpc(text,player);
    }

    [ClientRpc]
    public void MandarTextoChatClientRpc(string text,ulong player)
    {
        if (!IsOwner)
        {
            chatscrpt.EscribitText2(text,player);
        }
    }

}
