using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;

    [SerializeField] private int poolSize1 = 10;
    [SerializeField] private int poolSize2 = 5;

    [SerializeField] private float enemy1Interval = 3.5f;
    [SerializeField] private float enemy2Interval = 10f;

    private List<GameObject> enemy1Pool = new List<GameObject>();
    private List<GameObject> enemy2Pool = new List<GameObject>();

    void Start()
    {
        // Create pools
        for (int i = 0; i < poolSize1; i++)
        {
            GameObject obj = Instantiate(enemy1Prefab);
            obj.SetActive(false);
            enemy1Pool.Add(obj);
        }
        for (int i = 0; i < poolSize2; i++)
        {
            GameObject obj = Instantiate(enemy2Prefab);
            obj.SetActive(false);
            enemy2Pool.Add(obj);
        }

        // Start spawning
        StartCoroutine(SpawnEnemy(enemy1Interval, enemy1Pool));
        StartCoroutine(SpawnEnemy(enemy2Interval, enemy2Pool));
    }

    private IEnumerator SpawnEnemy(float interval, List<GameObject> pool)
    {
        yield return new WaitForSeconds(interval);

        GameObject enemy = GetPooledEnemy(pool);
        if (enemy != null)
        {
            enemy.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0);
            enemy.SetActive(true);
        }

        StartCoroutine(SpawnEnemy(interval, pool));
    }

    private GameObject GetPooledEnemy(List<GameObject> pool)
    {
        foreach (var enemy in pool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }
        return null; // pool empty
    }
}
