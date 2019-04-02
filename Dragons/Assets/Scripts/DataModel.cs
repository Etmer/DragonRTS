using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModel 
{
    public List<Dictionary<HexPoint, HexTile>> map = new List<Dictionary<HexPoint, HexTile>>();

    public DataModel()
    {
        map.Add(new Dictionary<HexPoint, HexTile>());
        map.Add(new Dictionary<HexPoint, HexTile>());
        map.Add(new Dictionary<HexPoint, HexTile>());
    }
}
