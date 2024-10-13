using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private static EnemyManager instance;
    public static EnemyManager Instance => instance;
    public static bool Init()
    {
        if(instance == null)
        {
            // instance = new GameObject("EnemyManager").AddComponent<EnemyManager>();
            instance = new EnemyManager();

            instance.prefabEnemyList = new Dictionary<EnemyType, GameObject>();
            instance.EnemyClassList = new Dictionary<EnemyType, Type>();
            instance.enemyList = new List<BaseEnemy>();
            instance.enemyPool = new Stack<BaseEnemy>[Enum.GetValues(typeof(EnemyType)).Length];
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
    private bool LoadData()
    {
        enemyConfig = Resources.Load<EnemyConfig>("EnemyConfig");
        prefabEnemyList[EnemyType.A] = Resources.Load<GameObject>("EnemyA");
        EnemyClassList[EnemyType.A] = typeof(EnemyA);

        if(instance.enemyConfig == null)
        {
            Debug.LogWarning("EnemyConfig not found");
            return false;
        }

        foreach(EnemyType type in Enum.GetValues(typeof(EnemyType)))
        {
            if(prefabEnemyList[type] == null)
            {
                Debug.LogWarning("Enemy Prefab " + type + " not found");
                return false;
            }
            if(EnemyClassList[type] == null)
            {
                Debug.LogWarning("Enemy Class " + type + " not found");
                return false;
            }
        }
        return true;
    }
    public enum EnemyType
    {
        A,
        B
    }
    public struct EnemyAttribute
    {
        public Vector3 maxHP;
        public Vector2 size;
        public float speed;
    }

    private Stack<BaseEnemy>[] enemyPool ; 
    private EnemyConfig enemyConfig;
    private Dictionary<EnemyType , GameObject> prefabEnemyList;
    private Dictionary<EnemyType , Type> EnemyClassList;
    private List<BaseEnemy> enemyList;
    // private HashSet<BaseEnemy> enemyList;
    private List<IEnemy>[,] enemyInGrid;
    public IEnemy CreateEnemy(EnemyType type , int pathIndex)
    {
        if(enemyPool[(int)type].Count > 0)
        {
            BaseEnemy enemy = enemyPool[(int)type].Peek();
            enemyPool[(int)type].Pop();
            enemy.ReInit(enemyConfig.GetEnemyAttribute(type) , pathIndex);
            enemyList.Add(enemy);
            return enemy;
        }
        else
        {
            BaseEnemy enemy = (BaseEnemy)Activator.CreateInstance(EnemyClassList[type]);
            GameObject gameObject = GameObject.Instantiate(prefabEnemyList[type]);
            enemy.Init(gameObject , enemyConfig.GetEnemyAttribute(type) , pathIndex);
            enemyList.Add(enemy);
            return enemy;
        }
    }


    public void Update(float deltaTime)
    {
        foreach(BaseEnemy enemy in enemyList)
        {
            enemy.OnUpDate(Time.deltaTime);
        }
        RefreshEnemyInGrid();
    }
    private void RefreshEnemyInGrid()
    {
        for (int i = 0; i < 25; i++)
            for (int j = 0; j < 25; j++)
                enemyInGrid[i, j].Clear();
        enemyList.Sort((a , b) => CmpEnemy(a , b));
        int midIndex = 0;
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
                    enemyInGrid[x, y].Add(enemyList[i]);
        }
        for(int i = enemyList.Count - 1 ; i >= midIndex ; i--)
        {
            enemyList[i].Die();
            enemyList.RemoveAt(i);
            enemyPool[(int)enemyList[i].Type].Push(enemyList[i]);
        }
    }
    private int CmpEnemy(BaseEnemy a , BaseEnemy b)
    {
        if(a.IsDead)
            return 1;
        if(b.IsDead)
            return -1;
        if(a.PathNodeIndex != b.PathNodeIndex)
            return b.PathNodeIndex - a.PathNodeIndex;
        return (b.MoveScale - a.MoveScale) > 0 ? 1 : -1;
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
    public List<IEnemy> GetEnemys(Vector2Int position)
    {
        return enemyInGrid[position.x , position.y];
    }
}
