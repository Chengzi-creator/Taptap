using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public float attackRange;
    public Vector2Int position;

    public float attackCD;
    protected float currentCD;

    public Vector3 damage;
}
