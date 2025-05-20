using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevelData;
    public LevelData[] allLevels;


    public Vector2 spawnAreaMin = new Vector2(-26, -26);
    public Vector2 spawnAreaMax = new Vector2(26, 26);

    public float initialSpawnDelay = 0.5f;
    public float spawnInterval = 0.3f;
    public float forbiddenRange = 7f;

    public static int currentLevelIndex = 0;
    private int _totalEnemiesSpawned;
    private int _enemiesDefeated;

    public System.Action OnLevelCompleted;

    void Start()
    {
        currentLevelIndex = PlayerPrefs.GetInt("SelectedLevel", 0);
        if (allLevels != null && allLevels.Length > 0 && currentLevelIndex < allLevels.Length)
        {
            currentLevelData = allLevels[currentLevelIndex];
        }
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        if (currentLevelData == null)
        {
            Debug.LogWarning("Level Data is Null!!!");
            yield break;
        }

        _totalEnemiesSpawned = 0;
        _enemiesDefeated = 0;

        foreach (EnemySpawnInfo enemyInfo in currentLevelData.enemySpawns)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                Vector2 spawnPosition = GetValidSpawnPosition();
                GameObject enemyGO = Instantiate(enemyInfo.enemyPrefab, spawnPosition, Quaternion.identity);
                _totalEnemiesSpawned++;
                Enemy enemyComp = enemyGO.GetComponent<Enemy>();

                if (enemyComp != null)
                {
                    enemyComp.OnEnemyDeath += HandleEnemyDeath;
                }
                else
                {
                    Debug.LogWarning("Enemy component not found on spawned prefab");
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void HandleEnemyDeath()
    {
        _enemiesDefeated++;

        if (_enemiesDefeated >= _totalEnemiesSpawned)
        {
            LevelCompleted();
        }
    }

    private void LevelCompleted()
    {
        Debug.Log("Level Completed!");
        OnLevelCompleted?.Invoke();
    }

    Vector2 GetValidSpawnPosition()
    {
        Vector2 position;

        int maxAttempts = 100;
        int attempt = 0;

        do
        {
            position = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y));
            attempt++;
            if (attempt > maxAttempts)
            {
                Debug.LogWarning("Valid Position is not found. Using current position.");
                break;
            }
        }
        while (IsInsideForbiddenZone(position));

        return position;
    }

    bool IsInsideForbiddenZone(Vector2 position)
    {
        return position.x > -forbiddenRange && position.x < forbiddenRange && position.y > -forbiddenRange && position.y < forbiddenRange;
    }

    public void SetNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex < allLevels.Length)
        {
            currentLevelData = allLevels[currentLevelIndex];
        }
    }
}
