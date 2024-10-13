using UnityEngine;

public interface IPathManager
{
    public int GetPath(Vector2 StartPos);

    public bool HasNextTarget(int pathId, Vector2 curPos);

    public Vector2 GetNextTarget(int pathId,Vector2 curPos);
}

