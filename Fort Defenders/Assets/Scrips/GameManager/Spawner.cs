using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;

    public float spawningRange = 15;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    WorldTimer worldTimer;


    public event System.Action<int> OnNewWave;

    private void Start()
    {
        worldTimer = FindObjectOfType<WorldTimer>();
        worldTimer.OnNightTime += NextWave;
    }

    private void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            int randomNumber = Random.Range(0, currentWave.enemiesToSpawn.Length);

            Vector3 randomSpawnPos = new Vector3(Random.Range(-spawningRange, spawningRange), 1, Random.Range(-spawningRange, spawningRange));

            Enemy spawnedEnemy = Instantiate(currentWave.enemiesToSpawn[randomNumber], transform.position + randomSpawnPos, transform.rotation) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            worldTimer.StartANewDay();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;


        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
        public Enemy[] enemiesToSpawn;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawningRange);
    }
}
