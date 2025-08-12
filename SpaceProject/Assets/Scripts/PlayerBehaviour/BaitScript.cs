using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitScript : MonoBehaviour
{
    public Rigidbody model;
    [SerializeField]
    private float range = 600f;
    [SerializeField]
    private float bulletSpeed = 90f;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody bullet = (Rigidbody)Instantiate(model, transform.position + transform.forward, transform.rotation);
        bullet.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        Destroy(bullet.gameObject, 3);
    }
}
