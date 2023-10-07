using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class ChatScript : NetworkBehaviour
{
    public TMP_Text chatTxt;
    string currenntText = "";
    public GameObject scroll;

    string code;

    public void OnEnable()
    {
        //coreManager.singleton.eventChat += WriteTextFromServer;
    }

    private void OnDisable()
    {
        //coreManager.singleton.eventChat -= WriteTextFromServer;
    }

    public void IpChange(string cadena)
    {
        Debug.Log(cadena);
        //NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = cadena;

        //RelayManager.singleton.JoinGame(cadena);

        code = cadena;
    }

    public void JoinByCode()
    {
        RelayManager.singleton.JoinGame(code);
    }

    private void Start()
    {
        WriteText("Texto inicial");
        coreManager.singleton.eventChat += WriteTextFromServer;
    }

    public override void OnDestroy()
    {
        coreManager.singleton.eventChat -= WriteTextFromServer;
    }

    public void WriteText(string newText)
    {
        //if (!IsOwner) return;

        currenntText += "\n" + "Player " + OwnerClientId + ": " + newText;

        chatTxt.text = currenntText;

        //scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        coreManager.singleton.MandarTextoAlChatServerRPC(newText, OwnerClientId);

        chatTxt.text = "";
    }

    public void WriteTextFromServer(string text, ulong playerID)
    {
        //if (!IsOwner) return;
        Debug.Log("Event chat");

        currenntText += "\n" + "Player " + OwnerClientId + ": " + text;

        chatTxt.text = currenntText;

        //scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        //coreManager.singleton.MandarTextoAlChatServerRPC(text, OwnerClientId);

        chatTxt.text = "";
    }
}
