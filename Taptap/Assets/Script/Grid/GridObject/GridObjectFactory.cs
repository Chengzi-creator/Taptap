
using Unity.Collections;
using UnityEngine;

public static class GridObjectFactory
{
    public static GridObject Create(GridObjectType type, GameObject gameObject = null)
    {
        switch (type)
        {
            case GridObjectType.None:
            case GridObjectType.Building:
            case GridObjectType.Obstacle:
            case GridObjectType.Start:
            case GridObjectType.NoBuildGround:
            case GridObjectType.NoPassGround:
                return new GridObject(type);
            case GridObjectType.End:
                return new GridObjectEnd(type, gameObject);

        }
        return new GridObject(type);
    }
}

