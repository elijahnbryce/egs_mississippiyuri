using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Enemy;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public EnemyType enemyType;   
        public int count;
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
        public float spawnInterval = 1f;
        public float postWaveDelay = 3f;
    }

    [Header("Prefab")]
    public GameObject enemyPrefab;     // Enemy prefab

    [Header("Waves")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform spawnedParent;

    [Header("Target")]
    [SerializeField] private Entity target; // Player or base

    [Header("UI - Wave Display")]
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI totalWaveText;

    private int currentWaveIndex = 0;

    private void Start()
    {
        InitializeWaveUI();
        ProcWaveSpawner(); // delete when yarn setup
    }

    public void ProcWaveSpawner() => StartCoroutine(SpawnWaves());

    private void InitializeWaveUI()
    {
        if (totalWaveText != null)
            totalWaveText.text = waves.Count.ToString();

        Debug.Log($"Total Waves: {waves.Count}");
    }

    public IEnumerator SpawnWaves()
    {
        if (currentWaveIndex < waves.Count)
        {
            UpdateCurrentWaveUI();

            Wave wave = waves[currentWaveIndex];

            Debug.Log($"Wave {currentWaveIndex + 1} Started");

            yield return StartCoroutine(SpawnWave(wave));

            yield return new WaitForSeconds(wave.postWaveDelay);

            currentWaveIndex++;
        }

        else Debug.Log("All waves complete");
    }

    private void UpdateCurrentWaveUI()
    {
        if (currentWaveText != null)
            currentWaveText.text = (currentWaveIndex +1).ToString();
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        foreach (EnemySpawnInfo enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                SpawnEnemy(enemyInfo);
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }
    }


    private void SpawnEnemy(EnemySpawnInfo info)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, spawnedParent);

        if (obj.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.enabled = false; //stop from running
            StartCoroutine(InitializeEnemy(enemy, info));
        }
        else
        {
            Debug.LogError("Enemy component missing!");
        }
    }


    private IEnumerator InitializeEnemy(Enemy enemy, EnemySpawnInfo info)
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy is NULL");
            yield break;
        }

        if (target == null)
        {
            Debug.LogError("Target is NULL!");
            yield break;
        }

        if (info.enemyType == null)
        {
            Debug.LogError("EnemyType is NULL!");
            yield break;
        }

        enemy.SetTarget(target);
        enemy.enabled = true;
        yield return null;
        enemy.Initialize(info.enemyType);
        Debug.Log($"Enemy initialized: {info.enemyType.name}");
    }
}