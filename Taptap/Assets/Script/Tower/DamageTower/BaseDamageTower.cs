using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageTower : BaseTower
{
    protected SpriteRenderer spriteRenderer;
    protected float bulletTime;
    protected float timeInterval;
    protected float currentTimeInterval;
    public List<IGrid> attackRange;

    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position, int faceDirection)
    {
        this.attackRange = new List<IGrid>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
        {
            Debug.LogError("spriteRenderer is null");
        }
        base.Init(towerAttribute, position, faceDirection);
    }
    protected virtual void WaitCD(float deltaTime)
    {
        currentTimeInterval += deltaTime;
    }

    public override void ReInit(ITowerManager.TowerAttribute towerAttribute, Vector2Int position, int faceDirection)
    {
        base.ReInit(towerAttribute, position, faceDirection);
        // Debug.Log("ReInit");
        this.damage = towerAttribute.damage;
        // this.elementDamage = towerAttribute.elementDamage;
        this.color = TowerManager.Instance.GetColor(position);
        ChangeColor(TowerManager.Instance.GetColorVector(position));

        this.bulletTime = towerAttribute.bulletTime;
        // this.currentBulletTime = 0;
        this.timeInterval = towerAttribute.timeInterval;
        this.currentTimeInterval = 0;
        this.attackRange.Clear();
        foreach (Vector2Int range in towerAttribute.attackRange)
        {
            Vector2Int midRange;
            if(faceDirection == 1)
                midRange = new Vector2Int(-range.y , range.x);
            else if(faceDirection == 2)
                midRange = new Vector2Int(-range.x , -range.y);
            else if(faceDirection == 3)
                midRange = new Vector2Int(range.y , -range.x);
            else
                midRange = range;
            midRange += position;
            if(MyGridManager.Instance.GetIGrid(midRange) == null)
            {
                continue;
            }
            this.attackRange.Add(MyGridManager.Instance.GetIGrid(midRange));
        }
        this.attackRange.Sort((a , b) => a.DisToEnd - b.DisToEnd);

    }


    public override void OnUpDate(float deltaTime)
    {
        deltaTime *= timeScale;
        WaitCD(deltaTime);
        
        if(currentTimeInterval >= timeInterval)
        {
            currentTimeInterval -= timeInterval;
            Attack();
        }
        BulletFly(deltaTime);
    }

    protected virtual void Attack()
    {}

    protected virtual void BulletFly(float deltaTime)
    {}

    public virtual void ChangeColor(Vector3 color )
    {
        color = color * 0.7f + Vector3.one * 0.3f;
        spriteRenderer.color = new Color(color.x , color.y , color.z);
    }


}
