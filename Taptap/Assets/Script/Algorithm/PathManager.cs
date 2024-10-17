using System.Collections.Generic;
using UnityEngine;
namespace Algorithm
{
    public class PathManager : IPathManager
    {
        public List<Paths> paths;

        private Dictionary<int, Path> pathDic = new Dictionary<int, Path>();
        private Dictionary<Vector2Int, Paths> pathsDic = new Dictionary<Vector2Int, Paths>();
        public PathManager()
        {
            paths = new List<Paths>();
        }

        public void AddPaths(Paths paths)
        {
            this.paths.Add(paths);
            pathsDic.Add(paths.startPos, paths);
            foreach (Path p in paths.firstpaths)
            {
                //Debug.Log(p.pathId);
                pathDic.Add(p.pathId, p);
            }
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

        public Vector2Int GetTarget(int pathId, int idx)
        {
            if (pathDic.ContainsKey(pathId))
                return pathDic[pathId].GetTarget(idx);
            else
            {
                Debug.LogError($"PathManager has't id = {pathId}, Error!!!!");
                return new Vector2Int(-1, -1);
            }
        }

        public int GetPath(Vector2Int StartPos)
        {
            return pathsDic[StartPos].GetFirstPath();
        }

        public int GetPathCost(int id)
        {
            return pathDic[id].Cost;
        }

        public void LogCost(Vector2Int vector2)
        {
            foreach (var path in paths)
            {
                path.LogCost(vector2);
            }
        }
    }
}
