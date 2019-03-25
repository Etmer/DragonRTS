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
    private Color _highLightColor = Color.red;
    private Color _defaultColor = Color.white;

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
        _highlightedPosition = _defaultPosition + Vector3.up / 5;
    }

    public void Select(bool state)
    {
        if (state)
        {
            ChangeState(HexStates.Selected);
        }
        else
        {
            ChangeState(HexStates.Idle);
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
            _renderer.material.color = _highLightColor;
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

        switch (_currentState)
        {
            case HexStates.Idle: // from Idle to Selected
                Lift(true);
                ChangeColor(true);
                _currentState = desiredState;
                break;
            case HexStates.Selected: // from Selected to Idle
                Lift(false);
                ChangeColor(false);
                _currentState = desiredState;
                break;
        }
    }

}
