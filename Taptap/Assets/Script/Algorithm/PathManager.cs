using System.Collections.Generic;
using UnityEngine;
namespace Algorithm
{
    public class PathManager
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

        public void LogCost(Vector2 vector2)
        {
            foreach (var path in paths)
            {
                path.LogCost(vector2);
            }
        }
    }
}
