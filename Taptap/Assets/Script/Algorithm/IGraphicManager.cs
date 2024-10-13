
using System.Collections.Generic;
using UnityEngine;

namespace Algorithm
{
    public interface IGraphicManager
    {
        public List<Vector2> GetPoints();
        public List<Vector2> GetLinkPoints(Vector2 point);

        public bool CanPass(Vector2 point);

        public bool IsInGraph(Vector2 point);
        public bool IsStartPoint(Vector2 point);
        public bool IsEndPoint(Vector2 point);
    }
}


