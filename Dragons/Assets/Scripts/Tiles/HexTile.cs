﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexTile
{
    [SerializeField] public Transform _worldtransform;
    public Vector3 highestPoint;
    [SerializeField] public MeshRenderer _renderer;

    public readonly Vector3 _defaultPosition;
    public readonly Vector3 _highlightedPosition;
    private float _width;
    [SerializeField] public HexColorCoding _colorCoding;
    private Color _currentColor;
    public HexStates _currentState;
    public TileType tileType;
    private Material _material;

    public int _q;
    public int _r;

    private List<HexTile> _neigbours = new List<HexTile>();

    public HexTile(HexTile tile ,GameObject worldObject, TileData data)
    {
        tileType = TileType.Default;
        _currentState = HexStates.Idle;
        _worldtransform = worldObject.transform;
        _defaultPosition = _worldtransform.position;
        _renderer = worldObject.GetComponentInChildren<MeshRenderer>();
        _material = _renderer.material;
        _highlightedPosition = _defaultPosition + Vector3.up * 0.1f;
        _colorCoding = tile._colorCoding;
        highestPoint = _defaultPosition + Vector3.up * (data.meshSizeY - MapGenerator.DefaultTile.meshSizeY/2);
    }

    public  HexTile(GameObject worldObject, HexPoint point, TileData data)
    {
        tileType = TileType.Default;
        _currentState = HexStates.Idle;
        _q = (int)point.q;
        _r = (int)point.r;
        _worldtransform = worldObject.transform;
        _defaultPosition = _worldtransform.position;
        _renderer = worldObject.GetComponentInChildren<MeshRenderer>();
        _material = _renderer.material;
        _highlightedPosition = _defaultPosition + Vector3.up * 0.1f;
        highestPoint = _defaultPosition + Vector3.up * (data.meshSizeY - MapGenerator.DefaultTile.meshSizeY/2);
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
            _renderer.material.color = _colorCoding.defaultColor;
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
                    _currentColor = _colorCoding.highLightColor;
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
                    _currentColor = _colorCoding.selectedColor;
                    ChangeColor(true);
                    _currentState = desiredState;
                    break;
                }
                else if (_currentState == HexStates.Highlighted)
                {
                    _currentColor = _colorCoding.selectedColor;
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

    public void Levitate(int index, Mesh newMesh)
    {
        _worldtransform.position = _defaultPosition + Vector3.up * index * 0.15f;

        if (index == 0)
        {
            _worldtransform.gameObject.GetComponent<MeshFilter>().mesh = newMesh;
        }
    }

    public void OnExit()
    {

    }

    public void OnEnter()
    {

    }

}
