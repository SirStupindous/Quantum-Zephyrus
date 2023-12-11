using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    [SerializeField] float spawnY = 30f; 
    [SerializeField] float spawnXInterval = 20f;
    [SerializeField] float terrainWidth = 1000f;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (float x = 0; x <= terrainWidth; x += spawnXInterval)
        {
            GameObject enemy = enemyPrefab[Random.Range(0, enemyPrefab.Length)];

            Vector3 spawnPosition = new Vector3(x, spawnY, 0);
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }
}
