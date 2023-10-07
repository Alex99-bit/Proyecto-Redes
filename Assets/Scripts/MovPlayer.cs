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

    public NetworkVariable<int> scoreN1 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> scoreN2 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public static MovPlayer player;

    public NetworkVariable<int> Hp = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
        if (IsOwnedByServer)
        {
            //StartCoroutine(gameRain());
        }

        if (IsServer)
        {
            Debug.Log("IsServer");
            //StartCoroutine(gameRain());
        }

        int x = UnityEngine.Random.Range(0, 7);
        //SetRandomColor(x);

        if (IsOwner)
        {
            Hp.Value = 10;
        }
        Hp.OnValueChanged += NotifyHpChanged;
    }

    void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        Vector3 mov = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        mov = transform.TransformDirection(mov);
        mov *= speed;
        transform.position += mov * Time.deltaTime;

        if (mov != Vector3.zero)
        {
            Quaternion rota = Quaternion.LookRotation(mov, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rota, rotationLerpSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddScore();
        }
    }

    public void GameRainOn()
    {
        StartCoroutine(gameRain());
    }

    IEnumerator gameRain()
    {
        yield return null;
        while (true)
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
        if (!IsOwner) return; //si no es el admin no va a hacer nada UN SALUDO A LA GRASAAAAAAAAAAAAAAAAA

        /*if (other.gameObject.tag == "Pelota")
        {
            AddScore();

            other.gameObject.GetComponent<BallScript>().TakeBall();

            DespawnBallServerRPC(other.gameObject);
        }*/

        if (other.gameObject.tag == "Pelota")
        {
            if (other.gameObject.GetComponent<NetworkObject>().IsOwner)
            {
                CambiarOwnerDePelotaServerRPC(other.gameObject, OwnerClientId);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;

        if (collision.gameObject.tag == "Pelota")
        {
            CambiarOwnerDePelotaServerRPC(collision.gameObject, OwnerClientId);

            if (collision.gameObject.GetComponent<BallScript>().isSpikes)
            {
                Debug.Log("Dañar jugador");
                collision.gameObject.GetComponent<BallScript>().TakeBall();

                DespawnBallServerRPC(collision.gameObject);

                //Bajar vida al jugador
                ReducirVida(OwnerClientId);
            }
        }
    }

    public void AddScore()
    {
        Debug.Log("Add Score");
        coreManager.singleton.SumarScoresServerRPC((ulong)OwnerClientId);
    }

    [ServerRpc]
    public void DespawnBallServerRPC(NetworkObjectReference ball)
    {
        //Debug.Log(ball + " " + ball.NetworkObjectId);
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
        ((GameObject)ball).GetComponent<NetworkObject>().ChangeOwnership(newClientId);
    }

    void ReducirVida (ulong id)
    {
        coreManager.singleton.ReducirHPServerRPC(id);

        Hp.Value--;
    }

    void NotifyHpChanged(int previo, int current)
    {
        Debug.Log("Hp previo: " + previo + "current: " + current);

        gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
    }
}
