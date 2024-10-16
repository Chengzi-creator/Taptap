using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : IGameState
{
    private BuildMode buildMode;
    private SourceText sourceText;
    public float waitTime = 3f;
    private float timer;

    public WaitState(float waitTime)
    {
        this.waitTime = waitTime;
    }

    public void EnterState()
    {
        timer = waitTime;
    }

    public void UpdateState()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameStateManager.Instance.IncreaseRound();
            GameStateManager.Instance.StartNewRound(); // 增加轮数，同时检查是否开始新一轮
        }
    }

    public void ExitState()
    {
      
    }
}
