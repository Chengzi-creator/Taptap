using Algorithm;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MouseTest : MonoBehaviour
{
    public Button build;
    public Button start;
    public Button end;
    public Button obstacle;
    public Button calculate;
    public Button draw;

    public GridObject HoldObject;

    public static MouseTest Instance;

    private void Start()
    {
        Instance = this;
        build.onClick.AddListener(() =>
        {
            HoldObject = new GridObject(GridObjectType.Building);
            MyGridManager.Instance.ShowGrid();
        });
        start.onClick.AddListener(() =>
        {
            HoldObject = new GridObject(GridObjectType.Start);
            MyGridManager.Instance.ShowGrid();

        });
        end.onClick.AddListener(() =>
        {
            HoldObject = new GridObject(GridObjectType.End);
            MyGridManager.Instance.ShowGrid();

        });
        obstacle.onClick.AddListener(() =>
        {
            HoldObject = new GridObject(GridObjectType.Obstacle);
            MyGridManager.Instance.ShowGrid();

        });

        calculate.onClick.AddListener(() =>
        {
            HoldObject = null;
            calculatePath();
        });

        draw.onClick.AddListener(() =>
        {
            HoldObject = null;
            DrawPath();
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MyGridManager.Instance.CancelShowGrid();
        }
    }

    public async void calculatePath()
    {
        Stopwatch sw = new Stopwatch();
        await Task.Run(() =>
        {
            sw.Start();
            UndirectedGraph myGraphic = new UndirectedGraph(MyGridManager.Instance);
            bool hasPath = myGraphic.HasPath();
            sw.Stop();
            UnityEngine.Debug.LogWarning("haspath:" + hasPath+ $" Time:{sw.ElapsedMilliseconds}ms");
        });
    }

    public async void DrawPath()
    {
        Stopwatch sw = new Stopwatch();
        PathManager pathManager = null;
        await Task.Run(() =>
        {
            sw.Start();
            UndirectedGraph myGraphic = new UndirectedGraph(MyGridManager.Instance);
            pathManager = myGraphic.CalculatePath();
            sw.Stop();
            UnityEngine.Debug.LogWarning($" Time:{sw.ElapsedMilliseconds}ms");
        });
        if (pathManager != null)
        {
            pathManager.DrawFirstPath();
            MyGridManager.Instance.PathManager = pathManager;
        }
    }
}
