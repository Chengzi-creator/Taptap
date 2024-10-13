using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour,IGrid
{
    /// <summary>
    /// 地图坐标
    /// </summary>
    public Vector2 MapPos { get; private set; }
    /// <summary>
    /// 世界坐标
    /// </summary>
    public Vector2 WorldPos { get; private set; }

    /// <summary>
    /// 在该格子上的物体(灯塔，障碍物，起点，终点),如果没有则为空
    /// </summary>
    public GridObject HoldObject;
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
        get => HoldObject == null ||
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
    public int DirToEnd { get; set; }

    /// <summary>
    /// 塔
    /// </summary>
    public BaseTower tower;

    /// <summary>
    /// 可放置物体
    /// </summary>
    public bool CanPutObj => HoldObject == null;

    /// <summary>
    /// 初始化格点
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="worldPos"></param>
    /// <param name="holdObject"></param>
    public void Init(Vector2 mapPos, Vector2 worldPos, GridObject holdObject = null)
    {
        MapPos = mapPos;
        WorldPos = worldPos;
        if (holdObject != null)
            SetHoldObject(holdObject);
    }
    /// <summary>
    /// 设置格子上的物体
    /// </summary>
    /// <param name="gridObject"></param>
    public void SetHoldObject(GridObject gridObject)
    {
        HoldObject = gridObject;
        //HoldObject.transform.position = new Vector3(WorldPos.x, WorldPos.y, 0);
        ShowGrid();
    }
    /// <summary>
    /// 点击格子
    /// </summary>
    public void OnClick()
    {
        //Todo
        if (true)
        {
            if (HoldObject == null && MouseTest.Instance.HoldObject != null)
                MyGridManager.Instance.SetGridHoldObject(this, MouseTest.Instance.HoldObject);
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
        if (HoldObject != null)
        {
            Color color = HoldObject.Type switch
            {
                GridObjectType.Start => Color.blue,
                GridObjectType.End => Color.yellow,
                GridObjectType.Obstacle => Color.black,
                GridObjectType.Building => Color.gray,
                _ => Color.red
            };
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.green;
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
        return null;
    }

    public int EnemysCount()
    {
        return 0;
    }

    public IEnemy GetKthEnemy(int k)
    {
        return null;
    }

    public BaseTower GetTower()
    {
        return tower;
    }

    public void SetTower(BaseTower tower)
    {
        this.tower = tower;
    }

}
