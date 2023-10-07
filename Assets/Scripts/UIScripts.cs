using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class UIScripts : MonoBehaviour
{
    public CanvasGroup canvasChatGroup;
    public Vector3 position;
    bool isActive = true;
    bool isFadingIn;
    bool isFadingOut;

    public Button chatButton;
    public Vector2 buttonInitialPos;

    public GameObject inputField;

    public InputField ipField;

    private void Start()
    {
        buttonInitialPos = chatButton.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        if (isFadingOut)
        {
            canvasChatGroup.alpha -= Time.deltaTime;
            if (canvasChatGroup.alpha <= 0)
            {
                isFadingOut = false;
            }
        }
        if (isFadingIn)
        {
            canvasChatGroup.alpha += Time.deltaTime;
            if (canvasChatGroup.alpha >= 1)
            {
                isFadingIn = false;
            }
        }
    }

    public void ButtonChat()
    {
        if (isActive)
        {
            //HideChat();
            isFadingOut = true;
            isActive = false;
            chatButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(chatButton.GetComponent<RectTransform>().anchoredPosition.x, -475);

            inputField.SetActive(false);
        }
        else
        {
            //ShowChat();
            isFadingIn = true;
            isActive = true;

            chatButton.GetComponent<RectTransform>().anchoredPosition = buttonInitialPos;

            inputField.SetActive(true);
        }
    }

    public void ShowChat()
    {
        canvasChatGroup.alpha = 1;
    }
    
    public void HideChat()
    {
        canvasChatGroup.alpha = 0;
    }
}
