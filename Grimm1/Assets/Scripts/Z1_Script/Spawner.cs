using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyprefab;
    public Transform spawnloc;
    public List<GameObject> enemyList = new List<GameObject>();
    [SerializeField]
    float spawndelay;
    [SerializeField]
    float spawnlimit;
    float currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnz();
    }

    public void spawnz()
    {
        if (enemyList.Count < spawnlimit)
        {

            currentTime += Time.deltaTime;
            if (currentTime > spawndelay)
            {
                spawndelay += currentTime;
                enemyList.Add(Instantiate(enemyprefab, spawnloc.position, Quaternion.identity));
                spawndelay -= currentTime;
                currentTime = 0.0f;
            }
        }
    }
}