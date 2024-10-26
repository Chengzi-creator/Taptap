using UnityEngine;

public class GridObject
{

    private GameObject gameObject;

    public GridObjectType Type { get; protected set; }

    public virtual void OnClick()
    {
        Debug.Log($"Type:{Type}");
    }

    public GridObject(GridObjectType type)
    {
        Type = type;
    }

    public void SetObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

}

