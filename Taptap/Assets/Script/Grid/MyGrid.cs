using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MyGrid : MonoBehaviour, IGrid
{
    /// <summary>
    /// 地图坐标
    /// </summary>
    public Vector2Int MapPos { get; private set; }
    /// <summary>
    /// 世界坐标
    /// </summary>
    public Vector2 WorldPos { get; private set; }

    /// <summary>
    /// 在该格子上的物体(灯塔，障碍物，起点，终点，空。。。)
    /// </summary>
    public GridObject HoldObject;

    /// <summary>
    /// 初始地图格子上的物体，为保存被BuildTower覆盖的物体
    /// </summary>
    public GridObject InitObject;


    /// <summary>
    /// 光
    /// </summary>
    //public MyLight light;
    /// <summary>
    /// 格子大小
    /// </summary>
    public static Vector2 GridSize = new Vector2(2, 2);



    /// <summary>
    /// 可通过格子
    /// </summary>
    public bool CanPass
    {
        get => HoldObject == null || HoldObject.Type == GridObjectType.None ||
            HoldObject.Type == GridObjectType.NoBuildGround ||
            HoldObject.Type == GridObjectType.Start ||
            HoldObject.Type == GridObjectType.End;
    }

    /// <summary>
    /// 是终点格子
    /// </summary>
    public bool IsEndGrid
    {
        get => HoldObject != null && HoldObject.Type == GridObjectType.End;
    }

    public bool IsStartGrid
    {
        get => HoldObject != null && HoldObject.Type == GridObjectType.Start;
    }

    /// <summary>
    /// 到终点的距离
    /// </summary>
    public int DisToEnd { get; set; }


    /// <summary>
    /// 可放置物体
    /// </summary>
    public bool CanPutObj => HoldObject == null || HoldObject.Type == GridObjectType.None
        || HoldObject.Type == GridObjectType.NoPassGround;


    public Sprite[] sprites;

    public Color showEmptyColor;
    /// <summary>
    /// 初始化格点
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="worldPos"></param>
    /// <param name="holdObject"></param>
    public void Init(Vector2Int mapPos, Vector2 worldPos, GridObjectType type = GridObjectType.None)
    {
        MapPos = mapPos;
        WorldPos = worldPos;
        var gridObj = CreateGridObject(type);
        SetHoldObject(gridObj);
        InitObject = gridObj;
    }
    private GridObject CreateGridObject(GridObjectType type)
    {
        return GridObjectFactory.Create(type, this.gameObject);
    }
    /// <summary>
    /// 设置格子上的物体
    /// </summary>
    /// <param name="gridObject"></param>
    public void SetHoldObject(GridObject gridObject)
    {
        HoldObject = gridObject;
        //HoldObject.transform.position = new Vector3(WorldPos.x, WorldPos.y, 0);
        //ShowGrid();
        switch (gridObject.Type)
        {
            case GridObjectType.None:
                GetComponent<SpriteRenderer>().sprite = (MapPos.x + MapPos.y) % 2 == 0 ? sprites[0] : sprites[1];
                break;
            case GridObjectType.Obstacle:
                GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
            case GridObjectType.Start:
                GetComponent<SpriteRenderer>().sprite = sprites[3];
                break;
            case GridObjectType.End:
                GetComponent<SpriteRenderer>().sprite = sprites[4];
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = 
                    (MapPos.x + MapPos.y) % 2 == 0 ? sprites[0] : sprites[1];
                break;
            case GridObjectType.NoBuildGround:
                GetComponent<SpriteRenderer>().sprite = sprites[5];
                break;
            case GridObjectType.NoPassGround:
                GetComponent<SpriteRenderer>().color = Color.black;
                GetComponent<SpriteRenderer>().sprite = sprites[5];
                break;

        }
    }
    /// <summary>
    /// 点击格子
    /// </summary>
    public void OnClick()
    {
        //Todo
        if (true)
        {
            //Debug.Log($"{HoldObject.Type} CanPutObj:{CanPutObj}");
            //if (CanPutObj && MouseTest.Instance.HoldObject != null)
            //    MyGridManager.Instance.SetGridHoldObject(this, MouseTest.Instance.HoldObject);
            //MyGridManager.Instance.LogCost(MapPos);
        }
        else
        {

            if (HoldObject != null)
            {
                HoldObject.OnClick();
            }
            else
            {
                Debug.Log("no object on this grid");
            }
        }

    }


    /// <summary>
    /// 显示是否空闲
    /// </summary>
    public void ShowGrid()
    {
        if (!CanPass)
        {
            Color color = HoldObject.Type switch
            {
                //GridObjectType.Start => Color.blue,
                //GridObjectType.End => Color.yellow,
                //GridObjectType.Obstacle => Color.black,
                //GridObjectType.Building => Color.gray,
                //GridObjectType.NoPassGround => Color.blue,
                _ => Color.white
            };
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = showEmptyColor;
        }
    }
    /// <summary>
    /// 取消显示是否空闲
    /// </summary>
    public void CancelShowGrid()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public List<IEnemy> GetEnemys()
    {
        return EnemyManager.Instance.GetEnemys(MapPos);
    }

    public int EnemysCount()
    {
        return EnemyManager.Instance.GetEnemys(MapPos).Count;
    }

    public IEnemy GetKthEnemy(int k)
    {
        return EnemyManager.Instance.GetKthEnemy(k, MapPos);
    }

    public ITower GetTower()
    {
        return TowerManager.Instance.GetTower(MapPos);
    }

    public void BuildTower()
    {
        SetHoldObject(CreateGridObject(GridObjectType.Building));
    }

    public void DestroyTower()
    {
        SetHoldObject(InitObject);
    }

    public void ColorChanged(int color)
    {

    }

    public void HPChange(int hp)
    {
        if (HoldObject != null)
        {
            (HoldObject as GridObjectEnd)?.HPChange(hp);
        }
    }
}
