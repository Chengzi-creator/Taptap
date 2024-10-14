using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuffTower : BaseTower
{
    protected List<Vector2Int> buffRange;
    public override void Init(TowerManager.TowerAttribute towerAttribute, Vector2Int position, int faceDirection)
    {
        buffRange = new List<Vector2Int>();
        base.Init(towerAttribute, position, faceDirection);
    }
    public override void ReInit(TowerManager.TowerAttribute towerAttribute, Vector2Int position, int faceDirection)
    {
        base.ReInit(towerAttribute, position, faceDirection);
        this.buffRange = towerAttribute.attackRange;
        this.color = towerAttribute.color;
        foreach (Vector2Int range in buffRange)
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
            midRange += position;
            TowerManager.Instance.AddColor(midRange, color);
        }
    }

    public override void DestroyTower()
    {
        foreach (Vector2Int range in buffRange)
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
            midRange += position;
            TowerManager.Instance.RemoveColor(midRange, color);
        }
        base.DestroyTower();
    }
}
