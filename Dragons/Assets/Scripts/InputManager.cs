using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestState
{
    uno,
    dos,
    tres
}

public class InputManager : MonoBehaviour
{
    public TileData midData;
    public TileData highData;
    TestState state;
    private Vector3 _mousePosition;
    [SerializeField] private Transform _testTransform;
    private HexPoint _currentHexPoint;
    private HexTile _currentHexTile;
    private HexPoint _currentSelectedHexPoint;

    private HexPoint[] _currentLine = new HexPoint[0];

    [SerializeField] private UnitManager _unitManager;
    private bool _selected;

    private void Start()
    {
        _currentHexPoint = new HexPoint(0, 0);
        _currentHexTile = null;

    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            CoordinateSystem.layer = (CoordinateSystem.layer + 1) % 3;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            CoordinateSystem.layer = (CoordinateSystem.layer - 1) % 3;
            if (CoordinateSystem.layer < 0)
            {
                CoordinateSystem.layer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //InputStateHandler.DeleteLine();
            //InputStateHandler.DeleteRange();
            state = TestState.uno;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            GlobalGameManager.instance.GetMapLayer(CoordinateSystem.layer)[_currentHexPoint].Highlight(false);
            //InputStateHandler.DeleteLine();
            //InputStateHandler.DeleteRange();
            state = TestState.dos;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GlobalGameManager.instance.GetMapLayer(CoordinateSystem.layer)[_currentHexPoint].Highlight(false);
            HexDrawTools.DeleteLine(ref _currentLine);
            HexDrawTools.DeleteRange(ref _currentLine);
            _currentSelectedHexPoint = _currentHexPoint;
            state = TestState.tres;
        }
        _testTransform.position = CalculateWorldPosition(CoordinateSystem.layer);

        _currentHexPoint = GetCurrentHexPoint();
        Debug.Log(_currentHexPoint.q + " / " + _currentHexPoint.r);
        
    }

    private Vector3 CalculateWorldPosition(int i)
    {
        Vector3 Offset = Vector3.up;
        _mousePosition.x = Input.mousePosition.x;
        _mousePosition.y = Input.mousePosition.y;
        _mousePosition.z = 10;
        if (i == 0)
        {
            Offset = Vector3.up * i;
        }
        else if (i == 1)
        {
            Offset = Vector3.up * midData.meshSizeY * 100;
        }
        else if (i == 2)
        {
            Offset = Vector3.up * highData.meshSizeY * 100;
        }
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        float delta = ray.origin.y - (Vector3.zero + Offset).y;

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
        if (CoordinateSystem.PointIsOnLayer(hex))
        {
          ProcessState(state, hex);
          return hex;
        }
        return _currentHexPoint;
    }

    public void ProcessState(TestState state, HexPoint newPoint)
    {
        switch (state)
        {
            case TestState.uno:
                if (newPoint != _currentHexPoint)
                {
                    if (_currentHexTile != null)
                    {
                        _currentHexTile.Highlight(false);
                    }
                    _currentHexTile = GlobalGameManager.instance.GetMapLayer(CoordinateSystem.layer)[newPoint];
                    if (_currentHexTile != null)
                    {
                        _currentHexTile.Highlight(true);
                    }
                }
                break;
            case TestState.dos:
                //InputStateHandler.RefreshRange(newPoint, 5);
                break;
            case TestState.tres:
                if (!_selected)
                {
                    HexDrawTools.DeleteLine(ref _currentLine);
                    HexDrawTools.DrawLine(_currentSelectedHexPoint, newPoint, ref _currentLine);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    _selected = true;
                    _unitManager.MoveUnit(null, _currentLine);
                }
                break;
        }
    }
}
