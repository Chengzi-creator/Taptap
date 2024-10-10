using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridObjectType Type { get; protected set; }

    public virtual void OnClick()
    {
        Debug.Log($"Type:{Type}");
    }
}

