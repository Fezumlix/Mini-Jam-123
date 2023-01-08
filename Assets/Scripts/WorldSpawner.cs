using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    public float worldSpeed = 1f;
    public int[] lanes = { -25, 0, 25 }; // x coord
    
    public int[] currentLaneProgress = {300, 300, 300}; // z coord
    public int mapGeneratedUntil;
    
    public GameObject[] enemyPrefabs;
    public GameObject[] gatePrefabs;
    public GameObject xpPrefab;
    
    public float currentPowerLevel = 1f;

    private void Start()
    {
        ExtendMap(3000);
        mapGeneratedUntil = 3000;
    }

    private void Update()
    {
        transform.position += Vector3.forward * worldSpeed * Time.deltaTime;

        // check if we need to extend the map
        if (transform.position.z > mapGeneratedUntil - 1000)
        {
            ExtendMap(3000 + mapGeneratedUntil);
            currentPowerLevel += 2f;
            mapGeneratedUntil += 3000;
        }
    }

    private void ExtendMap(int goalDistance)
    {
        for (int lane = 0; lane < lanes.Length; lane++)
        {
            while (currentLaneProgress[lane] <= goalDistance)
            {
                // spawn random thing
                var random = Random.Range(0, 100);
                // odds:
                // 0-30: enemy
                // 31-34: gate
                // 35-100: xp chain
                if (random <= 30)
                {
                    SpawnEnemy(lane);
                } else if (random <= 34)
                {
                    SpawnGate(lane);
                } else
                {
                    SpawnXPChain(lane);
                }
            }
        }
    }

    // private void SpawnRandom()
    // {
    //     // choose a lane
    //     // take the current lane progress as weight
    //     int lane = ChooseLaneForNextSpawn();
    //     // choose a type to spawn
    //     // possible types are: enemy, gate, xp, empty
    //     // weights are respectively: 20, 10, 20 50
    //     
    //     float random = Random.Range(0f, 100f);
    //     if (random < 20f)
    //     {
    //         SpawnEnemy(lane);
    //     }
    //     else if (random < 30f)
    //     {
    //         SpawnGate(lane);
    //     }
    //     else if (random < 50f)
    //     {
    //         SpawnXPChain(lane);
    //     }
    //     else
    //     {
    //         currentLaneProgress[lane] += Random.Range(100, 250);
    //     }
    // }

    private void SpawnEnemy(int lane)
    {
        int distanceAdd = Random.Range(50, 200);
        int enemy = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyObject = Instantiate(enemyPrefabs[enemy], new Vector3(lanes[lane], 0,
            -(currentLaneProgress[lane] + distanceAdd)) + transform.position, Quaternion.identity, transform);
        enemyObject.GetComponent<Enemy>().health = Mathf.RoundToInt(currentPowerLevel * 15f * Random.Range(.5f, 1.5f));
        currentLaneProgress[lane] += distanceAdd + 50;
    }
    
    private void SpawnGate(int lane)
    {
        int distanceAdd = Random.Range(50, 200);
        int gate = Random.Range(0, gatePrefabs.Length);
        GameObject gateObject = Instantiate(gatePrefabs[gate], new Vector3(lanes[lane], 0,
            -(currentLaneProgress[lane] + distanceAdd)) + transform.position, Quaternion.identity, transform);
        gateObject.transform.rotation = Quaternion.Euler(90, 0, 0);
        currentLaneProgress[lane] += distanceAdd + 50;
    }

    private void SpawnXPChain(int lane)
    {
        int amount = Mathf.CeilToInt(Random.Range(5 + currentPowerLevel, 10 + currentPowerLevel * 2));
        currentLaneProgress[lane] += Random.Range(50, 100);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(xpPrefab, new Vector3(lanes[lane], 0, -currentLaneProgress[lane]) + transform.position, Quaternion.identity, transform);
            currentLaneProgress[lane] += 30;
        }
    }
}