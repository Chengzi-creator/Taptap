using System;
using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    public class Path
    {
        public Vector2 StartPos;
        private List<Vector2> path;
        private int cost;
        public Path(Vector2 startPos)
        {
            path = new List<Vector2>();
            cost = 0;
            StartPos = startPos;
        }

        public Path(Path path)
        {
            this.path = new List<Vector2>();
            foreach(var point in path.path)
            {
                this.path.Add(point);
            }
            this.cost = path.cost;
            this.StartPos = path.StartPos;
        }

        public void AddPoint(Vector2 point)
        {
            path.Insert(0, point);
            cost += 1;
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

        internal void LogCost(Vector2 vector2)
        {
            if (path.Contains(vector2))
                Debug.Log("startPos:" + StartPos + " pos: " + vector2 + path.IndexOf(vector2));
        }
    }
}
