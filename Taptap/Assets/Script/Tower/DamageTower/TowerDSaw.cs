using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 溅射
public class TowerDSaw : BaseDamageTower
{
    protected LinkedList<Vector3Int> lockedDirection;
    protected LinkedList<Vector2Int> lockedPosition;
    protected LinkedList<float> lockedTime;
    public override void Init(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        Debug.Log("init saw");
        this.type = ITowerManager.TowerType.D_saw;
        this.lockedTime = new LinkedList<float>();
        this.lockedPosition = new LinkedList<Vector2Int>();
        this.lockedDirection = new LinkedList<Vector3Int>();
        base.Init(towerAttribute, position , faceDirection);
    }
    public override void ReInit(ITowerManager.TowerAttribute towerAttribute, Vector2Int position , int faceDirection)
    {
        base.ReInit(towerAttribute, position , faceDirection);
        this.lockedTime.Clear();
        this.lockedPosition.Clear();
        this.lockedDirection.Clear();

    }

    protected override void Attack()
    {
        for(int i = 1 ; i < 30 ; i ++)
        {
            IGrid lefGrid = MyGridManager.Instance.GetIGrid(Position - FaceDirectionVector2Int * i);
            IGrid rigGrid = MyGridManager.Instance.GetIGrid(Position + FaceDirectionVector2Int * i);
            if(lefGrid == null && rigGrid == null)
                break;
            if(lefGrid != null && lefGrid.EnemysCount() > 0)
            {
                lockedTime.AddLast(bulletTime);
                lockedPosition.AddLast(Position - FaceDirectionVector2Int * i);
                lockedDirection.AddLast(new Vector3Int(-FaceDirectionVector2Int.x , -FaceDirectionVector2Int.y , 1));
                VFXManager.Instance.CreateVFX_Attack_Lianju(Position, (faceDirection + 2) % 4, TowerManager.Instance.GetColor(position));
                VFXManager.Instance.CreateVFX_Attack_Tower_Self(position , TowerManager.Instance.GetColor(position));        
                break;
            }
            if(rigGrid != null && rigGrid.EnemysCount() > 0)
            {
                lockedTime.AddLast(bulletTime);
                lockedPosition.AddLast(Position + FaceDirectionVector2Int * i);
                lockedDirection.AddLast(new Vector3Int(FaceDirectionVector2Int.x , FaceDirectionVector2Int.y , 1));
                VFXManager.Instance.CreateVFX_Attack_Lianju(Position, faceDirection, TowerManager.Instance.GetColor(position));
                VFXManager.Instance.CreateVFX_Attack_Tower_Self(position , TowerManager.Instance.GetColor(position));
                break;
            }
        }

    }
    protected override void BulletFly(float deltaTime)
    {
        if(lockedDirection.Count != lockedPosition.Count || lockedDirection.Count != lockedTime.Count || lockedTime.Count != lockedPosition.Count)
        {
            Debug.LogError("error not the same");
        }
        var node = lockedTime.First;
        while(node != null)
        {
            node.Value -= deltaTime;
            node = node.Next;
        }
        node = lockedTime.First;
        while(node != null && node.Value <= 0)
        {
            if (lockedTime.Count == 0)
                break;
//            Debug.Log(lockedTime.Count + " " + lockedDirection.Count + " " + lockedPosition.Count);
//            if (lockedTime.First == null)
//                Debug.Log("lockedTimePosition is null");
//            if (lockedPosition.First == null)
//                Debug.Log("lockedPosition is null");
//            if (lockedDirection.First == null)
//                Debug.Log("lockedDirction is null");
            Vector2Int position = lockedPosition.First.Value;
            Vector3Int direction = lockedDirection.First.Value;
            lockedPosition.RemoveFirst();
            lockedDirection.RemoveFirst();
            lockedTime.RemoveFirst();
//            Debug.Log(position);

            IGrid midGrid = MyGridManager.Instance.GetIGrid(position);
//            midGrid.GetEnemys().ForEach(enemy => enemy.BeAttacked(damage* TowerManager.Instance.GetColorVector(position) , TowerManager.Instance.GetColor(position)));
            foreach(IEnemy midEnemy in midGrid.GetEnemys())
            {
                midEnemy.BeAttacked(damage * TowerManager.Instance.GetColorVector(Position), TowerManager.Instance.GetColor(Position));
            }
                Debug.Log(Position);
//                VFXManager.Instance.CreateVFX_Attack_Toushiqi(position , 1);
            
            Vector2Int nextPosition = position + new Vector2Int(direction.x, direction.y);
            if(MyGridManager.Instance.GetIGrid(nextPosition) == null)
            {

                if(direction.z == 0)
                    continue;
                else if(direction.z != 0)
                {
                    direction.z--;
                    direction.x = -direction.x;
                    direction.y = -direction.y;
                    nextPosition = position + new Vector2Int(direction.x, direction.y);
                }
            }
            lockedPosition.AddLast(nextPosition);
            lockedDirection.AddLast(direction);
            lockedTime.AddLast(bulletTime);

            node = lockedTime.First;
        }
    }
    protected override void WaitCD(float deltaTime)
    {
        base.WaitCD(deltaTime);
    }

    public override void DestroyTower()
    {
        base.DestroyTower();
    }
    public override void OnUpDate(float deltaTime)
    {
        base.OnUpDate(deltaTime);
        deltaTime *= timeScale;
    }
}
