using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData",menuName ="Create New TileData/Default")]
public class TileData : ScriptableObject
{
    [SerializeField] private Mesh _tileDefaultMesh;
    public float meshSizeX { get { return _tileDefaultMesh.bounds.size.x; } }
    public float meshSizeZ { get { return _tileDefaultMesh.bounds.size.z; } }
}
