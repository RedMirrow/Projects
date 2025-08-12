using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObjects : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem deathParticles; // death particles
    [SerializeField]
    private Transform location;
    [SerializeField]
    private string deathSound;
    public delegate void deadObject(float dust);
    public static deadObject OnObjectDestroyed;
    public float health = 100f;
    public float dustHeld;
    public int dropChance;
    public GameObject healUp, SpeedUp, battery;
    public void Start() { 
        location = GetComponent<Transform>();
    }
    public virtual void TakeDmg(float damage) { 
        health -= damage;
        if (health < 0) {
            ReleaseParticles();
            // If the object has no dust/exp reward, dont recover the system power on its death
            // Otherwise the missiles would trigger the system power recovery
            if (dustHeld > 0) { OnObjectDestroyed(dustHeld); }
            
            FindObjectOfType<AudioManager>().Play(deathSound);
            Destroy(this.gameObject); 
            
        }

    }
    public void Drop() {
        if (dropChance != null)
        {
            int dropped = UnityEngine.Random.Range(0, dropChance);
            switch (dropped)
            {
                case 0:
                    Instantiate(healUp, location.position, location.rotation);
                    break;
                case 1:
                    Instantiate(SpeedUp, location.position, location.rotation);
                    break;
                case 2:
                    Instantiate(battery, location.position, location.rotation);
                    break;
                default:
                    break;
            }
        }
    }
    public virtual void OnDestroy() { }
    public virtual void ReleaseParticles() { if (deathParticles != null) { Instantiate(deathParticles, location.position, location.rotation); Drop(); }
         
    }
}
