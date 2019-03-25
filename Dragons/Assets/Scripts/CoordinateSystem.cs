using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateSystem 
{
    public static HexPoint pixel_to_flat_hex(Vector3 position, float width)
    {
        double q = (2.0f / 3 * position.x) / (width / 2);
        double r = (-1.0f / 3 * position.x + Mathf.Sqrt(3) / 3 * position.z) / (width / 2);
        return RoundToNearestHex(new HexPoint(q,r,width));
    }

    public static CubePoint AxialToCubePoint(HexPoint hex)
    {
        double x = hex.q;
        double z = hex.r;
        double y = -x -z;
        return new CubePoint(x,y,z,hex.width);
    }

    public static HexPoint CubeToAxialPoint(CubePoint cube)
    {
        double q = cube.x;
        double r = cube.z;
        return new HexPoint(q,r,cube.width);
    }

    public static CubePoint RoundToNearestCube(CubePoint cube)
    {
        int rx = Mathf.RoundToInt((float)cube.x);
        int ry = Mathf.RoundToInt((float)cube.y);
        int rz = Mathf.RoundToInt((float)cube.z);

        int x_diff = (int)Mathf.Abs((float)(rx - cube.x));
        int y_diff = (int)Mathf.Abs((float)(ry - cube.y));
        int z_diff = (int)Mathf.Abs((float)(rz - cube.z));

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
        return new CubePoint(x,y,z,cube.width);
    }

    public static HexPoint RoundToNearestHex(HexPoint point)
    {
        return CubeToAxialPoint(RoundToNearestCube(AxialToCubePoint(point)));
    }

    public static Vector3 HexPointToPixel(HexPoint hPoint, float hexWidth)
    {
        double x = (hexWidth / 2f) * (3.0f / 2 * hPoint.q);
        double z = (hexWidth / 2f) * (Mathf.Sqrt(3) / 2 * hPoint.q + Mathf.Sqrt(3) * hPoint.r);
        return new Vector3((float)x, 0, (float)z);
    }

    public static List<HexPoint> CreateRings(HexPoint start, int radius)
    {
        List<HexPoint> result = new List<HexPoint>();

        HexPoint newNeighbour = GetNeighbour(start, 4);
        newNeighbour = Scale(newNeighbour, radius);

        for (int i = 0; i < 6; i++)
        {
            for (int d = 0; d < radius; d++)
            {
                newNeighbour = GetNeighbour(newNeighbour, i);
                result.Add(newNeighbour);
            }
        }
        return result;
    }
    
    public static HexPoint Scale(HexPoint point, int scale)
    {
        double q = point.q * scale;
        double r = point.r * scale;

        return new HexPoint(q,r, point.width);
    }

    public static HexPoint GetNeighbour(HexPoint hex, int index)
    {
        switch (index)
        {
            case 0:
                return new HexPoint(hex.q + 1, hex.r + 0, hex.width);
            case 1:
                return new HexPoint(hex.q + 1, hex.r - 1, hex.width);
            case 2:
                return new HexPoint(hex.q + 0, hex.r - 1, hex.width);
            case 3:
                return new HexPoint(hex.q - 1, hex.r + 0, hex.width);
            case 4:
                return new HexPoint(hex.q - 1, hex.r + 1, hex.width);
            case 5:
                return new HexPoint(hex.q + 0, hex.r + 1, hex.width);
            default:
                throw new System.Exception();
        }
    }

    public static HexPoint[] PointsBetweenHexPoints(HexPoint a, HexPoint b)
    {
        CubePoint ah = AxialToCubePoint(a);
        CubePoint bh = AxialToCubePoint(b);

        List<HexPoint> points = new List<HexPoint>();
        
        double N = CubeDistance(ah,bh);

        for (int i = 0; i <= N; i++)
        {
            CubePoint cubePoint = CubeLerp(ah, bh, 1.0d / N * i);
            cubePoint = RoundToNearestCube(cubePoint);
            HexPoint hexPoint = CubeToAxialPoint(cubePoint);
            if (!points.Contains(hexPoint))
            {
                points.Add(hexPoint);
            }
            else
            {
                hexPoint = RoundToNearestHex(hexPoint + new HexPoint(0.5f,0.5f,hexPoint.width));
                points.Add(hexPoint);
            }
        }
        return points.ToArray();
        
    }

    private static CubePoint CubeLerp(CubePoint a, CubePoint b, double step)
    {
        double x = a.x + (b.x - a.x) * step;
        double y = a.y + (b.y - a.y) * step;
        double z = a.z + (b.z - a.z) * step;

        return new CubePoint(x,y,z,a.width);
    }

    private static double HexDistance(HexPoint a, HexPoint b)
    {
        CubePoint ac = AxialToCubePoint(a);
        CubePoint bc = AxialToCubePoint(b);

        return CubeDistance(ac,bc);
    }

    private static double CubeDistance(CubePoint a, CubePoint b)
    {
        return (float)(Mathf.Max(Mathf.Abs((float)(a.x - b.x)), Mathf.Abs((float)(a.y - b.y)), Mathf.Abs((float)(a.z - b.z))));
    }

}

