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
    private TowerX towerX;
    [SerializeField] private Button _buttonX;
    
    private TowerY towerY;
    [SerializeField] private Button _buttonY;
    
    private TowerZ towerZ;
    [SerializeField] private Button _buttonZ;

    private bool _selectX;
    private bool _selectY;
    private bool _selectZ;
    private MyGridManager _myGridManager;
    
    private void Start()
    {
        _buttonX.onClick.AddListener(SelectBuildX);
        _buttonY.onClick.AddListener(SelectBuildY);
        _buttonZ.onClick.AddListener(SelectBuildZ);
        towerX = new TowerX();
        towerY = new TowerY();
        towerZ = new TowerZ();
        _myGridManager = gameObject.AddComponent<MyGridManager>();
    }

    private void Update()
    {
        if (_selectX)
        {
            TowerXBuild();
        }
        if (_selectY)
        {
            TowerYBuild();
        }
        if (_selectX)
        {
            TowerZBuild();
        }
    }

    private void SelectBuildX()
    {
        _selectX = true;
        _selectY = false;
        _selectZ = false;
    }
    private void TowerXBuild()
    {      
        Debug.Log("Click");
        Vector2 worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 增添炮塔图片跟随鼠标的效果
        Vector2Int gridposition = _myGridManager.GetMapPos(worldposition);
        if (_myGridManager.CanPutTower(gridposition))
        {
            //可建造就是绿色
            if (Input.GetMouseButtonDown(1))
            {
                _selectX = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _myGridManager.SetTower(gridposition, towerX);
                _selectX = false;
            }
        }
        else
        {
            Debug.Log("Can not find");
        }
    }
    
    private void SelectBuildY()
    {
        _selectY = true;
        _selectX = false;
        _selectZ = false;
    }
    private void TowerYBuild()
    {
        Vector2 worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridposition = _myGridManager.GetMapPos(worldposition);
        if (_myGridManager.CanPutTower(gridposition))
        {   
            if (Input.GetMouseButtonDown(1))
            {
                _selectY = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                _myGridManager.SetTower(gridposition, towerY);
                _selectY = false;
            }
        }
        else
        {
            
        }
    }
    
    private void SelectBuildZ()
    {
        _selectZ = true;
        _selectX = false;
        _selectZ = false;
    }
    private void TowerZBuild()
    {
        Vector2 worldposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridposition = _myGridManager.GetMapPos(worldposition);
        if (_myGridManager.CanPutTower(gridposition))
        {   
            if (Input.GetMouseButtonDown(1))
            {
                _selectZ = false;
            }
            if (Input.GetMouseButtonDown(0))
            {  
                _myGridManager.SetTower(gridposition, towerZ);
                _selectZ = false;
            }
        }
        else
        {
            //增添不可放置的效果
        }
    }
}
