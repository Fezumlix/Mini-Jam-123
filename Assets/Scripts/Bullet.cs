using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float damage = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("XP") || other.gameObject.CompareTag("Gate"))
            return;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }
}