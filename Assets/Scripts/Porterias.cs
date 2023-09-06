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
            coreManager.singleton.SumarScoresServerRPC(0,false);
            DespawnRecovery(this.gameObject);
        }

        if (other.gameObject.name == "PorteriaRoja")
        {
            Debug.Log("Le metieron al rojo alv");
            coreManager.singleton.SumarScoresServerRPC(1,false);
            DespawnRecovery(this.gameObject);
        }
    }

    void DespawnRecovery(GameObject ball)
    {
        Debug.Log("Este es ball: " + ball);
        ball.GetComponent<BallScript>().TakeBall();
        MovPlayer.instance.DespawnBallServerRPC(ball);
    }
}
