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
    /// 获取下一个路径点
    /// </summary>
    /// <param name="pathId"></param>
    /// <param name="idx"></param>
    /// <returns></returns>
    public Vector2 GetNextTarget(int pathId,int idx);
    /// <summary>
    /// 获取路径的总格子数
    /// </summary>
    /// <param name="StartPos"></param>
    /// <returns></returns>
    public int GetPathCost(int id);
}

