using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile
{
    [SerializeField] public Transform _worldtransform;
    [SerializeField] public MeshRenderer _renderer;

    public readonly Vector3 _defaultPosition;
    public readonly Vector3 _highlightedPosition;
    private float _width;
    private HexStates _currentState;
    private Color _highLightColor = Color.green;
    private Color _defaultColor = Color.white;
    private Color _selectedColor = Color.red;
    private Color _currentColor;

    public int _q;
    public int _r;

    private List<HexTile> _neigbours = new List<HexTile>();

    public HexTile(GameObject worldObject, HexPoint point, float width)
    {
        _currentState = HexStates.Idle;
        _q = (int)point.q;
        _r = (int)point.r;
        _width = width;
        _worldtransform = worldObject.transform;
        _defaultPosition = _worldtransform.position;
        _renderer = worldObject.GetComponentInChildren<MeshRenderer>();
        _highlightedPosition = _defaultPosition + Vector3.up;
    }

    public void Highlight(bool state)
    {
        if (state)
        {
            ChangeState(HexStates.Highlighted);
        }
        else if(!state && CurrentState != HexStates.Selected)
        {
            ChangeState(HexStates.Idle);
        }
    }
    public void Select(bool state)
    {
        if (state)
        {
            ChangeState(HexStates.Selected);
        }
        else
        {
            ChangeState(HexStates.Deselected);
        }
    }

    private void Lift(bool state)
    {
        if (state)
        {
            _worldtransform.position = _highlightedPosition;
        }
        else
        {
            _worldtransform.position = _defaultPosition;
        }
    }

    private void ChangeColor(bool state)
    {
        if (state)
        {
            _renderer.material.color = _currentColor;
        }
        else
        {
            _renderer.material.color = _defaultColor;
        }
    }

    private void ChangeState(HexStates desiredState)
    {
        if (desiredState == _currentState)
        {
            return;
        }

        switch (desiredState)
        {
            case HexStates.Idle:
                if (_currentState == HexStates.Highlighted || _currentState == HexStates.Deselected)
                {
                    Lift(false);
                    ChangeColor(false);
                    _currentState = desiredState;
                    break;
                }
                break;
            case HexStates.Highlighted:
                if (_currentState == HexStates.Idle)
                {
                    _currentColor = _highLightColor;
                    Lift(true);
                    ChangeColor(true);
                    _currentState = desiredState;
                    break;
                }
                if (_currentState == HexStates.Selected)
                {
                    break;
                }
                break;
            case HexStates.Selected:
                if (_currentState == HexStates.Idle)
                {
                    Lift(true);
                    _currentColor = _selectedColor;
                    ChangeColor(true);
                    _currentState = desiredState;
                    break;
                }
                else if (_currentState == HexStates.Highlighted)
                {
                    _currentColor = _selectedColor;
                    ChangeColor(true);
                    _currentState = desiredState;
                    break;
                }
                break;
            case HexStates.Deselected:
                if (_currentState == HexStates.Selected)
                {
                    Lift(false);
                    ChangeColor(false);
                    _currentState = desiredState;
                    ChangeState(HexStates.Idle);
                    break;
                }
                break;
        }

    }

    public HexStates CurrentState { get { return _currentState; } }

}
