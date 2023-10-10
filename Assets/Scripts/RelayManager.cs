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

public class RelayManager : MonoBehaviour
{
    public static RelayManager singleton;

    public TMP_Text txtCode;

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        //Autenticar();
    }
    /* Movido a lobbieManager
    async void Autenticar()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Signed in as " + AuthenticationService.Instance.PlayerId);
    }*/

    public async void CreateGame()
    {
        var alloc = await RelayService.Instance.CreateAllocationAsync(4);
        //string oinCode = alloc.AllocationId.ToString();
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
        Debug.Log("AllocId " + alloc.AllocationId);
        Debug.Log("Join Code " + joinCode);

        RelayServerData relayServerData = new RelayServerData(alloc, "udp");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        NetworkManager.Singleton.StartHost();

        txtCode.text = joinCode.ToString();
    }

    public async void JoinGame(string joinCode)
    {
        JoinAllocation joinAlloc = await RelayService.Instance.JoinAllocationAsync(joinCode);
        RelayServerData relayServerData = new RelayServerData(joinAlloc, "udp");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();
    }
}
