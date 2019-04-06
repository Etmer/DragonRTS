using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile<T> where T : HexTile
{
    private T _value;
    private HexTile _secondValue;
    public Transform _worldtransform;
    public MeshRenderer _renderer;

    public readonly Vector3 _defaultPosition;
    public readonly Vector3 _highlightedPosition;
    private HexStates _currentState;
    private Color _highLightColor = Color.green;
    private Color _defaultColor = Color.white;
    private Color _selectedColor = Color.red;
    private Color _currentColor;

    public T Value()
    {
        return _value;
    }

}
