
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    [SerializeField] private TileData _data;
    [SerializeField] private GameObject _groundTile;
    [SerializeField] private GameObject _midTile;
    [SerializeField] private GameObject _highTile;
    [SerializeField] private GameObject _tile;
    [SerializeField] private TileFactory _tileFactory;
    public Camera cam;
    [SerializeField] private Rect ScreenRect;

    private int _probability = 15;

    Vector3 min;
    Vector3 max;

    HexPoint[] leftArray = new HexPoint[0];
    HexPoint[] rigthArray = new HexPoint[0];
    HexPoint[] topArray = new HexPoint[0];
    HexPoint[] bottomArray = new HexPoint[0];

    private void Start()
    {
        min = CalculateWorldPosition(Vector3.zero);
        max = CalculateWorldPosition(new Vector3(Screen.width, Screen.height, 0));

        CoordinateSystem.width = _data.meshSizeX;
        HexPoint start = CoordinateSystem.pixel_to_flat_hex(Vector3.zero);
        CreateMap(start, 20);
        CoordinateSystem.isInitialized = true;
    }
    private void Update()
    {
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
           Transform t = PlacePrefab(h);

            if (h.q == 0 && h.r == 0)
            {
                cam.transform.position = t.position + Vector3.forward * 5 + Vector3.up * 5 ;
                cam.transform.LookAt(t);
            }
        }
        Debug.Log(string.Format("TileCount: {0}",results.Count));
    }

    private Transform PlacePrefab(HexPoint hPoint)
    {
        int currentProbability = Random.Range(0, 100);
        HexTile tile = null;
        if ( _probability > currentProbability)
        {

            if (_probability > 20)
            {
                _tile = Instantiate(_highTile, Vector3.zero, Quaternion.identity);
                _tile.transform.position = CoordinateSystem.HexPointToWorldCoordinate(hPoint);
                tile = new IslandTile(_tile, hPoint, _data);
                tile._colorCoding = _tileFactory.defaultIslandTile._colorCoding;
                GlobalGameManager.instance.AddToLayer(2, hPoint, tile);
            }
            else
            {
                _tile = Instantiate(_midTile, Vector3.zero, Quaternion.identity);
                _tile.transform.position = CoordinateSystem.HexPointToWorldCoordinate(hPoint);
                tile = new IslandTile(_tile, hPoint, _data);
                tile._colorCoding = _tileFactory.defaultIslandTile._colorCoding;
                GlobalGameManager.instance.AddToLayer(1, hPoint, tile);
            }
            _probability = 5;

        }
        else
        {
            _probability += 1;
            _tile = Instantiate(_groundTile, Vector3.zero, Quaternion.identity);
            _tile.transform.position = CoordinateSystem.HexPointToWorldCoordinate(hPoint);
            tile = new WaterTile(_tile, hPoint, _data);
            tile._colorCoding = _tileFactory.defaultWaterTile._colorCoding;
            GlobalGameManager.instance.AddToLayer(0, hPoint, tile);
        }

        return _tile.transform;
    }

    private Vector3 CalculateWorldPosition(Vector3 point)
    {
        point.z = 10;

        Ray ray = Camera.main.ScreenPointToRay(point);
        float delta = ray.origin.y - Vector3.zero.y;

        Vector3 dirNorm = ray.direction / ray.direction.y;
        Vector3 IntersectionPos = ray.origin - dirNorm * delta;
        return IntersectionPos;
    }

    private bool InRect(Vector3 position)
    {
        ScreenRect = new Rect(min.x -200, min.z -200, Screen.width +400, Screen.height+400);
        Vector3 screenPos = cam.WorldToScreenPoint(position);
        return ScreenRect.Contains(screenPos);
    }

  
}
