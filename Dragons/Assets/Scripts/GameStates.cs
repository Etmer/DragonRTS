using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexStates
{
    Idle,
    Highlighted,
    Selected,
    Deselected
}

public enum TileType
{
    Default,
    Island,
    Water
}

public enum GameStates
{
    Running,
}

public enum InputState
{
    Selected,
}

public enum WorldLayer
{
    zero,
    one, 
    two,
    three
}