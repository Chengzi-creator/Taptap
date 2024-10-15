using System;
using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    public class Path
    {
        private static int CreatePathId = 1;
        public Vector2Int StartPos;
        private List<Vector2Int> path;
        private int cost;
        public int Cost => cost;
        public int pathId;

        public Path(Vector2Int startPos)
        {
            //Debug.Log($"CreatePathId:{CreatePathId}");

            path = new List<Vector2Int>();
            cost = 0;
            StartPos = startPos;
            pathId = CreatePathId;
            CreatePathId++;
        }

        public Path(Path path)
        {
            this.path = new List<Vector2Int>();
            foreach(var point in path.path)
            {
                this.path.Add(point);
            }
            this.cost = path.cost;
            this.StartPos = path.StartPos;
        }

        public void AddPoint(Vector2Int point)
        {
            path.Insert(0, point);
            cost += 1;
        }

        public Vector2Int GetTarget(int idx)
        {
            if(idx < path.Count)
                return path[idx];
            else
                return new Vector2Int(-1, -1);
        }

        public void DrawPath(Color c)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2 world1 = MyGridManager.Instance.GetWorldPos(path[i]);
                Vector2 world2 = MyGridManager.Instance.GetWorldPos(path[i + 1]);
                Debug.DrawLine(world1, world2, c, 1);
            }
        }

        internal void LogCost(Vector2Int vector2)
        {
            if (path.Contains(vector2))
                Debug.Log("startPos:" + StartPos + " pos: " + vector2 + path.IndexOf(vector2));
        }
    }
}
