using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;
    
    [Serializable] 
    public struct ToggleViewPair
    {
        public Toggle toggle;
        public GameObject view;
    }

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private List<ToggleViewPair> toggleViewPairs;
    [SerializeField] private GameObject pauseMasks;
    [SerializeField] private GameObject setupMasks;
    [SerializeField] private GameObject buildMasks;
    [SerializeField] private Button exitButton, restartButton, homeButton,backButton,setupButton,menuButton;
    [SerializeField] private Button[] buildButtons;  //存储所有建造按钮
    [SerializeField] private Button destroyButton;
    
    private bool isPaused = false;
    private bool[] buildSelections;
    private float _value;
    public float Count = 100;
    private int faceDirection = 0;
    private Vector2 worldPosition;
    private Vector2Int gridPosition;
    private MyGridManager gridManager;
    private SourceText sourceText;
    private ITowerManager towerManager;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeUI();
        _text.text = "Icon : " + Count.ToString();
    }

    private void InitializeUI()
    {
        pauseMasks.SetActive(false);
        setupMasks.SetActive(false);
        buildMasks.SetActive(true);

        exitButton.onClick.AddListener(OnExitButtonClick);
        backButton.onClick.AddListener(OnbackButtonClick);
        setupButton.onClick.AddListener(OnsetupButtonClick);
        menuButton.onClick.AddListener(OnmenuButtonClick);
        restartButton.onClick.AddListener(() => RestartGame("UITest"));
        homeButton.onClick.AddListener(() => RestartGame("Start"));
        

        foreach (var pair in toggleViewPairs )
        {
            pair.view.SetActive(false);
        }
        
        buildSelections = new bool[buildButtons.Length];  //初始化建造选择状态
        for (int i = 0; i < buildButtons.Length; i++)
        {
            int index = i;  //得到当前索引
            buildButtons[i].onClick.AddListener(() => OnBuildButtonClick(index));
        }

        destroyButton.onClick.AddListener(DestroyTower);

        gridManager = gameObject.AddComponent<MyGridManager>();
        sourceText = gameObject.AddComponent<SourceText>();
        towerManager = GetComponent<ITowerManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (setupMasks.activeSelf)
            {
                setupMasks.SetActive(false);
                pauseMasks.SetActive(true);
                buildMasks.SetActive(false);
            }
            else
            {
                TogglePause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //PlayStateMachine.Instance.StartSpawnState();
        }
        
        //建造的输入
        
        CheckToggleViews();
        
        DetectBuildModeInput();
    }
    
    public void CheckToggleViews()
    {
        foreach (var pair in toggleViewPairs)
        {
            if (pair.toggle.isOn)
            {
                pair.view.SetActive(true);
                break;
            }
            else
            {
                pair.view.SetActive(false);
            }
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseMasks.SetActive(isPaused);
        buildMasks.SetActive(!isPaused);
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void RestartGame(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }
    
    private void OnbackButtonClick()
    {
        ResumeGame();//恢复游戏
    }
    
    private void OnmenuButtonClick()
    {
        if (setupMasks.activeSelf)
        {
            setupMasks.SetActive(false);
            pauseMasks.SetActive(true);
            buildMasks.SetActive(false);
        }
        else
        {
            TogglePause();
        }
    }
    
    private void OnsetupButtonClick()
    {   
        //设置界面可能要在单独设置？调整音量之类的？
        pauseMasks.SetActive(false);
        setupMasks.SetActive(true);
        buildMasks.SetActive(false);
    }
    
    private void ResumeGame()
    {   
        isPaused = false;
        Time.timeScale = 1f; //恢复游戏时间
        pauseMasks.SetActive(false); //禁用暂停菜单
        buildMasks.SetActive(true);
    }

    
    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Time.timeScale = 1f;
    }

    // private void OnBuildButtonClick(Button clickedButton)
    // {
    //    //建造方法如何放进来
    // }
    //
    public void IconIncrease(float increase)
    {
        Count += increase;
        _text.text = "Icon : " + Count.ToString();
    }

    public void IconDecrease(float decrease)
    {
        Count -= decrease;
        _text.text = "Icon : " + Count.ToString();
    }
    
    public void DetectBuildModeInput()
    {
        if (IsInBuildMode())
        {
            gridManager.ShowBuildModeGrid();
            RotateTower();
            UpdateMousePosition();

            if (gridManager.CanPutTower(gridPosition))
            {
                if (Input.GetMouseButtonDown(0))  //左键建造
                {
                    BuildSelectedTower();
                }
                if (Input.GetMouseButtonDown(1))  //右键退出建造模式
                {
                    ResetBuildSelection();
                }
            }
        }
    }

    private void RotateTower()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            faceDirection = (faceDirection + 1) % 4;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            faceDirection = (faceDirection - 1 + 4) % 4;
        }
    }

    private void UpdateMousePosition()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gridPosition = gridManager.GetMapPos(worldPosition);
    }

    private void BuildSelectedTower()
    {
        for (int i = 0; i < buildSelections.Length; i++)
        {
            if (buildSelections[i])
            {
                var type = (ITowerManager.TowerType)i;  //需要类型枚举顺序和按钮一致，需要调整
                BuildTower(type);
                //PlayStateMachine.Instance.BuildTower(type, gridPosition , faceDirection)
                break;
            }
        }
        
       
    }

    private void BuildTower(ITowerManager.TowerType type)
    {
        float cost = towerManager.GetTowerAttribute(type).cost;

        if (sourceText.Count >= cost)
        {
            towerManager.CreateTower(type, gridPosition, faceDirection);
            sourceText.IconDecrease(cost);
            gridManager.BuildTower(gridPosition);
            ResetBuildSelection();
        }
        else
        {
            ResetBuildSelection();
        }
    }

    private void OnBuildButtonClick(int index)
    {
        ResetBuildSelection();
        buildSelections[index] = true;
    }

    public void DestroyTower()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = gridManager.GetMapPos(worldPos);

        //实现具体的销毁逻辑,尚待开发
    }

    private void ResetBuildSelection()
    {
        for (int i = 0; i < buildSelections.Length; i++)
        {
            buildSelections[i] = false;
        }
    }

    private bool IsInBuildMode()
    {
        foreach (var selected in buildSelections)
        {
            if (selected) return true;
        }
        return false;
    }

}
