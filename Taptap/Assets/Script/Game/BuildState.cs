using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BuildState : IGameState
{
    private BuildMode buildMode;
    private SourceText sourceText;
    private float buildTime;
    private float timer;
    private float spawnTime;
    public float increaseIcon = 10 ;
    public int enemyCount;

    public BuildState(BuildMode buildMode, SourceText sourceText, float buildTime)
    {
        this.buildMode = buildMode;
        this.sourceText = sourceText;
        this.buildTime = buildTime;
    }

    public void EnterState()
    {
        timer = buildTime;
        sourceText.IconCrease(increaseIcon);
        buildMode.enabled = true;//建造模式启动
    }

    public void UpdateState()
    {   
        timer -= Time.deltaTime;
        if (timer <= 0 || Input.GetKeyDown(KeyCode.Space))//忘记是按哪个键了
        {
            GameStateManager.Instance.SwitchState(new SpawnState(buildMode, sourceText, enemyCount));//先设置10了
        }
    }

    public void ExitState()
    {
        buildMode.enabled = false;
    }
}
