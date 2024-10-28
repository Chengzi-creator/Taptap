using Algorithm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Debug = UnityEngine.Debug;
using System.Net.NetworkInformation;

public class MyGridManager : MonoBehaviour, IGraphicManager, IGridManager
{
    private static MyGridManager instance;
    public static MyGridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>("Prefab/GridManager")).GetComponent<MyGridManager>();
            }
            return instance;
        }
    }
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

    /// <summary>
    /// 终点格子
    /// </summary>
    private MyGrid endGrid = null;

    public void Init()
    {

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

    public void AdjustMapPos(ref Vector2 mapPos)
    {
        if (-0.05 < mapPos.x && mapPos.x < 0)
            mapPos.x = 0.01f;
        if (0.05 + width > mapPos.x && mapPos.x > width)
            mapPos.x = width - 0.01f;
        if (-0.05 < mapPos.y && mapPos.y < 0)
            mapPos.y = 0.01f;
        if (0.05 + length > mapPos.y && mapPos.y > length)
            mapPos.y = length - 0.01f;

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

    public Vector2 GetGridMidWorldPos(Vector2 worldPos, out bool isVaild)
    {
        Vector2Int mapPos = GetMapPos(worldPos);
        if (IsInMap(mapPos))
        {
            isVaild = true;
            return GetWorldPos(mapPos);
        }
        isVaild = false;
        return new Vector2(-100, -100);
    }
    public float GetWorldDistance(float mapDis)
    {
        return mapDis * MyGrid.GridSize.x;
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
    //    private Vector2 IsNearMap(Vector2 mapPos)
    //    {
    //        if(mapPos.x > -0.05)
    ///    }

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

    public bool LoadLevel(int levelIdx)
    {
        TextAsset text = null;
        try
        {
            text = Resources.Load<TextAsset>($"Map/Level{levelIdx}");
        }
        catch
        {
            Debug.LogError($"Resource hasn't Map/Level{levelIdx}");
        }
        return LoadMapFromFile(text);
    }

    public void UnloadLevel()
    {
        ClearMap();
    }
    private bool LoadMapFromFile(TextAsset text)
    {
        if (text == null)
        {
            Debug.LogError($"Load Map Error,please make sure has map file");
            return false;
        }
        MapState mapState = JsonConvert.DeserializeObject<MapState>(text.text);

        //Debug.Log("l:" + mapState.length + "w:" + mapState.width);

        Init(mapState);
        return true;
    }
    private void Init(MapState mapState)
    {
        width = mapState.width;
        length = mapState.length;
        myGrids = new MyGrid[width, length];
        MapStartPos = new Vector2(-width / 2 * 2, -length / 2 * 2);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                Vector3 WorldPos = new Vector3(i * MyGrid.GridSize.x, j * MyGrid.GridSize.y, 0) + MapStartPos;
                GameObject go = Instantiate(gridPrefab, WorldPos, Quaternion.identity, transform);
                var myGrid = go.GetComponent<MyGrid>();
                myGrid.Init(new Vector2Int(i, j), new Vector2(WorldPos.x, WorldPos.y),
                    GetGridType(mapState.Map[i, j][0].type));
                myGrids[i, j] = myGrid;
                //if(mapState.Map[i, j][0].type == MapObjectType.End)
                //{
                //    Debug.Log(myGrid.HoldObject.Type);
                //}
                if (myGrid.HoldObject.Type == GridObjectType.End)
                {
                    ColorBlockManager.Instance.SetTarget(WorldPos);
                    endGrid = myGrid;
                }
            }
        }
        myGraphic = new UndirectedGraph(this);
        CancelShowBuildModeGrid();
    }

    private void ClearMap()
    {
        myGrids = null;
        width = 0;
        length = 0;
        myGraphic = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }


    private GridObjectType GetGridType(MapObjectType mapObjectType)
    {
        switch (mapObjectType)
        {
            case MapObjectType.Ground:
                return GridObjectType.None;
            case MapObjectType.Start:
                return GridObjectType.Start;
            case MapObjectType.End:
                return GridObjectType.End;
            case MapObjectType.Obstacle:
                return GridObjectType.Obstacle;
            case MapObjectType.NoBuildGround:
                return GridObjectType.NoBuildGround;
            case MapObjectType.NoPassGround:
                return GridObjectType.NoPassGround;
        }
        return GridObjectType.None;
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


    public void ShowBuildModeGrid()
    {
        foreach (MyGrid myGrid in myGrids)
        {
            myGrid.ShowGrid();
        }
    }

    public void CancelShowBuildModeGrid()
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

    //public async void SetGridHoldObject(MyGrid grid, GridObject gridObject)
    //{
    //    bool hasPath = true;
    //    if (gridObject.Type == GridObjectType.Start || gridObject.Type == GridObjectType.End)
    //    {
    //        myGraphic.AddPoint(grid.MapPos, GetLinkPoints(grid.MapPos),
    //            gridObject.Type == GridObjectType.Start, gridObject.Type == GridObjectType.End);
    //        grid.SetHoldObject(gridObject);
    //        return;
    //    }

    //    if (gridObject.Type == GridObjectType.Obstacle || gridObject.Type == GridObjectType.Building)
    //    {
    //        myGraphic.RemovePoint(grid.MapPos);
    //    }

    //    Stopwatch sw = new Stopwatch();
    //    sw.Start();
    //    await Task.Run(() =>
    //    {
    //        hasPath = myGraphic.HasPath();
    //        if (!hasPath)
    //        {
    //            Debug.LogError("cant put obj");
    //        }
    //    });
    //    sw.Stop();
    //    Debug.Log("calculate time:" + sw.ElapsedMilliseconds + "ms");
    //    if (!hasPath)
    //    {
    //        myGraphic.AddPoint(grid.MapPos, GetLinkPoints(grid.MapPos));
    //    }
    //    else
    //    {
    //        grid.SetHoldObject(gridObject);
    //    }
    //}

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

    public ITower GetTower(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            return TowerManager.Instance.GetTower(mapPos);
        }
        return null;
    }

    public void BuildTower(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            GetGrid(mapPos).BuildTower();
            myGraphic.RemovePoint(mapPos);
        }
    }

    public void DestroyTower(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            GetGrid(mapPos).DestroyTower();
            if(GetGrid(mapPos).CanPass)
                myGraphic.AddPoint(mapPos, GetLinkPoints(mapPos));
        }
    }

    public void CalculateAllGridCanPutTower()
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if(!myGrids[i, j].CanPass)
                {
                    continue;
                }
                bool canPut = CanPutTower(new Vector2Int(i, j));
                //Debug.Log("canPut:" + canPut + " " + i + " " + j + myGrids[i, j].HoldObject.Type);  
                myGrids[i, j].CanPutTower = canPut;
            }
        }
        //sw.Stop();
        //Debug.LogWarning($" Time:{sw.ElapsedMilliseconds}ms");
    }

    public bool CanPutTower(Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            MyGrid grid = GetGrid(mapPos);
            if (grid.CanPutObj)
            {
                //Debug.Log(grid.HoldObject.Type + " " + mapPos + " " + grid.MapPos);
                return HasPathAfterPut(mapPos);
            }
        }
        return false;
    }

    public bool CanPutTower(ITowerManager.TowerType towerType, Vector2Int mapPos)
    {
        if (IsInMap(mapPos))
        {
            MyGrid grid = GetGrid(mapPos);

            return grid.CanPutObj && HasPathAfterPut(mapPos) && TowerManager.Instance.CanBuildTower(towerType, mapPos);
        }
        return false;
    }
    public void CalculatePath(bool drawPath = false)
    {
        // Debug.LogWarning("CalculatePath");
        PathManager = myGraphic.CalculatePath();
        if (drawPath)
            PathManager.DrawFirstPath(5);
    }

    public void DrawPath()
    {
        PathManager?.DrawFirstPath(-1);
    }

    public void ErasePath()
    {
        PathManager?.EraseFirstPath(-1);
    }

    public void DrawPath(int time)
    {
        PathManager?.DrawFirstPath(time);
    }

    public int GetPath(Vector2Int StartPos)
    {
        // Debug.Log($"GetPath:{StartPos}");
        return PathManager.GetPath(StartPos);
    }

    public Vector2Int GetTarget(int pathId, int idx)
    {
        return PathManager.GetTarget(pathId, idx);
    }

    public int GetPathCost(int id)
    {
        return PathManager.GetPathCost(id);
    }

    public int GetColor(int x, int y)
    {
        return TowerManager.Instance.GetColor(new Vector2Int(x, y));
    }

    public void ColorChanged(Vector2Int position)
    {
        if (IsInMap(position))
        {
            GetGrid(position).ColorChanged(GetColor(position.x, position.y));
        }
    }

    public void ChangeHomeHP(int hp)
    {
        Debug.Log("HP:" + hp);
        if (endGrid == null)
            Debug.Log("end is null");
        endGrid?.HPChange(hp);
    }
}

