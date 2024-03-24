using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]

    [Tooltip("Enemy spawn rate, speeds up tick rate")]
    [SerializeField] private float spawnRate = 1f;

    [Tooltip("The maximum number of enemies before spawning stops")]
    [SerializeField] private float entityMax = 5f;

    [Tooltip("The chance a slime will spawn each tick")]
    [SerializeField] private float spawnChance = 50f;
    
    private readonly float spawnTickRate = 3f;

    [Tooltip("Enemy that is spawned, NOTE: Only use slime prefab")]
    [SerializeField] private GameObject enemyPrefab;

    //Serialzed for debug purposes, can be unserialzed at a later date
    [SerializeField] private List<GameObject> spawnedEnemies;

    [SerializeField] private List<Transform> spawnLocations;

    private float spawnTime;
    private void Update()
    {
        if (spawnedEnemies.Count > entityMax) return;

        SpawnTick();
    }

    /// <summary>
    /// Calls enemy spawner every tick
    /// </summary>
    private void SpawnTick()
    {
        if (spawnTime <= spawnTickRate)
            spawnTime += Time.deltaTime * spawnRate;
        else
        {
            if (CanSpawn())
                SpawnEnemy();

            spawnTime = 0;
        }
    }

    /// <summary>
    /// Determines if an enemy will spawn this tick
    /// </summary>
    /// <returns></returns>
    private bool CanSpawn()
    {
        int currentChance = Random.Range(0, 100);

        if (spawnChance < currentChance)
            return true;

        return false;
    }
    /// <summary>
    /// Spawn an enemy at randomized position
    /// </summary>
    private void SpawnEnemy()
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, SpawnPosition().position, Quaternion.identity);

        enemyInstance.GetComponent<Slime>().OnEnemyDeath += RemoveEnemy;

        spawnedEnemies.Add(enemyInstance);
    }
    /// <summary>
    /// Returns a randomized spawn location between nodes placed in inspector
    /// </summary>
    /// <returns></returns>
    private Transform SpawnPosition()
    {
        return spawnLocations[Random.Range(0, spawnLocations.Count - 1)];
    }
    /// <summary>
    /// Remove Enemy from spawned enemies list
    /// </summary>
    /// <param name="enemyInstance"></param>
    public void RemoveEnemy(GameObject enemyInstance) => spawnedEnemies.Remove(enemyInstance);
}
