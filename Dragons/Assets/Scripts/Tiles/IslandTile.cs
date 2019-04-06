using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IslandTile : HexTile
{
    public IslandTile(GameObject worldObject, HexPoint point, TileData data) : base(worldObject, point, data)
    {
        tileType = TileType.Island;
    }
}
