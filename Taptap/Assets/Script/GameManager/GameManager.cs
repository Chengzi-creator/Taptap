using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.Init();
        TowerManager.Init();
        //PlayStateMachine.Instance.ReInit(0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayStateMachine.Instance.UpdateState(Time.deltaTime);
    }

    public void EnterMainMenu()
    {
        PlayStateMachine.Instance.ExitPlayState();
    }
    // public void EnterGame()
    // {
    // }

}
