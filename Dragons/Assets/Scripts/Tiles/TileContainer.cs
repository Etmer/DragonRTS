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
    [SerializeField] private TileData _layer04;

    public TileData Layer(TileHeight layer)
    {
        switch (layer)
        {
            case TileHeight.zero:
                return _layer00;
            case TileHeight.one:
                return _layer01;
            case TileHeight.two:
                return _layer02;
            case TileHeight.three:
                return _layer03;
            case TileHeight.four:
                return _layer04;
            default:
                throw new System.Exception();
        }
    }
}
