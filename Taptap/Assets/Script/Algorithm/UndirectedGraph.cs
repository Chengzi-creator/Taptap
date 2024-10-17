using Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Algorithm
{
    public class Point : IComparable<Point>
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector2Int pos;
        /// <summary>
        /// 邻居
        /// </summary>
        public List<Vector2Int> neighbors;

        /// <summary>
        /// 从起点到该点的最小代价
        /// </summary>
        public int cost;
        /// <summary>
        /// 从起点到该点的次小代价
        /// </summary>
        public int secondCost;

        /// <summary>
        /// 最短路径父节点,用于回溯路径
        /// </summary>
        public List<Point> parent;
        /// <summary>
        /// 次短路径父节点,用于回溯路径
        /// </summary>
        public List<Point> secondParent;
        public Point(Vector2Int pos)
        {
            this.pos = pos;
            neighbors = new List<Vector2Int>();
            parent = new List<Point>();
            secondParent = new List<Point>();
        }
        /// <summary>
        /// 设置最新的最小代价
        /// </summary>
        /// <param name="newcost"></param>
        /// <param name="newParent"></param>
        public void SetNewMinCost(int newcost, Point newParent)
        {
            secondCost = cost;
            cost = newcost;
            secondParent.Clear();
            foreach (Point parent in this.parent)
            {
                secondParent.Add(parent);
            }
            parent.Clear();
            parent.Add(newParent);
        }

        public static bool Equal(Point a, Point b)
        {
            return a.pos == b.pos;
        }

        public int CompareTo(Point other)
        {
            return cost.CompareTo(other.cost);
        }
    }


    public class UndirectedGraph
    {
        const int MAXCOST = 1000000;
        List<Point> points;

        List<Vector2Int> startPoints;

        List<Vector2Int> endPoints;

        IGraphicManager graphicManager;
        public UndirectedGraph(IGraphicManager graphicManager)
        {
            this.graphicManager = graphicManager;
            points = new List<Point>();
            startPoints = new List<Vector2Int>();
            endPoints = new List<Vector2Int>();

            foreach (Vector2Int point in graphicManager.GetPoints())
            {
                Point p = new Point(point);
                p.neighbors = graphicManager.GetLinkPoints(point);
                points.Add(p);

                if (graphicManager.IsStartPoint(point))
                {
                    startPoints.Add(point);
                }
                if (graphicManager.IsEndPoint(point))
                {
                    endPoints.Add(point);
                }
            }
        }
        /// <summary>
        /// 删除图中的点
        /// </summary>
        /// <param name="pos"></param>
        public void RemovePoint(Vector2Int pos)
        {
            Point point = GetPoint(pos);
            if (point != null)
            {
                points.Remove(point);
                foreach (Vector2Int p in point.neighbors)
                {
                    Point neighbor = GetPoint(p);
                    //Debug.Log(p+" remove neighbor:"+pos);
                    if (neighbor != null)
                        neighbor.neighbors.Remove(pos);
                    else
                        Debug.LogError("no point:" + neighbor);
                }
            }
        }
        /// <summary>
        /// 增加点
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="neighbors"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        public void AddPoint(Vector2Int pos, List<Vector2Int> neighbors, bool isStart = false, bool isEnd = false)
        {
            Point point = new Point(pos);
            point.neighbors = neighbors;
            points.Add(point);
            foreach (Vector2Int neighbor in neighbors)
            {
                Point neighborPoint = GetPoint(neighbor);
                //Debug.Log(pos + " add neighbor:" + neighbor);

                if (!neighborPoint.neighbors.Contains(pos))
                    neighborPoint.neighbors.Add(pos);
            }
            if (isStart)
            {
                startPoints.Add(pos);
            }
            if (isEnd)
            {
                endPoints.Add(pos);
            }
        }

        private bool IsStartPoint(Vector2Int pos)
        {
            return startPoints.Contains(pos);
        }
        private bool IsEndPoint(Vector2Int pos)
        {
            return endPoints.Contains(pos);
        }

        /// <summary>
        /// 是否有路径
        /// </summary>
        /// <returns></returns>
        public bool HasPath()
        {
            foreach (Vector2Int startPoint in startPoints)
            {
                //Debug.LogWarning("calculate startPoint:" + startPoint);
                if (HasPath(startPoint))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasPath(Vector2Int startPos)
        {
            List<Point> openList = new List<Point>();
            List<Vector2Int> closeList = new List<Vector2Int>();
            openList.Add(GetPoint(startPos));

            while (openList.Count != 0)
            {
                Point point = openList[0];
                openList.RemoveAt(0);
                closeList.Add(point.pos);
                if (point == null)
                {
                    Debug.LogError("point is null");
                    return false;
                }
                foreach (Vector2Int neighbor in point.neighbors)
                {
                    if (IsEndPoint(neighbor))
                    {
                        //Debug.Log("endPoint:" + neighbor);
                        return true;
                    }
                    if (!openList.Any(a => a.pos == neighbor) && !closeList.Contains(neighbor))
                    {
                        openList.Add(GetPoint(neighbor));
                    }
                }
            }
            return false;
        }

        public PathManager CalculatePath()
        {
            PathManager pathManager = new PathManager();
            foreach (Vector2Int startPoint in startPoints)
            {
                pathManager.AddPaths(CalculatePath(startPoint));
            }
            return pathManager;
        }

        public Paths CalculatePath(Vector2Int startPoint)
        {
            ResetGraphic();
            Point start = GetPoint(startPoint);

            PriorityList<Point> priorityList = new PriorityList<Point>();
            List<Vector2Int> openList = new List<Vector2Int>();
            List<Vector2Int> closeList = new List<Vector2Int>();
            start.cost = 0;
            start.secondCost = 0;
            priorityList.Enqueue(start);
            openList.Add(startPoint);
            while (!priorityList.IsEmpty)
            {
                Point cur = priorityList.Dequeue();
                openList.Remove(cur.pos);
                closeList.Add(cur.pos);

                foreach (Vector2Int neighbor in cur.neighbors)
                {
                    Point neighborPoint = GetPoint(neighbor);
                    if (neighborPoint == null)
                    {
                        //Debug.LogError("neighborPoint is null");
                        continue;
                    }
                    if (neighborPoint.cost > cur.cost + 1)
                    {
                        //Debug.Log("new cost neighborPoint:" + neighborPoint.pos + " cur:" + cur.pos +
                        //    " cost:" + neighborPoint.cost + " cur.cost:" + cur.cost);
                        neighborPoint.SetNewMinCost(cur.cost + 1, cur);
                        if (!openList.Contains(neighbor) && !closeList.Contains(neighbor))
                        {
                            priorityList.Enqueue(neighborPoint);
                            openList.Add(neighbor);
                        }
                    }
                    else if (neighborPoint.cost == cur.cost + 1)
                    {
                    //    Debug.Log("neighborPoint:" + neighborPoint.pos + " cur:" + cur.pos +
                    //        " cost:" + neighborPoint.cost + " cur.cost:" + cur.cost);
                        neighborPoint.parent.Add(cur);
                    }
                    else if (neighborPoint.secondCost > cur.cost + 1)
                    {
                        neighborPoint.secondCost = cur.cost + 1;
                        neighborPoint.secondParent.Clear();
                        neighborPoint.secondParent.Add(cur);
                    }
                    else if (neighborPoint.secondCost == cur.cost + 1)
                    {
                        neighborPoint.secondParent.Add(cur);
                    }
                }
            }


            Paths paths = new Paths(startPoint);

            foreach (Vector2Int endPoint in endPoints)
            {
                Point end = GetPoint(endPoint);
                if (end.cost < MAXCOST)
                {
                    Path myPath = new Path(startPoint);
                    myPath.AddPoint(endPoint);
                    paths.AddFirstPath(myPath);
                    nowpathCount += 1;
                    GetFirstPath(end, myPath, paths);
                }
            }
            return paths;
        }

        int maxPath = 100;
        int nowpathCount = 0;
        private void GetFirstPath(Point point, Path path, Paths paths)
        {
            int parentCount = point.parent.Count;

            if (parentCount == 0)
            {
                return;
            }
            //Debug.Log(point.pos + " new path:" + point.parent[0].pos);
            
            for (int i = 1; i < parentCount; i++)
            {
                if(nowpathCount >= maxPath)
                {
                    continue;
                }
                //Debug.Log(point.pos + " new path:" + point.parent[i].pos);
                Path myPath = new Path(path);
                myPath.AddPoint(point.parent[i].pos);
                nowpathCount++;
                paths.AddFirstPath(myPath);

                GetFirstPath(point.parent[i], myPath, paths);
            }

            path.AddPoint(point.parent[0].pos);
            GetFirstPath(point.parent[0], path, paths);
        }

        private void ResetGraphic()
        {
            foreach (Point point in points)
            {
                point.cost = MAXCOST;
                point.secondCost = MAXCOST;
                point.parent.Clear();
                point.secondParent.Clear();
            }
            nowpathCount = 0;
        }

        private Point GetPoint(Vector2Int pos)
        {
            return points.Find(p => p.pos == pos);
        }
    }
}

