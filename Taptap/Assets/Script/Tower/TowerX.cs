using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerX : BaseTower
{
    public override void Init(GameObject gameObject, TowerManager.TowerAttribute towerAttribute, Vector2Int position)
    {
        this.gameObject = gameObject;
        this.type = TowerManager.TowerType.X;
        this.attackRange = new List<MyGrid>();
        this.ReInit(towerAttribute, position);
    }
    public override void ReInit(TowerManager.TowerAttribute towerAttribute, Vector2Int position)
    {
        gameObject.SetActive(true);
        this.cost = towerAttribute.cost;
        this.damage = towerAttribute.damage;
        this.elementTime = towerAttribute.elementTime;
        this.timeInterval = towerAttribute.timeInterval;
        this.currentTimeInterval = 0;
        this.Position = position;
        this.attackRange.Clear();
        foreach (Vector2Int range in towerAttribute.attackRange)
        {
            this.attackRange.Add(MyGridManager.Instance.GetGridByVector2Int(range));
        }
        this.attackRange.Sort(new GridDistanceComparer());
    }

    protected override void Attack()
    {
        if(currentTimeInterval < timeInterval)
            return;
        else
            currentTimeInterval -= timeInterval;

        bool flag = false;
        for(int i = 0 ; i < attackRange.Count ; i++)
        {
            if(attackRange[i].enemyCount > 0)
            {
                attackRange[i].GetEnemy(0).BeAttacked(damage , elementTime);
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
        if(elementTime != Vector3.zero)
            elementTime -= deltaTime * Vector3.one;
        if(elementTime.x < 0 && elementTime.y < 0 && elementTime.z < 0)
            elementTime = Vector3.zero;
        currentTimeInterval += deltaTime;
    }

    public override void DestroyTower()
    {
        gameObject.SetActive(false);
    }
    public override void OnUpDate(float deltaTime)
    {
        WaitCD(deltaTime);
        Attack();
    }


}
public class GridDistanceComparer : IComparer<MyGrid>
{
    public int Compare(MyGrid a, MyGrid b)
    {
        return a.distance - b.distance;
    }
}