[System.Serializable]
public struct HexPoint
{
    public HexPoint(double _q, double _r, float _width)
    {
        q = _q;
        r = _r;
        width = _width;
    }
    public readonly double q;
    public readonly double r;
    public readonly float width;

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
        double q = lhs.q + rhs.q;
        double r = lhs.r + rhs.r;
        float width = lhs.width;

        return new HexPoint(q,r,width);
    }
    public static HexPoint operator -(HexPoint lhs, HexPoint rhs)
    {
        double q = lhs.q - rhs.q;
        double r = lhs.r - rhs.r;
        float width = lhs.width;

        return new HexPoint(q, r, width);
    }

    public static HexPoint operator *(HexPoint lhs, int rhs)
    {
        double q = lhs.q * rhs;
        double r = lhs.r * rhs;
        float width = lhs.width;

        return new HexPoint(q, r, width);
    }
    public static HexPoint operator *(int lhs, HexPoint rhs)
    {
        double q = lhs * rhs.q;
        double r = lhs * rhs.q;
        float width = rhs.width;

        return new HexPoint(q, r, width);
    }
    public static HexPoint operator *(HexPoint lhs, double rhs)
    {
        double q = lhs.q * rhs;
        double r = lhs.r * rhs;
        float width = lhs.width;

        return new HexPoint(q, r, width);
    }
    public static HexPoint operator *(double lhs, HexPoint rhs)
    {
        double q = lhs * rhs.q;
        double r = lhs * rhs.q;
        float width = rhs.width;

        return new HexPoint(q, r, width);
    }

}

[System.Serializable]
public struct CubePoint
{
    public double x;
    public double y;
    public double z;
    public float width;

    public CubePoint(double _x, double _y, double _z, float _width)
    {
        x = _x;
        y = _y;
        z = _z;
        width = _width;
    }


    public static CubePoint operator +(CubePoint lhs, CubePoint rhs)
    {
        double x = lhs.x + rhs.x;
        double y = lhs.y + rhs.y;
        double z = lhs.z + rhs.z;
        float width = lhs.width;

        return new CubePoint(x, y, z, width);
    }
    public static CubePoint operator -(CubePoint lhs, CubePoint rhs)
    {
        double x = lhs.x - rhs.x;
        double y = lhs.y - rhs.y;
        double z = lhs.z - rhs.z;
        float width = lhs.width;

        return new CubePoint(x, y, z, width);
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
        double x = lhs.x * rhs;
        double y = lhs.y * rhs;
        double z = lhs.z * rhs;
        float width = lhs.width;

        return new CubePoint(x, y, z, width);
    }
    public static CubePoint operator *(int lhs, CubePoint rhs)
    {
        double x = lhs * rhs.x;
        double y = lhs * rhs.y;
        double z = lhs * rhs.z;
        float width = rhs.width;

        return new CubePoint(x, y, z, width);
    }
    public static CubePoint operator *(CubePoint lhs, double rhs)
    {
        double x = lhs.x * rhs;
        double y = lhs.y * rhs;
        double z = lhs.z * rhs;
        float width = lhs.width;

        return new CubePoint(x, y, z, width);
    }
    public static CubePoint operator *(double lhs, CubePoint rhs)
    {
        double x = lhs * rhs.x;
        double y = lhs * rhs.y;
        double z = lhs * rhs.z;
        float width = rhs.width;

        return new CubePoint(x, y, z, width);
    }
}
