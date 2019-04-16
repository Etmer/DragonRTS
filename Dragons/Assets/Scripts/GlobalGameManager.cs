using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;

    private DataModel _model;

    [SerializeField] private InputManager _inputManager;

    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private UIManager _uiManager;

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

    private void Update()
    {
        Vector2 mousePosition = _inputManager.GetMousePosition();

        if (_uiManager.PointInRect(mousePosition))
        {
            _inputManager.Process();
        }
    }
    public Dictionary<HexPoint, HexTile> Map { get { return _model.map; } }
}
