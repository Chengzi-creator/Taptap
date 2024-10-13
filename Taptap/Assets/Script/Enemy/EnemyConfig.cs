using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 0)]
public class EnemyConfig : ScriptableObject , ISerializationCallbackReceiver
{

    [System.Serializable]
    public struct ShowFormat
    {
        public EnemyManager.EnemyType type;
        public Vector3 maxHP;
        public float speed;
    }

    [Header("Enemy Config")]
    [SerializeField]
    private List<ShowFormat> showEnemyData = new List<ShowFormat>();
    public void OnBeforeSerialize()
    {
        if(false)
        {
            showEnemyData.Clear();
            foreach(var data in enemyDataList)
            {
                showEnemyData.Add(new ShowFormat{type = data.Key, maxHP = data.Value.maxHP, speed = data.Value.speed});
            }
        }
    }
    public void OnAfterDeserialize()
    {
        enemyDataList.Clear();

        for(int i = 0; i < showEnemyData.Count; i++)
        {
            enemyDataList[showEnemyData[i].type] = new EnemyManager.EnemyAttribute { maxHP = showEnemyData[i].maxHP, speed = showEnemyData[i].speed };
        }
    }

    Dictionary<EnemyManager.EnemyType , EnemyManager.EnemyAttribute> enemyDataList = new Dictionary<EnemyManager.EnemyType, EnemyManager.EnemyAttribute>();

    public EnemyManager.EnemyAttribute GetEnemyAttribute(EnemyManager.EnemyType type)
    {
        if(!enemyDataList.ContainsKey(type))
        {
            Debug.LogWarning("EnemyConfig doesn't contain " + type + " data");
            return new EnemyManager.EnemyAttribute() ;
        }
        return enemyDataList[type];
    }
}
