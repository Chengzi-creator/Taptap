using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : IEnemyManager
{
    private static EnemyManager instance;
    public static EnemyManager Instance => instance;
    public static bool Init()
    {
        if(instance == null)
        {
            instance = new EnemyManager();

            instance.prefabEnemyList = new Dictionary<IEnemyManager.EnemyType, GameObject>();
            instance.enemyList = new List<BaseEnemy>();
            instance.enemyPool = new Stack<BaseEnemy>[Enum.GetValues(typeof(IEnemyManager.EnemyType)).Length];
            for(int i = 0; i < instance.enemyPool.Length; i++)  instance.enemyPool[i] = new Stack<BaseEnemy>();
                instance.enemyInGrid = new List<IEnemy>[25, 25];
            for (int i = 0; i < 25; i++)
                for (int j = 0; j < 25; j++)
                    instance.enemyInGrid[i, j] = new List<IEnemy>();
            return instance.LoadData();
        }
        else
        {
            return false;
        }
    }
    public void ReInit()
    {
        for(int i = enemyList.Count - 1 ; i >= 0 ; i--)
        {
            enemyPool[(int)enemyList[i].Type].Push(enemyList[i]);
            enemyList[i].Die();
            enemyList.RemoveAt(i);
        }
        RefreshEnemyInGrid();
    }
    public void Close()
    {
        for(int i = enemyList.Count - 1 ; i >= 0 ; i--)
        {
            enemyPool[(int)enemyList[i].Type].Push(enemyList[i]);
            enemyList[i].IsClosed = true;
            enemyList[i].Die();
            enemyList.RemoveAt(i);
        }
        RefreshEnemyInGrid();
    }
    private bool LoadData()
    {
        enemyConfig = Resources.Load<EnemyConfig>("SO/EnemyConfig");
        prefabEnemyList[IEnemyManager.EnemyType.A] = Resources.Load<GameObject>("Prefab/Enemy/EnemyA");
        prefabEnemyList[IEnemyManager.EnemyType.B] = Resources.Load<GameObject>("Prefab/Enemy/EnemyB");
        prefabEnemyList[IEnemyManager.EnemyType.C] = Resources.Load<GameObject>("Prefab/Enemy/EnemyC");
        prefabEnemyList[IEnemyManager.EnemyType.D] = Resources.Load<GameObject>("Prefab/Enemy/EnemyD");
        prefabEnemyList[IEnemyManager.EnemyType.E] = Resources.Load<GameObject>("Prefab/Enemy/EnemyE");
        prefabEnemyList[IEnemyManager.EnemyType.F] = Resources.Load<GameObject>("Prefab/Enemy/EnemyF");
        prefabEnemyList[IEnemyManager.EnemyType.G] = Resources.Load<GameObject>("Prefab/Enemy/EnemyG");
        prefabEnemyList[IEnemyManager.EnemyType.H] = Resources.Load<GameObject>("Prefab/Enemy/EnemyH");
        prefabEnemyList[IEnemyManager.EnemyType.I] = Resources.Load<GameObject>("Prefab/Enemy/EnemyI");

        if (instance.enemyConfig == null)
        {
            Debug.LogWarning("EnemyConfig not found");
            return false;
        }

        foreach(IEnemyManager.EnemyType type in Enum.GetValues(typeof(IEnemyManager.EnemyType)))
        {
            if(prefabEnemyList.ContainsKey(type) == false)
            {
                Debug.LogWarning("Enemy Prefab " + type + " not found");
                return false;
            }
        }
        return true;
    }
    private float refreshTime = GlobalSetting.Instance.GlobalSettingSO.RefreshTime;
    private float currentRefreshTime = 0;
    private Stack<BaseEnemy>[] enemyPool ;
    private EnemyConfig enemyConfig;
    private Dictionary<IEnemyManager.EnemyType , GameObject> prefabEnemyList;
    private List<BaseEnemy> enemyList;
    private List<IEnemy>[,] enemyInGrid;


    private void RefreshEnemyInGrid()
    {
        for (int i = 0; i < 25; i++)
            for (int j = 0; j < 25; j++)
                enemyInGrid[i, j].Clear();
        enemyList.Sort((a , b) => CmpEnemy(a , b));
        int midIndex = enemyList.Count;
        for(int i = 0 ; i < enemyList.Count ; i++)
        {
            if(enemyList[i].IsDead)
            {
                midIndex = i;
                break;
            }
            Vector2Int lef;
            Vector2Int rig;
            enemyList[i].GetOccupyGrid(out lef ,out rig);
            for(int x = lef.x ; x <= rig.x ; x++)
                for(int y = lef.y ; y <= rig.y ; y++)
                {
                    enemyInGrid[x, y].Add(enemyList[i]);
                }
        }
        for(int i = enemyList.Count - 1 ; i >= midIndex ; i--)
        {
            BaseEnemy dieEnemy = enemyList[i];
            enemyPool[(int)enemyList[i].Type].Push(dieEnemy);
            enemyList[i].Die();
            enemyList.RemoveAt(i);
            PlayStateMachine.Instance.EnemyDie(dieEnemy);
        }
    }
    private int CmpEnemy(BaseEnemy a , BaseEnemy b)
    {
        if(a.IsDead)
            return 1;
        if(b.IsDead)
            return -1;
        return (a.DisToDestination - b.DisToDestination) > 0 ? 1 : -1;
    }
    public IEnemy GetKthEnemy(int k , Vector2Int position)
    {
        if(k >= enemyInGrid[position.x , position.y].Count)
            return null;
        return enemyInGrid[position.x , position.y][k];
    }
    public int EnemysCount(Vector2Int position)
    {
        return enemyInGrid[position.x , position.y].Count;
    }
    public int AllEnemysCount()
    {
        return enemyList.Count;
    }
    public List<IEnemy> GetEnemys(Vector2Int position)
    {
        return enemyInGrid[position.x , position.y];
    }
    public void Update(float deltaTime)
    {
        // Debug.Log(deltaTime);
        foreach(BaseEnemy enemy in enemyList)
        {
            enemy.OnUpDate(Time.deltaTime);
        }
        currentRefreshTime += deltaTime;
        if(currentRefreshTime > refreshTime)
        {
            RefreshEnemyInGrid();
            currentRefreshTime -= refreshTime;
        }
    }
    public IEnemy CreateEnemy(IEnemyManager.EnemyType type , int pathIndex)
    {
        BaseEnemy enemy;
        if(enemyPool[(int)type].Count > 0)
        {
            enemy = enemyPool[(int)type].Peek();
            enemyPool[(int)type].Pop();
            enemy.ReInit(enemyConfig.GetEnemyAttribute(type) , pathIndex);
        }
        else
        {
            enemy = GameObject.Instantiate(prefabEnemyList[type]).GetComponent<BaseEnemy>();
            enemy.Init(enemyConfig.GetEnemyAttribute(type) , pathIndex);
        }
        enemyList.Add(enemy);
        return enemy;
    }
}
