using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerConfig", menuName = "ScriptableObjects/TowerConfig", order = 0)]
public class TowerConfig : ScriptableObject , ISerializationCallbackReceiver
{

    [System.Serializable]
    public struct ShowFormat
    {
        public TowerManager.TowerType type;
        public float cost;
        public Vector3 damage;
        public Vector3 elementDamage;
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
            showTowerData.Clear();
            foreach(var data in towerDataList)
            {
                showTowerData.Add(new ShowFormat{type = data.Key, cost = data.Value.cost, damage = data.Value.damage, elementDamage = data.Value.elementDamage, timeInterval = data.Value.timeInterval, attackRange = data.Value.attackRange});
            }
        }
    }
    public void OnAfterDeserialize()
    {
        towerDataList.Clear();

        for(int i = 0; i < showTowerData.Count; i++)
        {
            towerDataList[showTowerData[i].type] = new TowerManager.TowerAttribute {cost = showTowerData[i].cost, damage = showTowerData[i].damage, elementDamage = showTowerData[i].elementDamage, timeInterval = showTowerData[i].timeInterval, attackRange = showTowerData[i].attackRange };
        }
    }

    Dictionary<TowerManager.TowerType , TowerManager.TowerAttribute> towerDataList = new Dictionary<TowerManager.TowerType, TowerManager.TowerAttribute>();

    public TowerManager.TowerAttribute GetTowerAttribute(TowerManager.TowerType type)
    {
        if(!towerDataList.ContainsKey(type))
        {
            Debug.LogWarning("TowerConfig doesn't contain " + type + " data");
            return new TowerManager.TowerAttribute() ;
        }
        return towerDataList[type];
    }
}
