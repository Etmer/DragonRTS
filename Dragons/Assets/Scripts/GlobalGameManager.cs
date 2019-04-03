using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;

    private DataModel _model;

    public Dictionary<HexPoint, HexTile> Map { get { return _model.map; } }
   
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
