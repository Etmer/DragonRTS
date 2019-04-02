using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDrawTools
{

    public static void DrawLine(HexPoint a, HexPoint b, ref HexPoint[] output)
    {
        output = CoordinateSystem.PointsBetweenHexPoints(a, b);
        foreach (HexPoint p in output)
        {
            int layer = GlobalGameManager.instance.FindLayer(p);
            if (GlobalGameManager.instance.GetMapLayer(layer)[p].CurrentState != HexStates.Highlighted)
            {
                GlobalGameManager.instance.GetMapLayer(layer)[p].Highlight(true);
            }
        }
    }
    public static void DeleteLine(ref HexPoint[] line)
    {
        foreach (HexPoint p in line)
        {
            int layer = GlobalGameManager.instance.FindLayer(p);
            if (GlobalGameManager.instance.GetMapLayer(layer)[p].CurrentState == HexStates.Highlighted)
            {
                GlobalGameManager.instance.GetMapLayer(layer)[p].Highlight(false);
            }
        }
    }

    public static void CreateRange(HexPoint center, int rangeDistance,ref HexPoint[] range)
    {
        range = CoordinateSystem.CreateRings(center, rangeDistance);
        foreach (HexPoint p in range)
        {
            int layer = GlobalGameManager.instance.FindLayer(p);
            if (GlobalGameManager.instance.GetMapLayer(layer)[p].CurrentState != HexStates.Highlighted)
            {
                GlobalGameManager.instance.GetMapLayer(layer)[p].Highlight(true);
            }
        }
    }
    public static void DeleteRange(ref HexPoint[] range)
    {
        foreach (HexPoint p in range)
        {
            int layer = GlobalGameManager.instance.FindLayer(p);
            GlobalGameManager.instance.GetMapLayer(layer)[p].Highlight(false);
        }
    }

}
