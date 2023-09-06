using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkButtons : MonoBehaviour
{
    [SerializeField]
    GameObject btnClient, btnHost;

    private void Awake()
    {
        btnClient = GameObject.Find("ButtonClient");
        btnHost = GameObject.Find("ButtonHost");
    }

    public void InitHost()
    {
        NetworkManager.Singleton.StartHost();
        btnClient.SetActive(false);
        btnHost.SetActive(false);
    }

    public void InitClient()
    {
        NetworkManager.Singleton.StartClient();
        btnClient.SetActive(false);
        btnHost.SetActive(false);
    }
}
