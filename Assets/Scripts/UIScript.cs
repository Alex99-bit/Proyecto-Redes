using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    public List<GameObject> objectsToToggle; // Lista de objetos que se pueden ocultar/mostrar
    public bool isChatActive;

    // Start is called before the first frame update
    void Start()
    {
        isChatActive = true;
    }

    private void Update()
    {
        BotonHideChat();
    }

    public void BotonHideChat()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isChatActive = !isChatActive; // Invierte el estado

            // Itera a través de la lista de objetos para ocultarlos/mostrarlos
            foreach (GameObject obj in objectsToToggle)
            {
                obj.SetActive(isChatActive);
            }
        }
    }
}
