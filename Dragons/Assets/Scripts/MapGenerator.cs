
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    [SerializeField] private TileData _data;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _startPrefab;
    [SerializeField] private GameObject _tile;
    [SerializeField] private TileFactory _tileFactory;

    private int _probability = 3;

    private void Start()
    {
        CoordinateSystem.width = _data.meshSizeX;
        HexPoint start = CoordinateSystem.pixel_to_flat_hex(Vector3.zero);
        CreateMap(start, 20);
    }
    
    private void CreateMap(HexPoint start, int radius)
    {
        List<HexPoint> results = new List<HexPoint> { start };

        for (int i = 1; i < radius; i++)
        {
            results.AddRange(CoordinateSystem.CreateRings(start, i));
        }

        foreach (HexPoint h in results)
        {
            PlacePrefab(h);
        }
    }

    private void PlacePrefab(HexPoint hPoint)
    {
        int currentProbability = Random.Range(0, 100);
        HexTile tile = null;
        if (hPoint == new HexPoint(0, 0) || _probability> currentProbability)
        {
            _tile = Instantiate(_startPrefab, Vector3.zero, Quaternion.identity);
            _tile.transform.position = CoordinateSystem.HexPointToPixel(hPoint);
            tile = new IslandTile(_tile,hPoint);
            tile._colorCoding = _tileFactory.defaultIslandTile._colorCoding;
            _probability = 5;
        }
        else
        {
            _probability += 1;
            _tile = Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            _tile.transform.position = CoordinateSystem.HexPointToPixel(hPoint);
            tile = new WaterTile(_tile, hPoint);
            tile._colorCoding = _tileFactory.defaultWaterTile._colorCoding;
        }

        GlobalGameManager.instance.Map.Add(hPoint, tile);
    }
}
