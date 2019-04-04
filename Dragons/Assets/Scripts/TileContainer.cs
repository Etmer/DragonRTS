using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileContainer", menuName = "Create TileContainer/New")]
public class TileContainer : ScriptableObject
{
    [SerializeField] private TileData _layer00;
    [SerializeField] private TileData _layer01;
    [SerializeField] private TileData _layer02;
    [SerializeField] private TileData _layer03;

    public TileData Layer(WorldLayer layer)
    {
        switch (layer)
        {
            case WorldLayer.zero:
                return _layer00;
            case WorldLayer.one:
                return _layer01;
            case WorldLayer.two:
                return _layer02;
            case WorldLayer.three:
                return _layer03;
            default:
                throw new System.Exception();
        }
    }
}
