using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterTile : HexTile
{
    public WaterTile(GameObject worldObject, HexPoint points) : base(worldObject,points)
    {
        tileType = TileType.Water;
    }
}
