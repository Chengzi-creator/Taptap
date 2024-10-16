using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelDataSO : ScriptableObject
{
    [System.Serializable]
    public struct EnemyData
    {
        public IEnemyManager.EnemyType type;
        public float spawnTime;
    }
    [System.Serializable]
    public struct Wave
    {
        public List<EnemyData> enemyDataList;
    }
    [System.Serializable]
    public struct LevelData
    {
        public Vector2Int beginPosition;
        public List<Wave> waveList;
    }

    [SerializeField]
    private List<LevelData> levelData;

    public int GetMaxWave(int levelIndex)
    {
        return levelData[levelIndex].waveList.Count;
    }
    public List<EnemyData> GetWaveData(int levelIndex , int waveIndex)
    {
        return levelData[levelIndex].waveList[waveIndex].enemyDataList;
    }
    public Vector2Int GetBeginPosition(int levelIndex)
    {
        return levelData[levelIndex].beginPosition;
    }
}
