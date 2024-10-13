using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager
{
    private static TowerManager instance;
    public static TowerManager Instance => instance;
    public static bool Init()
    {
        if(instance == null)
        {
            // instance = new GameObject("TowerManager").AddComponent<TowerManager>();
            instance = new TowerManager();

            instance.prefabTowerList = new Dictionary<TowerType, GameObject>();
            instance.towerClassList = new Dictionary<TowerType, Type>();
            instance.towerList = new HashSet<BaseTower>();
            instance.towerPool = new Stack<BaseTower>[Enum.GetValues(typeof(TowerType)).Length];
            for(int i = 0; i < instance.towerPool.Length; i++)  instance.towerPool[i] = new Stack<BaseTower>();
            
            return instance.LoadData();
            
        }
        else
        {
            return false;
        }
    }
    private bool LoadData()
    {
        towerConfig = Resources.Load<TowerConfig>("TowerConfig");
        prefabTowerList[TowerType.X] = Resources.Load<GameObject>("TowerX");
        towerClassList[TowerType.X] = typeof(TowerX);

        if(instance.towerConfig == null)
        {
            Debug.LogWarning("TowerConfig not found");
            return false;
        }

        foreach(TowerType type in Enum.GetValues(typeof(TowerType)))
        {
            if(prefabTowerList[type] == null)
            {
                Debug.LogWarning("Tower Prefab " + type + " not found");
                return false;
            }
            if(towerClassList[type] == null)
            {
                Debug.LogWarning("Tower Class " + type + " not found");
                return false;
            }
        }
        return true;
    }
    public enum TowerType
    {
        X,
        Y,
        Z
    }
    public struct TowerAttribute
    {
        public float cost;
        public Vector3 damage;
        public Vector3 elementDamage;
        public float timeInterval;
        public List<Vector2Int> attackRange;
    }

    private Stack<BaseTower>[] towerPool ; 
    private TowerConfig towerConfig;
    private Dictionary<TowerType , GameObject> prefabTowerList;
    private Dictionary<TowerType , Type> towerClassList;
    private HashSet<BaseTower> towerList;
    public void ReInit()
    {
        foreach(BaseTower tower in towerList)
        {
            DestroyTower(tower);
        }
    }
    public ITower CreateTower(TowerType type , Vector2Int position , int faceDirection)
    {
        if(towerPool[(int)type].Count > 0)
        {
            BaseTower tower = towerPool[(int)type].Peek();
            towerPool[(int)type].Pop();
            tower.ReInit(towerConfig.GetTowerAttribute(type) , position , faceDirection);
            towerList.Add(tower);
            return tower;
        }
        else
        {
            BaseTower tower = (BaseTower)Activator.CreateInstance(towerClassList[type]);
            GameObject gameObject = GameObject.Instantiate(prefabTowerList[type]);
            tower.Init(gameObject , towerConfig.GetTowerAttribute(type) , position , faceDirection);
            towerList.Add(tower);
            return tower;
        }
    }

    public void DestroyTower(BaseTower tower)
    {
        towerList.Remove(tower);
        towerPool[(int)tower.type].Push(tower);
        tower.DestroyTower();
    }
    public void Update(float deltaTime)
    {
        foreach(BaseTower tower in towerList)
        {
            tower.OnUpDate(deltaTime);
        }
    }
    
}
