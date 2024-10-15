using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour , ITower
{
    protected ITowerManager.TowerType type;
    public ITowerManager.TowerType Type => type;
    protected float cost;
    protected Vector3 damage;
    protected int color;

    protected float timeScale;
    
    protected Vector2Int position;
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



    public virtual void Init(ITowerManager.TowerAttribute towerAttribute , Vector2Int position , int faceDirection)
    {
        this.ReInit(towerAttribute, position , faceDirection);
    }

    public virtual void ReInit(ITowerManager.TowerAttribute towerAttribute , Vector2Int position , int faceDirection)
    {
        gameObject.SetActive(true);
        this.cost = towerAttribute.cost;
        this.timeScale = 1;
        this.Position = position;
        this.faceDirection = faceDirection;
}

    public virtual void DestroyTower()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnUpDate(float deltaTime)
    {
    }

}

