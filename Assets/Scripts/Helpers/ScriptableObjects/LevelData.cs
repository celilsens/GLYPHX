using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level/Level Data")]

public class LevelData : ScriptableObject
{
    public string levelNumber;
    public string levelName;
    public List<EnemySpawnInfo> enemySpawns;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int count;
}