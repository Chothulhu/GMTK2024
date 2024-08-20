using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject lizardEnemy;
    [SerializeField] GameObject gorillaEnemy;
    [SerializeField] GameObject bigEnemy;
    [SerializeField] GameObject bossEnemy;

    private float lizardSpawnCooldown;
    [SerializeField] private float startLizardSpawnCooldown;

    private float bigEnemySpawnCooldown;
    [SerializeField] private float startBigEnemySpawnCooldown;

    [SerializeField] private Transform[] spawnPositions;
    private int progress = 0;
    [SerializeField] private int requiredProgress;
    [SerializeField] private HealthBar progressBar;

    private void Start()
    {
        lizardSpawnCooldown = startLizardSpawnCooldown;
        bigEnemySpawnCooldown = startBigEnemySpawnCooldown;
        bossEnemy.SetActive(false);

        progressBar.SetMaxHealth(requiredProgress);
        progressBar.SetHealth(0);
    }
    private void Update()
    {
        if (lizardSpawnCooldown > 0)
        {
            lizardSpawnCooldown -= Time.deltaTime;
        }
        else
        {
            var rand = Random.Range(0, spawnPositions.Length);
            ObjectPoolManager.SpawnObject(lizardEnemy, spawnPositions[rand].position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            lizardSpawnCooldown = startLizardSpawnCooldown;
        }

        if (bigEnemySpawnCooldown > 0)
        {
            bigEnemySpawnCooldown -= Time.deltaTime;
        }
        else
        {
            var rand = Random.Range(0, spawnPositions.Length);
            ObjectPoolManager.SpawnObject(bigEnemy, spawnPositions[rand].position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
            bigEnemySpawnCooldown = startBigEnemySpawnCooldown;
        }

        if (progress >= requiredProgress)
        {
            //activate boss
            bossEnemy.SetActive(true);
        }
    }

    public void AddProgress(int value)
    {
        progress += value;
        progressBar.SetHealth(progress);
    }
}
