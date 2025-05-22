using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevelData;
    [SerializeField] private LevelData[] allLevels;

    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-26, -26);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(26, 26);

    [SerializeField] private float initialSpawnDelay = 0.5f;
    [SerializeField] private float spawnInterval = 0.3f;
    [SerializeField] private float forbiddenRange = 8f;
    private List<Vector2> _usedSpawnPositions = new List<Vector2>();
    private float minDistanceBetweenEnemies = 2f;

    private int _totalEnemiesSpawned;
    private int _enemiesDefeated;

    public static int currentLevelIndex = 0;

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

        _usedSpawnPositions.Clear();

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
        Debug.Log(currentLevelIndex + "Level Completed!");
        OnLevelCompleted?.Invoke();
        UnlockNextLevel();
    }

    Vector2 GetValidSpawnPosition()
    {
        Vector2 position;
        int maxAttempts = 100;
        int attempt = 0;

        do
        {
            position = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            attempt++;
            if (attempt > maxAttempts)
            {
                Debug.LogWarning("Uygun spawn pozisyonu bulunamadı. Son üretilen pozisyon kullanılacak.");
                break;
            }
        }
        while (
            IsInsideForbiddenZone(position) ||
            IsTooCloseToOtherEnemies(position)
        );

        _usedSpawnPositions.Add(position);
        return position;
    }


    bool IsInsideForbiddenZone(Vector2 position)
    {
        return position.x > -forbiddenRange && position.x < forbiddenRange &&
               position.y > -forbiddenRange && position.y < forbiddenRange;
    }

    bool IsTooCloseToOtherEnemies(Vector2 position)
    {
        foreach (Vector2 usedPos in _usedSpawnPositions)
        {
            if (Vector2.Distance(position, usedPos) < minDistanceBetweenEnemies)
            {
                return true;
            }
        }
        return false;
    }


    public void SetNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex < allLevels.Length)
        {
            currentLevelData = allLevels[currentLevelIndex];
        }
    }

    public void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 0);
        int nextLevel = unlockedLevel + 1;

        if (nextLevel < allLevels.Length)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();
            Debug.Log("Yeni Seviye Açıldı: " + nextLevel);
        }
    }

    public void LoadNextLevel()
    {
        int nextLevel = currentLevelIndex + 1;

        if (nextLevel < allLevels.Length)
        {
            PlayerPrefs.SetInt("SelectedLevel", nextLevel);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("Tüm seviyeler tamamlandı!");
        }
    }
}
