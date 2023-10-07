using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public bool isTaken;

    public bool isSpikes;

    public void TakeBall()
    {
        if (isTaken)
        {
            return;
        }

        isTaken = true;
        GetComponent<Collider>().enabled = false;

        GetComponent<Renderer>().enabled = false;
    }
}
