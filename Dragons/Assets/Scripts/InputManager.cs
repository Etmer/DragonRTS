using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestState
{
    free,
    building,
    other
}

public class InputManager : MonoBehaviour
{
    [SerializeField] private EffectManager _effectManager;
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
    
    private void Start()
    {
        _currentHexPoint = new HexPoint(0, 0);
        _currentHexTile = null;
        state = TestState.free;
    }

    public void Process()
    {
        MoveCamera();
        ChangeCameraHeight();
        ConvertWorldPositionToHexPosition();
        //ProcessMouseEvents();
        ProcessState(state, _currentHexPoint);
    }
    public Vector2 GetMousePosition()
    {
        _mousePosition.x = Input.mousePosition.x;
        _mousePosition.y = Input.mousePosition.y;
        _mousePosition.z = 10;
        return _mousePosition;
    }

    private void ConvertWorldPositionToHexPosition()
    {
        Vector3 worlPos = CalculateWorldPosition();

        _currentHexPoint = GetCurrentHexPoint(worlPos);
    }

    private Vector3 CalculateWorldPosition()
    {
        Vector3 Offset = Vector3.up;
        
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        float delta = ray.origin.y - Vector3.zero.y;

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
            case TestState.free:
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
                if (Input.GetMouseButtonDown(0))
                {
                    GlobalGameManager.instance.GlobalUIManager.SetInfoBox("Hello, I am an Info Box", _currentHexTile.highestPoint);
                    Debug.Log("clicked");
                }
                break;
            case TestState.building:
                break;
            case TestState.other:
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
            HexDrawTools.CreateRange(_currentHexPoint, 1, ref _currentLine);
            _effectManager.FlipRange(_currentLine);
        }
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Wheel clicked");
        }
    }
}
