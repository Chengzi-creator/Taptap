using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBFlash_B : BaseBuffTower
{
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = ITowerManager.TowerType.B_flash_B;
        base.Init(towerAttribute, position , faceDirection);
    }
}
