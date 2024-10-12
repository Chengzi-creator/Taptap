using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance => instance;
    public static bool Init()
    {
        if(instance == null)
        {
            instance = new GameObject("EnemyManager").AddComponent<EnemyManager>();

            instance.prefabEnemyList = new Dictionary<EnemyType, GameObject>();
            instance.EnemyClassList = new Dictionary<EnemyType, Type>();
            instance.enemyList = new HashSet<BaseEnemy>();
            instance.enemyPool = new Stack<BaseEnemy>[Enum.GetValues(typeof(EnemyType)).Length];
            for(int i = 0; i < instance.enemyPool.Length; i++)  instance.enemyPool[i] = new Stack<BaseEnemy>();
            
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
        public float speed;
    }

    private Stack<BaseEnemy>[] enemyPool ; 
    private EnemyConfig enemyConfig;
    private Dictionary<EnemyType , GameObject> prefabEnemyList;
    private Dictionary<EnemyType , Type> EnemyClassList;
    private HashSet<BaseEnemy> enemyList;
    public BaseEnemy CreateEnemy(EnemyType type , int pathIndex)
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
            GameObject gameObject = Instantiate(prefabEnemyList[type]);
            enemy.Init(gameObject , enemyConfig.GetEnemyAttribute(type) , pathIndex);
            enemyList.Add(enemy);
            return enemy;
        }
    }

    public void DestroyEnemy(BaseEnemy enemy)
    {
        enemyList.Remove(enemy);
        enemyPool[(int)enemy.type].Push(enemy);
    }

    private void Update()
    {
        foreach(BaseEnemy enemy in enemyList)
        {
            enemy.OnUpDate(Time.deltaTime);
        }
    }
}
