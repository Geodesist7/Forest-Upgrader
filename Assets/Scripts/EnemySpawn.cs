using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timeToSpawn;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private void Spawn() 
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }
    IEnumerator Spawner() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeToSpawn);
            Spawn();
        }
    }
}
