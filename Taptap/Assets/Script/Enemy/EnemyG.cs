using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyG : BaseEnemy
{
    public override void Init(IEnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        this.type = IEnemyManager.EnemyType.G;
        base.Init(enemyAttribute , pathIndex);
    }
    public override void ReInit(IEnemyManager.EnemyAttribute enemyAttribute , int pathIndex)
    {
        base.ReInit(enemyAttribute , pathIndex);
    }
    public override void BeAttacked(Vector3 damage , int colorDamage)
    {
        base.BeAttacked(damage , colorDamage);
    }
    public override void Die()
    {
        base.Die();
    }
    protected override void Move(float deltaTime)
    {
        base.Move(deltaTime);
    }
    protected override void WaitCD(float deltaTime)
    {
        base.WaitCD(deltaTime);
    }
    protected override void ArriveDestination()
    {
        base.ArriveDestination();
    }
    protected override void SetHorn(Vector3 color)
    {
        base.SetHorn(color);
    }
    public override void OnUpDate(float deltaTime)
    {
        base.OnUpDate(deltaTime);
        deltaTime *= timeScale;
    }
}
