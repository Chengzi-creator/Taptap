using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyManager 
{
    public enum EnemyType
    {
        A,
        B,
        C
    }
    public struct EnemyAttribute
    {
        public Vector3 maxHP;
        public Vector2 size;
        public float speed;
        public int money;
        public int damage;
    }

    public IEnemy CreateEnemy(IEnemyManager.EnemyType type , int pathIndex);
    public void Update(float deltaTime);
    public int EnemysCount(Vector2Int position);
    public List<IEnemy> GetEnemys(Vector2Int position);
    public IEnemy GetKthEnemy(int k , Vector2Int position);
    public int AllEnemysCount();




}
