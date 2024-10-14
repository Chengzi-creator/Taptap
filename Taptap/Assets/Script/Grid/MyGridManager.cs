using Algorithm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MyGridManager : MonoBehaviour, IGraphicManager, IGridManager
{
    public static MyGridManager Instance;
    public int width;
    public int length;
    private MyGrid[,] myGrids;
    public GameObject gridPrefab;

    /// <summary>
    /// 地图起始位置
    /// </summary>
    [SerializeField]
    private Vector3 MapStartPos;

    /// <summary>
    /// 寻路算法
    /// </summary>
    UndirectedGraph myGraphic;

    public PathManager PathManager;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        //测试
        Init();
    }

    #region 坐标转换
    /// <summary>
    /// 返回Grid的世界坐标,计算得到
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public Vector2 GetWorldPos(Vector2 mapPos)
    {
        if (IsInMap(mapPos))
        {
            //注意起始格子的中心点在StarPos,因此不需要加上1/2的格子大小
            float x = mapPos.x * MyGrid.GridSize.x + MapStartPos.x;
            float y = mapPos.y * MyGrid.GridSize.y + MapStartPos.y;
            return new Vector2(x, y);
        }
        else
        {
            Debug.Log("out of range " + mapPos);
            return new Vector2(0, 0);
        }
    }

    public Vector2Int GetMapPos(Vector2 worldPos)
    {
        //注意起始格子的中心点在StarPos,因此加上1/2的格子大小
        Vector2 pos = worldPos - new Vector2(MapStartPos.x, MapStartPos.y) + MyGrid.GridSize * 1 / 2;
        int x = (int)(pos.x / MyGrid.GridSize.x);
        int y = (int)(pos.y / MyGrid.GridSize.y);
        Vector2 mapPos = new Vector2(x, y);
        if (IsInMap(mapPos))
            return new Vector2Int(x, y);
        return new Vector2Int(-1, -1);
    }
    #endregion

    #region 寻路使用
    /// <summary>
    /// 能否通过Grid，返回值为true则可以通过
    /// </summary>
    /// <param name="mapPos">地图坐标</param>
    /// <returns></returns>
    private bool CanPassGrid(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return myGrids[(int)mapPos.x, (int)mapPos.y].CanPass;
        }
        Debug.LogError("Wrong mapPos");
        return false;
    }

    private bool IsInMap(Vector2 mapPos)
    {
        return mapPos.x >= 0 && mapPos.x < width && mapPos.y >= 0 && mapPos.y < length;
    }

    private bool IsInMap(Vector2Int mapPos)
    {
        return mapPos.x >= 0 && mapPos.x < width && mapPos.y >= 0 && mapPos.y < length;
    }
    public bool IsStartGrid(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return myGrids[mapPos.x, mapPos.y].IsStartGrid;
        }
        Debug.LogError("Wrong mapPos");
        return false;
    }

    public bool IsEndGrid(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return myGrids[(int)mapPos.x, (int)mapPos.y].IsEndGrid;
        }
        Debug.LogError("Wrong mapPos");
        return false;
    }

    public List<Vector2Int> GetPoints()
    {
        List<Vector2Int> points = new List<Vector2Int>();
        foreach (MyGrid myGrid in myGrids)
        {
            if (CanPassGrid(myGrid.MapPos))
            {
                points.Add(myGrid.MapPos);
            }
        }
        return points;
    }

    public List<Vector2Int> GetLinkPoints(Vector2Int point)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        if (IsInMap(point))
        {
            Vector2Int p = new Vector2Int(point.x + 1, point.y);
            if (IsInMap(p) && CanPassGrid(p))
            {
                points.Add(p);
            }
            p = new Vector2Int(point.x - 1, point.y);
            if (IsInMap(p) && CanPassGrid(p))
            {
                points.Add(p);
            }
            p = new Vector2Int(point.x, point.y + 1);
            if (IsInMap(p) && CanPassGrid(p))
            {
                points.Add(p);
            }
            p = new Vector2Int(point.x, point.y - 1);
            if (IsInMap(p) && CanPassGrid(p))
            {
                points.Add(p);
            }
        }
        return points;
    }

    public bool CanPass(Vector2Int point)
    {
        return CanPassGrid(point);
    }

    public bool IsInGraph(Vector2Int point)
    {
        return IsInMap(point);
    }

    public bool IsStartPoint(Vector2Int point)
    {
        return IsStartGrid(point);
    }

    public bool IsEndPoint(Vector2Int point)
    {
        return IsEndGrid(point);
    }
    #endregion

    private void Init()
    {
        myGrids = new MyGrid[width, length];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                Vector3 WorldPos = new Vector3(i * MyGrid.GridSize.x, j * MyGrid.GridSize.y, 0) + MapStartPos;
                GameObject go = Instantiate(gridPrefab, WorldPos, Quaternion.identity, transform);
                var myGrid = go.GetComponent<MyGrid>();
                myGrid.Init(new Vector2Int(i, j), new Vector2(WorldPos.x, WorldPos.y));
                myGrids[i, j] = myGrid;
            }
        }
        myGraphic = new UndirectedGraph(this);
    }

    public IGrid GetIGrid(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return myGrids[mapPos.x, mapPos.y];
        }
        return null;
    }

    private MyGrid GetGrid(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return myGrids[mapPos.x, mapPos.y];
        }
        return null;
    }

    public List<IGrid> GetIGrids(List<Vector2Int> gridsPos)
    {
        List<IGrid> myGrids = new List<IGrid>();
        foreach (var mapPos in gridsPos)
        {
            var myGrid = GetGrid(mapPos);
            if (myGrid != null)
            {
                myGrids.Add(myGrid);
            }
        }
        return myGrids;
    }


    public void ShowGrid()
    {
        foreach (MyGrid myGrid in myGrids)
        {
            myGrid.ShowGrid();
        }
    }

    public void CancelShowGrid()
    {
        foreach (MyGrid myGrid in myGrids)
        {
            myGrid.CancelShowGrid();
        }
    }

    private bool HasPathAfterPut(Vector2Int mapPos)
    {
        myGraphic.RemovePoint(mapPos);
        bool hasPath = myGraphic.HasPath();
        myGraphic.AddPoint(mapPos, GetLinkPoints(mapPos));
        return hasPath;
    }

    public async void SetGridHoldObject(MyGrid grid, GridObject gridObject)
    {
        bool hasPath = true;
        if (gridObject.Type == GridObjectType.Start || gridObject.Type == GridObjectType.End)
        {
            myGraphic.AddPoint(grid.MapPos, GetLinkPoints(grid.MapPos),
                gridObject.Type == GridObjectType.Start, gridObject.Type == GridObjectType.End);
            grid.SetHoldObject(gridObject);
            return;
        }

        if (gridObject.Type == GridObjectType.Obstacle || gridObject.Type == GridObjectType.Building)
        {
            myGraphic.RemovePoint(grid.MapPos);
        }

        Stopwatch sw = new Stopwatch();
        sw.Start();
        await Task.Run(() =>
        {
            hasPath = myGraphic.HasPath();
            if (!hasPath)
            {
                Debug.LogError("cant put obj");
            }
        });
        sw.Stop();
        Debug.Log("calculate time:" + sw.ElapsedMilliseconds + "ms");
        if (!hasPath)
        {
            myGraphic.AddPoint(grid.MapPos, GetLinkPoints(grid.MapPos));
        }
        else
        {
            grid.SetHoldObject(gridObject);
        }
    }

    internal void LogCost(Vector2Int mapPos)
    {
        if (PathManager != null)
            PathManager.LogCost(mapPos);
    }

    public List<IEnemy> GetEnemys(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return GetGrid(mapPos).GetEnemys();
        }
        return null;
    }

    public int DirToEnd(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return GetGrid(mapPos).DisToEnd;
        }
        return 0;
    }

    public int EnemysCount(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return GetGrid(mapPos).EnemysCount();
        }
        return 0;
    }

    public IEnemy GetKthEnemy(Vector2Int mapPos, int k)
    {
        if (IsInMap(mapPos))
        {
            return GetGrid(mapPos).GetKthEnemy(k);
        }
        return null;
    }

    public BaseTower GetTower(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return GetGrid(mapPos).GetTower();
        }
        return null;
    }

    public void SetTower(Vector2Int mapPos, BaseTower tower)
    {
        if (IsInMap(mapPos))
        {
            GetGrid(mapPos).SetTower(tower);
        }
    }

    public bool CanPutTower(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            MyGrid grid = GetGrid(mapPos);

            return grid.CanPutObj && HasPathAfterPut(mapPos);
        }
        return false;
    }

    List<Vector2Int> testPath;
    public int GetPath(Vector2Int StartPos)
    {
        testPath = new List<Vector2Int>();
        testPath.Add(StartPos);
        testPath.Add(new Vector2Int(StartPos.x + 1, StartPos.y));
        testPath.Add(new Vector2Int(StartPos.x + 2, StartPos.y));
        return 0;
    }

    public Vector2Int GetTarget(int pathId, int idx)
    {
        if (idx < testPath.Count)
            return testPath[idx];
        return new Vector2Int(-1, -1);
    }

    public int GetPathCost(int id)
    {
        return 3;
    }
}

