using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerManager
{
    
    public enum TowerType
    {
        D_spike,
        B_torch,
        B_flash,
        B_lazor,
        D_hammer,
        D_catapult,
        X,
        Y,
        Z
    }
    public struct TowerAttribute
    {
        public int cost;
        public int damage;
        public int color;
        public float timeInterval;
        public float bulletTime;
        public List<Vector2Int> attackRange;
    }
    public ITower GetTower(Vector2Int position);
    public int GetColor(Vector2Int position);
    public void Update(float deltaTime);
    public ITower DestroyTower(ITower midTower);
    public ITower CreateTower(TowerType type , Vector2Int position , int faceDirection);
    public TowerAttribute GetTowerAttribute(TowerType type);
    public bool CanBuildTower(TowerType type , Vector2Int position);

}
