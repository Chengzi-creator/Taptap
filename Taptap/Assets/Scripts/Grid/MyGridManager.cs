using UnityEngine;

public class MyGridManager : MonoBehaviour
{
    public static MyGridManager Instance;
    public int width;
    public int length;
    private MyGrid[,] myGrids;
    public GameObject gridPrefab;

    /// <summary>
    /// 地图起始位置
    /// </summary>
    public Vector3 StartPos;

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
    /// 返回Grid的世界坐标，是中心值
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public Vector2 GetWorldPos(Vector2 mapPos)
    {
        if (mapPos.x >= 0 && mapPos.x < width && mapPos.y >= 0 && mapPos.y < length)
        {
            return myGrids[(int)mapPos.x, (int)mapPos.y].WorldPos;
        }
        else
        {
            Debug.Log("out of range");
            return new Vector2(0, 0);
        }
    }
    /// <summary>
    /// 获取世界坐标对应的格子坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2 GetMapPos(Vector2 worldPos)
    {
        //注意起始格子的中心点在StarPos,因此加上1/2的格子大小
        Vector2 pos = worldPos - new Vector2(StartPos.x, StartPos.y) + MyGrid.GridSize * 1 / 2;
        int x = (int)(pos.x / MyGrid.GridSize.x);
        int y = (int)(pos.y / MyGrid.GridSize.y);
        return new Vector2(x, y);
    }
    #endregion

    #region 寻路使用
    /// <summary>
    /// 能否通过Grid，返回值为true则可以通过
    /// </summary>
    /// <param name="mapPos">地图坐标</param>
    /// <returns></returns>
    public bool CanPassGrid(Vector2 mapPos)
    {
        if (IsInMap(mapPos))
        {
           return myGrids[(int)mapPos.x, (int)mapPos.y].CanPass;
        }
        Debug.LogError("Wrong mapPos");
        return false;
    }

    public bool IsInMap(Vector2 mapPos)
    {
        return mapPos.x >= 0 && mapPos.x < width && mapPos.y >= 0 && mapPos.y < length;
    }

    public bool IsEndGrid(Vector2 mapPos)
    {
        if (IsInMap(mapPos))
        {
            return myGrids[(int)mapPos.x, (int)mapPos.y].IsEndGrid;
        }
        Debug.LogError("Wrong mapPos");
        return false;
    }
    #endregion

    public void Init()
    {
        myGrids = new MyGrid[width, length];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                Vector3 WorldPos = new Vector3(i * MyGrid.GridSize.x, j * MyGrid.GridSize.y, 0) + StartPos;
                GameObject go = Instantiate(gridPrefab, WorldPos, Quaternion.identity, transform);
                var myGrid = go.GetComponent<MyGrid>();
                myGrid.Init(new Vector2(i, j), new Vector2(WorldPos.x, WorldPos.y));
                myGrids[i,j] = myGrid;
            }
        }
    }

    public void ShowEmpty()
    {
        foreach (MyGrid myGrid in myGrids)
        {
            myGrid.ShowEmpty();
        }
    }

    public void CancelShowEmpty()
    {
        foreach (MyGrid myGrid in myGrids)
        {
            myGrid.CancelShowEmpty();
        }
    }


}

