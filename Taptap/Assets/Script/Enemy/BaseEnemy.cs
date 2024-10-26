using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    private static int maxID;
    protected int id;
    public int ID => id; 
    // protected GameObject gameObject;
    protected IEnemyManager.EnemyType type;
    public IEnemyManager.EnemyType Type => type;

    protected bool isArrived;
    public bool IsArrived => isArrived;
    protected int damage;
    protected Vector3 maxHP;
    protected Vector3 currentHP;
    public bool IsDead => currentHP.x <= 0 && currentHP.y <= 0 && currentHP.z <= 0;
    protected float[] colorTime;
    protected bool[] currentColor = new bool[10];
    protected float defense;
    protected float timeScale;

    protected int money;
    // public int Money => money;
    public int Money => currentColor[3] ? (int)(money * GlobalSetting.Instance.GlobalSettingSO.GetBuffValue(3)) : money;

    protected float speed;
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
            MyGridManager.Instance.AdjustMapPos(ref position);
            transform.position = MyGridManager.Instance.GetWorldPos(value);
        }
    }
    protected float moveScale;
    // public float MoveScale => moveScale;
    public float DisToDestination => MyGridManager.Instance.GetPathCost(pathIndex) - pathNodeIndex - moveScale ;

    public virtual void BeAttacked(Vector3 damage , int colorDamage)
    {
        currentHP -= damage * defense;
        Debug.LogWarning(currentHP);
        this.colorTime[colorDamage] = GlobalSetting.Instance.GlobalSettingSO.GetColorRemainTime(colorDamage);
        SetHorn(new Vector3(currentHP.x/maxHP.x , currentHP.y/maxHP.y , currentHP.z/maxHP.z));
    }
    public virtual void Die()
    {
        gameObject.SetActive(false);
//        Debug.Log("die");
    }

    protected virtual void Move(float deltaTime)
    {
        moveScale += speed * deltaTime;
        Position = ((Vector2)(nextPosition - beginPosition)) * moveScale + beginPosition;
        if(moveScale >= 1)
        {
            Position = nextPosition;
            moveScale -= 1;
            pathNodeIndex++;
            beginPosition = nextPosition;
            if(pathNodeIndex >= MyGridManager.Instance.GetPathCost(pathIndex))
            {
                ArriveDestination();
            }
            else
            {
                nextPosition = MyGridManager.Instance.GetTarget(pathIndex , pathNodeIndex);
            }
        }
    }

    protected virtual void SetHorn(Vector3 color)
    {}

    protected virtual void ArriveDestination()
    {
        Debug.Log("arrive destination");
        speed = 0;
        PlayStateMachine.Instance.HP -= damage;
        currentHP = Vector3.one * -1;
        isArrived = true;
    }

    protected virtual void WaitCD(float deltaTime)
    {
        // 红   绿  黄  蓝  紫   青  白
        for(int i = 0 ; i < colorTime.Length ; i ++)
        {
            if(colorTime[i] > 0)
                colorTime[i] -= deltaTime;
            if(colorTime[i] > 0)
                currentColor[i] = true;
        }
        Vector2Int lef , rig;
        GetOccupyGrid(out lef , out rig);
        for(int x = lef.x ; x <= rig.x ; x++)
            for(int y = lef.y ; y <= rig.y ; y++)
            {
                currentColor[MyGridManager.Instance.GetColor(x , y)] = true;
            }
        // if(currentColor[3])
        // {
//钱
        // }
        if(currentColor[5])
        {
            timeScale = GlobalSetting.Instance.GlobalSettingSO.GetBuffValue(5);
        }
        else
        {
            timeScale = 1;
        }

        if(currentColor[6])
        {
            currentHP -= GlobalSetting.Instance.GlobalSettingSO.GetBuffValue(6) * deltaTime * Vector3.one;
        }
        if(currentColor[7])
        {
            defense = GlobalSetting.Instance.GlobalSettingSO.GetBuffValue(7);
        }
        else
        {
            defense = 1;
        }
    }

    public virtual void Init(IEnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        this.id = maxID++;
        this.gameObject.name = "Enemy_" + this.type + "_" + this.id;
        this.colorTime = new float[10];
        this.currentColor = new bool[10];
        this.ReInit(enemyAttribute , pathIndex);
    }

    public virtual void ReInit(IEnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        
        gameObject.SetActive(true);
        this.size = enemyAttribute.size;
        this.speed = enemyAttribute.speed;
        this.money = enemyAttribute.money;
        this.maxHP = enemyAttribute.maxHP;
        this.currentHP = maxHP;
        this.damage = enemyAttribute.damage;
        this.isArrived = false;
        this.defense = 1;
        this.timeScale = 1;
        this.pathIndex = pathIndex;
        this.pathNodeIndex = 1;
        this.beginPosition = MyGridManager.Instance.GetTarget(pathIndex , 0);
        this.nextPosition = MyGridManager.Instance.GetTarget(pathIndex , 1);
        this.Position = beginPosition;
        this.moveScale = 0;
        for(int i = 0 ; i < 10 ; i ++) {this.colorTime[i] = 0 ; this.currentColor[i] = false;}
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
