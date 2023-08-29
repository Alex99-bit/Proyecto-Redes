using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Porterias : NetworkBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PorteriaAzul")
        {
            Debug.Log("Le metieron al azul alv");
            coreManager.singleton.SumarScoresServerRPC(0);
        }

        if (other.gameObject.name == "PorteriaRoja")
        {
            Debug.Log("Le metieron al rojo alv");
            coreManager.singleton.SumarScoresServerRPC(1);
        }
    }
}
