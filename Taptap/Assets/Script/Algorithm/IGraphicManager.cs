
using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    public interface IGraphicManager
    {
        public List<Vector2Int> GetPoints();
        public List<Vector2Int> GetLinkPoints(Vector2Int point);

        public bool CanPass(Vector2Int point);

        public bool IsInGraph(Vector2Int point);
        public bool IsStartPoint(Vector2Int point);
        public bool IsEndPoint(Vector2Int point);
    }
}


