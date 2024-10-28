using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 单体
public class TowerDDart : BaseDamageTower
{
    protected LinkedList<IEnemy> lockedEnemy;
    protected LinkedList<float> lockedTime;
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = ITowerManager.TowerType.D_dart;
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

    List<IEnemy> attackedEnemy;
    protected override void Attack()
    {
        attackedEnemy = new List<IEnemy>();
        int cnt = 0;
        for(int i = 0 ; i < attackRange.Count ; i++)
        {
            for(int j = 0 ; j < attackRange[i].EnemysCount() ; j ++)
            {
                IEnemy midEnemy = attackRange[i].GetKthEnemy(j);
                if(attackedEnemy.Contains(midEnemy))
                    continue;

                attackedEnemy.Add(midEnemy);
                lockedEnemy.AddLast(midEnemy);
                lockedTime.AddLast(bulletTime);
                cnt++;
                VFXManager.Instance.CreateVFX_Attack_FeiBiao(position, midEnemy.Position, TowerManager.Instance.GetColor(position));
                if (cnt >= 2)
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
            lockedEnemy.First.Value.BeAttacked(damage* TowerManager.Instance.GetColorVector(position) , TowerManager.Instance.GetColor(position));
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
