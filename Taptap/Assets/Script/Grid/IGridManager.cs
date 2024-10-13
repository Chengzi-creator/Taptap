﻿using System.Collections.Generic;
using UnityEngine;


public interface IGridManager:IPathManager
{
    /// <summary>
    /// 获取mapPos格子Grid
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public IGrid GetIGrid(Vector2Int mapPos);

    /// <summary>
    /// 获取一系列Grid
    /// </summary>
    /// <param name="gridsPos"></param>
    /// <returns></returns>
    public List<IGrid> GetIGrids(List<Vector2Int> gridsPos);

    /// <summary>
    /// 获取mapPos格子的敌人
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public List<IEnemy> GetEnemys(Vector2Int mapPos);

    /// <summary>
    /// 获取格子mapPos到终点的距离
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public int DirToEnd(Vector2Int mapPos);

    /// <summary>
    /// 获取MapPos格子的敌人数量
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public int EnemysCount(Vector2Int mapPos);

    /// <summary>
    /// 获取mappos格子第k个敌人
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public IEnemy GetKthEnemy(Vector2Int mapPos,int k);

    /// <summary>
    /// 是否可以放置Tower
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public bool CanPutTower(Vector2Int mapPos);


    /// <summary>
    /// 获取MapPos格子的Tower
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public BaseTower GetTower(Vector2Int mapPos);

    /// <summary>
    /// 给mapPos格子设置Tower
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="tower"></param>
    public void SetTower(Vector2Int mapPos, BaseTower tower);

    /// <summary>
    /// mapPos的世界坐标，此处mapPos可为浮点数，计算得到对应的浮点WorldPos
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public Vector2 GetWorldPos(Vector2 mapPos);

    /// <summary>
    /// 获取世界坐标对应的map坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns>如果计算出来不在map中返回（-1，-1）</returns>
    public Vector2Int GetMapPos(Vector2 worldPos);

}

