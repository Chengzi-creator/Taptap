
using System.Collections.Generic;
using UnityEngine;

public interface IGrid
{
    /// <summary>
    /// 获取当前格子的敌人
    /// </summary>
    /// <returns></returns>
    public List<IEnemy> GetEnemys();

    /// <summary>
    /// 当前格子到终点的距离
    /// </summary>
    /// <returns></returns>
    public int DisToEnd { get; set; }


    /// <summary>
    /// 当前格子敌人数量
    /// </summary>
    /// <returns></returns>
    public int EnemysCount();

    /// <summary>
    /// 获取第k个敌人
    /// </summary>
    /// <param name="k"></param>
    /// <returns></returns>

    public IEnemy GetKthEnemy(int k);

    /// <summary>
    /// 获取Tower
    /// </summary>
    /// <returns></returns>
    public ITower GetTower();

    /// <summary>
    /// 设置Tower
    /// </summary>
    /// <param name="tower"></param>
    public void BuildTower();


    public Vector2Int MapPos { get;}
    public Vector2 WorldPos { get; }

}

