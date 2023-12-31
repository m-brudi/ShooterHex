using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    
    public enum HexOperationType {
        Nothing,
        NewHex,
        RotateSingle,
        SwitchPlace,
        RotateAround
    }

    [SerializeField] GameObject gridCellPrefab;
    [SerializeField] float xOffset;
    [SerializeField] float zOffset;

    [Space]
    [SerializeField] List<GridCell> cells = new List<GridCell>();
    [SerializeField] List<GridCell> closests = new List<GridCell>();
    [SerializeField] List<GridCell> takenCells = new List<GridCell>();

    GridCell startingCell;
    GridCell endingCell;

    public Hex firstToSwitch;   

    public void GenerateGrid(int sizeX, int sizeZ, float chance) {
        for (int i = 0; i < sizeZ; i++) {
            for (int k = 0; k < sizeX; k++) {
                float xCoordinate = (k + (i * .5f) - (i / 2))*(xOffset);
                float zCoordinate = i * (zOffset);
                Vector3 coordinate = new(xCoordinate, -6f, zCoordinate);
                CreateCell(coordinate, k, i,false);
            }
        }

        ChooseStartHex(sizeX, sizeZ);
        GenerateExtraHexes(sizeX,sizeZ,chance);

        foreach (var cell in cells.ToArray()) {
            cell.DistanceFromStart = GetDistanceBetweenCells(cell, startingCell);
        }
        PlaceEndingHex();

        foreach (var cell in cells.ToArray()) {
            AddNeighbours(cell, true);
        }
        //second run to sort existing neighbours
        StartCoroutine(DelaySortingNeighbours());
    }

    void ChooseStartHex(int sizeX, int sizeZ) {
        //int randX = Random.Range(Mathf.FloorToInt(sizeX / 4), sizeX - Mathf.FloorToInt(sizeX / 4));
        //int randZ = Random.Range(Mathf.FloorToInt(sizeZ / 4), sizeZ - Mathf.FloorToInt(sizeZ / 4));
        int randX = Random.Range(0, sizeX);
        int randZ = Random.Range(0, sizeZ);
        GridCell cell = cells.Find(x => x.XCoordinate == randX && x.ZCoordinate == randZ);
        startingCell = cell;
        cell.startingCell = true;
        cell.CreateHex(Controller.Instance.hexCollection.GetStartingHex());
        Controller.Instance.SetupStartingHex(cell);
    }

    public int GetDistanceBetweenCells(GridCell cell, GridCell cell2) {
        Vector3 cellCube = ConvertToCube(cell.XCoordinate, cell.ZCoordinate);
        Vector3 cell2Cube = ConvertToCube(cell2.XCoordinate, cell2.ZCoordinate);
        Vector3 sub = new (cellCube.x - cell2Cube.x, cellCube.y - cell2Cube.y, cellCube.z - cell2Cube.z);
        int dist = (int)(Mathf.Abs(sub.x) + Mathf.Abs(sub.y) + Mathf.Abs(sub.z)) / 2;
        return dist;
    }

    Vector3 ConvertToCube(int x, int z) {
        int q = x - (z - (z&1)) / 2;
        int r = z;
        int s = -q - r;
        Vector3 cube = new(q, r, s);
        return cube;
    }

    void GenerateExtraHexes(int sizeX, int sizeZ, float chance) {
        foreach (var item in cells) {
            if (!item.Taken) {
                if (item.XCoordinate == sizeX && item.ZCoordinate == sizeZ) {
                    item.CreateHex(Controller.Instance.hexCollection.GetHex());
                } else {
                    if (Random.value < chance) item.CreateHex(Controller.Instance.hexCollection.GetHex());
                }
            }
        }
    }

    IEnumerator DelaySortingNeighbours() {
        yield return new WaitForSeconds(.5f);
        foreach (var cell in cells.ToArray()) {
            AddNeighbours(cell, false);
        }
    }

    void PlaceEndingHex() {
        GridCell furthest = cells.OrderByDescending(x => x.DistanceFromStart).FirstOrDefault();
        endingCell = furthest;
        furthest.CreateHex(Controller.Instance.hexCollection.GetEndingHex());
    }

    GridCell CreateCell(Vector3 pos,int xCoordinate, int zCoordinate, bool createHex) {
        GridCell cell = Instantiate(gridCellPrefab, pos, Quaternion.identity, transform).GetComponent<GridCell>();
        cell.XCoordinate = xCoordinate;
        cell.ZCoordinate = zCoordinate;
        Hex hexToSpawn = Controller.Instance.hexCollection.GetHex();
        if (createHex) cell.CreateHex(hexToSpawn);
        cells.Add(cell);
        return cell;
    }

    public void AddNeighbours(GridCell cell, bool createEmpty) {
        cell.neighbours.Clear();
        int[,] neighborOffsetsOddRow = {
            { 1, 0 },   // East
            { 1, -1 },  // Southeast
            { 0, -1 },  // Southwest
            { -1, 0 }, // West
            { 0, 1 },  // Northwest
            { 1, 1 }    // Northeast
         };
        int[,] neighborOffsetsEvenRow = {
            { 1, 0 },   // East
            { 0, -1 },  // Southeast
            { -1, -1 }, // Southwest
            { -1, 0 },  // West
            { -1, 1 },  // Northwest
            { 0, 1 }    // Northeast
        };
        int cellX = cell.XCoordinate;
        int cellZ = cell.ZCoordinate;
        bool isEvenRow = cellZ % 2 == 0;
        int[,] neighborOffsets = isEvenRow ? neighborOffsetsEvenRow : neighborOffsetsOddRow;
        for (int i = 0; i < neighborOffsets.GetLength(0); i++) {
            int offsetX = neighborOffsets[i, 0];
            int offsetZ = neighborOffsets[i, 1];

            int neighborX = cellX + offsetX;
            int neighborZ = cellZ + offsetZ;
            GridCell neighborCell = cells.Find(c => c.XCoordinate == neighborX && c.ZCoordinate == neighborZ);
            if (neighborCell != null) {
                cell.neighbours.Add(neighborCell);
            } else {
                if (createEmpty) cell.neighbours.Add(CreateMissingNeighbour(offsetX, offsetZ, cell));
                else cell.neighbours.Add(null);
            }
        }
        ClearEmptyCells();
        GetTheListOfTakenCells();
    }

    GridCell CreateMissingNeighbour(int x, int z, GridCell cell) {
        GridCell c = CreateCell(GetPositionOnGrid(x, z, cell), cell.XCoordinate+x, cell.ZCoordinate+z, false);
        AddNeighbours(c,false);
        return c;
    }
    public void RotateAround(GridCell cell) {
        List<GridCell> cells = cell.neighbours;
        GridCell first = cells[0];
        for (int i = 0; i < cells.Count-1; i++) {
            cells[i].MyHex?.ChangePosition(cells[i + 1], i %2 == 0);
        }
        cells[cells.Count - 1].MyHex?.ChangePosition(first, (cells.Count - 1)%2 == 0);
        StartCoroutine(DelaySortingNeighbours());
    }

    void GetTheListOfTakenCells() {
        takenCells.Clear();
        foreach (var item in cells) {
            if (item.Taken) takenCells.Add(item);
        }
    }

    void CheckNeighboursInTakenCells() {
        foreach (var item in takenCells) {
            AddNeighbours(item, true);
        }
    }

    public void ClearEmptyCells() {
        //if all neighbours of a cell are empty - delete that cell
        foreach (var item in cells.ToArray()) {
            if (!item.Taken) {
                bool notTaken = true;
                for (int i = 0; i < item.neighbours.Count; i++) {
                    if (item.neighbours[i] != null) {
                        if (item.neighbours[i].Taken) notTaken = false;
                    }
                }
                if (notTaken) {
                    cells.Remove(item);
                    Destroy(item.gameObject);
                }
            }
        }
    }

    public void SwitchPlace(GridCell cell) {
        if (firstToSwitch == null) {
            firstToSwitch = cell.MyHex;
            cell.MyHex.RiseToMove();
        } else {
            GridCell target = firstToSwitch.MyCell;
            cell.MyHex.RiseToMove();
            firstToSwitch.ChangePosition(cell,true);
            cell.MyHex.ChangePosition(target,false);
            firstToSwitch = null;
        }
    }

    Vector3 GetPositionOnGrid(int x,int z,GridCell cell) {
        float cellX = cell.transform.position.x;
        float cellZ = cell.transform.position.z;
        float xOff = 0;
        float zOff = 0;
        bool isEvenRow = cell.ZCoordinate % 2 == 0;
        if (isEvenRow) {
            if(x == 1 && z == 0) {
                xOff = xOffset;
                zOff = 0;
            } else if(x == 0 && z == -1) {
                xOff = xOffset / 2;
                zOff = -zOffset;
            }else if(x == -1 && z == -1){
                xOff = -xOffset / 2;
                zOff = -zOffset;
            }else if(x == -1 && z == 0) {
                xOff = -xOffset;
                zOff = 0;
            }else if(x == -1 && z == 1) {
                xOff = -xOffset / 2;
                zOff = zOffset;
            }else if(x == 0 && z == 1) {
                xOff = xOffset / 2;
                zOff = zOffset;
            }
        } else {
            if (x == 1 && z == 0) {
                xOff = xOffset;
            } else if (x == 1 && z == -1) {
                xOff = xOffset / 2;
                zOff = -zOffset;
            } else if (x == 0 && z == -1) {
                xOff = -xOffset / 2;
                zOff = -zOffset;
            } else if (x == -1 && z == 0) {
                xOff = -xOffset;
                zOff = 0;
            } else if (x == 0 && z == 1) {
                xOff = -xOffset / 2;
                zOff = zOffset;
            } else if (x == 1 && z == 1) {
                xOff = xOffset / 2;
                zOff = zOffset;
            }
        }
        return new Vector3(cellX + xOff, -6f, cellZ + zOff);
    }

    public void CreateHexOnMousePosition(Vector3 clickPos, Hex hexToSpawn) {
        closests = cells.FindAll(x=> Vector3.Distance(x.transform.position, clickPos) < xOffset/2);
        for (int i = 0; i < closests.Count; i++) {
            if (!closests[i].Taken) {
                closests[i].CreateHex(hexToSpawn);
                Controller.Instance.Coins -= Controller.Instance.CostOfNewHex;
                Controller.Instance.GenerateNextHex();
                Controller.Instance.StartShowingNewHex();
                AddNeighbours(closests[i],true);
                break;
            }
        }
    }

    public Vector3 GetPositionOnGrid(Vector3 mousePos) {
        closests = cells.FindAll(x => Vector3.Distance(x.transform.position, mousePos) < xOffset / 2);
        for (int i = 0; i < closests.Count; i++) {
            if (!closests[i].Taken) {
                Vector3 pos = new (closests[i].transform.position.x, 15, closests[i].transform.position.y);
                return closests[i].transform.position;
            }
        }
        return Vector3.zero;
    }

    public void ShowNextHexOnNewPos(Vector3 mousePos, Hex hexToDisplay) {
        closests = cells.FindAll(x => Vector3.Distance(x.transform.position, mousePos) < xOffset / 2);
        for (int i = 0; i < closests.Count; i++) {
            if (!closests[i].Taken) {
                Instantiate(hexToDisplay, closests[i].transform.position, Quaternion.identity);
                break;
            }
        }
    }
    void Start(){
        zOffset = 0.866025404f* xOffset;
    }
}
