using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform targetPos;

    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject[] shipObjs;
    [SerializeField]
    private GameObject shipType;

    [SerializeField]
    private int shipCountLimit;
    private int select = 0;

    [SerializeField]
    private float minRandSpawn = 1f;
    [SerializeField]
    private float maxRandSpawn = 4f;



    [SerializeField]
    private float spawnCooldown;
    [SerializeField]
    private float currentCooldown;

    public void CheckCD()
    {
        if (currentCooldown / spawnCooldown >= 1f) { SpawnShip(); currentCooldown = 0; }
    }

    // It is added to the code to instantly kill all enemy ship objects to reduce post death lag
    void getTarget()
    {
        try
        {
            if (targetPos == null) { targetPos = GameObject.Find("Player").transform; }
        }
        catch (Exception e)
        {
            if (targetPos == null) { Destroy(this.gameObject); }
        }
    }
    public void FixedUpdate() { Cooldown(); CheckCD(); getTarget(); }
    public void Cooldown() { currentCooldown += 0.01f; }
    public void Start() { SpawnShip(); getTarget(); }
    public void SpawnShip()
    {
        for (int i = 0; i < shipCountLimit; i++)
        {

            float randomX = UnityEngine.Random.Range(minRandSpawn, maxRandSpawn);
            float randomY = UnityEngine.Random.Range(minRandSpawn, maxRandSpawn);
            float randomZ = UnityEngine.Random.Range(minRandSpawn, maxRandSpawn);
            Vector3 randSpawnPoint = new Vector3(spawnPoint.position.x + randomX,
                spawnPoint.position.y + randomY,
                spawnPoint.position.z + randomZ);
            GameObject tempObj;
            tempObj = shipType;
            tempObj = Instantiate(shipObjs[0], randSpawnPoint, UnityEngine.Random.rotation);
        }
        shipCountLimit += 2;
        spawnCooldown += 5f;
    }
}

