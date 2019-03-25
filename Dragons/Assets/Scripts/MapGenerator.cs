
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    [SerializeField] private TileData _data;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _startPrefab;
    [SerializeField] private GameObject _tile;
    private int _probability = 3;

    public Dictionary<HexPoint,HexTile> _map = new Dictionary<HexPoint, HexTile>();

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
        if (hPoint == new HexPoint(0, 0) || _probability> currentProbability)
        {
            _tile = Instantiate(_startPrefab, Vector3.zero, Quaternion.identity);
            _probability = 5;
        }
        else
        {
            _probability += 1;
            _tile = Instantiate(_prefab, Vector3.zero, Quaternion.identity);
        }
        _tile.transform.position = CoordinateSystem.HexPointToPixel(hPoint);
        HexTile tile = new HexTile(_tile, hPoint, _data.meshSizeX);
        _tile.GetComponent<DebugTile>().tile = hPoint;
        _map.Add(hPoint, tile);
    }
}
