using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputStateHandler : MonoBehaviour
{

    public static void DrawLine(HexPoint a, HexPoint b, ref HexPoint[] output)
    {
        output = CoordinateSystem.PointsBetweenHexPoints(a, b);
        foreach (HexPoint p in output)
        {
            if (GlobalGameManager.instance.Map[p].CurrentState != HexStates.Highlighted)
            {
                GlobalGameManager.instance.Map[p].Highlight(true);
            }
        }
    }
    public static void DeleteLine(ref HexPoint[] line)
    {
        foreach (HexPoint p in line)
        {
            if (GlobalGameManager.instance.Map[p].CurrentState == HexStates.Highlighted)
            {
                GlobalGameManager.instance.Map[p].Highlight(false);
            }
        }
    }

    public static void CreateRange(HexPoint center, int rangeDistance,ref HexPoint[] range)
    {
        range = CoordinateSystem.CreateRings(center, rangeDistance);
        foreach (HexPoint p in range)
        {
            if (GlobalGameManager.instance.Map[p].CurrentState != HexStates.Highlighted)
            {
                GlobalGameManager.instance.Map[p].Highlight(true);
            }
        }
    }
    public static void DeleteRange(ref HexPoint[] range)
    {
        foreach (HexPoint p in range)
        {
            GlobalGameManager.instance.Map[p].Highlight(false);
        }
    }

}
