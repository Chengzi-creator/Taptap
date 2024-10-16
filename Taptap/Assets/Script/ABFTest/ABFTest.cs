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
    // bool a = true;
    bool b = false;
    bool c = false;
    bool d = false;
        ITower tower = null;
    void Update()
    {
        if(!a && Time.time > 0.1)
        {
            a = true;
        }
        if(!b && Time.time > 0.2)
        {
            // TowerManager.Instance.CreateTower(ITowerManager.TowerType.B_torch, new Vector2Int(1, 0) , 0);
            EnemyManager.Instance.CreateEnemy(IEnemyManager.EnemyType.A, MyGridManager.Instance.GetPath(Vector2Int.zero));
            b = true;
        }
        if(!c && Time.time > 0.3)
        {

            //     TowerManager.Instance.DestroyTower(tower);
            //TowerManager.Instance.CreateTower(ITowerManager.TowerType.B_torch, new Vector2Int(2, 0), 0);
            //     Debug.Log("Destroy tower");
            c = true;
        }
        if(!d && Time.time > 0.4)
        {
            d = true;
        }
        EnemyManager.Instance.Update(Time.deltaTime);
        TowerManager.Instance.Update(Time.deltaTime);
    }
}
