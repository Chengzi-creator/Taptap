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
    
    // [Serializable] 
    // public struct ToggleImagePair
    // {
    //     public Toggle toggle;
    //     public GameObject image;
    // }

    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _roundText;
    
    //[SerializeField] private List<ToggleImagePair> toggleImagePairs;
    [SerializeField] private GameObject pauseMasks;
    [SerializeField] private GameObject setupMasks;
    [SerializeField] private GameObject buildMasks;
    [SerializeField] private GameObject overMasks;
    [SerializeField] private GameObject buildButtons;
    [SerializeField] private GameObject rImages;
    [SerializeField] private GameObject gImages;
    [SerializeField] private GameObject bImages;
    [SerializeField] private GameObject buildBack;
    
    [SerializeField] private Button exitButton, restartButton, homeButton,backButton,setupButton,menuButton,overhomeButton,overLevelButton;
    [SerializeField] private Button _buttonRF;
    [SerializeField] private Button _buttonRL;
    [SerializeField] private Button _buttonRT;
    [SerializeField] private Button _buttonGF;
    [SerializeField] private Button _buttonGL;
    [SerializeField] private Button _buttonGT;
    [SerializeField] private Button _buttonBF;
    [SerializeField] private Button _buttonBL;
    [SerializeField] private Button _buttonBT;
    [SerializeField] private Button _buttonDC;
    [SerializeField] private Button _buttonDH;
    [SerializeField] private Button _buttonDS;
    [SerializeField] private Button destroyButton;
    [SerializeField] private Button buildBackButton;
    [SerializeField] private Button rButton;
    [SerializeField] private Button gButton;
    [SerializeField] private Button bButton;

    
    //[SerializeField] private Button[] buildButtons;  //存储所有建造按钮
    
    private ITowerManager.TowerType _selectedTowerType = ITowerManager.TowerType.NULL;

    private bool isPaused = false;
    //private bool[] buildSelections;
    // private bool _selectRF;
    // private bool _selectRL;
    // private bool _selectRT;
    // private bool _selectGF;
    // private bool _selectGL;
    // private bool _selectGT;
    // private bool _selectBF;
    // private bool _selectBL;
    // private bool _selectBT;
    // private bool _selectDC;
    // private bool _selectDH;
    // private bool _selectDS;
    private bool _selectDestroy;
    //private bool[] _select;
    private float _value;
    private int faceDirection = 0;
    private Vector2 worldPosition;
    private Vector2Int gridPosition;
    private MyGridManager gridManager;
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
        buildButtons.SetActive(true);
        buildBack.SetActive(false);
        rImages.SetActive(false);
        gImages.SetActive(false);
        bImages.SetActive(false);
        exitButton.onClick.AddListener(OnexitButtonClick);
        backButton.onClick.AddListener(OnbackButtonClick);
        setupButton.onClick.AddListener(OnsetupButtonClick);
        menuButton.onClick.AddListener(OnmenuButtonClick);
        overLevelButton.onClick.AddListener(OnoverLevelButtonClick);
        overhomeButton.onClick.AddListener((() => RestartGame("Start")));
        restartButton.onClick.AddListener(() => RestartGame("UITest"));
        homeButton.onClick.AddListener(() => RestartGame("Start"));
        destroyButton.onClick.AddListener(OndestroyButtonClick);
        buildBackButton.onClick.AddListener(BuildBack);
        rButton.onClick.AddListener(OnRButtonClick);
        gButton.onClick.AddListener(OnGButtonClick);
        bButton.onClick.AddListener(OnBButtonClick);
        

        // foreach (var pair in toggleImagePairs)
        // {
        //     pair.image.SetActive(false);
        // }
        //
        // buildSelections = new bool[buildButtons.Length];  //初始化建造选择状态
        // for (int i = 0; i < buildButtons.Length; i++)
        // {
        //     int index = i;  //得到当前索引
        //     buildButtons[i].onClick.AddListener(() => OnBuildButtonClick(index));
        // }
        
        _buttonRF.onClick.AddListener(ClickRF);
        _buttonRT.onClick.AddListener(ClickRT);
        _buttonRL.onClick.AddListener(ClickRL);
        _buttonGF.onClick.AddListener(ClickGF);
        _buttonGT.onClick.AddListener(ClickGT);
        _buttonGL.onClick.AddListener(ClickGL);
        _buttonBF.onClick.AddListener(ClickBF);
        _buttonBT.onClick.AddListener(ClickBT);
        _buttonBL.onClick.AddListener(ClickBL);
        _buttonDH.onClick.AddListener(ClickDH);
        _buttonDS.onClick.AddListener(ClickDS);
        _buttonDC.onClick.AddListener(ClickDC);
        
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
        
        
       
        
        DetectBuildModeInput();
        DetectDestroyModeInput();
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
                MyGridManager.Instance.CancelShowBuildModeGrid();
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
        TowerBuild(_selectedTowerType);
        // if (_selectRF)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_flash);
        // }
        // if (_selectRL)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_lazor);
        // }
        // if (_selectRT)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_torch);
        // }
        // if (_selectGF)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_flash);
        // }
        // if (_selectGL)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_lazor);
        // }
        // if (_selectGT)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_torch);//上面几种还没做出来，后续再调
        // }
        // if (_selectBF)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_flash);
        // }
        // if (_selectBL)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_lazor);
        // }
        // if (_selectBT)
        // {
        //     TowerBuild(ITowerManager.TowerType.B_torch);
        // }
        // if (_selectDC)
        // {
        //     TowerBuild(ITowerManager.TowerType.D_catapult);
        // }
        // if (_selectDH)
        // {
        //     TowerBuild(ITowerManager.TowerType.D_hammer);
        // }
        // if (_selectDS)
        // {
        //     TowerBuild(ITowerManager.TowerType.D_spike);
        // }
        
    }
    
    private void TowerBuild(ITowerManager.TowerType type)
    {   
        //在按下按键的时候后面的逻辑都由TowerBuild接管？  
        PlayStateMachine.Instance.BuildTower(type, gridPosition, faceDirection);//这个也是建造吗，没问出来
        //Debug.Log(type);
        //这里不再ClickOut保证同一种类的塔可以连续建造
        //MyGridManager.Instance.CancelShowBuildModeGrid();
    }
    
    public void TowerDestroy()
    {
        //实现具体的销毁逻辑,尚待开发
        
    }
    
    #region 按钮点击
    private void ClickRF()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_flash_R;
        // _selectRF = true;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("out");
    }
    
    private void ClickRL()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_lazor_R;
        // _selectRF = false;
        // _selectRL = true;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("out");
    }
    
    private void ClickRT()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_torch_R;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = true;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("out");
    }
    
    private void ClickGF()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_flash_G;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = true;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("out");
    }
    
    private void ClickGL()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_lazor_G;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = true;
        // _selectGT = false;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("out");
    }
    
    private void ClickGT()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_torch_G;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = true;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("out");
    }
    
    private void ClickBF()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_flash_B;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBF = true;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("BF");
    }
    
    private void ClickBL()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_lazor_B;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBL = true;
        // _selectBF = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("BL");
    }
    private void ClickBT()
    {   
        _selectedTowerType = ITowerManager.TowerType.B_torch_B;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBT = true;
        // _selectBL = false;
        // _selectBF = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("BT");
    }
    
    private void ClickDC()
    {   
        _selectedTowerType = ITowerManager.TowerType.D_catapult;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBT = false;
        // _selectBL = false;
        // _selectBF = false;
        // _selectDC = true;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("DC");
    }

    private void ClickDH()
    {   
        _selectedTowerType = ITowerManager.TowerType.D_hammer;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBT = false;
        // _selectBL = false;
        // _selectBF = false;
        // _selectDC = false;
        // _selectDH = true;
        // _selectDS = false;
        //Debug.Log("DH");
    }
    
    private void ClickDS()
    {   
        _selectedTowerType = ITowerManager.TowerType.D_spike;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = true;
        //Debug.Log("DS");
    }

    private void ClickOut()
    {   
        _selectedTowerType = ITowerManager.TowerType.NULL;
        // _selectRF = false;
        // _selectRL = false;
        // _selectRT = false;
        // _selectGF = false;
        // _selectGL = false;
        // _selectGT = false;
        // _selectBF = false;
        // _selectBL = false;
        // _selectBT = false;
        // _selectDC = false;
        // _selectDH = false;
        // _selectDS = false;
        //Debug.Log("out");
    }

    private bool HasClick()
    {
        // if (_selectRF||_selectRL||_selectRT||_selectGF||_selectGL||_selectGT||_selectBT || _selectBL || _selectBT || _selectDH || _selectDC || _selectDS)
        if(_selectedTowerType != ITowerManager.TowerType.NULL)
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

    public void OnRButtonClick()
    {
        buildButtons.SetActive(false);
        buildBack.SetActive(true);
        rImages.SetActive(true);
    }
    
    public void OnGButtonClick()
    {
        buildButtons.SetActive(false);
        buildBack.SetActive(true);
        gImages.SetActive(true);
    }
    
    public void OnBButtonClick()
    {
        buildButtons.SetActive(false);
        buildBack.SetActive(true);
        bImages.SetActive(true);
    }
    
    public void BuildBack()
    {   
        buildButtons.SetActive(true);
        buildBack.SetActive(false);
        rImages.SetActive(false);
        gImages.SetActive(false);
        bImages.SetActive(false);
        MyGridManager.Instance.CancelShowBuildModeGrid();
    }

    public void overMasksOn()
    {
        overMasks.SetActive(true);
    }

    public void OnoverLevelButtonClick()
    {
        //LoadScene("");//进入下一关
        Debug.Log("还没做好下一个场景");
    }
}