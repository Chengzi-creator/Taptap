using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerConfig", menuName = "ScriptableObjects/TowerConfig", order = 0)]
public class TowerConfig : ScriptableObject , ISerializationCallbackReceiver
{
    public enum Color
    {
        black,
        red,
        green,
        yellow,
        blue,
        purple,
        cyan,
        white
    }

    [System.Serializable]
    public struct ShowFormat
    {
        public ITowerManager.TowerType type;
        public int cost;
        public Vector3 damage;
        public Color color;
        public float bulletTime;
        public float timeInterval;
        public List<Vector2Int> attackRange;
    }

    [Header("Tower Config")]
    [SerializeField]
    private List<ShowFormat> showTowerData = new List<ShowFormat>();
    public void OnBeforeSerialize()
    {
        if(false)
        {
            // showTowerData.Clear();
            // foreach(var data in towerDataList)
            // {
            //     // showTowerData.Add(new ShowFormat{type = data.Key, cost = data.Value.cost, damage = data.Value.damage, elementDamage = data.Value.elementDamage, timeInterval = data.Value.timeInterval, attackRange = data.Value.attackRange});
            // }
        }
    }
    public void OnAfterDeserialize()
    {
        towerDataList.Clear();

        for(int i = 0; i < showTowerData.Count; i++)
        {
            towerDataList[showTowerData[i].type] = new ITowerManager.TowerAttribute {
                cost = showTowerData[i].cost,
                damage = showTowerData[i].damage,
                color = (int)showTowerData[i].color,
                bulletTime = showTowerData[i].bulletTime,
                timeInterval = showTowerData[i].timeInterval,
                attackRange = showTowerData[i].attackRange };
        }
    }

    Dictionary<ITowerManager.TowerType , ITowerManager.TowerAttribute> towerDataList = new Dictionary<ITowerManager.TowerType, ITowerManager.TowerAttribute>();

    public ITowerManager.TowerAttribute GetTowerAttribute(ITowerManager.TowerType type)
    {
        if(!towerDataList.ContainsKey(type))
        {
            Debug.LogWarning("TowerConfig doesn't contain " + type + " data");
            return new ITowerManager.TowerAttribute() ;
        }
        return towerDataList[type];
    }
}
