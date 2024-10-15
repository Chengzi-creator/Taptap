using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTorch : BaseBuffTower
{
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = ITowerManager.TowerType.B_torch;
        base.Init(towerAttribute, position , faceDirection);
    }
}
