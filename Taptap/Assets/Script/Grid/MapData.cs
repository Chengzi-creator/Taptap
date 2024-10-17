public enum MapObjectType
{
    Ground,
    Start = 10,
    End = 11,
    Obstacle = 12,
    NoBuildGround = 13,
    NoPassGround = 14,
}

public class MapData
{
    public string Name;
    public MapObjectType type;
}