using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : IEnemy
{
    private static int maxID;
    protected int id;
    public int ID => id; 
    protected GameObject gameObject;
    protected EnemyManager.EnemyType type;
    public EnemyManager.EnemyType Type => type;

    public Vector3 maxHP;
    protected Vector3 currentHP;
    public bool IsDead => currentHP.x <= 0 && currentHP.y <= 0 && currentHP.z <= 0;
    protected Vector3 elementTime;

    protected float timeScale;

    public float speed;
    protected int pathIndex;
    protected int pathNodeIndex;
    public int PathNodeIndex => pathNodeIndex;

    protected Vector2 size;

    protected Vector2Int nextPosition;
    protected Vector2Int beginPosition;
    protected Vector2 position;
    public Vector2 Position
    {
        get => position;
        protected set
        {
            position = value;
            gameObject.transform.position = MyGridManager.Instance.GetWorldPos(value);
        }
    }
    protected float moveScale;
    public float MoveScale => moveScale;

    public virtual void BeAttacked(Vector3 damage , Vector3 elementDamage)
    {
        currentHP -= damage;
        this.elementTime = new Vector3(
            Mathf.Max(elementDamage.x, this.elementTime.x),
            Mathf.Max(elementDamage.y, this.elementTime.y),
            Mathf.Max(elementDamage.z, this.elementTime.z)
        );
        SetHorn(new Vector3(currentHP.x/maxHP.x , currentHP.y/maxHP.y , currentHP.z/maxHP.z));
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Move(float deltaTime)
    {
        moveScale += speed * deltaTime;
        Position = ((Vector2)(nextPosition - beginPosition)) * moveScale + beginPosition;
        if(moveScale >= 1)
        {
            moveScale -= 1;
            pathNodeIndex++;
            beginPosition = nextPosition;
            nextPosition = GetPositionFromPathNodeIndex(pathIndex , pathNodeIndex);
            
            if(nextPosition == Vector2Int.one * -1)
            {
                ArriveDestination();
            }
        }
    }

    protected virtual void SetHorn(Vector3 color)
    {}

    protected virtual void ArriveDestination()
    {}

    protected virtual void WaitCD(float deltaTime)
    {
        if(elementTime != Vector3.zero)
            elementTime -= deltaTime * Vector3.one;
        if(elementTime.x < 0 && elementTime.y < 0 && elementTime.z < 0)
            elementTime = Vector3.zero;
    }

    public virtual void Init(GameObject gameObject , EnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        this.id = maxID++;
        this.gameObject = gameObject;
        this.type = EnemyManager.EnemyType.A;
        this.ReInit(enemyAttribute , pathIndex);
    }

    public virtual void ReInit(EnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        
        gameObject.SetActive(true);
        this.size = enemyAttribute.size;
        this.speed = enemyAttribute.speed;
        this.maxHP = enemyAttribute.maxHP;
        this.currentHP = maxHP;
        this.timeScale = 1;
        this.pathIndex = pathIndex;
        this.pathNodeIndex = 1;
        this.beginPosition = GetPositionFromPathNodeIndex(pathIndex , 0);
        this.nextPosition = GetPositionFromPathNodeIndex(pathIndex , 1);
        this.Position = beginPosition;
        this.moveScale = 0;
    }

    public virtual void OnUpDate(float deltaTime)
    {
        deltaTime *= timeScale;
        Move(deltaTime);
        WaitCD(deltaTime);
    }

    public virtual void GetOccupyGrid(out Vector2Int position1, out Vector2Int position2)
    {
        position1 = Vector2Int.RoundToInt(position - size/2);
        position2 = Vector2Int.RoundToInt(position + size/2);
    }
}
