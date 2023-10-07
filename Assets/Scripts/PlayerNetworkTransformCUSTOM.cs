using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerNetworkTransformCUSTOM : NetworkBehaviour
{
    //NetworkVariable<float> x = new NetworkVariable<float>();
    //NetworkVariable<float> y = new NetworkVariable<float>();
    //NetworkVariable<float> z = new NetworkVariable<float>();

    public NetworkVariable<Vector3> playerPos = new NetworkVariable<Vector3>(writePerm : NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> playerRot = new NetworkVariable<Quaternion>(writePerm : NetworkVariableWritePermission.Owner);

    [SerializeField] NetworkVariable<PlayerData> pD = new(writePerm: NetworkVariableWritePermission.Owner);

    NetworkVariable<int> playerColor = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Owner);

    struct PlayerData : INetworkSerializable
    {
        public float x, z;
        public float playerRot;

        internal Vector3 position
        {
            get => new Vector3(x, 0, z);
            set { x = value.x; z = value.z; }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            //throw new System.NotImplementedException();

            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref z);
            serializer.SerializeValue(ref playerRot);
        }
    }

    public override void OnNetworkSpawn()
    {
        playerColor.OnValueChanged += onPaletteColorChanged;
    }

    void onPaletteColorChanged(int prevValue, int newValue)
    {
        if(newValue == 1)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        }
        if (newValue == 2)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        }
        if (newValue == 3)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
        }
    }


    void Update()
    {
        /*if (IsOwner)
        {
            playerPos.Value = transform.position;
            playerRot.Value = transform.rotation;
        }
        else
        {
            transform.position = playerPos.Value;
            transform.rotation = playerRot.Value;
        }*/

        if (IsOwner)
        {
            var playerDatas = new PlayerData();

            playerDatas.position = transform.position;
            playerDatas.playerRot = transform.eulerAngles.y;
            pD.Value = playerDatas;

            Debug.Log("Player Data: "+playerDatas);
        }
        else
        {
            transform.position = pD.Value.position;
            transform.rotation = Quaternion.Euler(0, pD.Value.playerRot, 0);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            playerColor.Value = 1;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            playerColor.Value = 2;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            playerColor.Value = 3;
        }
    }
}
