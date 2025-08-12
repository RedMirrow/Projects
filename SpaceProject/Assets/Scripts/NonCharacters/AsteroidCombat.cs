using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCombat : MonoBehaviour
{
    public float damage = 1f;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null)
        {
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }

    }
}
