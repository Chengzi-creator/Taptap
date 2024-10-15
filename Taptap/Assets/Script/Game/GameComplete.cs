using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComplete : IGameState
{
   
    public void EnterState()
    {
        ShowVictoryScreen();
    }

    public void UpdateState()
    {
       
    }

    public void ExitState()
    {
        
    }

    private void ShowVictoryScreen()
    {
        // 显示胜利ui
    }
}
