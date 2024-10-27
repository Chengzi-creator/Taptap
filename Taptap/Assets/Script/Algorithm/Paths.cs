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
        public void DrawFirstPath(int time)
        {
            foreach (var path in firstpaths)
            {
                path.DrawPath(Color.red, time);
            }
        }

        public void EraseFirstPath(int time)
        {
            for (int i = 0;i<Path.pathsParent.transform.childCount; i++)
            {
                GameObject.Destroy(Path.pathsParent.transform.GetChild(i).gameObject);
            }
        }
        public void DrawSecondPath()
        {
            foreach (var path in secondPaths)
            {
                path.DrawPath(Color.black,5);
            }
        }

        internal int GetFirstPath()
        {
            if(firstpaths.Count == 0)
            {
                Debug.LogError("firstpaths.Count == 0");
                return -1;
            }
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
