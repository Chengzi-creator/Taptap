using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour , IEnemy
{
    public Vector3 maxHP;
    protected Vector3 currentHP;
    public float speed;
    public Vector2 position;
    protected BaseHorn horn;

    public void BeAttacked(Vector3 damage)
    {}

    protected void Die()
    {}

    protected void Move()
    {}
}
