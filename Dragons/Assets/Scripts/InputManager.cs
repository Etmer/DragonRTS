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
    TestState state;
    private Vector3 _mousePosition;
    [SerializeField] private Transform _testTransform;
    private HexPoint _currentHexPoint;

    private HexPoint _currentSelectedHexPoint;

    private void Start()
    {
        _currentHexPoint = new HexPoint(0, 0);
        GlobalGameManager.instance.Map[_currentHexPoint].Highlight(true);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //InputStateHandler.DeleteLine();
            //InputStateHandler.DeleteRange();
            state = TestState.uno;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            GlobalGameManager.instance.Map[_currentHexPoint].Highlight(false);
            //InputStateHandler.DeleteLine();
            //InputStateHandler.DeleteRange();
            state = TestState.dos;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GlobalGameManager.instance.Map[_currentHexPoint].Highlight(false);
            //InputStateHandler.DeleteLine();
            //InputStateHandler.DeleteRange();
            state = TestState.tres;
        }
        _testTransform.position = CalculateWorldPosition();

        _currentHexPoint = GetCurrentHexPoint();
        
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
        if (CoordinateSystem.PointIsOnMap(hex))
        {
            if (CoordinateSystem.PointIsOnMap(hex) && CoordinateSystem.PointIsOnMap(_currentHexPoint))
            {
                ProcessState(state, hex);
                return hex;
            }
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
                    GlobalGameManager.instance.Map[_currentHexPoint].Highlight(false);
                    GlobalGameManager.instance.Map[newPoint].Highlight(true);
                }
                break;
            case TestState.dos:
                //InputStateHandler.RefreshRange(newPoint, 5);
                break;
            case TestState.tres:
                //InputStateHandler.RefreshLine(_currentSelectedHexPoint, newPoint);
                break;
        }
    }
}
