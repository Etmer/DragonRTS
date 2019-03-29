using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterTile : HexTile
{
    public WaterTile(GameObject worldObject, HexPoint points, TileData data) : base(worldObject,points, data)
    {
        tileType = TileType.Water;
    }
}
