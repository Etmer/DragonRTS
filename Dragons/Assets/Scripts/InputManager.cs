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

    private bool clicked;
    private HexPoint pointA;
    private HexPoint pointB;
    public HexPoint[] hexPoints;

    private void Start()
    {
        _currentHexPoint = new HexPoint(0, 0,_data.meshSizeX);
        _generator._map[_currentHexPoint].Select(true);
    }

    void Update()
    {
        _testTransform.position = CalculateWorldPosition();

        _currentHexPoint = GetCurrentHexPoint();

        if (Input.GetMouseButtonDown(0))
        {
            if (!clicked)
            {
                pointA = _currentHexPoint;
                pointA = pointA;
                clicked = true;
            }
            else
            {
                pointB = _currentHexPoint;
                hexPoints = CoordinateSystem.PointsBetweenHexPoints(pointA, pointB);
                foreach (HexPoint p in hexPoints)
                {
                    _generator._map[p].Select(true);
                    clicked = false;
                }
            }
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
        //IntersectionPos.z = IntersectionPos.z * -1;
        return IntersectionPos;
    }

    private HexPoint GetCurrentHexPoint()
    {
        HexPoint hex = CoordinateSystem.pixel_to_flat_hex(_testTransform.position, _data.meshSizeX);

        if (_generator._map.ContainsKey(hex) && _generator._map.ContainsKey(_currentHexPoint))
        {
            if (hex != _currentHexPoint)
            {
                _generator._map[_currentHexPoint].Select(false);
                _generator._map[hex].Select(true);
            }
            return hex;
        }
        return _currentHexPoint;
    }

    private void OnDrawGizmos()
    {
        if (hexPoints != null)
        {
            foreach (HexPoint p in hexPoints)
            {
                Gizmos.DrawWireSphere(CoordinateSystem.HexPointToPixel(p,p.width), 1);
            }
        }
    }
}
