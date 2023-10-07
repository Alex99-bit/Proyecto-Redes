using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteriaScript : MonoBehaviour
{
    public int bando = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pelota")
        {
            if (other.gameObject.GetComponent<BallScript>().isSpikes)
            {
                return;
            }
            else
            {
                coreManager.singleton.SumarScoresServerRPC((ulong)bando);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Pelota")
        {
            coreManager.singleton.RestarScoresServerRPC((ulong)bando);
        }
    }
}
