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
    [SerializeField] private GameObject bossWarningCanvas;
    public bool isStart = false;

    private void Start()
    {
        lizardSpawnCooldown = startLizardSpawnCooldown;
        bigEnemySpawnCooldown = startBigEnemySpawnCooldown;
        bossEnemy.SetActive(false);
        bossWarningCanvas.SetActive(false);
        progressBar.SetMaxHealth(requiredProgress);
        progressBar.SetHealth(0);
    }
    private void Update()
    {
        /*while (isStart)
        {*/
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
                progress = -9999999;
                //activate boss
                bossEnemy.SetActive(true);
                progressBar.gameObject.SetActive(false);
                StartCoroutine(BossWarning(4f, 0.3f));
            }
        /*}*/  
    }

    private IEnumerator BossWarning(float duration, float blinkRate)
    {
        while (duration > 0)
        {
            bossWarningCanvas.SetActive(true);
            CinemachineScreenShake.Instance.ShakeCamera(4f, 0.5f);
            duration -= Time.deltaTime;
            yield return new WaitForSeconds(blinkRate);
            duration -= blinkRate;
            bossWarningCanvas.SetActive(false);
        }
    }

    public void AddProgress(int value)
    {
        progress += value;
        progressBar.SetHealth(progress);
    }
}
