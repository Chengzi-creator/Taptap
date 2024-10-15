using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : IGameState
{
    private BuildMode buildMode;
    private SourceText sourceText;
    private IEnemyManager _enemyManager; 
    private int enemyCount;
    private float waitTime;

    public SpawnState(BuildMode buildMode, SourceText sourceText, int enemyCount,float waitTime)
    {
        this.buildMode = buildMode;
        this.sourceText = sourceText;
        this.enemyCount = enemyCount;
        this.waitTime = waitTime;        
        _enemyManager = new EnemyManager();
    }

    public void EnterState()
    {
        buildMode.enabled = false;//建造模式禁用
        SpawnEnemy();
    }

    public void UpdateState()
    {
        if (_enemyManager.AllEnemysCount() == 0)
        {
            GameStateManager.Instance.SwitchState(new WaitState(buildMode,sourceText,waitTime));//等五秒进入下一轮？
        }
    }

    public void ExitState()
    {
        
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i <= enemyCount; i++)
        {
            //怪物生成
            EnemyManager.Instance.CreateEnemy(IEnemyManager.EnemyType.A,MyGridManager.Instance.GetPath(Vector2Int.zero));
        }
    }
}
