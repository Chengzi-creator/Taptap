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
    bool c = true;
    bool d = false;
        ITower tower = null;
    void Update()
    {
        if(!a && Time.time > 1)
        {
            tower = TowerManager.Instance.CreateTower(TowerManager.TowerType.B_torch, new Vector2Int(1, 1) , 0);
            // if(tower == null) Debug.Log("tower is null");
            a = true;
        }
        if(!b && Time.time > 2)
        {
            EnemyManager.Instance.CreateEnemy(EnemyManager.EnemyType.A,MyGridManager.Instance.GetPath(Vector2Int.zero));
            b = true;
        }
        if(!c && Time.time > 0.3)
        {

            TowerManager.Instance.DestroyTower(tower);
            Debug.Log("Destroy tower");
            c = true;
        }
        if(!d && Time.time > 3)
        {
            tower = TowerManager.Instance.CreateTower(TowerManager.TowerType.D_spike, new Vector2Int(2, 1) , 0);
            d = true;
        }
        EnemyManager.Instance.Update(Time.deltaTime);
        TowerManager.Instance.Update(Time.deltaTime);
    }
}
