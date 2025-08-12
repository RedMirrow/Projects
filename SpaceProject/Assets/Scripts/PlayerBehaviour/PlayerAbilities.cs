using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    // Referred to so heat methods can be called
    private PlayerShooting shooting;
    private ShipControl control;
    private PlayerStats player;
    private PlayerHealth playerHealth;
    private PlayerHeat heatSystem;
    private Transform location;
    bool boosting = false;

    [SerializeField]
    private ParticleSystem healParticles; // heal effect particles


    // Start is called before the first frame update
    void Start()
    {
        location = GetComponent<Transform>();
        heatSystem = GetComponent<PlayerHeat>();
        playerHealth = GetComponent<PlayerHealth>();
        player = GetComponent<PlayerStats>();
        shooting = GetComponent<PlayerShooting>();
        control = GetComponent<ShipControl>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerHealth.timeSinceLastHit <= 0 && player.choiceHeal) {
            if (playerHealth.health < playerHealth.maxHealth) {
                Instantiate(healParticles, location.position, location.rotation);
                playerHealth.ChangeHealth(1f);
            }
            
        }
        // If Z or X is pressed
        if (Input.GetAxisRaw("AbilitySet1") != 0)
        {
            //  X 
            if (Input.GetAxisRaw("AbilitySet1") > 0)
            {

            }
            // Z
            else
            {

            }
        }
        

    }

}
