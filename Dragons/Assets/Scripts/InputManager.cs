using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector3 _mousePosition;
    [SerializeField] private Transform _testTransform;
    [SerializeField] private TileData _data;
    [SerializeField] private MapGenerator _generator;
    private HexPoint _currentHexPoint;
    public HexPoint[] _line = new HexPoint[0];

    private HexPoint _currentSelectedHexPoint;
    public Transform Debug1;
    public Transform Debug2;
    private bool _hexSelected;

    Vector3 minXY;
    Vector3 maxXY;
    
    public HexPoint[] hexPoints = new HexPoint[0];

    private void Start()
    {
        _currentHexPoint = new HexPoint(0, 0);
        GlobalGameManager.instance.Map[_currentHexPoint].Highlight(true);
    }

    void Update()
    {
        _testTransform.position = CalculateWorldPosition();

        _currentHexPoint = GetCurrentHexPoint();

        maxXY = WorldRect(new Vector3(0,0,0));
        minXY = WorldRect(new Vector3(Screen.width, Screen.height, 0));

        HexPoint min = CoordinateSystem.pixel_to_flat_hex(new Vector3(minXY.x,0,-minXY.z));

        HexPoint max = CoordinateSystem.pixel_to_flat_hex(new Vector3(maxXY.x, 0, -maxXY.z));

        GlobalGameManager.instance.Map[min].Highlight(true);
        GlobalGameManager.instance.Map[max].Highlight(true);
        _testTransform.position += Vector3.up;

        if (Input.GetMouseButtonDown(0))
        {
            _hexSelected = !_hexSelected;
            Debug.Log(_currentHexPoint.q + " / " + _currentHexPoint.r);
            _currentSelectedHexPoint = _currentHexPoint;
            GlobalGameManager.instance.Map[_currentHexPoint].Select(true);
            DeleteRange();
        }
    }
    private void DeleteRange()
    {
        foreach (HexPoint p in hexPoints)
        {
            GlobalGameManager.instance.Map[p].Select(false);
        }
    }
        private void RefreshLine(HexPoint a, HexPoint b)
    {
        foreach (HexPoint p in _line)
        {
            if(GlobalGameManager.instance.Map.ContainsKey(p))
                GlobalGameManager.instance.Map[p].Highlight(false);
        }

        _line = CoordinateSystem.PointsBetweenHexPoints(a,b);

        if (a != b)
        {
            foreach (HexPoint p in _line)
            {
                GlobalGameManager.instance.Map[p].Highlight(true);
            }
        }
    }
    private void RefreshRange(HexPoint center)
    {
        foreach (HexPoint p in hexPoints)
        {
            GlobalGameManager.instance.Map[p].Select(false);
        }

        hexPoints = CoordinateSystem.CreateRings(center, 4).ToArray();
        
        foreach (HexPoint p in hexPoints)
        {
            GlobalGameManager.instance.Map[p].Select(true);
        }
    }

    private Vector3 CalculateWorldPosition()
    {
        _mousePosition.x = Input.mousePosition.x;
        _mousePosition.y = Input.mousePosition.y;
        _mousePosition.z = 10;

        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        float delta = ray.origin.y - Vector3.zero.y;

        Vector3 dirNorm = ray.direction / ray.direction.y;
        Vector3 IntersectionPos = ray.origin - dirNorm * delta;
        return IntersectionPos;
    }
    private Vector3 WorldRect(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        float delta = ray.origin.y - Vector3.zero.y;

        Vector3 dirNorm = ray.direction / ray.direction.y;
        Vector3 IntersectionPos = ray.origin - dirNorm * delta;
        return IntersectionPos;
    }

    private HexPoint GetCurrentHexPoint()
    {
        HexPoint hex = CoordinateSystem.pixel_to_flat_hex(new Vector3(_testTransform.position.x,0,-_testTransform.position.z));

        if (GlobalGameManager.instance.Map.ContainsKey(hex) && GlobalGameManager.instance.Map.ContainsKey(_currentHexPoint))
        {
            if (hex != _currentHexPoint)
            {
                if (!_hexSelected)
                {
                    GlobalGameManager.instance.Map[_currentHexPoint].Highlight(false);
                    GlobalGameManager.instance.Map[hex].Highlight(true);
                    if (GlobalGameManager.instance.Map[hex].tileType == TileType.Island)
                    {
                        RefreshRange(hex);
                    }
                    else
                    {
                        DeleteRange();
                    }
                }
                else
                {
                    RefreshLine(_currentSelectedHexPoint, hex);
                }

            }
            return hex; 
        }
        return _currentHexPoint;
    }

    private void OnDrawGizmos()
    {
        if (hexPoints != null)
        {
            for (int p = 0; p < hexPoints.Length; p++)
            {
                if (p != 0)
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireSphere(CoordinateSystem.HexPointToPixel(hexPoints[p]), 1);
            }
        }
    }
}
