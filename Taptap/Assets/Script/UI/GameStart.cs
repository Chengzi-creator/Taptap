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
    private Button[] levelButton;
    
    private void Awake()
    {
        Start();
        startButton.onClick.AddListener(OnstartButtonClick);
        exitButton.onClick.AddListener(OnexitButtonClick);
        
    }

    private void Start()
    {
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
        SceneManager.LoadScene("Scenes/yyl");
        //ChooseLevel.SetActive(true);
        enabled = false;
    }

    private void levelChoose()
    {
        
    }
    
    private void OnexitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
}
