using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public bool startingCell;
    [SerializeField] bool taken;
    [SerializeField] int xCoordinate;
    [SerializeField] int zCoordinate;
    [SerializeField] Hex hexPrefab;
    [SerializeField] Hex myHex;
    [SerializeField] int distFromStart;
    public List<GridCell> neighbours = new List<GridCell>(); // [0] east then clockwise
    public Hex MyHex {
        get { return myHex; }
        set { 
            myHex = value;
            if (value != null) taken = true;
        }
    }

    public int DistanceFromStart {
        get { return distFromStart; }
        set { distFromStart = value; }
    }
    public bool Taken {
        get { return taken; }
        set { taken = value; }
    }
    public int XCoordinate {
        get { return xCoordinate; }
        set { xCoordinate = value; }
    }
    public int ZCoordinate {
        get { return zCoordinate; }
        set { zCoordinate = value; }
    }

    public void CreateHex(Hex hexToSpawn) {
        Hex hex;
        if (myHex) {
            Hex h = myHex;
            Destroy(h.gameObject);
            myHex = null;
        }
        hex = Instantiate(hexToSpawn, transform.position, Quaternion.identity);
        hex.Setup(this);
        myHex = hex;
        taken = true;
        if (startingCell) hex.MakeMeStartHex();
    }
}
