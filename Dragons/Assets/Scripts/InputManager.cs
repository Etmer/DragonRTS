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
    private HexTile _lastHexTile;

    private HexPoint _currentSelectedHexPoint;
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _camHolder;
    private HexPoint[] _currentLine = new HexPoint[0];

    [SerializeField] private UnitManager _unitManager;
    private bool _selected;

    private void Start()
    {
        _currentHexPoint = new HexPoint(0, 0);
        _currentHexTile = null;
        state = TestState.uno;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = TestState.tres;
        }
        MoveCamera();
        ChangeCameraHeight();
        ConvertWorldPositionToHexPosition();
        ProcessMouseEvents();

        ProcessState(state, _currentHexPoint);
    }

    private void ConvertWorldPositionToHexPosition()
    {
        Vector3 worlPos = CalculateWorldPosition(CoordinateSystem.layer);

        _currentHexPoint = GetCurrentHexPoint(worlPos);
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

    private HexPoint GetCurrentHexPoint(Vector3 worldPos)
    {
        HexPoint hex = CoordinateSystem.pixel_to_flat_hex(new Vector3(worldPos.x,0,-worldPos.z), out hex);
        if (CoordinateSystem.PointIsOnMap(hex))
        {
            _currentHexTile = GlobalGameManager.instance.Map[hex];
            return hex;
        }
        return _currentHexPoint;
    }
    
    public void ProcessState(TestState state, HexPoint newPoint)
    {
        switch (state)
        {
            case TestState.uno:
                if (_lastHexTile != _currentHexTile)
                {
                    if (_lastHexTile != null)
                    {
                        _lastHexTile.Highlight(false);
                    }
                    if (_currentHexTile != null)
                    {
                        _currentHexTile.Highlight(true);
                    }
                    _lastHexTile = _currentHexTile;
                }
                break;
            case TestState.dos:
                HexDrawTools.DeleteRange(ref _currentLine);
                HexDrawTools.CreateRange(newPoint, 5,ref _currentLine);
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

    private void MoveCamera()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _camHolder.position += Vector3.left * Input.GetAxisRaw("Horizontal");
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            _camHolder.position += Vector3.back * Input.GetAxisRaw("Vertical");
        }
    }

    private void ChangeCameraHeight()
    {
        Vector3 lookDirection = _cam.transform.rotation * Vector3.forward;

        if (_camHolder.position.y <= 15)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                _camHolder.position -= lookDirection;
            }
        }

        if (_camHolder.position.y > 3)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                _camHolder.position += lookDirection;
            }
        }

        else
        {
            float yAxis = Mathf.Clamp(_camHolder.position.y, 3, 15);
            _camHolder.position = new Vector3(_camHolder.position.x, yAxis, _camHolder.position.z);
        }
    }

    private void ProcessMouseEvents()
    {

        if (Input.GetMouseButtonDown(0))
        {
            _testTransform.position = _currentHexTile.highestPoint;
        }
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Wheel clicked");
        }
    }
}
