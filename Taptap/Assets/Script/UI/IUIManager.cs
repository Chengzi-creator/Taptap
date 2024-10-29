using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIManager
{
    public void coinChange(int coinCount);//直接传入最终的coin数

    public void RoundChange(int level, int round,int roundCount);//传入当前关卡和当前轮数

    public void overMasksOn();
    //建造栏更改
}
