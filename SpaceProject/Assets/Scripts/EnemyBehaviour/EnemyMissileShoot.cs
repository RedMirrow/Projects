using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject[] missileObjs;
    [SerializeField]
    private GameObject missile;

    [SerializeField]
    private int missileBarrage;
    [SerializeField]
    private float barrageCooldown;
    [SerializeField]
    private float currentCooldown;

    public void CheckCD() {
        if (currentCooldown / barrageCooldown >= 1f) { SpawnMissile(); currentCooldown = 0; }
    }
    public void FixedUpdate() { Cooldown();  CheckCD(); }
    public void Cooldown() { currentCooldown += 0.1f; }
    public void SpawnMissile()
    {
        for (int i = 0; i < missileBarrage; i++)
        {
            float X = 0.4f;
            float Y = 0.1f;
            float Z = 0.1f;
            Vector3 randSpawnPoint = new Vector3(transform.position.x + X,
                transform.position.y + Y,
                transform.position.z + Z);
            GameObject tempObj;
            tempObj = missile;
            tempObj = Instantiate(missileObjs[0], randSpawnPoint, transform.rotation);
        }
    }
}
