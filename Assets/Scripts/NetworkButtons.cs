using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;

public class NetworkButtons : MonoBehaviour
{
    public Button hostButton, clienntButton;

    public void Start()
    {
        SetUpConnection();
    }

    public void SetUpConnection()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionPending;
    }

    public void InitHost()
    {
        //NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionPending;

        //NetworkManager.Singleton.StartHost();

        RelayManager.singleton.CreateGame();
    }

    private void ConnectionPending(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count < 2)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            Debug.Log("Conexión aprobada");
        }
        else
        {
            response.Approved = false;
            response.Reason = "Partida llena";
            Debug.Log("Partida llena");
        }

        Debug.Log("Revisando conexión");
    }

    public void InitClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
