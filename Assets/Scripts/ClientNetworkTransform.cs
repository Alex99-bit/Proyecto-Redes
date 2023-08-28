using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;

public class ClientNetworkTransform : NetworkTransform
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
