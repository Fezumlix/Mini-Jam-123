using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    public float speed = 1f;
    public GameObject laser;
    public Transform[] laserSpawnPoints;
    public GameObject xpEffect;

    public float fireRate = 2f;
    public float maxFireRate = 10f;
    public float damage = 1f;
    public float maxDamage = 10f;
    public float abilityPower = 1f;
    public float maxAbilityPower = 10f;
    
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 75;
    
    [Header("UI")]
    public Image xpBar;
    public Image attackSpeedBar;
    public Image attackDamageBar;
    public Image abilityPowerBar;
    public Text[] xpTexts;
    public Text[] attackSpeedTexts;
    public Text[] attackDamageTexts;
    public Text[] abilityPowerTexts;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0f, 1 / fireRate);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // clamp movement at +- 25
        float newX = Mathf.Clamp(transform.position.x + -horizontalInput * speed * Time.deltaTime * 10, -25f, 25f);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
    
    public void Fire()
    {
        foreach (Transform laserSpawnPoint in laserSpawnPoints)
        {
            Instantiate(laser, laserSpawnPoint.position, Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().velocity = Vector3.back * 120;
        }
    }
    
    private void LevelUp()
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("XP"))
        {
            Destroy(other.gameObject);
            // Instantiate(xpEffect, other.transform.position, Quaternion.identity);
            currentXP += 1;
            UpdateUI();
            if (currentXP >= xpToNextLevel)
            {
                LevelUp();
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            // @TODO: add game over
        }
    }

    public void UpdateUI()
    {
        xpBar.transform.localScale = new Vector3((float)currentXP / xpToNextLevel, 1, 1);
        attackSpeedBar.transform.localScale = new Vector3(fireRate / maxFireRate, 1, 1);
        attackDamageBar.transform.localScale = new Vector3(damage / maxDamage, 1, 1);
        abilityPowerBar.transform.localScale = new Vector3(abilityPower / maxAbilityPower, 1, 1);
        
        xpTexts[0].text = currentLevel.ToString();
        xpTexts[1].text = (currentLevel + 1).ToString();
        attackSpeedTexts[0].text = "2";
        attackSpeedTexts[1].text = "10";
        attackDamageTexts[0].text = "1";
        attackDamageTexts[1].text = "10";
        abilityPowerTexts[0].text = "1";
        abilityPowerTexts[1].text = "10";
    }

}
