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
    private bool _hexSelected;
    
    public HexPoint[] hexPoints = new HexPoint[0];

    private void Start()
    {
        _currentHexPoint = new HexPoint(0, 0);
        _generator._map[_currentHexPoint].Highlight(true);
    }

    void Update()
    {
        _testTransform.position = CalculateWorldPosition();

        _currentHexPoint = GetCurrentHexPoint();
        
        _testTransform.position += Vector3.up;

        if (Input.GetMouseButtonDown(0))
        {
            _hexSelected = !_hexSelected;

            _currentSelectedHexPoint = _currentHexPoint;
            _generator._map[_currentHexPoint].Select(true);
            DeleteRange();
        }
    }
    private void DeleteRange()
    {
        foreach (HexPoint p in hexPoints)
        {
            _generator._map[p].Select(false);
        }
    }
        private void RefreshLine(HexPoint a, HexPoint b)
    {
        foreach (HexPoint p in _line)
        {
            _generator._map[p].Highlight(false);
        }

        _line = CoordinateSystem.PointsBetweenHexPoints(a,b);

        if (a != b)
        {
            foreach (HexPoint p in _line)
            {
                _generator._map[p].Highlight(true);
            }
        }
    }
    private void RefreshRange(HexPoint center)
    {
        foreach (HexPoint p in hexPoints)
        {
            _generator._map[p].Select(false);
        }

        hexPoints = CoordinateSystem.CreateRings(center, 4).ToArray();
        
        foreach (HexPoint p in hexPoints)
        {
            _generator._map[p].Select(true);
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

    private HexPoint GetCurrentHexPoint()
    {
        HexPoint hex = CoordinateSystem.pixel_to_flat_hex(new Vector3(_testTransform.position.x,0,-_testTransform.position.z));

        if (_generator._map.ContainsKey(hex) && _generator._map.ContainsKey(_currentHexPoint))
        {
            if (hex != _currentHexPoint)
            {
                if (!_hexSelected)
                {
                    _generator._map[_currentHexPoint].Highlight(false);
                    _generator._map[hex].Highlight(true);
                    RefreshRange(hex);
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
