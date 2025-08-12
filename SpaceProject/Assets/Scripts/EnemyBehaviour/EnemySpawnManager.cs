using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Given a target and a limit of ships it will create the ships of a given type
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    private Transform targetPos;
    [SerializeField]
    private GameObject[] shipObjs;
    [SerializeField]
    private GameObject shipType;

    [SerializeField]
    private int shipCountLimit;
    private int select = 0;

    [SerializeField]
    private float minRandSpawn = -2000f;
    [SerializeField]
    private float maxRandSpawn = 2000f;

    

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
    public void Start() {getTarget(); }
    public void SpawnShip()
    {
        for (int i = 0; i < shipCountLimit; i++)
        {
            
            float randomX = UnityEngine.Random.Range(minRandSpawn, maxRandSpawn);
            float randomY = UnityEngine.Random.Range(minRandSpawn, maxRandSpawn);
            float randomZ = UnityEngine.Random.Range(minRandSpawn, maxRandSpawn);
            Vector3 randSpawnPoint = new Vector3(transform.position.x + randomX,
                transform.position.y + randomY,
                transform.position.z + randomZ);
            GameObject tempObj;
            tempObj = shipType;
            tempObj = Instantiate(shipObjs[0], randSpawnPoint, UnityEngine.Random.rotation);
            tempObj.transform.parent = this.transform;
        }
        shipCountLimit += 2;
        spawnCooldown += 15f;
    }
}
