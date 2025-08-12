using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    public GameObject[] asteroidObjs;
    public GameObject rock, rockSmall, rockLarge, rockCluster;
    private int select = 0;
    public int amountToSpawn = 400;
    public float minRandSpawn = -1000f;
    public float maxRandSpawn = 1000f;


    private void Start() { SpawnAsteroid(); }
    private void SpawnAsteroid() {
        for (int i = 0; i < amountToSpawn; i++) {
            select = Random.Range(1, 5);
            float randomX = Random.Range(minRandSpawn,maxRandSpawn);
            float randomY = Random.Range(minRandSpawn, maxRandSpawn);
            float randomZ = Random.Range(minRandSpawn, maxRandSpawn);
            Vector3 randSpawnPoint = new Vector3(transform.position.x + randomX, 
                transform.position.y + randomY, 
                transform.position.z + randomZ);
            GameObject tempObj;
            if (select == 1) { tempObj = rockSmall; }
            else if (select == 2) { tempObj = rock; }
            else if (select == 3) { tempObj = rockLarge; }
            else { tempObj = rockCluster; }

            tempObj = Instantiate(asteroidObjs[0], randSpawnPoint, Random.rotation);
            tempObj.transform.parent = this.transform;
        }
    }
}
