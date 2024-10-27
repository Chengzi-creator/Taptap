using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

            instance.colorMap = new int[50,50,5];
            instance.VFXMap = new VFX[50,50];
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
        // HomeRender = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/Home")).GetComponent<SpriteRenderer>();
        // HomeRender.gameObject.SetActive(false);

        towerConfig = Resources.Load<TowerConfig>("SO/TowerConfig");
        prefabTowerList[ITowerManager.TowerType.B_torch_R] = Resources.Load<GameObject>("Prefab/Tower/TowerBTorchR");
        prefabTowerList[ITowerManager.TowerType.B_torch_G] = Resources.Load<GameObject>("Prefab/Tower/TowerBTorchG");
        prefabTowerList[ITowerManager.TowerType.B_torch_B] = Resources.Load<GameObject>("Prefab/Tower/TowerBTorchB");
        prefabTowerList[ITowerManager.TowerType.B_flash_R] = Resources.Load<GameObject>("Prefab/Tower/TowerBFlashR");
        prefabTowerList[ITowerManager.TowerType.B_flash_G] = Resources.Load<GameObject>("Prefab/Tower/TowerBFlashG");
        prefabTowerList[ITowerManager.TowerType.B_flash_B] = Resources.Load<GameObject>("Prefab/Tower/TowerBFlashB");
        prefabTowerList[ITowerManager.TowerType.B_lazor_R] = Resources.Load<GameObject>("Prefab/Tower/TowerBLazorR");
        prefabTowerList[ITowerManager.TowerType.B_lazor_G] = Resources.Load<GameObject>("Prefab/Tower/TowerBLazorG");
        prefabTowerList[ITowerManager.TowerType.B_lazor_B] = Resources.Load<GameObject>("Prefab/Tower/TowerBLazorB");
        prefabTowerList[ITowerManager.TowerType.D_spike] = Resources.Load<GameObject>("Prefab/Tower/TowerDSpike");
        prefabTowerList[ITowerManager.TowerType.D_dart] = Resources.Load<GameObject>("Prefab/Tower/TowerDDart");
        prefabTowerList[ITowerManager.TowerType.D_hammer] = Resources.Load<GameObject>("Prefab/Tower/TowerDHammer");
        prefabTowerList[ITowerManager.TowerType.D_catapult] = Resources.Load<GameObject>("Prefab/Tower/TowerDCatapult");

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
                //return false;
                continue;
            }
        }
        return true;
    }
    // private SpriteRenderer HomeRender ;
    private Stack<BaseTower>[] towerPool ; 
    private TowerConfig towerConfig;
    private Dictionary<ITowerManager.TowerType , GameObject> prefabTowerList;
    private HashSet<BaseTower> towerList;
    private ITower[,] towerMap;
    private int[,,] colorMap;
    private VFX[,] VFXMap;
    public void ReInit()
    {
        foreach(BaseTower tower in towerList)
        {
            DestroyTower(tower);
        }
    }
    public void Close()
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
        MyGridManager.Instance.BuildTower(position);
        return tower;
    }

    public ITower DestroyTower(ITower midTower)
    {
        if(midTower == null)
        {
            Debug.LogWarning("DestroyTower is null");
            return null;
        }
        BaseTower tower = (BaseTower)midTower;
        towerList.Remove(tower);
        towerPool[(int)tower.Type].Push(tower);
        towerMap[tower.Position.x , tower.Position.y] = null;
        MyGridManager.Instance.DestroyTower(tower.Position);
        tower.DestroyTower();
        return tower;
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
    public Vector3Int GetColorVector(Vector2Int position)
    {
        Vector3Int ans = Vector3Int.zero;
        for(int i = 0 ; i < 3 ; i ++)
        {
            if(colorMap[position.x , position.y , i] > 0)
                ans[i] = 1;
        }
        return ans;
    }
    public void AddColor(Vector2Int position , int color)
    {
        for(int i = 0 ; i < 3 ; i ++)
        {
            if((color & (1 << i)) > 0)
            {
                Debug.Log("color para " + position + " " + i);
                colorMap[position.x , position.y , i] ++;
            }
        }
        ColorChanged(position);
        // // MyGridManager.Instance.ColorChanged(position);
        // towerMap[position.x , position.y].ColorChanged();
        // if(VFXMap[position.x , position.y] != null)
        // {
        //     VFXManager.Instance.ReduceVFX(VFXMap[position.x , position.y]);
        //     VFXMap[position.x , position.y] = null;
        // }
        // VFXMap[position.x , position.y] = VFXManager.Instance.CreateVFX_Range_Single(position , GetColor(position));
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

        ColorChanged(position);
        // if(towerMap[position.x , position.y] != null && towerMap[position.x , position.y] is BaseDamageTower)
        // {
        //     if(GetColor(position) == 0)
        //     {
        //         PlayStateMachine.Instance.RemoveTower(position);
        //     }
        //     else
        //     {
        //         (towerMap[position.x , position.y] as BaseDamageTower).ChangeColor(GetColorVector(position));
        //     }
        // }

        // // if(GetColor(position) == 0 && towerMap[position.x , position.y] != null && towerMap[position.x , position.y] is BaseDamageTower)
        // // {
        // //     PlayStateMachine.Instance.RemoveTower(position);
        // // }
        // if(VFXMap[position.x , position.y] != null)
        // {
        //     VFXManager.Instance.ReduceVFX(VFXMap[position.x , position.y]);
        //     VFXMap[position.x , position.y] = null;
        // }
        // VFXMap[position.x , position.y] = VFXManager.Instance.CreateVFX_Range_Single(position , GetColor(position));
    }
    private void ColorChanged(Vector2Int position)
    {
        if(towerMap[position.x , position.y] != null && towerMap[position.x , position.y] is BaseDamageTower)
        {
            if(GetColor(position) == 0)
            {
                PlayStateMachine.Instance.RemoveTower(position);
            }
            else
            {
                (towerMap[position.x , position.y] as BaseDamageTower).ChangeColor(GetColorVector(position));
            }
        }
        if(VFXMap[position.x , position.y] != null)
        {
            VFXManager.Instance.ReduceVFX(VFXMap[position.x , position.y]);
            VFXMap[position.x , position.y] = null;
        }
        VFXMap[position.x , position.y] = VFXManager.Instance.CreateVFX_Range_Single(position , GetColor(position));
    }

    public ITowerManager.TowerAttribute GetTowerAttribute(ITowerManager.TowerType type)
    {
        return towerConfig.GetTowerAttribute(type);
    }
    
    public bool CanBuildTower(ITowerManager.TowerType type , Vector2Int position)
    {
        // prefabTowerList[type].GetComponent<BaseTower>() is BaseBuffTower
        if(towerMap[position.x , position.y] != null)
            return false;
        if(prefabTowerList[type].GetComponent<BaseTower>() is BaseBuffTower)
            return true;
        if(GetColor(position) != 0)
            return true;
        return false;
    }
}

// 红   绿  黄  蓝  紫   青  白
// 1    2   3   4   5   6   7