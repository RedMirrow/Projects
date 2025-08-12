using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyHealth : HealthObjects
{
    Rigidbody rb;
    bool collided = false;

    public override void TakeDmg(float damage)
{
    base.TakeDmg(damage);
}
public override void OnDestroy()
{
    base.OnDestroy();
}
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float damage = 30f;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collided)
        {
            if (collision.gameObject.GetComponent<PlayerHealth>() != null)
            {
                TakeDmg(damage);

                collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
                collided = true;
            }
        }
        collided = false;

    }

}
