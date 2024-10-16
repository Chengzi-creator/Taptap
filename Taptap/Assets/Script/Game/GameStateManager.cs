using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
   private IGameState currentState;
   public static GameStateManager Instance { get; private set; }
   private int currentRound = 1;
   public int maxRound;
   public int enemyCount;
   public float buildTime = 10f;
   public float waitTime;
   
   
   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
      EnemyManager.Init();
      TowerManager.Init();
      EnemyManager.Instance.Update(Time.deltaTime);
      TowerManager.Instance.Update(Time.deltaTime);
   }

   private void Start()
   {
      StartNewRound();
   }

   public void SwitchState(IGameState newState)
   {
      currentState?.ExitState();
      currentState = newState;
      currentState.EnterState();
   }

   private void Update()
   {
      currentState?.UpdateState();
   }
   
   public void StartNewRound()
   {
      if (currentRound <= maxRound)
      {
         SwitchState(new BuildState(buildTime,enemyCount,waitTime));//暂定建造时间30
      }
      else
      {
         // 超过最大轮次的时候就算当前关卡结束
         SwitchState(new GameComplete()); // 进入游戏结束状态
      }
   }

   public void IncreaseRound()
   {
      currentRound++;
   }
}

