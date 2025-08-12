using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string name;
    [SerializeField]
    private ParticleSystem pickupEffect; // Pickup Particles

    [SerializeField]
    private ShipControl control;

    public float amount = 0.4f; // 40% improvement
    public float duration = 15f; // 15sec buff duration

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }
    void Pickup(Collider player)
    {

        Instantiate(pickupEffect, transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("Pickup");
        switch (name)
        {
            case "HealUp":
                // Player gains 20 health points or up to their max amount, whichever is lower
                
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                playerHealth.ChangeHealth(20f);

                break;
            case "Battery":
                // Picking up a battery pickup gives 15 sec to the power bar
                
                PlayerStats stats = player.GetComponent<PlayerStats>();
                stats.UpdatePower(15f);

                break;
            case "Speed":
                // Player loses a bit of heat
                
                PlayerHeat heatSystem = player.GetComponent<PlayerHeat>();
                heatSystem.currentHeat *= amount;
                if (heatSystem.currentHeat < heatSystem.minHeat) { heatSystem.currentHeat = heatSystem.minHeat; }

                break;
            default:
                Debug.LogWarning("Unknown Skill name");
                break;
        }
        Destroy(gameObject);


    }
}
