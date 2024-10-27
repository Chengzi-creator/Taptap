using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBLazor_R : BaseBuffTower
{
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        this.type = ITowerManager.TowerType.B_lazor_R;
        base.Init(towerAttribute, position , faceDirection);
    }
    public override void ReInit(ITowerManager.TowerAttribute towerAttribute, Vector2Int position, int faceDirection)
    {
        base.ReInit(towerAttribute, position, faceDirection);
        switch(faceDirection)
        {
            case 0:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rR");
                break;
            case 1:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rU");
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rL");
                break;
            case 3:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rD");
                break;
        }
        // rangeVFX = VFXManager.Instance.CreateVFX_Range_Lazor(Position, this.faceDirection , color);
    }
}
