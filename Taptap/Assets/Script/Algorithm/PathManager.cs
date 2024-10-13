﻿using System.Collections.Generic;
using UnityEngine;
namespace Algorithm
{
    public class PathManager : IPathManager
    {
        public List<Paths> paths;

        public PathManager()
        {
            paths = new List<Paths>();
        }

        public void AddPaths(Paths path)
        {
            paths.Add(path);
        }

        public void DrawFirstPath()
        {
            foreach (var path in paths)
            {
                path.DrawFirstPath();
            }
        }

        public void DrawSecondPath()
        {
            foreach (var path in paths)
            {
                path.DrawSecondPath();
            }
        }

        public Vector2 GetNextTarget(int pathId, Vector2 curPos)
        {
            throw new System.NotImplementedException();
        }

        public int GetPath(Vector2 StartPos)
        {
            throw new System.NotImplementedException();
        }

        public bool HasNextTarget(int pathId, Vector2 curPos)
        {
            throw new System.NotImplementedException();
        }

        public void LogCost(Vector2 vector2)
        {
            foreach (var path in paths)
            {
                path.LogCost(vector2);
            }
        }
    }
}