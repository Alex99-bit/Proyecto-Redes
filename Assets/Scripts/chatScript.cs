using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class chatScript : NetworkBehaviour
{
    public TMP_Text chat_text;
    public ScrollRect scroll;
    string current;

    private void Awake()
    {
        current = "";
        //chat_text = GetComponent<TMP_Text>();
    }

    public void EscribirText(string newText)
    {
        /*if (!IsOwner) { return; }

        current += "\nPlayer " + OwnerClientId + ": " + newText;
        chat_text.text = current;

        //scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        scroll.verticalNormalizedPosition = 0;

        coreManager.singleton.MandarTextoChatServerRpc(newText,OwnerClientId);
        // Ensure the scroll adjustment happens in the next frame
        //StartCoroutine(ScrollToBottom());*/

        if (!IsOwner) return;

        current += "\n" + "Player " + OwnerClientId + ": " + newText;

        chat_text.text = current;

        scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        coreManager.singleton.MandarTextoChatServerRpc(newText, OwnerClientId);
    }

    public void EscribitText2(string newText, ulong player)
    {
        /*if (!IsOwner) { return; }
        current += "\nPlayer " + player + ": " + newText;
        chat_text.text = current;
        scroll.verticalNormalizedPosition = 0;*/
        if (!IsOwner) return;

        current += "\n" + "Player " + OwnerClientId + ": " + newText;

        chat_text.text = current;

        scroll.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        coreManager.singleton.MandarTextoChatServerRpc(newText, OwnerClientId);
    }

    /* Test */
    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame(); // Esperar al final del frame

        // Ajustar la posición vertical del ScrollRect
        Canvas.ForceUpdateCanvases(); // Actualizar primero las canvas
        scroll.verticalNormalizedPosition = 0;
    }
}
