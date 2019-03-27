using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HexColors", menuName ="Create HexTile ScriptableObject/New HexColors")]
public class HexColorCoding : ScriptableObject
{
    public Color highLightColor = Color.green;
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.red;
}
