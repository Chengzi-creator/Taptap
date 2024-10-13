using UnityEngine;

public interface IPathManager
{
    /// <summary>
    /// 获取路径，传入起点
    /// </summary>
    /// <param name="StartPos"></param>
    /// <returns></returns>
    public int GetPath(Vector2 StartPos);
    /// <summary>
    /// 获取pathid对应path的第idx路径点
    /// </summary>
    /// <param name="pathId"></param>
    /// <param name="idx"></param>
    /// <returns></returns>
    public Vector2 GetTarget(int pathId,int idx);
    /// <summary>
    /// 获取路径的总格子数
    /// </summary>
    /// <param name="StartPos"></param>
    /// <returns></returns>
    public int GetPathCost(int id);
}

