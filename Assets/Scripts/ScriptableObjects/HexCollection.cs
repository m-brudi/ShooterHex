using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexCollection", menuName = "Hexes/HexCollection")]
public class HexCollection : ScriptableObject
{
    [SerializeField] List<Hex> hexes;
    [SerializeField] Hex startingHex;
    int hexIndex = 0;
    public Hex GetHex() {
        hexIndex++;
        if (hexIndex == hexes.Count) hexIndex = 0;
        return hexes[hexIndex];
    }

    public Hex GetStartingHex() {
        return startingHex;
    }
}
