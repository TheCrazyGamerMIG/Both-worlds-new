using UnityEngine;

/// <summary>
/// Assign this to a spawn point in the scene.
/// Holds possible enemy types and patrol points.
/// Spawns enemies with proper type data and patrol routes.
/// </summary>
public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Tooltip("Enemy types that can spawn from this point.")]
    public EnemyTypeData[] possibleEnemies;

    [Header("Patrol Points")]
    [Tooltip("Assign 2 or more patrol points for the spawned enemy.")]
    public Transform[] patrolPoints;

    /// <summary>
    /// Spawns a random enemy from the possibleEnemies list at this spawn point.
    /// Automatically assigns patrol points to the enemy.
    /// </summary>
    /// <param name="enemyPrefab">Prefab of the enemy to spawn</param>
    /// <returns>The instantiated EnemyAI object</returns>
    public EnemyAI SpawnEnemy(EnemyAI enemyPrefab)
    {
        if (possibleEnemies == null || possibleEnemies.Length == 0)
        {
            Debug.LogWarning("No enemy types assigned to spawn point: " + gameObject.name);
            return null;
        }

        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab not assigned for spawn point: " + gameObject.name);
            return null;
        }

        // Pick a random enemy type
        EnemyTypeData chosenEnemy = possibleEnemies[Random.Range(0, possibleEnemies.Length)];

        // Instantiate enemy at this spawn point
        EnemyAI newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Assign type data safely via public method
        newEnemy.SetEnemyTypeData(chosenEnemy);

        // Assign patrol points via public method
        if (patrolPoints != null && patrolPoints.Length >= 2)
            newEnemy.AssignPatrolPoints(patrolPoints);
        else
            Debug.LogWarning("Not enough patrol points assigned for spawn point: " + gameObject.name);

        return newEnemy;
    }
}
