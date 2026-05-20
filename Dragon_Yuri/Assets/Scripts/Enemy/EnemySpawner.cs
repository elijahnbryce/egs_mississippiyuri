using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Assets.Scripts.Enemy;
using UnityEngine.SceneManagement;

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

    public static EnemySpawner _Instance;
    public UnityEvent OnWaveComplete;

    [Header("Prefab")]
    public GameObject enemyPrefab;

    [Header("Waves")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    [Header("End Game Settings")]
    [SerializeField] private int endWaveIndex = -1; // -1 = use last wave
    [SerializeField] private string winSceneName = "WinScene";

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform spawnedParent;

    [Header("Target")]
    [SerializeField] private Entity target;

    [Header("UI - Wave Display")]
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private TextMeshProUGUI totalWaveText;

    private int currentWaveIndex = 0;
    private bool isSpawning;
    private bool isWinTriggered;

    private List<Enemy> currentEnemies = new();
    public List<Enemy> CurrentEnemies => currentEnemies;

    public bool WaveCompleted => currentEnemies.Count <= 0;

    private void Awake()
    {
        if (_Instance == null) _Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeWaveUI();
    }

    public void ProcWaveSpawner()
    {
        if (isSpawning) return;

        StartCoroutine(SpawnWaves());
    }

    private void InitializeWaveUI()
    {
        if (totalWaveText != null)
            totalWaveText.text = waves.Count.ToString();
    }

    public IEnumerator SpawnWaves()
    {
        isSpawning = true;

        while (currentWaveIndex < waves.Count)
        {
            UpdateCurrentWaveUI();

            Wave wave = waves[currentWaveIndex];

            Debug.Log($"Wave {currentWaveIndex + 1} Started");

            yield return StartCoroutine(SpawnWave(wave));

            yield return new WaitUntil(() => WaveCompleted);

            yield return new WaitForSeconds(wave.postWaveDelay);

            currentWaveIndex++;

            OnWaveComplete?.Invoke();
        }

        isSpawning = false;

        TryEndGame();
    }

    private void UpdateCurrentWaveUI()
    {
        if (currentWaveText != null)
            currentWaveText.text = (currentWaveIndex + 1).ToString();
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
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject obj = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity,
            spawnedParent
        );

        if (obj.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.enabled = false;
            StartCoroutine(InitializeEnemy(enemy, info));
            currentEnemies.Add(enemy);
        }
    }

    private IEnumerator InitializeEnemy(Enemy enemy, EnemySpawnInfo info)
    {
        if (enemy == null || target == null || info.enemyType == null)
            yield break;

        enemy.SetTarget(target);

        enemy.enabled = true;

        yield return null;

        enemy.Initialize(info.enemyType);
    }

    public void DespawnEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        currentEnemies.Remove(enemy);

        Destroy(enemy.gameObject);

        TryEndGame();
    }

    private void TryEndGame()
    {
        if (isWinTriggered) return;

        bool lastWaveFinished =
            currentWaveIndex >= waves.Count;

        bool noEnemiesLeft =
            currentEnemies.Count == 0;

        if (lastWaveFinished && noEnemiesLeft)
        {
            isWinTriggered = true;
            StartCoroutine(WinRoutine());
        }
    }

    private IEnumerator WinRoutine()
    {
        Debug.Log("Game Won - loading scene");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(winSceneName);
    }
}