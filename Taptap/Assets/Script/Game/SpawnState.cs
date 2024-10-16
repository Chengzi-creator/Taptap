using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : IGameState,IGetEnemy
{   
    public static SpawnState Instance { get; private set; }
    
    private BuildMode buildMode;
    private SourceText sourceText;
    private EnemyManager _enemyManager;
    private MyGridManager _gridManager;
    public int enemyCount = 1;
    private float waitTime;
    
    public SpawnState(int enemyCount,float waitTime)
    {
        this.enemyCount = enemyCount;
        this.waitTime = waitTime;        
    }

    public void EnterState()
    {   
        if (Instance == null)
        {
            Instance = new SpawnState(enemyCount,waitTime);
        }
        else
        {
            
        }
        
        MyGridManager.Instance.CalculatePath();
        SpawnEnemy();
        Debug.Log("SpawnEnter");
    }

    public void UpdateState()
    {
        if (EnemyManager.Instance.AllEnemysCount() == 0)
        {
            GameStateManager.Instance.SwitchState(new WaitState(waitTime));
        }
        GameStateManager.Instance.SwitchState(new WaitState(waitTime));
    }

    public void ExitState()
    {
        
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i <= enemyCount; i++)
        {
            //怪物生成
            //EnemyManager.Instance.CreateEnemy(IEnemyManager.EnemyType.A,MyGridManager.Instance.GetPath(Vector2Int.zero));
        }
    }

    public void GetEnemy(IEnemy enemy)
    {
        //获取敌人死亡得到的钱
       
    }
}
