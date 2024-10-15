﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    /// <summary>
    /// 记录一个起点的所有最短和次短路径
    /// </summary>
    public class Paths
    {
        public Vector2Int startPos;
        public List<Path> firstpaths;
        public List<Path> secondPaths;


        public Paths(Vector2Int startPos)
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

        internal int GetFirstPath()
        {
            var pathIdx = UnityEngine.Random.Range(0, firstpaths.Count);
            return firstpaths[pathIdx].pathId;
        }

        internal void LogCost(Vector2Int vector2)
        {
            foreach (var path in firstpaths)
            {
                path.LogCost(vector2);
            }
        }
    }
}
