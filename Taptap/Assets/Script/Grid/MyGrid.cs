using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
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
        get => HoldObject == null || HoldObject.Type != GridObjectType.Obstacle;
    }

    /// <summary>
    /// 是终点格子
    /// </summary>
    public bool IsEndGrid
    {
        get => HoldObject != null && HoldObject.Type == GridObjectType.End;
    }
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
        HoldObject.transform.position = new Vector3(WorldPos.x, WorldPos.y, 0);
    }
    /// <summary>
    /// 点击格子
    /// </summary>
    public void OnClick()
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


    /// <summary>
    /// 显示是否空闲
    /// </summary>
    public void ShowEmpty()
    {
        if (HoldObject)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    /// <summary>
    /// 取消显示是否空闲
    /// </summary>
    public void CancelShowEmpty()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }



}
