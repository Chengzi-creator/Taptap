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


        public static GameObject head = Resources.Load<GameObject>("Prefab/Path/Head");
        public static GameObject tail = Resources.Load<GameObject>("Prefab/Path/Tail");
        public static GameObject mid = Resources.Load<GameObject>("Prefab/Path/Mid");
        public static GameObject pathsParent = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/Path/PathsParent"));

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
            foreach (var point in path.path)
            {
                this.path.Add(point);
            }
            this.cost = path.cost;
            this.StartPos = path.StartPos;
            pathId = CreatePathId;
            CreatePathId++;
        }

        public void AddPoint(Vector2Int point)
        {
            path.Insert(0, point);
            cost += 1;
        }

        public Vector2Int GetTarget(int idx)
        {
            if (idx < path.Count)
                return path[idx];
            else
                return new Vector2Int(-1, -1);
        }

        private Vector2Int dir;
        public void DrawPath(Color c, int time)
        {
            if (path.Count < 2)
                return;
            dir = path[1] - path[0];
            InstantiatePath(path[0], FaceDir(dir), head, time);
            for (int i = 1; i < path.Count; i++)
            {
                if (i < path.Count - 1 && path[i + 1] - path[i] == dir)
                {
                    InstantiatePath(path[i], FaceDir(dir), mid, time);
                }
                else if (i < path.Count - 1)
                {
                    InstantiatePath(path[i], FaceDir(dir), tail, time);
                    dir = path[i + 1] - path[i];
                    InstantiatePath(path[i], FaceDir(dir), head, time);
                }
                else
                {
                    InstantiatePath(path[i], FaceDir(dir), tail, time);
                }
            }
            //for(int i = 0; i < path.Count-1; i++)
            //{
            //    Debug.DrawLine(MyGridManager.Instance.GetWorldPos(path[i]), MyGridManager.Instance.GetWorldPos(path[i + 1]), c, time);
            //}
        }

        private int FaceDir(Vector2Int dir)
        {
            if (dir == Vector2Int.right)
                return 3;
            if (dir == Vector2Int.up)
                return 0;
            if (dir == Vector2Int.left)
                return 1;
            return 2;

        }

        private void InstantiatePath(Vector2 pos, int faceDir, GameObject prefab, int time)
        {
            GameObject go = GameObject.Instantiate(prefab, MyGridManager.Instance.GetWorldPos(pos),
                Quaternion.identity, pathsParent.gameObject.transform);
            go.transform.eulerAngles = new Vector3(0, 0, faceDir * 90);
            if (time != -1)
                DelayToInvoke.Instance.DestoryGameObject(go, time);
        }

        internal void LogCost(Vector2Int vector2)
        {
            if (path.Contains(vector2))
                Debug.Log("startPos:" + StartPos + " pos: " + vector2 + path.IndexOf(vector2));
        }
    }
}
