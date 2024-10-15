using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    [SerializeField] private GameObject PauseMasks;
    [SerializeField] private GameObject SetupMasks;
    [SerializeField] private GameObject MenuMasks;
    [SerializeField] private GameObject BuildMasks;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button setupButton;
    [SerializeField] private Button menuButton;
    protected bool m_isPaused = false;
    
    private void Awake()
    {
        PauseMasks.SetActive(false);//先隐藏
        SetupMasks.SetActive(false);
        MenuMasks.SetActive(true);
        BuildMasks.SetActive(true);
        exitButton.onClick.AddListener(OnexitButtonClick);//监听
        restartButton.onClick.AddListener(OnrestartButtonClick);
        backButton.onClick.AddListener(OnbackButtonClick);
        homeButton.onClick.AddListener(OnhomeButtonClick);
        setupButton.onClick.AddListener(OnsetupButtonClick);
        menuButton.onClick.AddListener(OnmenuButtonClick);
    }
    
    private void Update()
    {
        //检测是否按下ESC键来切换暂停状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SetupMasks.activeSelf)
            {
                SetupMasks.SetActive(false);
                PauseMasks.SetActive(true);
                BuildMasks.SetActive(false);
            }
            else
            {
                TogglePause();
            }
        }
    }
    
    //两次按ESC
    private void TogglePause()
    {
        m_isPaused = !m_isPaused;//只要ESC就会切换当前状态

        if(m_isPaused)
        {
            Time.timeScale = 0f; //暂停游戏时间
            PauseMasks.SetActive(true); //启用暂停菜单
            BuildMasks.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f; //恢复游戏时间
            PauseMasks.SetActive(false); //禁用暂停菜单
            BuildMasks.SetActive(true);
        }
    }
    
    
    private void OnexitButtonClick()
    {
        //当点击退出
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    private void OnrestartButtonClick()
    {
        StartCoroutine(LoadAndRestartScene("UITest"));
    }
   
    private void OnbackButtonClick()
    {
        ResumeGame();//恢复游戏
    }
    
    private void OnmenuButtonClick()
    {
        if (SetupMasks.activeSelf)
        {
            SetupMasks.SetActive(false);
            PauseMasks.SetActive(true);
            BuildMasks.SetActive(false);
        }
        else
        {
            TogglePause();
        }
    }
    
    private void OnsetupButtonClick()
    {   
        //设置界面可能要在单独设置？调整音量之类的？
        PauseMasks.SetActive(false);
        SetupMasks.SetActive(true);
        BuildMasks.SetActive(false);
    }
    
    private void ResumeGame()
    {   
        m_isPaused = false;
        Time.timeScale = 1f; //恢复游戏时间
        PauseMasks.SetActive(false); //禁用暂停菜单
        BuildMasks.SetActive(true);
    }
    
    
    
    private void OnhomeButtonClick()    
    {   
        StartCoroutine(LoadAndRestartScene("Start"));
    }

    private IEnumerator LoadAndRestartScene(string sceneName)
    {
        //加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        //等待场景加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        //恢复游戏时间
        Time.timeScale = 1f;
    }

}

