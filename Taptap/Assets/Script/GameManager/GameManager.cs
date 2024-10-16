using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.Init();
        TowerManager.Init();
        PlayStateMachine.Instance.ReInit(0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayStateMachine.Instance.UpdateState(Time.deltaTime);
    }
}
