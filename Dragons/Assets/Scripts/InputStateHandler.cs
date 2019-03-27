using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputStateHandler : MonoBehaviour
{
    private HexPoint[] _oldLine = new HexPoint[0];
    private HexPoint[] _newLine = new HexPoint[0];

    public void DrawLine(HexPoint a, HexPoint b)
    {
        _newLine = CoordinateSystem.PointsBetweenHexPoints(a, b);
        foreach (HexPoint p in _newLine)
        {
            if (GlobalGameManager.instance.Map[p].CurrentState != HexStates.Highlighted)
            {
                GlobalGameManager.instance.Map[p].Highlight(true);
            }
        }
        DeleteLine(_oldLine);
    }

    public void DeleteLine(HexPoint[] line)
    {
        foreach (HexPoint p in line)
        {
            if (GlobalGameManager.instance.Map[p].CurrentState != HexStates.Highlighted)
            {
                GlobalGameManager.instance.Map[p].Highlight(false);
            }
        }
    }
}
