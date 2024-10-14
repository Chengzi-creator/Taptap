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
            instance = new TowerManager();

            instance.colorMap = new int[25,25,5];
            instance.prefabTowerList = new Dictionary<TowerType, GameObject>();
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
        towerConfig = Resources.Load<TowerConfig>("SO/TowerConfig");
        prefabTowerList[TowerType.D_spike] = Resources.Load<GameObject>("Prefab/TowerDSpike");
        prefabTowerList[TowerType.B_torch] = Resources.Load<GameObject>("Prefab/TowerBTorch");

        if(instance.towerConfig == null)
        {
            Debug.LogWarning("TowerConfig not found");
            return false;
        }

        foreach(TowerType type in Enum.GetValues(typeof(TowerType)))
        {
            if(prefabTowerList.ContainsKey(type) == false)
            {
                Debug.LogWarning("Tower Prefab " + type + " not found");
                return false;
            }
        }
        return true;
    }
    public enum TowerType
    {
        X,
        Y,
        Z,
        D_spike,
        B_torch,
        B_flash,
        B_lazor,
        D_hammer,
        D_catapult

    }
    public struct TowerAttribute
    {
        public float cost;
        public Vector3 damage;
        public int color;
        public float timeInterval;
        public float bulletTime;
        public List<Vector2Int> attackRange;
    }

    private Stack<BaseTower>[] towerPool ; 
    private TowerConfig towerConfig;
    private Dictionary<TowerType , GameObject> prefabTowerList;
    private HashSet<BaseTower> towerList;
    private int[,,] colorMap;
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
            BaseTower tower = GameObject.Instantiate(prefabTowerList[type]).GetComponent<BaseTower>();
            tower.Init(towerConfig.GetTowerAttribute(type) , position , faceDirection);
            towerList.Add(tower);
            return tower;
        }
    }

    public void DestroyTower(ITower midTower)
    {
        BaseTower tower = (BaseTower)midTower;
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
    public int GetColor(Vector2Int position)
    {
        int ans = 0;
        for(int i = 0 ; i < 3 ; i ++)
        {
            if(colorMap[position.x , position.y , i] > 0)
                ans += 1 << i;
        }
        return ans;
    }
    public void AddColor(Vector2Int position , int color)
    {
        for(int i = 0 ; i < 3 ; i ++)
        {
            if((color & (1 << i)) > 0)
            {
                colorMap[position.x , position.y , i] ++;
            }
        }
    }
    public void RemoveColor(Vector2Int position , int color)
    {
        for(int i = 0 ; i < 3 ; i ++)
        {
            if((color & (1 << i)) > 0)
            {
                colorMap[position.x , position.y , i] --;
            }
        }
    }
    
}

// 红   绿  黄  蓝  紫   青  白
// 1    2   3   4   5   6   7