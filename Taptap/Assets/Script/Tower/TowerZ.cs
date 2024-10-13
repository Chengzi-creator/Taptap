using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerZ : BaseTower
{
    public override void Init(GameObject gameObject, TowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        base.Init(gameObject, towerAttribute, position , faceDirection);
        this.type = TowerManager.TowerType.Z;
    }
    public override void ReInit(TowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        base.ReInit(towerAttribute, position , faceDirection);
    }

    protected override void Attack()
    {
        AttackedEnemyID.Clear();
        for(int i = 0 ; i < attackRange.Count ; i++)
        {
            for(int j = 0 ; j < attackRange[i].EnemysCount() ; j++)
            {
                if(AttackedEnemyID.Contains(attackRange[i].GetKthEnemy(j).ID))
                    continue;
                AttackedEnemyID.Add(attackRange[i].GetKthEnemy(j).ID);
                attackRange[i].GetKthEnemy(j).BeAttacked(damage , elementDamage);
            }
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

