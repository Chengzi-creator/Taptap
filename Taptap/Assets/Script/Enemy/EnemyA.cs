using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : BaseEnemy
{
    public override void Init(GameObject gameObject , EnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        this.gameObject = gameObject;
        this.type = EnemyManager.EnemyType.A;
        this.ReInit(enemyAttribute , pathIndex);
    }
    public override void ReInit(EnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        gameObject.SetActive(true);
        this.maxHP = enemyAttribute.maxHP;
        this.currentHP = maxHP;
        this.speed = enemyAttribute.speed;
        this.pathIndex = pathIndex;
        this.pathNodeIndex = 1;
        this.beginPosition = GetPositionFromPathNodeIndex(pathIndex , 0);
        this.nextPosition = GetPositionFromPathNodeIndex(pathIndex , 1);
        this.Position = beginPosition;
        this.moveScale = 0;
    }
    public override void BeAttacked(Vector3 damage , Vector3 elementTime)
    {
        currentHP -= damage;
        this.elementTime = new Vector3(
            Mathf.Max(elementTime.x, this.elementTime.x),
            Mathf.Max(elementTime.y, this.elementTime.y),
            Mathf.Max(elementTime.z, this.elementTime.z)
        );
        SetHorn(new Vector3(currentHP.x/maxHP.x , currentHP.y/maxHP.y , currentHP.z/maxHP.z));
        if(currentHP.x <= 0 && currentHP.y <= 0 && currentHP.z <= 0)
        {
            Die();
        }
    }
    protected override void Die()
    {
        gameObject.SetActive(false);
        EnemyManager.Instance.DestroyEnemy(this);
    }
    protected override void Move(float deltaTime)
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
    public override void OnUpDate(float deltaTime)
    {
        Move(deltaTime);
    }
    protected override void SetHorn(Vector3 color)
    {
        
    }
}
