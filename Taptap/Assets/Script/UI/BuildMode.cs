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
    [SerializeField] private Button _buttonF;
    [SerializeField] private Button _buttonL;
    [SerializeField] private Button _buttonT;
    [SerializeField] private Button _buttonDestroy;

    private bool _selectFlash;
    private bool _selectLazor;
    private bool _selectTorch;
    private MyGridManager _myGridManager;
    private SourceText _sourceText;
    private ITowerManager _towerManager;
    private float _value;
    private int _faceDirection = 0;
    
    private void Awake()
    {
        _buttonF.onClick.AddListener(ClickF);
        _buttonT.onClick.AddListener(ClickT);
        _buttonL.onClick.AddListener(ClickL);
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
        if (_selectFlash)
        {
            TowerBuild(ITowerManager.TowerType.B_flash);
        }

        if (_selectLazor)
        {
            TowerBuild(ITowerManager.TowerType.B_lazor);
        }

        if (_selectTorch)
        {
            TowerBuild(ITowerManager.TowerType.B_torch);
        }
    }

    #region 点击建造
    private void TowerBuild(ITowerManager.TowerType type)
    {   
        // 增添炮塔图片跟随鼠标的效果
        //进入这个状态时显示地图的可建造方块
        _myGridManager.ShowBuildModeGrid();
        
        Vector2 worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridposition = _myGridManager.GetMapPos(worldposition);

        _value = _towerManager.GetTowerAttribute(type).cost;
        
        //控制旋转方向
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _faceDirection = (_faceDirection + 1) % 4;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _faceDirection = (_faceDirection - 1 + 4) % 4;
        }
        
        //检测是否可以建造并建造
        if (_myGridManager.CanPutTower(gridposition))
        {
            if (_sourceText.Count >= _value)
            {
                if (Input.GetMouseButtonDown(0))//左键建造
                {
                    //建造
                    _towerManager.CreateTower(type, gridposition, _faceDirection);
                    _sourceText.IconDecrease(_value);
                    //还要将建造的信息传回去
                    _myGridManager.BuildTower(gridposition);
                }

                if (Input.GetMouseButtonDown(1))//右键退出
                {   
                    //退出建造函数
                    ClickOut();
                }
            }
            else
            {
                //钱不够，无法建造，显示下提示?
            }
        }
        else
        {
            //显示无法建造？
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
    private void ClickF()
    {
        _selectFlash = true;
        _selectLazor = false;
        _selectTorch = false;
    }
    
    private void ClickL()
    {
        _selectLazor = true;
        _selectFlash = false;
        _selectTorch = false;
    }
    private void ClickT()
    {
        _selectTorch = true;
        _selectLazor = false;
        _selectFlash = false;
    }

    private void ClickOut()
    {
        _selectFlash = false;
        _selectLazor = false;
        _selectTorch = false;
    }
    #endregion
}