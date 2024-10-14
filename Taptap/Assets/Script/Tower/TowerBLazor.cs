using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBLazor : BaseBuffTower
{
    public override void Init(TowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = TowerManager.TowerType.B_lazor;
        base.Init(towerAttribute, position , faceDirection);
    }
}
