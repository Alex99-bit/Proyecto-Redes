using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public class coreManager : NetworkBehaviour
{
    public TMP_Text txtScore1, txtScore2;

    public NetworkVariable<int> scoreN1 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> scoreN2 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public static coreManager singleton;

    public ChatScript chat;

    public delegate void onChat(string cadena, ulong id);
    public onChat eventChat;

    bool isGameStarted = false;
    public GameObject ballPrefab;
    public GameObject spikePrefab;

    public float gameDuration = 30;

    public NetworkVariable<int> hp1 = new();
    public NetworkVariable<int> hp2 = new();

    public int maxHp;

    public TMP_Text txtTimer;
    public NetworkVariable<float> elapsedTime = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        singleton = this;
    }

    public override void OnNetworkSpawn()
    {
        ActualizarScoreClientRPC();

        NetworkManager.Singleton.OnClientConnectedCallback += GameStart;

        if (IsServer)
        {
            hp1.Value = maxHp;
            hp2.Value = maxHp;
        }
    }

    public override void OnNetworkDespawn()
    {
        ActualizarScoreClientRPC();

        NetworkManager.Singleton.OnClientConnectedCallback -= GameStart;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SumarScoresServerRPC(ulong OwnerClientId)
    {
        Debug.Log("Servidor _ Anoto jugador: " + OwnerClientId);

        if (OwnerClientId == 0)
        {
            scoreN1.Value++;
            ActualizarScoreClientRPC();
        }
        else if (OwnerClientId == 1)
        {
            scoreN2.Value++;
            ActualizarScoreClientRPC();
        }
    }

    [ClientRpc]
    void ActualizarScoreClientRPC()
    {
        txtScore1.text = "Score = " + scoreN1.Value.ToString();
        txtScore2.text = "Score = " + scoreN2.Value.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RestarScoresServerRPC(ulong OwnerClientId)
    {
        if (OwnerClientId == 0)
        {
            scoreN1.Value--;
            ActualizarScoreClientRPC();
        }
        else if (OwnerClientId == 1)
        {
            scoreN2.Value--;
            ActualizarScoreClientRPC();
        }
    }

    [ServerRpc (RequireOwnership = false)]
    public void MandarTextoAlChatServerRPC(string text, ulong player)
    {
        MandarTextoAlChatClientRPC(text, player);
    }

    [ClientRpc]
    public void MandarTextoAlChatClientRPC(string txt, ulong plyr)
    {
        //if (!IsOwner)
        //{
        //chat.WriteTextFromServer(txt, plyr);
        Debug.Log("Antes de event chat");
        chat.WriteTextFromServer(txt, plyr);
        eventChat?.Invoke(txt, plyr);                                          //El signo de interrogación verifica si el eventChat no es null
        //}
    }

    public void GameStart(ulong playerID)
    {
        Debug.Log("Cliente conectado es: " + playerID);
        if (playerID >= 1 && !isGameStarted)
        {
            isGameStarted = true;
            StartCoroutine(gameRain());

            StartCoroutine(GameTimer());
        }
    }

    [ServerRpc]
    public void ReducirHPServerRPC(ulong id)
    {
        if (id == 0)
        {
            hp1.Value--;
        }
        else if (id == 1)
        {
            hp2.Value--;
        }
    }

    public void CheckHP()
    {
        if (hp1.Value <= 0)
        {
            Debug.Log("Jugador 1 perdió");
        }
        else if (hp2.Value <= 0)
        {
            Debug.Log("Jugador 2 perdió");
        }
    }

    IEnumerator gameRain()
    {
        yield return null;
        while (true)
        {
            int x = UnityEngine.Random.Range(-10, 10);
            int z = UnityEngine.Random.Range(-10, 10);

            int randomBall = UnityEngine.Random.Range(0, 6);

            NetworkObject obj;

            Debug.Log("HOLA");
            if(randomBall <= 3)
            {
                //GameObject ballInstance = Instantiate(ballPrefab, new Vector3(x, 10, z), Quaternion.identity);
                obj = NetworkObjectPool.Singleton.GetNetworkObject(ballPrefab, new Vector3(x, 10, z), Quaternion.identity);
                //ballInstance.GetComponent<NetworkObject>().Spawn();
                //yield return new WaitForSecondsRealtime(2);

                Debug.Log("HOLA");
            }
            else
            {
                //GameObject ballInstance = Instantiate(spikePrefab, new Vector3(x, 10, z), Quaternion.identity);
                //ballInstance.GetComponent<NetworkObject>().Spawn();
                obj = NetworkObjectPool.Singleton.GetNetworkObject(spikePrefab, new Vector3(x, 10, z), Quaternion.identity);
                //yield return new WaitForSecondsRealtime(2);
                Debug.Log("HOLA");
            }

            obj.Spawn();
            obj.transform.SetParent(NetworkObjectPool.Singleton.gameObject.transform);
            Debug.Log("HOLA");
            yield return new WaitForSecondsRealtime(2);            
        }
    }

    private IEnumerator GameTimer()
    {
        while (elapsedTime.Value < gameDuration)
        {
            elapsedTime.Value += Time.deltaTime;
            Debug.Log(elapsedTime);
            ActualizarTimerClientRPC();
            //UpdateTimerText();
            yield return null;
        }

        Debug.Log("El juego ha terminado");

        //isGameStarted = false;
        //scoreN1.Value = 0;
        //scoreN2.Value = 0;
        //ActualizarScoreClientRPC();
    }

    [ClientRpc]
    void ActualizarTimerClientRPC()
    {
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime.Value / 60);
        int seconds = Mathf.FloorToInt(elapsedTime.Value % 60);
        txtTimer.text = string.Format("{0:00}:{1:00}", minutes,seconds);
    }
}
