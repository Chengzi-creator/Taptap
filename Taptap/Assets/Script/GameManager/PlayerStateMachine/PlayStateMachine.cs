using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateMachine 
{
    public static PlayStateMachine instance;
    public static PlayStateMachine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayStateMachine();
                instance.Init();
            }
            return instance;
        }
    }
    private int levelIndex;
    private int waveIndex;
    private int money;
    IPlayState currentState;
    private IPlayState[] playStateList;

    private LevelDataSO levelDataSO;

    public enum PlayStateType
    {
        Build,
        Spawn
    }

    private void Init()
    {
        playStateList = new IPlayState[2];
        playStateList[(int)PlayStateType.Build] = new BuildState();
        playStateList[(int)PlayStateType.Spawn] = new SpawnState();
        levelDataSO = Resources.Load<LevelDataSO>("SO/LevelData");
        if(levelDataSO == null)
        {
            Debug.LogWarning("LevelData not found");
        }
    }

    public void ReInit(int levelIndex)
    {
        this.levelIndex = levelIndex;
        waveIndex = 0;
        money = 0;
        ChangeState(PlayStateType.Build);
        EnemyManager.Instance.ReInit();
        TowerManager.Instance.ReInit();
        MyGridManager.Instance.LoadLevel(levelIndex);
        MyGridManager.Instance.LoadLevelMap(levelIndex);
    }

    public void UpdateState(float deltaTime)
    {
        currentState.UpdateState(deltaTime);
        EnemyManager.Instance.Update(deltaTime);
        TowerManager.Instance.Update(deltaTime);
    }

    private void ChangeState(PlayStateType stateType)
    {
        currentState?.ExitState();
        currentState = playStateList[(int)stateType];
        currentState.EnterState();
    }

    public void EnemyDie(IEnemy enemy)
    {
        if(currentState is SpawnState)
        {
            (currentState as SpawnState).EnemyDie(enemy);
        }
    }

    public void StartSpawnState()
    {
        ChangeState(PlayStateType.Spawn);
    }

    public void BuildTower(ITowerManager.TowerType towerType, Vector2Int position , int faceDirection)
    {
        if(currentState is BuildState)
        {
            (currentState as BuildState).BuildTower(towerType, position, faceDirection);
        }
    }




    private class BuildState : IPlayState
    {
        public void EnterState()
        {}
        public void UpdateState(float deltaTime)
        {}
        public void ExitState()
        {}

        public void BuildTower(ITowerManager.TowerType towerType, Vector2Int position , int faceDirection)
        {
            int midCost = TowerManager.Instance.GetTowerAttribute(towerType).cost;
            if(PlayStateMachine.Instance.money < midCost)
                return;
            // if(MyGridManager.Instance.CanPutTower(position , towerType))
            if(MyGridManager.Instance.CanPutTower(position))
                return;
            PlayStateMachine.Instance.money -= midCost;
            TowerManager.Instance.CreateTower(towerType, position , faceDirection);
        }
    }
    
    private class SpawnState : IPlayState
    {
        private float totalTime;
        private int enemyIndex;
        private List<LevelDataSO.EnemyData> enemyList;
        public void EnterState()
        {
            totalTime = 0;
            enemyIndex = 0;
            enemyList = PlayStateMachine.Instance.levelDataSO.GetWaveData(PlayStateMachine.Instance.levelIndex , PlayStateMachine.Instance.waveIndex);
        }
        public void UpdateState(float deltaTime)
        {
            totalTime += deltaTime;
            while(enemyIndex < enemyList.Count)
            {
                if(enemyList[enemyIndex].spawnTime <= totalTime)
                {
                    // EnemyManager.Instance.CreateEnemy(enemyList[enemyIndex].type , PlayStateMachine.Instance.levelDataSO.GetBeginPosition(PlayStateMachine.Instance.levelIndex));
                    EnemyManager.Instance.CreateEnemy(enemyList[enemyIndex].type , MyGridManager.Instance.GetPath(PlayStateMachine.Instance.levelDataSO.GetBeginPosition(PlayStateMachine.Instance.levelIndex)));
                    enemyIndex++;
                }
                else
                {
                    break;
                }
            }
        }
        public void ExitState()
        {}

        public void EnemyDie(IEnemy enemy)
        {
            PlayStateMachine.Instance.money += enemy.Money;
            if(EnemyManager.Instance.AllEnemysCount() == 0 && enemyIndex == enemyList.Count)
            {
                if(PlayStateMachine.Instance.waveIndex == PlayStateMachine.Instance.levelDataSO.GetMaxWave(PlayStateMachine.Instance.levelIndex) - 1)
                {
                    Debug.Log("Victory!!!!");
                    return;
                }
                PlayStateMachine.Instance.waveIndex++;
                PlayStateMachine.Instance.ChangeState(PlayStateType.Build);
            }
        }
    }


}
