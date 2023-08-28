using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkButtons : MonoBehaviour
{
    public void InitHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void InitClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
