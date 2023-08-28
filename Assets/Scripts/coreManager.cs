using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class coreManager : NetworkBehaviour
{

    public TMP_Text txtScore1, txtScore2;

    public NetworkVariable<int> scoreN1 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> scoreN2 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public static coreManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    [ServerRpc (RequireOwnership = false)]
    public void SumarScoresServerRPC(ulong OwnerClientId)
    {
        Debug.Log("Servidor _ Anoto jugador: " + OwnerClientId);

        if (OwnerClientId == 0)
        {
            scoreN1.Value++;
            //txtScore1.text = "Score = " + scoreN1.Value.ToString();
         ActualizarScoreClientRPC();
        }
        else if (OwnerClientId == 1)
        {
            scoreN2.Value++;
            //txtScore2.text = "Score = " + scoreN2.Value.ToString();
            ActualizarScoreClientRPC();
        }
    }

    [ClientRpc]
    void ActualizarScoreClientRPC()
    {
        txtScore1.text = "Score = " + scoreN1.Value.ToString();
        txtScore2.text = "Score = " + scoreN2.Value.ToString();
    }
}
