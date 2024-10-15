using System.Collections.Generic;
using UnityEngine;

public class MapState
{
    public int length, width;

    public List<MapData>[,] Map;
    public string[,] MapName;
    public MapState(int l, int w, MapData defaultData, string defaultName)
    {
        length = l;
        width = w;
        Map = new List<MapData>[length, width];
        MapName = new string[length, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Map[i, j] = new List<MapData>
                {
                    defaultData
                };
                MapName[i, j] = defaultName;
            }
        }
    }
}