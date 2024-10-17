using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerZ : BaseDamageTower
{
    protected LinkedList<float> lockedTime;
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = ITowerManager.TowerType.Z;
        this.lockedTime = new LinkedList<float>();
        base.Init(towerAttribute, position , faceDirection);
    }
    public override void ReInit(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        base.ReInit(towerAttribute, position , faceDirection);
        this.lockedTime.Clear();
    }

    protected override void Attack()
    {
        for(int i = 0 ; i < attackRange.Count ; i++)
        {
            if(attackRange[i].EnemysCount() > 0)
            {
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
            for(int i = 0 ; i < attackRange.Count ; i++)
            {
                for(int j = 0 ; j < attackRange[i].EnemysCount() ; j++)
                {
                    attackRange[i].GetKthEnemy(j).BeAttacked(damage* TowerManager.Instance.GetColorVector(position) , TowerManager.Instance.GetColor(position));
                }
            }
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

