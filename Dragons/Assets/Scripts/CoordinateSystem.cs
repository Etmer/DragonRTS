using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem 
{
    public static int layer;

    public static float width;

    public static bool isInitialized = false;

    public static bool PointIsOnLayer(HexPoint point)
    {
        return GlobalGameManager.instance.Map[layer].ContainsKey(point);
    }
    public static bool PointIsOnMap(HexPoint point)
    {
        return GlobalGameManager.instance.Map[0].ContainsKey(point) || GlobalGameManager.instance.Map[1].ContainsKey(point) || GlobalGameManager.instance.Map[2].ContainsKey(point);
    }

    public static HexPoint pixel_to_flat_hex(Vector3 position)
    {
        float q = ((2.0f / 3) * position.x) / (width / 2);
        float r = (-(1.0f / 3) * position.x + (Mathf.Sqrt(3) / 3) * position.z) / (width / 2);
        return RoundToNearestHex(new HexPoint(q,r));
    }

    public static Vector3 HexPointToWorldCoordinate(HexPoint hPoint)
    {
        float x = (width / 2f) * ((3.0f / 2) * hPoint.q);
        float z = (width / 2f) * (Mathf.Sqrt(3) / 2 * hPoint.q + Mathf.Sqrt(3) * hPoint.r);
        return new Vector3((float)x, 0, (float)-z);
    }
    public static Vector2 HexPointToScreenPixel(HexPoint hPoint)
    {
        float x = (width / 2f) * ((3.0f / 2) * hPoint.q);
        float y = (width / 2f) * (Mathf.Sqrt(3) / 2 * hPoint.q + Mathf.Sqrt(3) * hPoint.r);
        return new Vector2(x,y);
    }

    public static CubePoint AxialToCubePoint(HexPoint hex)
    {
        float x = hex.q;
        float z = hex.r;
        float y = -x -z;
        return new CubePoint(x,y,z);
    }

    public static HexPoint CubeToAxialPoint(CubePoint cube)
    {
        float q = cube.x;
        float r = cube.z;
        return new HexPoint(q,r);
    }

    public static CubePoint RoundToNearestCube(CubePoint cube)
    {
        int rx = Mathf.RoundToInt((float)cube.x);
        int ry = Mathf.RoundToInt((float)cube.y);
        int rz = Mathf.RoundToInt((float)cube.z);

        float x_diff = Mathf.Abs((float)(rx - cube.x));
        float y_diff = Mathf.Abs((float)(ry - cube.y));
        float z_diff = Mathf.Abs((float)(rz - cube.z));

        if (x_diff > y_diff && x_diff > z_diff)
        {
            rx = -ry - rz;
        }
        else if (y_diff > z_diff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }
        float x = rx;
        float y = ry;
        float z = rz;
        return new CubePoint(x,y,z);
    }

    public static HexPoint RoundToNearestHex(HexPoint point)
    {
        return CubeToAxialPoint(RoundToNearestCube(AxialToCubePoint(point)));
    }

    public static HexPoint[] CreateRings(HexPoint start, int radius)
    {
        List<HexPoint> result = new List<HexPoint>();
        
        HexPoint newNeighbour = start + Direction(4) * radius;

        for (int i = 0; i < 6; i++)
        {
            for (int d = 0; d < radius; d++)
            {
                if (PointIsOnMap(newNeighbour) || !isInitialized)
                {
                    result.Add(newNeighbour);
                }
                newNeighbour = newNeighbour + Direction(i);
            }
        }
        return result.ToArray();
    }

    public static HexPoint Scale(HexPoint point, int scale)
    {
        point.q *= scale;
        point.r *= scale;

        return point;
    }

    public static HexPoint Direction(int index)
    {
        switch (index)
        {
            case 0:
                return new HexPoint( + 1, + 0);
            case 1:
                return new HexPoint( + 1, - 1);
            case 2:
                return new HexPoint( + 0, - 1);
            case 3:
                return new HexPoint( - 1, + 0);
            case 4:
                return new HexPoint( - 1, + 1);
            case 5:
                return new HexPoint( + 0, + 1);
            default:
                throw new System.Exception();
        }
    }

    public static HexPoint[] PointsBetweenHexPoints(HexPoint a, HexPoint b)
    {
        CubePoint ah = AxialToCubePoint(a);
        CubePoint bh = AxialToCubePoint(b);

        List<HexPoint> points = new List<HexPoint>();
        
        int N = Mathf.RoundToInt(CubeDistance(ah,bh));

        for (int i = 0; i <= N; i++)
        {
            CubePoint cubePoint = CubeLerp(ah, bh, 1.0f / N * i);
            HexPoint hexPoint = CubeToAxialPoint(RoundToNearestCube(cubePoint));
            if (!points.Contains(hexPoint))
            {
                if (PointIsOnMap(hexPoint))
                {
                    points.Add(hexPoint);
                }
            }
            else
            {
                hexPoint = CubeToAxialPoint(RoundToNearestCube(cubePoint - new CubePoint(1 * 0.2f, 2 * 0.2f, -3 * 0.2f)));
                if (PointIsOnMap(hexPoint))
                {
                    points.Add(hexPoint);
                }
            }
        }
        return points.ToArray();
        
    }

    private static CubePoint CubeLerp(CubePoint a, CubePoint b, float step)
    {
        float x = a.x + (b.x - a.x) * step;
        float y = a.y + (b.y - a.y) * step;
        float z = a.z + (b.z - a.z) * step;

        return new CubePoint(x,y,z);
    }

    private static float HexDistance(HexPoint a, HexPoint b)
    {
        CubePoint ac = AxialToCubePoint(a);
        CubePoint bc = AxialToCubePoint(b);

        return CubeDistance(ac,bc);
    }

    private static float CubeDistance(CubePoint a, CubePoint b)
    {
        return (float)(Mathf.Max(Mathf.Abs((float)(a.x - b.x)), Mathf.Abs((float)(a.y - b.y)), Mathf.Abs((float)(a.z - b.z))));
    }

}

