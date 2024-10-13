using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : IEnemy
{
    protected GameObject gameObject;
    public EnemyManager.EnemyType type;
    public Vector3 maxHP;
    protected Vector3 currentHP;
    protected Vector3 elementTime;
    public float speed;
    protected Vector2 position;
    protected float moveScale;
    protected float timeScale;
    protected int pathIndex;
    protected int pathNodeIndex;
    protected Vector2Int nextPosition;
    protected Vector2Int beginPosition;
#region IEnemy
    public Vector2 Position
    {
        get => position;
        protected set
        {
            position = value;
            gameObject.transform.position = MyGridManager.Instance.GetWorldPos(value);
        }
    }
    public float MoveScale => moveScale;

    public virtual void BeAttacked(Vector3 damage , Vector3 elementTime)
    {}
#endregion
    public  virtual void Die()
    {}

    protected virtual void Move(float deltaTime)
    {}

    protected virtual void SetHorn(Vector3 color)
    {}

    protected virtual void ArriveDestination()
    {}

    protected virtual void WaitCD(float deltaTime)
    {}

    public virtual void Init(GameObject gameObject , EnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {}

    public virtual void ReInit(EnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {}

    public virtual void OnUpDate(float deltaTime)
    {}

}
