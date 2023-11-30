using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BossSpawner : MonoBehaviourPunCallbacks
{
    public GameObject BossPrefab;

    public float minX, minY;
    public float maxX, maxY;
    private string enemyId;

    private void Start()
    {
        SpawnOverNetwork();
    }

    private void SpawnOverNetwork()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject(BossPrefab.name, new Vector2(10000f,100000f), Quaternion.identity);
        }
    }
}
