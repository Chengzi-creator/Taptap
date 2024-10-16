using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BuildState : IGameState
{
    private BuildMode buildMode;
    private SourceText sourceText;
    private ISource _source;
    public float buildTime = 10f;
    public float waitTime;
    private float timer;
    public float increaseIcon = 100 ;
    public int enemyCount;

    public BuildState(float buildTime,int enemyCount,float waitTime)
    {
        this.buildTime = buildTime;
        this.enemyCount = enemyCount;
        this.waitTime = waitTime;
    }

    public void EnterState()
    {
        timer = buildTime;
        buildMode = GameObject.FindObjectOfType<BuildMode>();
        sourceText = GameObject.FindObjectOfType<SourceText>();
        sourceText.IconIncrease(increaseIcon);//金钱增加还有怪物部分
        Debug.Log("BuildEnter");
    }

    public void UpdateState()
    {   
        timer -= Time.deltaTime;
        //Debug.Log(timer);
        if (timer <= 0 || Input.GetKeyDown(KeyCode.Space))//忘记是按哪个键了
        {
            GameStateManager.Instance.SwitchState(new SpawnState(enemyCount,waitTime));//先设置10了
        }
    }

    public void ExitState()
    {
        
    }
}
