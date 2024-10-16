using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayState
{
    public void EnterState();
    public void UpdateState(float deltaTime);
    public void ExitState();
}
