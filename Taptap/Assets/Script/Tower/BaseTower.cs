using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower :ITower
{
    protected GameObject gameObject;
    public TowerManager.TowerType type;
    public float cost;

    public Vector3 damage;
    public Vector3 elementDamage;

    public Vector3 elementTime;

    public float timeInterval;
    protected float currentTimeInterval;
    protected float timeScale;
    
    public List<IGrid> attackRange;
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
    protected int faceDirection;
    public int FaceDirection => faceDirection;

    public List<int> AttackedEnemyID;

    protected virtual void Attack()
    {}

    protected virtual void WaitCD(float deltaTime)
    {
        if(elementTime != Vector3.zero)
            elementTime -= deltaTime * Vector3.one;
        if(elementTime.x < 0 && elementTime.y < 0 && elementTime.z < 0)
            elementTime = Vector3.zero;
        currentTimeInterval += deltaTime;
    }

    public virtual void Init(GameObject gameObject , TowerManager.TowerAttribute towerAttribute , Vector2Int position , int faceDirection)
    {
        this.gameObject = gameObject;
        this.attackRange = new List<IGrid>();
        this.AttackedEnemyID = new List<int>();
        this.ReInit(towerAttribute, position , faceDirection);
    }

    public virtual void ReInit(TowerManager.TowerAttribute towerAttribute , Vector2Int position , int faceDirection)
    {
        gameObject.SetActive(true);
        this.cost = towerAttribute.cost;
        this.damage = towerAttribute.damage;
        this.elementDamage = towerAttribute.elementDamage;
        this.timeInterval = towerAttribute.timeInterval;
        this.currentTimeInterval = 0;
        this.timeScale = 1;
        this.Position = position;
        this.faceDirection = faceDirection;
        this.attackRange.Clear();
        foreach (Vector2Int range in towerAttribute.attackRange)
        {
            Vector2Int midRange;
            if(faceDirection == 1)
                midRange = new Vector2Int(-range.y , range.x);
            else if(faceDirection == 2)
                midRange = new Vector2Int(-range.x , -range.y);
            else if(faceDirection == 3)
                midRange = new Vector2Int(range.y , range.x);
            else
                midRange = range;
            this.attackRange.Add(MyGridManager.Instance.GetGrid(midRange));
        }
        // this.attackRange.Sort(new GridDistanceComparer());
        this.attackRange.Sort((a , b) => a.DirToEnd - b.DirToEnd);
    }

    public virtual void BeAttacked(Vector3 elementDamage)
    {
        this.elementTime = new Vector3(
            Mathf.Max(elementDamage.x, this.elementTime.x),
            Mathf.Max(elementDamage.y, this.elementTime.y),
            Mathf.Max(elementDamage.z, this.elementTime.z)
        );
    }
    public virtual void DestroyTower()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnUpDate(float deltaTime)
    {
        deltaTime *= timeScale;
        WaitCD(deltaTime);
        
        if(currentTimeInterval >= timeInterval)
        {
            currentTimeInterval -= timeInterval;
            Attack();
        }
    }

}

