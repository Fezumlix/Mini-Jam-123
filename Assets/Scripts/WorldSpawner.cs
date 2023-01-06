using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldSpawner : MonoBehaviour
{
    public float worldSpeed = 1f;
    public int[] lanes = { -25, 0, 25 }; // x coord
    
    public int[] currentLaneProgress = {300, 300, 300}; // z coord
    
    public Phase currentPhase = Phase.normal;
    public float progressOfCurrentPhase = 0f;
    
    public GameObject[] enemyPrefabs;
    public GameObject[] gatePrefabs;
    public GameObject xpPrefab;
    
    public int currentPowerLevel = 1;

    private void Start()
    {
        NormalPhase(15);
    }

    private void NormalPhase(int amount)
    {
        int enemyAmount = Random.Range(2, 4);
        int gateAmount = Random.Range(1, 3);
        int fillerAmount = amount - enemyAmount - gateAmount;
        char[] spawnOrder = new char[amount];
        for (int i = 0; i < enemyAmount; i++)
        {
            spawnOrder[i] = 'e';
        }
        for (int i = enemyAmount; i < enemyAmount + gateAmount; i++)
        {
            spawnOrder[i] = 'g';
        }
        for (int i = enemyAmount + gateAmount; i < 10; i++)
        {
            spawnOrder[i] = 'f';
        }
        spawnOrder = spawnOrder.OrderBy(x => Random.value).ToArray();
        for (int i = 0; i < amount; i++)
        {
            int lane = ChooseLaneForNextSpawn();
            switch (spawnOrder[i])
            {
                case 'e':
                    SpawnEnemy(lane);
                    break;
                case 'g':
                    // SpawnGate(lane);
                    Debug.Log("Gate");
                    break;
                case 'f':
                    // spawn either xp or nothing
                    if (Random.value < 0.5f)
                    {
                        SpawnXPChain(lane);
                    } else
                    {
                        currentLaneProgress[lane] += Random.Range(100, 200);
                    }
                    break;
            }
        }
    }

    private void SpawnRandom()
    {
        // choose a lane
        // take the current lane progress as weight
        int lane = ChooseLaneForNextSpawn();
        // choose a type to spawn
        // possible types are: enemy, gate, xp, empty
        // weights are respectively: 20, 10, 20 50
        
        float random = Random.Range(0f, 100f);
        if (random < 20f)
        {
            SpawnEnemy(lane);
        }
        else if (random < 30f)
        {
            SpawnGate(lane);
        }
        else if (random < 50f)
        {
            SpawnXPChain(lane);
        }
        else
        {
            currentLaneProgress[lane] += Random.Range(100, 250);
        }
    }

    private int ChooseLaneForNextSpawn()
    {
        // use the current lane progress as weight
        // the weight per lane is 1 / (currentLaneProgress / totalLaneProgress)
        float[] weights = new float[lanes.Length];
        float total = currentLaneProgress.Sum();
        for (int i = 0; i < lanes.Length; i++)
        {
            weights[i] = (currentLaneProgress[i] / total);
        }
        
        float random = Random.value;
        if (random < weights[0])
        {
            return 0;
        }
        else if (random < weights[0] + weights[1])
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private void SpawnEnemy(int lane)
    {
        int distanceAdd = Random.Range(250, 400);
        int enemy = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyObject = Instantiate(enemyPrefabs[enemy], new Vector3(lanes[lane], 0,
            -(currentLaneProgress[lane] + distanceAdd)), Quaternion.identity, transform);
        enemyObject.GetComponent<Enemy>().health = currentPowerLevel * 25;
        currentLaneProgress[lane] += distanceAdd + 50;
    }
    
    private void SpawnGate(int lane)
    {        
        int distanceAdd = Random.Range(250, 400);
        int gate = Random.Range(0, gatePrefabs.Length);
        GameObject gateObject = Instantiate(gatePrefabs[gate], new Vector3(lanes[lane], 0,
            -(currentLaneProgress[lane] + distanceAdd)), Quaternion.identity, transform);
        gateObject.GetComponent<Gate>().strength = currentPowerLevel;
        currentLaneProgress[lane] += distanceAdd + 50;
    }

    private void SpawnXPChain(int lane)
    {
        int amount = Random.Range(5 + currentPowerLevel, 10 + currentPowerLevel * 2);
        currentLaneProgress[lane] += Random.Range(50, 100);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(xpPrefab, new Vector3(lanes[lane], 0, -(currentLaneProgress[lane])), Quaternion.identity, transform);
            currentLaneProgress[lane] += 30;
        }
    }

    private void Update()
    {
        // apply movement to all children
        foreach (Transform child in transform)
        {
            // add z
            child.position += new Vector3(0, 0, worldSpeed * Time.deltaTime);
        }
    }
}

public enum Phase
{
    normal,
    preBoss,
    boss
}