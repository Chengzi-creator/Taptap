using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 单体
public class TowerDHammer : BaseDamageTower
{
    protected LinkedList<IEnemy> lockedEnemy;
    protected LinkedList<float> lockedTime;
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = ITowerManager.TowerType.D_hammer;
        this.lockedEnemy = new LinkedList<IEnemy>();
        this.lockedTime = new LinkedList<float>();
        base.Init(towerAttribute, position , faceDirection);
    }
    public override void ReInit(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
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
                // attackRange[i].GetKthEnemy(0).BeAttacked(damage , elementDamage);
                lockedEnemy.AddLast(attackRange[i].GetKthEnemy(0));
                lockedTime.AddLast(bulletTime);
                
                VFXManager.Instance.CreateVFX_Attack_Tower_Self(position , TowerManager.Instance.GetColor(position));
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
            if(lockedEnemy.First.Value != null && lockedEnemy.First.Value.IsDead == false)
            {
                VFXManager.Instance.CreateVFX_Attack_Chuizi(Vector2Int.RoundToInt(lockedEnemy.First.Value.Position) , TowerManager.Instance.GetColor(position));
                lockedEnemy.First.Value.BeAttacked(damage* TowerManager.Instance.GetColorVector(position) , TowerManager.Instance.GetColor(position));
            }
<<<<<<< Updated upstream

            //VFXManager.Instance.CreateVFX_Attack_FeiBiao(position , lockedEnemy.First.Value.Position);
            //lockedEnemy.First.Value.BeAttacked(damage* TowerManager.Instance.GetColorVector(position) , TowerManager.Instance.GetColor(position));
            lockedEnemy.RemoveFirst();
            lockedTime.RemoveFirst();
            node = lockedTime.First;
            // VFXManager.Instance.CreateVFX_Attack_FeiBiao(position , lockedEnemy.First.Value.Position);
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
        deltaTime *= timeScale;
    }
}
