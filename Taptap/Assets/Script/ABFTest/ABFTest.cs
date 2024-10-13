using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABFTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.Init();
        TowerManager.Init();
    }

    // Update is called once per frame
    bool a = false;
    bool b = false;
    void Update()
    {
        if(!a && Time.time > 0.1)
        {
            TowerManager.Instance.CreateTower(TowerManager.TowerType.X, new Vector2Int(1, 1) , 0);
            a = true;
        }
        if(!b && Time.time > 0.2)
        {
            EnemyManager.Instance.CreateEnemy(EnemyManager.EnemyType.A,MyGridManager.Instance.GetPath(Vector2.zero));
            b = true;
        }
        EnemyManager.Instance.Update(Time.deltaTime);
        TowerManager.Instance.Update(Time.deltaTime);
    }
}
