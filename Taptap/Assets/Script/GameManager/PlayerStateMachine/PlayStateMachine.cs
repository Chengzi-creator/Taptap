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
            UIManager.Instance.coinChange(value);
            money = value;
        }
    }
    private int lastWaveHP;
    private int lastWaveMoney;
    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Debug.Log("Game Over");
                UIManager.Instance.overMasksOn();
            }
            else
            {
                ChangeHomeHP(hp);
            }
        }
    }
    IPlayState currentState;
    private IPlayState[] playStateList;

    private LevelDataSO levelDataSO;

    public enum PlayStateType
    {
        Build,
        Spawn,
        Empty
    }

    
    public List<IEnemyManager.EnemyType> enemyTypeList ;
    public List<int> enemyCountList ;

    private void Init()
    {
        playStateList = new IPlayState[3];
        playStateList[(int)PlayStateType.Build] = new BuildState();
        playStateList[(int)PlayStateType.Spawn] = new SpawnState();
        playStateList[(int)PlayStateType.Empty] = new EmptyState();
        enemyTypeList = new List<IEnemyManager.EnemyType>();
        enemyCountList = new List<int>();
        levelDataSO = Resources.Load<LevelDataSO>("SO/LevelData");
        if(levelDataSO == null)
        {
            Debug.LogWarning("LevelData not found");
        }
        ChangeState(PlayStateType.Empty);
    }

    // public void EmptyFunction()
    // {
    //     Debug.Log("EmptyFunction");
    // }

    public void ReInit(int levelIndex)
    {
        this.levelIndex = levelIndex;
        waveIndex = 0;
        Money = levelDataSO.GetBeginMoney(levelIndex);
        MyGridManager.Instance.LoadLevel(levelIndex);
        ChangeState(PlayStateType.Build);
        EnemyManager.Instance.ReInit();
        TowerManager.Instance.ReInit();
        HP = 100;
    }

    // public void Close()
    // {
    //     EnemyManager.Instance.Close();
    //     TowerManager.Instance.Close();
    //     // MyGridManager.Instance.Close();
    //     MyGridManager.Instance.UnloadLevel();
    //     ChangeState(PlayStateType.Empty);
    //     Debug.Log("Close PlayStateMachine");

    // }
    public void UpdateState(float deltaTime)
    {
        // Debug.Log(deltaTime);
        currentState.UpdateState(deltaTime);
        EnemyManager.Instance.Update(deltaTime);
        TowerManager.Instance.Update(deltaTime);
        ColorBlockManager.Instance.OnUpdate(deltaTime);
    }

    public void RestartWave()
    {
        hp = lastWaveHP;
        Money = lastWaveMoney;
        ChangeState(PlayStateType.Build);
    }

    private void ChangeHomeHP(int hp)
    {
        // TowerManager.Instance.ChangeHomeHP(hp);
        MyGridManager.Instance.ChangeHomeHP(hp);
    }
    private void ChangeState(PlayStateType stateType)
    {
        currentState?.ExitState();
        currentState = playStateList[(int)stateType];
        currentState.EnterState();
        if(stateType != PlayStateType.Empty)
        {
            UIManager.Instance.RoundChange(PlayStateMachine.Instance.levelIndex , PlayStateMachine.Instance.waveIndex);
        }
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
            // Debug.Log("CalculatePath Begin");
            MyGridManager.Instance.CalculatePath(true);
            Debug.Log("Spawn State Start");
            ChangeState(PlayStateType.Spawn);
        }
    }

    public void BuildTower(ITowerManager.TowerType towerType, Vector2Int position , int faceDirection)
    {
        if(currentState is BuildState)
        {
        // Debug.Log("BuildTower " + towerType + " in " + position);
            (currentState as BuildState).BuildTower(towerType, position, faceDirection);
        }
        // PlayStateMachine.Instance.RemoveTower(position);
    }

    public void RemoveTower(Vector2Int position)
    {
        Debug.Log("RemoveTower " + position);
        if(currentState is BuildState)
        {
            (currentState as BuildState).RemoveTower(position);
        }
    }

    public void ExitPlayState()
    {
        // return ;
        EnemyManager.Instance.Close();
        Debug.Log("exit play state");
        TowerManager.Instance.Close();
        // MyGridManager.Instance.Close();
        MyGridManager.Instance.UnloadLevel();
        ChangeState(PlayStateType.Empty);
        Debug.Log("Close PlayStateMachine");
        return ;
    }

    
    private class SpawnState : IPlayState
    {

        private float totalTime;
        private int enemyIndex;
        private List<LevelDataSO.EnemyData> enemyList;
        public void EnterState()
        {
            UIManager.Instance.isSpawning = true;
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
        {
            // UIManager.Instance.isSpawning = false;
            EnemyManager.Instance.Close();
            ColorBlockManager.Instance.ReduceAllColorBlock();
        }

        public void EnemyDie(IEnemy enemy)
        {
            // UIManager.Instance.EnemyDie(enemy);
            PlayStateMachine.Instance.enemyCountList[PlayStateMachine.Instance.enemyTypeList.IndexOf(enemy.Type)]--;
            // UIManager.Instance.EnemyReduce(PlayStateMachine.Instance.enemyTypeList , PlayStateMachine.Instance.enemyCountList , enemy.Type);
            UIManager.Instance.ShowEnemyCountAndTypes(PlayStateMachine.Instance.enemyTypeList , PlayStateMachine.Instance.enemyCountList);
            if(enemy.IsArrived == false)
            {
                PlayStateMachine.Instance.Money += enemy.Money;
            }
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
            }
        }
    }


    private class EmptyState : IPlayState
    {
        public void EnterState()
        {}
        public void UpdateState(float deltaTime)
        {}
        public void ExitState()
        {}
    }

    
    private class BuildState : IPlayState
    {
        
        public void EnterState()
        {
            UIManager.Instance.isSpawning = false;
            List<LevelDataSO.EnemyData> enemyList = PlayStateMachine.Instance.levelDataSO.GetWaveData(PlayStateMachine.Instance.levelIndex , PlayStateMachine.Instance.waveIndex);
            PlayStateMachine.Instance.enemyTypeList.Clear();
            PlayStateMachine.Instance.enemyCountList.Clear();
            foreach(var enemyData in enemyList)
            {
                if(PlayStateMachine.Instance.enemyTypeList.Contains(enemyData.type))
                {
                    PlayStateMachine.Instance.enemyCountList[PlayStateMachine.Instance.enemyTypeList.IndexOf(enemyData.type)]++;
                }
                else
                {
                    PlayStateMachine.Instance.enemyCountList.Add(1);
                    PlayStateMachine.Instance.enemyTypeList.Add(enemyData.type);
                }
            }
            UIManager.Instance.ShowEnemyCountAndTypes(PlayStateMachine.Instance.enemyTypeList , PlayStateMachine.Instance.enemyCountList);
            //Debug.Log("show enemy count and types");
            //Debug.Log("Total enemy count " + enemyList.Count);

            MyGridManager.Instance.CalculatePath();
            MyGridManager.Instance.DrawPath();
            MyGridManager.Instance.CalculateAllGridCanPutTower();
            //Debug.Log("drawpath");
    
        }



        public void UpdateState(float deltaTime)
        {}
        public void ExitState()
        {
            PlayStateMachine.Instance.lastWaveHP = PlayStateMachine.Instance.HP;
            PlayStateMachine.Instance.lastWaveMoney = PlayStateMachine.Instance.Money;
            MyGridManager.Instance.ErasePath();
            MyGridManager.Instance.CalculateAllGridCanPutTower();
            // Debug.Log("closepath");
        }

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
            MyGridManager.Instance.ErasePath();
            MyGridManager.Instance.DrawPath();
            MyGridManager.Instance.CalculateAllGridCanPutTower();
        }

        public void RemoveTower(Vector2Int position)
        {
            if(position == Vector2Int.one * -1)
                return;
            ITower tower = TowerManager.Instance.GetTower(position);
            if(tower == null)
                return;
            tower = TowerManager.Instance.DestroyTower(tower);
            PlayStateMachine.Instance.Money += tower.Cost;
            MyGridManager.Instance.ErasePath();
            MyGridManager.Instance.DrawPath();
            MyGridManager.Instance.CalculateAllGridCanPutTower();
        }

    }

}

