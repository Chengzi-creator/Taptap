using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject StartMasks;
    [SerializeField] private GameObject ChooseLevel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button level0Button;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private GameObject GameStartMasks;
    private Button[] levelButton;
    
    
    private void Awake()
    {
        Start();
        startButton.onClick.AddListener(OnstartButtonClick);
        exitButton.onClick.AddListener(OnexitButtonClick);
        level0Button.onClick.AddListener(() => levelChoose(0));
        level1Button.onClick.AddListener(() => levelChoose(1));
        level2Button.onClick.AddListener(() => levelChoose(2));
    }

    private void Start()
    {   
        //GameManager.SetActive(false);
        StartMasks.SetActive(true);
        ChooseLevel.SetActive(false);
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnstartButtonClick();
        }
    }

    private void OnstartButtonClick()
    {   
        Time.timeScale = 1f;
        StartMasks.SetActive(false);
        //SceneManager.LoadScene("Scenes/yyl");
        ChooseLevel.SetActive(true);
        enabled = false;
    }

    public void levelChoose(int index)
    {
        //UIManager.Instance.index = index;
        PlayStateMachine.Instance.ReInit(index);
        ChooseLevel.SetActive(false);
        GameStartMasks.SetActive(false);
    }
    
    private void OnexitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
