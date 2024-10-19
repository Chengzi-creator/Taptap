using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTorch_G : BaseBuffTower
{
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = ITowerManager.TowerType.B_torch_G;
        base.Init(towerAttribute, position , faceDirection);
    }
}
