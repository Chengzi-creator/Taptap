using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public int ID{get;}
    public EnemyManager.EnemyType Type{get;}
    public bool IsDead{get;}
    public void BeAttacked(Vector3 damage , int color);
    public Vector2 Position{get;}
    public float MoveScale{get;}
    public int PathNodeIndex{get;}
    public void GetOccupyGrid(out Vector2Int position1 , out Vector2Int position2);
}
