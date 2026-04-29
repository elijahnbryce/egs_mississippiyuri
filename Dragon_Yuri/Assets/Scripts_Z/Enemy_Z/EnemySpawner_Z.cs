using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// Spawns enemies in waves spawning each enemy after x interval (fast spawns or slow spawns). Each wave has its own spawn timing for each enemy (uniform) and cooldown afterwards before the next one auto starts. to burst enemies, set a low interval
public class EnemySpawner_Z : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public EnemyData enemyData;   // ScriptableObject data
        public GameObject prefab;     // Enemy prefab
        public int count;             // How many to spawn of that enemy
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
        public float spawnInterval = 1f;   // time between each spawn (Uniform)
        public float postWaveDelay = 3f;   // delay after wave finishes before next one begins
    }

    [Header("Waves")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    [Header("Spawn Settings")]
    [Tooltip("8 directions")]
    [SerializeField] private Transform[] spawnPoints; //points to choose from to spawn the enemies


    [Header("UI - Wave Display")]
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI totalWaveText;


    private int currentWaveIndex = 0;
    private bool isSpawning = false;  //for ui

    private void Start()
    {
        InitializeWaveUI();
        StartCoroutine(SpawnWaves());
    }


    /// Sets total wave count once at start
    private void InitializeWaveUI()
    {
        if (totalWaveText != null)
        {
            totalWaveText.text = waves.Count.ToString();
        }

        Debug.Log($"Total Waves Set: {waves.Count}");

    }



    private IEnumerator SpawnWaves()
    {
        isSpawning = true;

        while (currentWaveIndex < waves.Count)
        {
            UpdateCurrentWaveUI();

            Wave wave = waves[currentWaveIndex];

            Debug.Log($"Wave {currentWaveIndex + 1} Started");

            yield return StartCoroutine(SpawnWave(wave));

            isSpawning = false; //for ui

            // wait after wave for cooldown
            yield return new WaitForSeconds(wave.postWaveDelay);



            currentWaveIndex++;

            isSpawning = true;
        }

        isSpawning = false;
        Debug.Log("All waves complete");
        //Game win
    }
    /// Updates current wave display
    private void UpdateCurrentWaveUI()
    {
        if (currentWaveText != null)
        {
            currentWaveText.text = (currentWaveIndex + 1).ToString();
        }

        Debug.Log($"Current Wave Updated: {currentWaveIndex + 1}");
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        foreach (EnemySpawnInfo enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                SpawnEnemy(enemyInfo);

                yield return new WaitForSeconds(wave.spawnInterval); //Time inbetween each spawn
            }
        }
    }

    private void SpawnEnemy(EnemySpawnInfo info)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemyObj = Instantiate(info.prefab, spawnPoint.position, Quaternion.identity);

        Enemy_Z enemy = enemyObj.GetComponent<Enemy_Z>();

        if (enemy != null)
        {
            enemy.Initialize(info.enemyData);
        }
    }





}