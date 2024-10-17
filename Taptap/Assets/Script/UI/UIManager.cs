using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class UIManager : MonoBehaviour , IUIManager
{
    private static UIManager instance;
    
    [Serializable] 
    public struct ToggleViewPair
    {
        public Toggle toggle;
        public GameObject view;
    }

    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _roundText;
    [SerializeField] private List<ToggleViewPair> toggleViewPairs;
    [SerializeField] private GameObject pauseMasks;
    [SerializeField] private GameObject setupMasks;
    [SerializeField] private GameObject buildMasks;
    [SerializeField] private GameObject overMasks;
    [SerializeField] private Button exitButton, restartButton, homeButton,backButton,setupButton,menuButton,overhomeButton,overLevelButton;
    [SerializeField] private Button _buttonBF;
    [SerializeField] private Button _buttonBL;
    [SerializeField] private Button _buttonBT;
    [SerializeField] private Button _buttonDC;
    [SerializeField] private Button _buttonDH;
    [SerializeField] private Button _buttonDS;
    [SerializeField] private Button destroyButton;
    //[SerializeField] private Button[] buildButtons;  //存储所有建造按钮
    
    private bool isPaused = false;
    //private bool[] buildSelections;
    private bool _selectBF;
    private bool _selectBL;
    private bool _selectBT;
    private bool _selectDC;
    private bool _selectDH;
    private bool _selectDS;
    private bool _selectDestroy;
    private float _value;
    private int faceDirection = 0;
    private Vector2 worldPosition;
    private Vector2Int gridPosition;
    private MyGridManager gridManager;
    private SourceText sourceText;
    private ITowerManager towerManager;
    
    public int Coin = 100;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>("Prefab/GameCanvas")).GetComponent<UIManager>();
                instance.InitializeUI();
                instance._coinText.text = "Coin:" + instance.Coin;
            }
            return instance;
        }
    }
    
    private void InitializeUI()
    {
        pauseMasks.SetActive(false);
        setupMasks.SetActive(false);
        buildMasks.SetActive(true);
        overMasks.SetActive(false);
        exitButton.onClick.AddListener(OnexitButtonClick);
        backButton.onClick.AddListener(OnbackButtonClick);
        setupButton.onClick.AddListener(OnsetupButtonClick);
        menuButton.onClick.AddListener(OnmenuButtonClick);
        overLevelButton.onClick.AddListener(OnoverLevelButtonClick);
        overhomeButton.onClick.AddListener((() => RestartGame("Start")));
        restartButton.onClick.AddListener(() => RestartGame("UITest"));
        homeButton.onClick.AddListener(() => RestartGame("Start"));
        destroyButton.onClick.AddListener(OndestroyButtonClick);
        

        foreach (var pair in toggleViewPairs )
        {
            pair.view.SetActive(false);
        }
        
        // buildSelections = new bool[buildButtons.Length];  //初始化建造选择状态
        // for (int i = 0; i < buildButtons.Length; i++)
        // {
        //     int index = i;  //得到当前索引
        //     buildButtons[i].onClick.AddListener(() => OnBuildButtonClick(index));
        // }
        
        _buttonBF.onClick.AddListener(ClickBF);
        _buttonBT.onClick.AddListener(ClickBT);
        _buttonBL.onClick.AddListener(ClickBL);
        _buttonDH.onClick.AddListener(ClickDH);
        _buttonDS.onClick.AddListener(ClickDS);
        _buttonDC.onClick.AddListener(ClickDC);
        
        sourceText = GetComponent<SourceText>();
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
            //进入出怪阶段
            PlayStateMachine.Instance.StartSpawnState();
        }
        
        
        CheckToggleViews();
        
        DetectBuildModeInput();
        DetectDestroyModeInput();
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

    private void OnexitButtonClick()
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

   
    // public void IconIncrease(float increase)
    // {
    //     Count += increase;
    //     _text.text = "Icon : " + Count.ToString();
    // }
    //
    // public void IconDecrease(float decrease)
    // {
    //     Count -= decrease;
    //     _text.text = "Icon : " + Count.ToString();
    // }
    
    public void DetectBuildModeInput()
    {
        if (HasClick())
        {   
            //Debug.Log("Click");
            MyGridManager.Instance.ShowBuildModeGrid();//这个还要不要？
            //Debug.Log("Show");
            RotateTower();
            UpdateMousePosition();

            
            if (Input.GetMouseButtonDown(0))  //左键建造
            {
                BuildSelectedTower();
            }
            if (Input.GetMouseButtonDown(1))  //右键退出建造模式
            {
                ClickOut();
            }
            
        }
    }
    
    private void DetectDestroyModeInput()
    {
        if (_selectDestroy)
        {
            UpdateMousePosition();
            
            if (Input.GetMouseButtonDown(0))  //左键销毁
            {   
                //获取当前鼠标所指地图上的塔，然后传入TowerDestroy？
                TowerDestroy();
            }
            if (Input.GetMouseButtonDown(1))  //右键退出建造模式
            {
                _selectDestroy = false;
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
        gridPosition = MyGridManager.Instance.GetMapPos(worldPosition);
    }

    private void BuildSelectedTower()
    {
        if (_selectBF)
        {
            TowerBuild(ITowerManager.TowerType.B_flash);
        }
        if (_selectBL)
        {
            TowerBuild(ITowerManager.TowerType.B_lazor);
        }
        if (_selectBT)
        {
            TowerBuild(ITowerManager.TowerType.B_torch);
        }
        if (_selectDC)
        {
            TowerBuild(ITowerManager.TowerType.D_catapult);
        }
        if (_selectDH)
        {
            TowerBuild(ITowerManager.TowerType.D_hammer);
        }
        if (_selectDS)
        {
            TowerBuild(ITowerManager.TowerType.D_spike);
        }
        
    }
    
    private void TowerBuild(ITowerManager.TowerType type)
    {   
        //在按下按键的时候后面的逻辑都由TowerBuild接管？  
        PlayStateMachine.Instance.BuildTower(type, gridPosition, faceDirection);//这个也是建造吗，没问出来
        //Debug.Log(type);
        ClickOut();
    }
    
    public void TowerDestroy()
    {
        //实现具体的销毁逻辑,尚待开发
        
    }
    
    #region 按钮点击
    private void ClickBF()
    {
        _selectBF = true;
        _selectBL = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
        //Debug.Log("BF");
    }
    
    private void ClickBL()
    {
        _selectBL = true;
        _selectBF = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
        //Debug.Log("BL");
    }
    private void ClickBT()
    {
        _selectBT = true;
        _selectBL = false;
        _selectBF = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
        //Debug.Log("BT");
    }
    
    private void ClickDC()
    {
        _selectBT = false;
        _selectBL = false;
        _selectBF = false;
        _selectDC = true;
        _selectDH = false;
        _selectDS = false;
        //Debug.Log("DC");
    }

    private void ClickDH()
    {
        _selectBT = false;
        _selectBL = false;
        _selectBF = false;
        _selectDC = false;
        _selectDH = true;
        _selectDS = false;
        //Debug.Log("DH");
    }
    
    private void ClickDS()
    {
        _selectBF = false;
        _selectBL = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = true;
        //Debug.Log("DS");
    }

    private void ClickOut()
    {
        _selectBF = false;
        _selectBL = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
        //Debug.Log("out");
    }

    private bool HasClick()
    {
        if (_selectBT || _selectBL || _selectBT || _selectDH || _selectDC || _selectDS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OndestroyButtonClick()
    {
        ClickOut();
        _selectDestroy = true;
    }
    
    #endregion
    
    public void coinChange(int coinCount)
    {
        _coinText.text = "Coin : " + coinCount.ToString();
    }

    public void RoundChange(int level,int round)
    {
        _roundText.text = "Level" + level + "      " + "Round" + round;
    }

    public void overMasksOn()
    {
        
    }

    public void OnoverLevelButtonClick()
    {
        //LoadScene("");//进入下一关
    }
}