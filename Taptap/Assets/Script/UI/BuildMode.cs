using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对于建造模式最早的想法，是在游戏开始时开始计时，在一定时间内可以点击菜单选择塔来进行建造
/// 建造时需要注意地块是否可以建造（障碍物或者已经有别的塔或者堵住必经路线），资源icon是否足够承担花费
/// 在一定时间结束后或者玩家提前按出怪键可以停止建造模式，进入出怪时间
/// 出怪时间是必须时间跑完才会结束（？或者核心被彻底破坏，直接gameover）
/// </summary>
public class BuildMode : MonoBehaviour
{
    [SerializeField] private Button _buttonBF;
    [SerializeField] private Button _buttonBL;
    [SerializeField] private Button _buttonBT;
    [SerializeField] private Button _buttonDC;
    [SerializeField] private Button _buttonDH;
    [SerializeField] private Button _buttonDS;
    [SerializeField] private Button _buttonDestroy;

    private bool _selectBF;
    private bool _selectBL;
    private bool _selectBT;
    private bool _selectDC;
    private bool _selectDH;
    private bool _selectDS;
    private float _value;
    private int _faceDirection = 0;
    private Vector2 worldposition;
    private Vector2Int gridposition;
    private MyGridManager _myGridManager;
    private SourceText _sourceText;
    private ITowerManager _towerManager;
    
    private void Awake()
    {
        _buttonBF.onClick.AddListener(ClickBF);
        _buttonBT.onClick.AddListener(ClickBT);
        _buttonBL.onClick.AddListener(ClickBL);
        _buttonDH.onClick.AddListener(ClickDH);
        _buttonDS.onClick.AddListener(ClickDS);
        _buttonDC.onClick.AddListener(ClickDC);
        _buttonDestroy.onClick.AddListener(DestroyTower);
    }

    private void Start()
    {
        _myGridManager = gameObject.AddComponent<MyGridManager>();
        _sourceText = gameObject.AddComponent<SourceText>();
        _towerManager = GetComponent<ITowerManager>();
    }

    private void Update()
    {
       //TowerBuildPosition();//时时检测鼠标位置
    }
    
    
    #region 点击建造
    private void TowerBuildPosition()
    {   
        //增添炮塔图片跟随鼠标的效果，到时候旋转效果要实现的喵，尚待开发
        
        
        //只要点到任意一个按钮就进入这个状态，显示地图的可建造方块
        if (HasClick())
        {
            _myGridManager.ShowBuildModeGrid();
            
            //控制该塔的旋转方向
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _faceDirection = (_faceDirection + 1) % 4;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _faceDirection = (_faceDirection - 1 + 4) % 4;
            }
        }

        worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gridposition = _myGridManager.GetMapPos(worldposition);
        
        //检测是否可以建造并建造
        if (_myGridManager.CanPutTower(gridposition))
        {
            if (Input.GetMouseButtonDown(0)) //左键建造
            {
                TowerLoad();
            }
            if (Input.GetMouseButtonDown(1)) //右键退出
            {
                //退出建造该塔
                ClickOut();
            }
        }
        else
        {
            //显示无法建造？格子颜色改变？
        }
    }
    
    //想把鼠标位置，与放置塔的逻辑分开，否则在Update里会不断传入type
    private void TowerBuild(ITowerManager.TowerType type)
    {   
        //在按下按键的时候后面的逻辑都由TowerBuild接管？  
        _value = _towerManager.GetTowerAttribute(type).cost;

        if (_sourceText.Count >= _value)
        {
            _towerManager.CreateTower(type, gridposition, _faceDirection);//建造
            _sourceText.IconDecrease(_value);
            _myGridManager.BuildTower(gridposition); //还要将建造的信息传回去
            ClickOut();
        }
        else
        {
            ClickOut();    
        }
    }

    private void TowerLoad()
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
    #endregion

    #region 点击销毁
    public void DestroyTower()
    {
        //还不知道用什么键销毁，也是用一个button实现吧，当按了这个销毁键就会进入销毁模式
        Vector2 worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridposition = _myGridManager.GetMapPos(worldposition);
        
        //如果点击处有塔就在点击左键销毁？等我找找大家的函数先
        
    }
    #endregion


    #region 按钮点击
    private void ClickBF()
    {
        _selectBF = true;
        _selectBL = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
    }
    
    private void ClickBL()
    {
        _selectBL = true;
        _selectBF = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
    }
    private void ClickBT()
    {
        _selectBT = true;
        _selectBL = false;
        _selectBF = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
    }
    
    private void ClickDC()
    {
        _selectBT = false;
        _selectBL = false;
        _selectBF = false;
        _selectDC = true;
        _selectDH = false;
        _selectDS = false;
    }

    private void ClickDH()
    {
        _selectBT = false;
        _selectBL = false;
        _selectBF = false;
        _selectDC = false;
        _selectDH = true;
        _selectDS = false;
    }
    
    private void ClickDS()
    {
        _selectBF = false;
        _selectBL = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = true;
    }

    private void ClickOut()
    {
        _selectBF = false;
        _selectBL = false;
        _selectBT = false;
        _selectDC = false;
        _selectDH = false;
        _selectDS = false;
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
    
    #endregion
}