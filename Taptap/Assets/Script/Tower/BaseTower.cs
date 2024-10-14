using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour , ITower
{
    public TowerManager.TowerType type;
    public float cost;
    public Vector3 damage;
    public int color;

    protected float timeScale;
    
    public Vector2Int position;
    public Vector2Int Position
    {
        get => position;
        protected set
        {
            position = value;
            Vector2 midPos = MyGridManager.Instance.GetWorldPos(value);
            transform.position = midPos;
        }
    }
    protected int faceDirection;
    public int FaceDirection => faceDirection;



    public virtual void Init(TowerManager.TowerAttribute towerAttribute , Vector2Int position , int faceDirection)
    {
        // this.gameObject = gameObject;
        // this.AttackedEnemyID = new List<int>();
        this.ReInit(towerAttribute, position , faceDirection);
    }

    public virtual void ReInit(TowerManager.TowerAttribute towerAttribute , Vector2Int position , int faceDirection)
    {
        gameObject.SetActive(true);
        this.cost = towerAttribute.cost;
        this.timeScale = 1;
        this.Position = position;
        this.faceDirection = faceDirection;
}

    // public virtual void BeAttacked(Vector3 elementDamage)
    // {
    //     this.elementTime = new Vector3(
    //         Mathf.Max(elementDamage.x, this.elementTime.x),
    //         Mathf.Max(elementDamage.y, this.elementTime.y),
    //         Mathf.Max(elementDamage.z, this.elementTime.z)
    //     );
    // }
    public virtual void DestroyTower()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnUpDate(float deltaTime)
    {
    }

}

