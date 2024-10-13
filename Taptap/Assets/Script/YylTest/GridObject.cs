using UnityEngine;

public class GridObject
{
    public GridObjectType Type { get; protected set; }

    public virtual void OnClick()
    {
        Debug.Log($"Type:{Type}");
    }

    public GridObject(GridObjectType type)
    {
        Type = type;
    }
}

