using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    public Vector2Int Position{get;}
    public int Cost{get;}
    // public void BeAttacked(Vector3 elementDamage);
    public int FaceDirection{get;}
    public ITowerManager.TowerType Type{get;}
}
