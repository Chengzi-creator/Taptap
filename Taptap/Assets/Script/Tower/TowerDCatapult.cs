using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDCatapult : BaseDamageTower
{
    protected LinkedList<IGrid> lockedEnemy;
    protected LinkedList<float> lockedTime;
    public override void Init(TowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = TowerManager.TowerType.D_catapult;
        this.lockedEnemy = new LinkedList<IGrid>();
        this.lockedTime = new LinkedList<float>();
        base.Init(towerAttribute, position , faceDirection);
    }
    public override void ReInit(TowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        base.ReInit(towerAttribute, position , faceDirection);
        this.lockedEnemy.Clear();
        this.lockedTime.Clear();
    }

    protected override void Attack()
    {
        for(int i = 0 ; i < attackRange.Count ; i++)
        {
            if(attackRange[i].EnemysCount() > 0)
            {
                lockedEnemy.AddLast(attackRange[i]);
                lockedTime.AddLast(bulletTime);
                break;
            }
        }
    }
    protected override void BulletFly(float deltaTime)
    {
        var node = lockedTime.First;
        while(node != null)
        {
            node.Value -= deltaTime;
            node = node.Next;
        }
        node = lockedTime.First;
        while(node != null && node.Value <= 0)
        {
            for(int i = 0 ; i < lockedEnemy.First.Value.EnemysCount() ; i++)
                lockedEnemy.First.Value.GetKthEnemy(i).BeAttacked(damage , TowerManager.Instance.GetColor(position));
            lockedEnemy.RemoveFirst();
            lockedTime.RemoveFirst();
            node = lockedTime.First;
        }
    }
    protected override void WaitCD(float deltaTime)
    {
        base.WaitCD(deltaTime);
    }

    public override void DestroyTower()
    {
        base.DestroyTower();
    }
    public override void OnUpDate(float deltaTime)
    {
        base.OnUpDate(deltaTime);
    }

}
