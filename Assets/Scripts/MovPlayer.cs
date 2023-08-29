using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class MovPlayer : NetworkBehaviour
{
    public float speed;
    public float rotationLerpSpeed;
    public GameObject ballPrefab;

    int score1;
    int score2;

    public TMP_Text txtScore1, txtScore2;

    public NetworkVariable<int> scoreN1 = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> scoreN2 = new NetworkVariable<int>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        Debug.Log("OnAwake");
    }

    void Start()
    {
        Debug.Log("OnStart");

        txtScore1 = GameObject.Find("Score1").GetComponent<TMP_Text>();
        txtScore2 = GameObject.Find("Score2").GetComponent<TMP_Text>();

        txtScore1.text = "Score = " + scoreN1.Value.ToString();
        txtScore2.text = "Score = " + scoreN2.Value.ToString();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn");
        if(IsOwnedByServer)
        {
            //StartCoroutine(gameRain());
        }

        if(IsServer)
        {
            Debug.Log("IsServer");
            StartCoroutine(gameRain());
        }

        int x = UnityEngine.Random.Range(0, 7);
        SetRandomColor(x);
    }

    void FixedUpdate()
    {
        if(!IsOwner)
        {
            return;
        }
        Vector3 mov = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        mov = transform.TransformDirection(mov);
        mov *= speed;
        transform.position += mov * Time.deltaTime;

        if(mov != Vector3.zero)
        {
            Quaternion rota = Quaternion.LookRotation(mov, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rota, rotationLerpSpeed);
        }

        //Pruebas
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddScore();
        }
    }

    IEnumerator gameRain()
    {
        yield return null;
        while(true)
        {
            int x = Random.Range(-10, 10);
            int z = Random.Range(-10, 10);
            GameObject ballInstance = Instantiate(ballPrefab, new Vector3(x, 10, z), Quaternion.identity);
            ballInstance.GetComponent<NetworkObject>().Spawn();
            yield return new WaitForSecondsRealtime(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return; 
        
        if (other.gameObject.tag == "Pelota")
        {
            AddScore();
            //other.gameObject.GetComponent<NetworkObject>().NetworkHide(OwnerClientId);
            //Destroy(other.gameObject);

            //other.gameObject.GetComponent<BallScript>().TakeBall();

            //DespawnBallServerRPC(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;

        Debug.Log("Collision");

        if (collision.gameObject.tag == "Pelota")
        {
            Debug.Log("Ball Collision" + OwnerClientId);
            CambiarOwnerDePelotaServerRPC(collision.gameObject, OwnerClientId);
        }        
    }

    public void AddScore()
    {
        //Debug.Log("Add Score");
        coreManager.singleton.SumarScoresServerRPC((ulong)OwnerClientId);
    }

    [ServerRpc]
    public void DespawnBallServerRPC(NetworkObjectReference ball)
    {
        Debug.Log(ball + " " + ball.NetworkObjectId);
        ((GameObject)ball).GetComponent<NetworkObject>().Despawn();
    }

    void SetRandomColor(int color)
    {
        if (color == 1)
        {
           transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
        }
        else if (color == 2)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (color == 3)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (color == 4)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        }
        else if (color == 5)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (color == 6)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
        }
        else if (color == 7)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    [ServerRpc]
    public void CambiarOwnerDePelotaServerRPC(NetworkObjectReference ball, ulong newClientId)
    {
        Debug.Log("si entra y ball es: " + ball);
        ((GameObject)ball).GetComponent<NetworkObject>().ChangeOwnership(newClientId);
    }
}
