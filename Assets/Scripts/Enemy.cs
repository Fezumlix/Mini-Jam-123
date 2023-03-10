using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    public GameObject deathEffect;
    public GameObject hitEffect;
    public int amountOfXP = 20;
    public GameObject xp;
    
    public Text healthText;

    public void Start()
    {
        healthText.text = health.ToString();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        //Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            Die();
        }
        healthText.text = Math.Round(health, 1).ToString();
    }

    private void Die()
    {
        for (int i = 0; i < amountOfXP; i++)
        {
            Vector3 dropPosition = transform.position;
            dropPosition.y = 0;
            dropPosition.x += Random.Range(-5f, 5f);
            dropPosition.z += Random.Range(-5f, 5f);
            Instantiate(xp, dropPosition, Quaternion.identity, transform.parent);
        }
        
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane2"))
        {
            Destroy(gameObject);
        }
    }
}