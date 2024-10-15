using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : ITowerManager
{
    private static TowerManager instance;
    public static TowerManager Instance => instance;
    public static bool Init()
    {
        if(instance == null)
        {
            instance = new TowerManager();

            instance.colorMap = new int[25,25,5];
            instance.prefabTowerList = new Dictionary<ITowerManager.TowerType, GameObject>();
            instance.towerList = new HashSet<BaseTower>();
            instance.towerMap = new ITower[25,25];
            instance.towerPool = new Stack<BaseTower>[Enum.GetValues(typeof(ITowerManager.TowerType)).Length];
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
        prefabTowerList[ITowerManager.TowerType.D_spike] = Resources.Load<GameObject>("Prefab/Tower/TowerDSpike");
        prefabTowerList[ITowerManager.TowerType.B_torch] = Resources.Load<GameObject>("Prefab/Tower/TowerBTorch");

        if(instance.towerConfig == null)
        {
            Debug.LogWarning("TowerConfig not found");
            return false;
        }

        foreach(ITowerManager.TowerType type in Enum.GetValues(typeof(ITowerManager.TowerType)))
        {
            if(prefabTowerList.ContainsKey(type) == false)
            {
                Debug.LogWarning("Tower Prefab " + type + " not found");
                return false;
            }
        }
        return true;
    }

    private Stack<BaseTower>[] towerPool ; 
    private TowerConfig towerConfig;
    private Dictionary<ITowerManager.TowerType , GameObject> prefabTowerList;
    private HashSet<BaseTower> towerList;
    private ITower[,] towerMap;
    private int[,,] colorMap;
    public void ReInit()
    {
        foreach(BaseTower tower in towerList)
        {
            DestroyTower(tower);
        }
    }
    public ITower CreateTower(ITowerManager.TowerType type , Vector2Int position , int faceDirection)
    {
        BaseTower tower;
        if(towerPool[(int)type].Count > 0)
        {
            tower = towerPool[(int)type].Peek();
            towerPool[(int)type].Pop();
            tower.ReInit(towerConfig.GetTowerAttribute(type) , position , faceDirection);
        }
        else
        {
            tower = GameObject.Instantiate(prefabTowerList[type]).GetComponent<BaseTower>();
            tower.Init(towerConfig.GetTowerAttribute(type) , position , faceDirection);
            // towerList.Add(tower);
            // return tower;
        }
        towerList.Add(tower);
        towerMap[position.x , position.y] = tower;
        return tower;
    }

    public void DestroyTower(ITower midTower)
    {
        if(midTower == null)
        {
            Debug.LogWarning("DestroyTower is null");
            return;
        }
        BaseTower tower = (BaseTower)midTower;
        towerList.Remove(tower);
        towerPool[(int)tower.Type].Push(tower);
        towerMap[tower.Position.x , tower.Position.y] = null;
        tower.DestroyTower();
    }
    public ITower GetTower(Vector2Int position)
    {
        return towerMap[position.x , position.y];
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
        MyGridManager.Instance.ColorChanged(position);
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
    public ITowerManager.TowerAttribute GetTowerAttribute(ITowerManager.TowerType type)
    {
        return towerConfig.GetTowerAttribute(type);
    }
}

// 红   绿  黄  蓝  紫   青  白
// 1    2   3   4   5   6   7