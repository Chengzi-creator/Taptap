using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : IGameState
{
    private BuildMode buildMode;
    private SourceText sourceText;
    private int enemyCount;
    private int enemyRemain;
    
    public SpawnState(BuildMode buildMode, SourceText sourceText, int enemyCount)
    {
        this.buildMode = buildMode;
        this.sourceText = sourceText;
        this.enemyCount = enemyCount;
    }

    public void EnterState()
    {
        buildMode.enabled = false;//建造模式禁用
        SpawnEnemy();
    }

    public void UpdateState()
    {
        if (enemyRemain <= 0)
        {
            GameStateManager.Instance.SwitchState(new WaitState(buildMode,sourceText,5));//等五秒进入下一轮？
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
            enemyRemain++;
        }
    }

    private void OnEnemyDeath()
    {   
        enemyRemain--;
    }
}