[System.Serializable]
public struct HexPoint
{
    public HexPoint(float _q, float _r)
    {
        q = _q;
        r = _r;
    }
    public float q;
    public float r;

    public static bool operator ==(HexPoint lhs, HexPoint rhs)
    {
        return lhs.q == rhs.q && lhs.r == rhs.r;
    }
    public static bool operator !=(HexPoint lhs, HexPoint rhs)
    {
        return lhs.q != rhs.q || lhs.r != rhs.r;
    }
    public static HexPoint operator +(HexPoint lhs, HexPoint rhs)
    {
        float q = lhs.q + rhs.q;
        float r = lhs.r + rhs.r;

        return new HexPoint(q,r);
    }
    public static HexPoint operator -(HexPoint lhs, HexPoint rhs)
    {
        float q = lhs.q - rhs.q;
        float r = lhs.r - rhs.r;

        return new HexPoint(q, r);
    }

    public static HexPoint operator *(HexPoint lhs, int rhs)
    {
        float q = lhs.q * rhs;
        float r = lhs.r * rhs;

        return new HexPoint(q, r);
    }
    public static HexPoint operator *(int lhs, HexPoint rhs)
    {
        float q = lhs * rhs.q;
        float r = lhs * rhs.q;

        return new HexPoint(q, r);
    }
    public static HexPoint operator *(HexPoint lhs, float rhs)
    {
        float q = lhs.q * rhs;
        float r = lhs.r * rhs;

        return new HexPoint(q, r);
    }
    public static HexPoint operator *(float lhs, HexPoint rhs)
    {
        float q = lhs * rhs.q;
        float r = lhs * rhs.q;

        return new HexPoint(q, r);
    }

}

[System.Serializable]
public struct CubePoint
{
    public float x;
    public float y;
    public float z;

    public CubePoint(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }


    public static CubePoint operator +(CubePoint lhs, CubePoint rhs)
    {
        float x = lhs.x + rhs.x;
        float y = lhs.y + rhs.y;
        float z = lhs.z + rhs.z;

        return new CubePoint(x, y, z);
    }
    public static CubePoint operator -(CubePoint lhs, CubePoint rhs)
    {
        float x = lhs.x - rhs.x;
        float y = lhs.y - rhs.y;
        float z = lhs.z - rhs.z;

        return new CubePoint(x, y, z);
    }

    public static bool operator ==(CubePoint lhs, CubePoint rhs)
    {
        return lhs.x == rhs.x && lhs.x == rhs.x && lhs.z == rhs.z;
    }
    public static bool operator !=(CubePoint lhs, CubePoint rhs)
    {
        return lhs.x != rhs.x || lhs.x != rhs.x || lhs.z != rhs.z;
    }

    public static CubePoint operator *(CubePoint lhs, int rhs)
    {
        float x = lhs.x * rhs;
        float y = lhs.y * rhs;
        float z = lhs.z * rhs;

        return new CubePoint(x, y, z);
    }
    public static CubePoint operator *(int lhs, CubePoint rhs)
    {
        float x = lhs * rhs.x;
        float y = lhs * rhs.y;
        float z = lhs * rhs.z;

        return new CubePoint(x, y, z);
    }
    public static CubePoint operator *(CubePoint lhs, float rhs)
    {
        float x = lhs.x * rhs;
        float y = lhs.y * rhs;
        float z = lhs.z * rhs;

        return new CubePoint(x, y, z);
    }
    public static CubePoint operator *(float lhs, CubePoint rhs)
    {
        float x = lhs * rhs.x;
        float y = lhs * rhs.y;
        float z = lhs * rhs.z;

        return new CubePoint(x, y, z);
    }
}
