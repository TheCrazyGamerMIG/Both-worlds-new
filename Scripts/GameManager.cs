using UnityEngine;
using System.Collections;

/// <summary>
/// Manages enemy spawning, round timer, and player kill counts.
/// Assigns patrol points and enemy type data on spawn.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Enemy Prefabs")]
    [Tooltip("All enemy prefabs that can spawn.")]
    public EnemyAI[] spawnPrefabs;

    [Header("Spawn Points")]
    [Tooltip("Spawn points in the scene with patrol points assigned.")]
    public EnemySpawnPoint[] spawnPoints;

    [Header("Spawning Settings")]
    [Tooltip("Time in seconds between spawn attempts.")]
    public float spawnInterval = 2f;
    [Tooltip("Maximum enemies alive at one time.")]
    public int maxConcurrentEnemies = 10;

    private int currentEnemies = 0;

    [Header("Gameplay Settings")]
    [Tooltip("Total round duration in seconds.")]
    public float roundDuration = 300f;
    private float roundTimer;
    private bool roundActive = true;

    private int player1Kills = 0;
    private int player2Kills = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        roundTimer = roundDuration;
        StartCoroutine(EnemySpawnLoop());
    }

    private void Update()
    {
        if (!roundActive) return;

        roundTimer -= Time.deltaTime;

        if (roundTimer <= 0f)
            EndRound();
    }

    /// <summary>
    /// Continuously spawns enemies while the round is active.
    /// Respects maxConcurrentEnemies limit and spawnInterval timing.
    /// </summary>
    private IEnumerator EnemySpawnLoop()
    {
        while (roundActive)
        {
            if (currentEnemies < maxConcurrentEnemies && spawnPoints.Length > 0 && spawnPrefabs.Length > 0)
                SpawnEnemy();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Spawns an enemy at a random spawn point and assigns patrol points.
    /// Subscribes to its OnEnemyDeath event.
    /// </summary>
    private void SpawnEnemy()
    {
        // Pick a random spawn point
        EnemySpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Pick a random enemy prefab
        EnemyAI prefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];

        // Spawn enemy using the spawn point's method
        EnemyAI enemy = spawnPoint.SpawnEnemy(prefab);
        if (enemy != null)
        {
            currentEnemies++;

            // Subscribe to death event
            enemy.OnEnemyDeath += OnEnemyKilled;
        }
    }

    /// <summary>
    /// Called when an enemy dies. Updates kill counts based on playerName.
    /// </summary>
    private void OnEnemyKilled(string playerName)
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);

        if (playerName == "Player 1") player1Kills++;
        else if (playerName == "Player 2") player2Kills++;
    }

    /// <summary>
    /// Ends the round, stops spawning, and loads the appropriate win/draw scene.
    /// </summary>
    private void EndRound()
    {
        roundActive = false;
        StopAllCoroutines();

        if (player1Kills > player2Kills)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Player1WinScene");
        else if (player2Kills > player1Kills)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Player2WinScene");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("DrawScene");
    }

    // UI getters
    public int GetPlayer1Kills() => player1Kills;
    public int GetPlayer2Kills() => player2Kills;
    public float GetRemainingTime() => roundTimer;
}
