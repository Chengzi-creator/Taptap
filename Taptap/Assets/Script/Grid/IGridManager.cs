using System.Collections.Generic;
using UnityEngine;


public interface IGridManager:IPathManager
{

    /// <summary>
    /// 初始化GridManager
    /// </summary>
    public void Init();

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
    /// 是否可以放置Tower
    /// </summary>
    /// <param name="towerType"></param>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public bool CanPutTower(ITowerManager.TowerType towerType, Vector2Int mapPos);

    /// <summary>
    /// 计算路径
    /// </summary>
    public void CalculatePath(bool drawPath = false);


    /// <summary>
    /// 获取MapPos格子的Tower
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public ITower GetTower(Vector2Int mapPos);

    /// <summary>
    /// 在某个格子建造了Tower
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="tower"></param>
    public void BuildTower(Vector2Int mapPos);

    /// <summary>
    /// 删除格子处的Tower
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="tower"></param>
    public void DestroyTower(Vector2Int mapPos);

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

    /// <summary>
    /// 显示建造模式地块颜色
    /// </summary>
    public void ShowBuildModeGrid();

    /// <summary>
    /// 取消显示建造模式地块颜色
    /// </summary>
    public void CancelShowBuildModeGrid();


    /// <summary>
    /// 获取对应地块颜色int值
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetColor(int x, int y);

    /// <summary>
    /// 提示地块颜色改变
    /// </summary>
    /// <param name="position"></param>
    public void ColorChanged(Vector2Int position);


    /// <summary>
    /// 加载对应关卡
    /// </summary>
    /// <param name="levelIdx"></param>
    /// <returns></returns>
    public bool LoadLevel(int levelIdx);

    /// <summary>
    /// 卸载关卡
    /// </summary>
    public void UnloadLevel();

    /// <summary>
    /// 终点颜色改变
    /// </summary>
    /// <param name="hp"></param>
    public void HPChanged(int hp);
}

