using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower 
{
    protected GameObject gameObject;
    public TowerManager.TowerType type;
    public float cost;
    public Vector3 damage;
    public Vector3 elementTime;
    public float timeInterval;
    protected float currentTimeInterval;
    public List<MyGrid> attackRange;
    public Vector2Int position;
    public Vector2Int Position
    {
        get => position;
        protected set
        {
            position = value;
            gameObject.transform.position = MyGridManager.Instance.GetWorldPos(value);
        }
    }

    protected virtual void Attack()
    {}

    protected virtual void WaitCD(float deltaTime)
    {}

    public virtual void Init(GameObject gameObject , TowerManager.TowerAttribute towerAttribute , Vector2Int position)
    {}

    public virtual void ReInit(TowerManager.TowerAttribute towerAttribute , Vector2Int position)
    {}

    public virtual void DestroyTower()
    {}

    public virtual void OnUpDate(float deltaTime)
    {}
}
