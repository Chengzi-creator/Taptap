using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour , IUIManager
{
    private static UIManager instance;
    
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _roundText;
    [SerializeField] private TextMeshProUGUI[] enemyTexts;
    [SerializeField] private GameObject buildText;
    [SerializeField] private GameObject destroyText;
    [SerializeField] private GameObject rotateText;
    
    [Header("ImageAndText")]
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
    [SerializeField] private GameObject ChooseLevel;
    [SerializeField] private GameObject GameStartMasks;
    [SerializeField] private GameObject SpawnButtons;
    [SerializeField] private GameObject teachText;
    [SerializeField] private GameObject gameEvents;
    [SerializeField] private GameObject round;
    [SerializeField] private GameObject enemyImage;
    //[SerializeField] private Image[] enemyImages;
    [SerializeField] private Image enemyImageA;
    [SerializeField] private Image enemyImageB;
    [SerializeField] private Image enemyImageC;
    [SerializeField] private Image enemyImageD;
    [SerializeField] private Image enemyImageE;
    [SerializeField] private Image enemyImageF;
    [SerializeField] private Image enemyImageG;
    [SerializeField] private Image enemyImageH;
    [SerializeField] private Image[] enemyImages;
    
    [SerializeField] private Image rf;
    [SerializeField] private Image rt;
    [SerializeField] private Image rl;
    [SerializeField] private Image gf;
    [SerializeField] private Image gt;
    [SerializeField] private Image gl;
    [SerializeField] private Image bf;
    [SerializeField] private Image bt;
    [SerializeField] private Image bl;
    [SerializeField] private Image dd;
    [SerializeField] private Image dh;
    [SerializeField] private Image ds;
    [SerializeField] private Image dc;
    [SerializeField] private Image dsaw;
   
    [Header("PauseButton")]
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button setupButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button overhomeButton;
    [SerializeField] private Button overLevelButton;
    [SerializeField] private Button overRestartButton;
        
    [Header("Build")]
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
    [SerializeField] private Button _buttonDD;
    [SerializeField] private Button _buttonDSA;
    [SerializeField] private Button destroyButton;
    [SerializeField] private Button buildBackButton;
    [SerializeField] private Button rButton;
    [SerializeField] private Button gButton;
    [SerializeField] private Button bButton;
    [SerializeField] private Button spawnButton;
    [SerializeField] private GameObject rF;
    [SerializeField] private GameObject rT;
    [SerializeField] private GameObject rL;
    [SerializeField] private GameObject gF;
    [SerializeField] private GameObject gT;
    [SerializeField] private GameObject gL;
    [SerializeField] private GameObject bF;
    [SerializeField] private GameObject bT;
    [SerializeField] private GameObject bL;
    [SerializeField] private GameObject dD;
    [SerializeField] private GameObject dH;
    [SerializeField] private GameObject dC;
    [SerializeField] private GameObject dS;
    [SerializeField] private GameObject dSA;
    [SerializeField] private GameObject attackTable;

    
    [Header("Home")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button homeexitButton;
    [SerializeField] private Button levelButton0;
    [SerializeField] private Button levelButton1;
    [SerializeField] private Button levelButton2;
    [SerializeField] private Button levelButton3;
    [SerializeField] private Button levelButton4;
    [SerializeField] private Button levelButton5;
    [SerializeField] private Button levelButton6;
    [SerializeField] private Button levelButton7;
    [SerializeField] private Button levelButton8;
    [SerializeField] private Button levelButton9;
    [SerializeField] private Button levelButton10;
    [SerializeField] private Button levelButton11;
    [SerializeField] private GameObject startB;
    [SerializeField] private GameObject exitB;
    
    
    [SerializeField] private Image towerX;
    //private Image image;
    //[SerializeField] private Button[] buildButtons;  //存储所有建造按钮
    
    private ITowerManager.TowerType _selectedTowerType = ITowerManager.TowerType.NULL;
    
    public bool IsSpawning = false;
    public bool isSpawning
    {
        get { return IsSpawning; }
        set
        {
            if (IsSpawning != value)  // 检测值是否改变
            {
                IsSpawning = value;
                OnIsSpawningChanged(IsSpawning);  //当值改变时调用函数
                Debug.Log("ChangeAudio");
            }
        }
    }

    private void OnIsSpawningChanged(bool isSpawning)
    {
        AudioControl.Instance.SwitchMusic();
    }
    
    
    public bool isTeaching0 = true;
    public bool isTeaching1 = true;
    public bool isTeaching2 = true;
    public bool isTeaching3 = true;
    public bool isTeaching4 = true;
    public bool isTeaching5 = true;
    
    private bool isPaused = false;
    private bool isValid = true;
    private bool _selectDestroy;
    private bool enterGame = false;
    private float _value;
    private int faceDirection = 0;
    private int showCount = 0;
    public int Coin;
    public int mIndex;
    public int mRound;
    private Vector2 worldPosition;
    private Vector2Int gridPosition;
    private MyGridManager gridManager;
    private ITowerManager towerManager;
    private Button lastClickedButton = null;
    private GameObject tower;
    
    
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
        Time.timeScale = 0f;
        GameStartMasks.SetActive(true);
        ChooseLevel.SetActive(false);
        SpawnButtons.SetActive(false);
        pauseMasks.SetActive(false);
        setupMasks.SetActive(false);
        buildMasks.SetActive(false);//
        overMasks.SetActive(false);
        buildButtons.SetActive(false);//
        buildBack.SetActive(false);
        rImages.SetActive(false);
        gImages.SetActive(false);
        bImages.SetActive(false);
        gameEvents.SetActive(false);
        round.SetActive(false);
        teachText.SetActive(false);
        enemyImage.SetActive(false);
        attackTable.SetActive(false);
        buildText.SetActive(false);
        destroyText.SetActive(false);
        rotateText.SetActive(false);
        ChooseLevel.SetActive(false);
        exitButton.onClick.AddListener(OnexitButtonClick);
        backButton.onClick.AddListener(OnbackButtonClick);
        setupButton.onClick.AddListener(OnsetupButtonClick);
        menuButton.onClick.AddListener(OnmenuButtonClick);
        overLevelButton.onClick.AddListener(OnoverLevelButtonClick);
        overhomeButton.onClick.AddListener(OnHomeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        overRestartButton.onClick.AddListener(OnRestartButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);
        homeexitButton.onClick.AddListener(OnexitButtonClick);
        destroyButton.onClick.AddListener(OndestroyButtonClick);
        buildBackButton.onClick.AddListener(BuildBack);
        rButton.onClick.AddListener(OnRButtonClick);
        gButton.onClick.AddListener(OnGButtonClick);
        bButton.onClick.AddListener(OnBButtonClick);
        rButton.onClick.AddListener(() => OnButtonClick(rButton));
        gButton.onClick.AddListener(() => OnButtonClick(gButton));
        bButton.onClick.AddListener(() => OnButtonClick(bButton));
        
        startButton.onClick.AddListener(OnstartButtonClick);
        exitButton.onClick.AddListener(OnexitButtonClick);
        spawnButton.onClick.AddListener(SpawnEnemy);
        
        levelButton0.onClick.AddListener(() => levelChoose(0));
        levelButton1.onClick.AddListener(() => levelChoose(1));
        levelButton2.onClick.AddListener(() => levelChoose(2));
        levelButton3.onClick.AddListener(() => levelChoose(3));
        levelButton4.onClick.AddListener(() => levelChoose(4));
        levelButton5.onClick.AddListener(() => levelChoose(5));
        levelButton6.onClick.AddListener(() => levelChoose(6));
        levelButton7.onClick.AddListener(() => levelChoose(7));
        levelButton8.onClick.AddListener(() => levelChoose(8));
        levelButton9.onClick.AddListener(() => levelChoose(9));
        levelButton10.onClick.AddListener(() => levelChoose(10));
        levelButton11.onClick.AddListener(() => levelChoose(11));
        
        /*
        _showUnRotation = GetComponent<IShow>();
        Debug.LogWarning("在这");*/
        
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
        _buttonDD.onClick.AddListener(ClickDD);
        _buttonDSA.onClick.AddListener(ClickDSA);
        _buttonRF.onClick.AddListener(() => OnButtonClick(_buttonRF));
        _buttonRT.onClick.AddListener(() => OnButtonClick(_buttonRT));
        _buttonRL.onClick.AddListener(() => OnButtonClick(_buttonRL));
        _buttonGF.onClick.AddListener(() => OnButtonClick(_buttonGF));
        _buttonGT.onClick.AddListener(() => OnButtonClick(_buttonGT));
        _buttonGL.onClick.AddListener(() => OnButtonClick(_buttonGL));
        _buttonBF.onClick.AddListener(() => OnButtonClick(_buttonBF));
        _buttonBT.onClick.AddListener(() => OnButtonClick(_buttonBT));
        _buttonBL.onClick.AddListener(() => OnButtonClick(_buttonBL));
        _buttonDH.onClick.AddListener(() => OnButtonClick(_buttonDH));
        _buttonDS.onClick.AddListener(() => OnButtonClick(_buttonDS));
        _buttonDC.onClick.AddListener(() => OnButtonClick(_buttonDC));
        _buttonDD.onClick.AddListener(() => OnButtonClick(_buttonDD));
        _buttonDSA.onClick.AddListener(() => OnButtonClick(_buttonDSA));

        
        
    }

    private void Update()
    {   
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     OnstartButtonClick();
        //     enterGame = true;
        // }
        
        if (!isSpawning)
        {
            //enemyImage.SetActive(false);
        }
        else
        {
            enemyImage.SetActive(true);
        }
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

        if (Input.GetKeyDown(KeyCode.Space) && enterGame) 
        {   
            //进入出怪阶段
            SpawnEnemy();
        }

        if (mIndex != 1 || (mIndex == 1 && isTeaching5))
        {
            if (!isSpawning)
            {
                DetectBuildModeInput();
            }

            DetectDestroyModeInput();
        }
    }

    #region 开始界面
    private void OnstartButtonClick()
    {   
        //Time.timeScale = 1f;
        ChooseLevel.SetActive(true);
        startB.SetActive(false);
        exitB.SetActive(false);
       
    }
    
    public void levelChoose(int index)
    {
        mIndex = index;
        PlayStateMachine.Instance.ReInit(index);

        if (mIndex == 0 || mIndex == 1)
        {
            if (mIndex == 1)
            {
                TeachText.Instance.talkCount = 6;
                TeachText.Instance.LoadDialogue();
            }

            teachText.SetActive(true);
            //isTeaching0 = true;
            TeachText.Instance.LoadDialogue();
        }
        else
        {
            teachText.SetActive(false);
            attackTable.SetActive(true);
        }
        
        Time.timeScale = 1f;
        #region 禁用

        if (mIndex == 0)
        {   
            rf.color = Color.gray;
            rl.color = Color.gray;
            bf.color = Color.gray;
            bt.color = Color.gray;
            bl.color = Color.gray;
            gf.color = Color.gray;
            gl.color = Color.gray;
            dsaw.color = Color.gray;
            dh.color = Color.gray;
            ds.color = Color.gray;
            dc.color = Color.gray;
        }
        else
        {
            rf.color = Color.white;
            rl.color = Color.white;
            bf.color = Color.white;
            bt.color = Color.white;
            bl.color = Color.white;
            gf.color = Color.white;
            gl.color = Color.white;
            dsaw.color = Color.white;
            dh.color = Color.white;
            ds.color = Color.white;
            dc.color = Color.white;
        }
        
        #endregion
        
        ResumeGame();
        overMasks.SetActive(false);
        ChooseLevel.SetActive(false);
        GameStartMasks.SetActive(false);
        gameEvents.SetActive(true);
        enemyImage.SetActive(true);
        round.SetActive(true);
        buildMasks.SetActive(true);
        buildButtons.SetActive(true);
        SpawnButtons.SetActive(true);
        enterGame = true;
    }
    #endregion
    
    #region 暂停界面

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseMasks.SetActive(isPaused);
        buildMasks.SetActive(!isPaused);
    }

    public void OnHomeButtonClick()
    {
        PlayStateMachine.Instance.ExitPlayState();
        // PlayStateMachine.Instance.Close();
        GameStartMasks.SetActive(true);
        ChooseLevel.SetActive(false);
        SpawnButtons.SetActive(false);
        pauseMasks.SetActive(false);
        setupMasks.SetActive(false);
        buildMasks.SetActive(false);
        overMasks.SetActive(false);
        buildButtons.SetActive(false);
        buildBack.SetActive(false);
        rImages.SetActive(false);
        gImages.SetActive(false);
        bImages.SetActive(false);
        gameEvents.SetActive(false);
        round.SetActive(false);
        teachText.SetActive(false);
        enemyImage.SetActive(false);
        ClickOut();
        _selectDestroy = false;
        startB.SetActive(true);
        exitB.SetActive(true);
        enemyImage.SetActive(false);
        TeachText.Instance.talkCount = 0;
    }
    
    public void OnRestartButtonClick()
    {   
        PlayStateMachine.Instance.RestartWave();
        ClickOut();
        _selectDestroy = false;
        //PlayStateMachine.Instance.ReInit(mIndex);
        overMasks.SetActive(false);
        ResumeGame();//恢复游戏
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

    #endregion

    #region 建造方法

     public void DetectBuildModeInput()
    {
        if (HasClick() )
        {
            if (_selectDestroy == true)
            {
                _selectDestroy = false;
            }
            ShowModeGrid();
            RotateTower();
            UpdateMousePosition();
            buildText.SetActive(true);
            destroyText.SetActive(false);
            rotateText.SetActive(true);
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //鼠标在 UI 上，不进行建造
                return;
            }
            
            if (Input.GetMouseButtonDown(0))  //左键建造
            {   
                BuildSelectedTower();
                //DestroyImage(image);
                buildText.SetActive(false);
                rotateText.SetActive(false);
                if (isTeaching0)
                {
                    if (TeachText.Instance.talkCount == 0)
                    {
                        TeachText.Instance.talkCount = 1;
                        //TeachText.Instance.LoadDialogue();
                        //isTeaching1 = true;
                    }
                }

                if (isTeaching0 && isTeaching1)
                {
                    if (TeachText.Instance.talkCount == 1)
                    {
                        TeachText.Instance.talkCount = 2;
                        //TeachText.Instance.LoadDialogue();
                        //isTeaching2 = true;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1)) //右键退出建造模式
            {   
                DestroyTowerImage();
                ClickOut();
                MyGridManager.Instance.CancelShowBuildModeGrid();
                showCount = 0;
                faceDirection = 0;
                _selectDestroy = false;
                buildText.SetActive(false);
                rotateText.SetActive(false);
            }
        }
    }
    
    private void DetectDestroyModeInput()
    {
        if (_selectDestroy && isTeaching3)
        {
            UpdateMousePosition();
            ClickOut();
            DestroyTowerImage();
            buildText.SetActive(false);
            rotateText.SetActive(false);
            destroyText.SetActive(true);
            
            if (Input.GetMouseButtonDown(0))  //左键销毁
            {   
                //获取当前鼠标所指地图上的塔，然后传入TowerDestroy？
                TowerDestroy();
                destroyText.SetActive(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _selectDestroy = false;
                ClickOut();
                showCount = 0;
                faceDirection = 0;
                destroyText.SetActive(false);
            }
        }
    }

    private void RotateTower()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            faceDirection = (faceDirection + 1) % 4;
            Debug.Log(faceDirection);
            tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            faceDirection = (faceDirection + 3) % 4;
            Debug.Log(faceDirection);
            tower.GetComponent<IShow>().SetFaceDirection(faceDirection); 
        }
    }

    private void UpdateMousePosition()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gridPosition = MyGridManager.Instance.GetMapPos(worldPosition);
        if (tower != null)
        {
            tower.transform.position = MyGridManager.Instance.GetGridMidWorldPos(worldPosition,out isValid);
            // Debug.Log(tower.transform.position);
        }
    }

    private void ShowModeGrid()
    {
        if (showCount == 1)
        {
            MyGridManager.Instance.ShowBuildModeGrid();
        }
    }

    private void DestroyTowerImage()
    {   
        Destroy(tower);
        tower = null;
    }
    
    private void BuildSelectedTower()
    {   
        TowerBuild(_selectedTowerType);
    }
    
    private void TowerBuild(ITowerManager.TowerType type)
    {   
        PlayStateMachine.Instance.BuildTower(type, gridPosition, faceDirection);
        showCount = 0;
//        faceDirection = 0;
    }
    
    public void TowerDestroy()
    {
        PlayStateMachine.Instance.RemoveTower(gridPosition);
    }
    
    public void BuildBack()
    {   
        buildButtons.SetActive(true);
        buildBack.SetActive(false);
        rImages.SetActive(false);
        gImages.SetActive(false);
        bImages.SetActive(false);
        ClickOut();
        MyGridManager.Instance.CancelShowBuildModeGrid();//为什么函数没调用成功
        showCount = 0;
        //Debug.Log("back");
    }
    
    #endregion
    
    #region 建造按钮点击
    private void ClickRF()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.B_flash_R;

            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(rF); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/

                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickRL()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.B_lazor_R;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(rL); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/

                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickRT()
    {
        if (isTeaching0)
        {
            _selectedTowerType = ITowerManager.TowerType.B_torch_R;
            
            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(rT); 
                // Debug.Log("生成");
                /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/

                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }

           
        }
    }
    
    private void ClickGF()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.B_flash_G;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(gF); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/

                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickGL()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.B_lazor_G;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(gL); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickGT()
    {
        if (isTeaching3)
        {
            _selectedTowerType = ITowerManager.TowerType.B_torch_G;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(gT); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickBF()
    {
        if (mIndex != 0)
        {

            _selectedTowerType = ITowerManager.TowerType.B_flash_B;


            if ( !isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(bF); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickBL()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.B_lazor_B;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(bL); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    private void ClickBT()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.B_torch_B;

            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(bT); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickDC()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.D_catapult;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(dC); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }

    private void ClickDH()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.D_hammer;

            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(dH); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/

                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickDS()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.D_spike;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(dS); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }
    
    private void ClickDD()
    {
        if (isTeaching1)
        {
            _selectedTowerType = ITowerManager.TowerType.D_dart;
            // Debug.Log("HelloDD");
            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(dD); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/
                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
            
           
        }
    }
    
    private void ClickDSA()
    {
        if (mIndex != 0)
        {
            _selectedTowerType = ITowerManager.TowerType.D_saw;


            if (!isSpawning)
            {
                DestroyTowerImage();
                tower = GameObject.Instantiate(dSA); /*
                _showUnRotation.ShowRange();
                _showUnRotation.SetFaceDirection(faceDirection);*/

                tower.GetComponent<IShow>().ShowRange();
                tower.GetComponent<IShow>().SetFaceDirection(faceDirection);
            }
        }
    }

    private void ClickOut()
    {   
        _selectedTowerType = ITowerManager.TowerType.NULL;
    }

    private bool HasClick()
    {
        if(_selectedTowerType != ITowerManager.TowerType.NULL)
        {
            showCount++;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void OndestroyButtonClick()
    {
        if (isTeaching3)
        {
            _selectDestroy = true;
            ClickOut();
            MyGridManager.Instance.CancelShowBuildModeGrid();
            showCount = 0;
            faceDirection = 0;
        }
    }
    
    void OnButtonClick(Button clickedButton)
    {
        if (lastClickedButton == null)
        {
            
        }
        else if (lastClickedButton != clickedButton)
        {
            faceDirection = 0;
        }
        else
        {
            
        }
        
        //更新上一次点击的按钮
        lastClickedButton = clickedButton;
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
    #endregion

    #region 金钱轮次显示

    public void coinChange(int coinCount)
    {
        _coinText.text = "Coin : " + coinCount.ToString();
    }

    public void RoundChange(int level,int round)
    {
        mRound = round;
        _roundText.text = "Level" + level + "      " + "Round" + round;
    }

    #endregion

    #region 游戏结束

    public void overMasksOn()
    {
        overMasks.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnoverLevelButtonClick()
    {
        overMasks.SetActive(false);
        Time.timeScale = 1f;
        PlayStateMachine.Instance.ExitPlayState();
        PlayStateMachine.Instance.ReInit(++mIndex);

        if (mIndex == 1)
        {
            TeachText.Instance.talkCount = 6;
            //TeachText.Instance.LoadDialogue();
            teachText.SetActive(true);
        }
        else
        {
            teachText.SetActive(false);
            attackTable.SetActive(true);
        }
        
        
        #region 禁用

        if (mIndex == 0)
        {   
            rf.color = Color.gray;
            rl.color = Color.gray;
            bf.color = Color.gray;
            bt.color = Color.gray;
            bl.color = Color.gray;
            gf.color = Color.gray;
            gl.color = Color.gray;
            dsaw.color = Color.gray;
            dh.color = Color.gray;
            ds.color = Color.gray;
            dc.color = Color.gray;
        }
        else
        {
            rf.color = Color.white;
            rl.color = Color.white;
            bf.color = Color.white;
            bt.color = Color.white;
            bl.color = Color.white;
            gf.color = Color.white;
            gl.color = Color.white;
            dsaw.color = Color.white;
            dh.color = Color.white;
            ds.color = Color.white;
            dc.color = Color.white;
        }
        #endregion
    }

    #endregion

    #region 出怪阶段相关设置

    public void ShowEnemyBuff()
    {
        //还未纳入进程？
    }
    
    public void ShowEnemyCountAndTypes(List<IEnemyManager.EnemyType> types, List<int> counts)
    {   
        if (types == null || counts == null || types.Count != counts.Count)
        {
            Debug.LogError("传入的列表为空或长度不一致！");
            return;
        }
        
        if (types.Count != counts.Count)
        {
            return;
        }
        
        string infoText = "Enemy:\n";
        for (int i = 0; i < types.Count; i++)
        {
            infoText += $"{types[i]}: {counts[i]}\n";
        }
        
        
        for (int i = 0,j = 0; i < types.Count; i++)
        {   
            if (types.Count > enemyImages.Length || types.Count > enemyTexts.Length)
            {
                Debug.LogError("enemyImages 或 enemyTexts 数组长度不足！");
                return;
            }
            // Debug.Log(types[i]);
            switch (types[i])
            {   
                
                case IEnemyManager.EnemyType.A:
                    enemyImages[j].sprite = enemyImageA.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                case IEnemyManager.EnemyType.B:
                    enemyImages[j].sprite = enemyImageB.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                case IEnemyManager.EnemyType.C:
                    enemyImages[j].sprite = enemyImageC.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                case IEnemyManager.EnemyType.D:
                    enemyImages[j].sprite = enemyImageD.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                case IEnemyManager.EnemyType.E:
                    enemyImages[j].sprite = enemyImageE.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                case IEnemyManager.EnemyType.F:
                    enemyImages[j].sprite = enemyImageF.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                case IEnemyManager.EnemyType.G:
                    enemyImages[j].sprite = enemyImageG.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                case IEnemyManager.EnemyType.H:
                    enemyImages[j].sprite = enemyImageH.sprite;
                    enemyImages[j].color = new Color(255, 255, 255, 255);
                    enemyTexts[j++].text = $"{counts[i]}";
                    break;
                
                // case IEnemyManager.EnemyType.I:
                //     enemyImages[j].sprite = enemyImageI.sprite;
                //     enemyImages[j].color = new Color(255, 255, 255, 255);
                //     enemyTexts[j++].text = $"{counts[i]}";
                //     break;
                
                default:
                    continue;
            }

            for (; j < enemyImages.Length; j++)
            {
                enemyImages[j].sprite = null;
                enemyTexts[j].text = "";
                enemyImages[j].color = new Color(255, 255, 255, 0); //隐藏图片
            }
        }
    }

   
    private void ShowEnemyImage(List<IEnemyManager.EnemyType> types)
    {
        
    }
    
    public void SpawnEnemy()
    {
        if (enterGame && !isSpawning && isTeaching2)
        {
            PlayStateMachine.Instance.StartSpawnState();
            Debug.Log("Switch");
            AudioControl.Instance.SwitchMusic();
            ClickOut();
            MyGridManager.Instance.CancelShowBuildModeGrid();
            showCount = 0;
            faceDirection = 0;
            _selectDestroy = false;
            isSpawning = true;
            
            if (TeachText.Instance.talkCount == 2)
            {
                TeachText.Instance.talkCount = 3;
                //TeachText.Instance.LoadDialogue();
            }
            
            if (isTeaching3)
            {
                if (TeachText.Instance.talkCount == 3)
                {
                    TeachText.Instance.talkCount = 4;
                    //TeachText.Instance.LoadDialogue();
                }
            }
            
            if (isTeaching4)
            {
                if (TeachText.Instance.talkCount == 4)
                {
                    TeachText.Instance.talkCount = 5;
                    //TeachText.Instance.LoadDialogue();
                }
            }
            

        }
        
    }
    
    
    #endregion
    
    
   
}