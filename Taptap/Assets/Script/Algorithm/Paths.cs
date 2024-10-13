using System;
using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    /// <summary>
    /// 记录一个起点的所有最短和次短路径
    /// </summary>
    public class Paths
    {
        public Vector2 startPos;
        public List<Path> firstpaths;
        public List<Path> secondPaths;

        public Paths(Vector2 startPos)
        {
            this.startPos = startPos;
            firstpaths = new List<Path>();
            secondPaths = new List<Path>();
        }

        public void AddFirstPath(Path path)
        {
            firstpaths.Add(path);
        }

        public void AddSecondPath(Path path)
        {
            secondPaths.Add(path);
        }
        public void DrawFirstPath()
        {
            foreach (var path in firstpaths)
            {
                path.DrawPath(Color.red);
            }
        }
        public void DrawSecondPath()
        {
            foreach (var path in secondPaths)
            {
                path.DrawPath(Color.black);
            }
        }

        internal void LogCost(Vector2 vector2)
        {
            foreach (var path in firstpaths)
            {
                path.LogCost(vector2);
            }
        }
    }
}
