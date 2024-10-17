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
    private int Money
    {
        get => money;
        set
        {
            // UIManager.Instance.SetMoney(value);
            UIManager.Instance.coinChange(value);
            money = value;
        }
    }
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
        Money = levelDataSO.GetBeginMoney(levelIndex);
        ChangeState(PlayStateType.Build);
        EnemyManager.Instance.ReInit();
        TowerManager.Instance.ReInit();
        MyGridManager.Instance.LoadLevel(levelIndex);
        // MyGridManager.Instance.LoadLevelMap(levelIndex);
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
        UIManager.Instance.RoundChange(PlayStateMachine.Instance.waveIndex , PlayStateMachine.Instance.levelIndex);
    }

    public void EnemyDie(IEnemy enemy)
    {
        if(currentState is SpawnState)
        {
            (currentState as SpawnState).EnemyDie(enemy);
        }
    }

//    int cnt = 0;
    public void StartSpawnState()
    {
        if(currentState is BuildState)
        {
//            if (cnt > 0)
//                return;
//            cnt++;
//            return;
            Debug.Log("CalculatePath Begin");
            MyGridManager.Instance.CalculatePath(true);
            Debug.Log("Spawn State Start");
            ChangeState(PlayStateType.Spawn);
        }
    }

    public void BuildTower(ITowerManager.TowerType towerType, Vector2Int position , int faceDirection)
    {
        if(currentState is BuildState)
        {
        Debug.Log("BuildTower " + towerType + " in " + position);
            (currentState as BuildState).BuildTower(towerType, position, faceDirection);
        }
    }

    public void RemoveTower(Vector2Int position)
    {
        Debug.Log("RemoveTower " + position);
        if(currentState is BuildState)
        {
            (currentState as BuildState).RemoveTower(position);
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
            //Debug.Log(PlayStateMachine.Instance.Money + " midCost" + midCost);
            if(PlayStateMachine.Instance.Money < midCost)
                return;
            if(MyGridManager.Instance.CanPutTower(towerType , position) == false)
                return;
            PlayStateMachine.Instance.Money -= midCost;
            TowerManager.Instance.CreateTower(towerType, position , faceDirection);
            //Debug.Log("BuildTower " + towerType + " " + position + " succeed");
            // Debug.Log("cost " + midCost);
        }
        public void RemoveTower(Vector2Int position)
        {
            ITower tower = TowerManager.Instance.GetTower(position);
            if(tower == null)
                return;
            tower = TowerManager.Instance.DestroyTower(tower);
            PlayStateMachine.Instance.Money += tower.Cost;
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
            PlayStateMachine.Instance.Money += enemy.Money;
            Debug.Log("Enemy Die all enemy " + EnemyManager.Instance.AllEnemysCount() + " enemyIndex " + enemyIndex);
            if(EnemyManager.Instance.AllEnemysCount() == 0 && enemyIndex == enemyList.Count)
            {
                if(PlayStateMachine.Instance.waveIndex == PlayStateMachine.Instance.levelDataSO.GetMaxWave(PlayStateMachine.Instance.levelIndex) - 1)
                {
                    Debug.Log("Victory!!!!");
                    UIManager.Instance.overMasksOn();
                    return;
                }
                PlayStateMachine.Instance.waveIndex++;
                PlayStateMachine.Instance.ChangeState(PlayStateType.Build);
                // UIManager.Instance.RoundChange(PlayStateMachine.Instance.waveIndex , PlayStateMachine.Instance.levelIndex);
            }
        }
    }


}
