using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;

    private DataModel _model;

    public List<Dictionary<HexPoint, HexTile>> Map { get { return _model.map; } }

    public void AddToLayer(int layer, HexPoint Key, HexTile Value)
    {
        Map[layer].Add(Key, Value);
    }

    public Dictionary<HexPoint, HexTile> GetMapLayer(int layer)
    {
        return Map[layer];
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _model = new DataModel();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}
