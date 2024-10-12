using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerX : BaseTower
{
    public override void Init(GameObject gameObject, TowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        base.Init(gameObject, towerAttribute, position , faceDirection);
        this.type = TowerManager.TowerType.X;
    }
    public override void ReInit(TowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        base.ReInit(towerAttribute, position , faceDirection);
    }

    protected override void Attack()
    {
        bool flag = false;
        for(int i = 0 ; i < attackRange.Count ; i++)
        {
            if(attackRange[i].enemyCount() > 0)
            {
                attackRange[i].GetEnemy(0).BeAttacked(damage , elementDamage);
                flag = true;
                break;
            }
        }
        if(flag)
        {

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
    public override void BeAttacked(Vector3 elementDamage)
    {
        base.BeAttacked(elementDamage);
    }
    public override void OnUpDate(float deltaTime)
    {
        base.OnUpDate(deltaTime);
    }

}

