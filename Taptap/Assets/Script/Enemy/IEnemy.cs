using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void BeAttacked(Vector3 damage , Vector3 elementTime);
    public Vector2 Position{get;}
    public float MoveScale{get;}
}
