using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private LevelData currentLevelData;

    [SerializeField] private Vector2 _spawnAreaMin = new Vector2(-26, -26);
    [SerializeField] private Vector2 _spawnAreaMax = new Vector2(26, 26);
    [SerializeField] private float _initialSpawnDelay = 0.5f;
    [SerializeField] private float _spawnInterval = 0.3f;

    private int _activeEnemyCount = 0;
    private bool _spawningCompleted = false;
    private bool _levelCompleted = false;

    public System.Action OnLevelCompleted;

    private void Start()
    {
        currentLevelData = GameManager.Instance.SelectedLevelData;

        if (currentLevelData == null)
        {
            Debug.LogError("Selected LevelData null!");
            return;
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_initialSpawnDelay);
        _activeEnemyCount = 0;

        foreach (var enemyInfo in currentLevelData.enemySpawns)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                Vector2 spawnPos = GetValidSpawnPosition();
                GameObject enemy = Instantiate(enemyInfo.enemyPrefab, spawnPos, Quaternion.identity);
                var enemyComp = enemy.GetComponent<Enemy>();

                _activeEnemyCount++;
                enemyComp.OnEnemyDeath += HandleEnemyDeath;
                yield return new WaitForSeconds(_spawnInterval);
            }
        }

        _spawningCompleted = true;

        if (_activeEnemyCount <= 0) LevelCompleted();
    }

    private void HandleEnemyDeath()
    {
        _activeEnemyCount--;

        if (_spawningCompleted && _activeEnemyCount <= 0) LevelCompleted();
    }

    private void LevelCompleted()
    {
        if (_levelCompleted) return;
        _levelCompleted = true;

        GameManager.Instance.UnlockNewLevel();
        OnLevelCompleted?.Invoke();
    }

    private Vector2 GetValidSpawnPosition()
    {
        return new Vector2(
            Random.Range(_spawnAreaMin.x, _spawnAreaMax.x),
            Random.Range(_spawnAreaMin.y, _spawnAreaMax.y)
        );
    }
}
