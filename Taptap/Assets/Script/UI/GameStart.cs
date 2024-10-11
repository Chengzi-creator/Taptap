using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject StartMasks;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Start();
        startButton.onClick.AddListener(OnstartButtonClick);
        exitButton.onClick.AddListener(OnexitButtonClick);
    }

    private void Start()
    {
        StartMasks.SetActive(true);
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
        SceneManager.LoadScene("UITest");
        enabled = false;
    }
    
    private void OnexitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
}
