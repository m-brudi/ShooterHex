using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEditor;
using UnityEngine.EventSystems;

public class Controller : SingletonMonoBehaviour<Controller>
{
    public HexGrid hexGrid;
    public HexGrid.HexOperationType currentOperationType;
    public HexCollection hexCollection;
    public int costOfNewHex;
    public int costOfOperation;
    [SerializeField] CinemachineVirtualCamera playerCam;
    [SerializeField] CinemachineVirtualCamera hexCam;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform startingHexObjects;
    [SerializeField] Transform playerStartPos;
    [SerializeField] int gridSizeX;
    [SerializeField] int gridSizeZ;
    [SerializeField] float gridChanceToSpawnExtraHexes;
    bool canTerraform;
    int coins = 100;
    Hex hexToShow;
    Hex nextHexToPlace;
    bool isInPlayMode;
    [SerializeField] List<MineEntry> mines;
    Plane plane = new Plane(Vector3.up, 0);

    #region public variables
    public List<MineEntry> Mines {
        get { return mines; }
        set { mines = value; }
    }
    public int Coins {
        get { return coins; }
        set {
            coins = value;
            UiManager.Instance.SetupCoinsCounter(coins);     
        }
    }
    public bool IsPlayerDead {
        get { return playerController.dead; }
    }
    public bool CanPlaceNewHex {
        get { return coins >= costOfNewHex; }
    }
    public bool CanManipulateHex {
        get { return coins >= costOfOperation; }
    }
    public bool CanTerraform {
        get { return canTerraform; }
        set { canTerraform = value; }
    }
    public PlayerController Player {
        get { return playerController; }
    }


    public Transform PlayerTransform {
        get { return playerController.transform; }
    }
    public HexGrid.HexOperationType CurrentOperation {
        get { return currentOperationType; }
        set { currentOperationType = value; }
    }
    public bool IsInPlayerMode {
        get { return isInPlayMode; }
    }
    #endregion
    #region events
    public static Action hexMode;
    public static Action playMode;
    #endregion


    void Start()
    {
        UiManager.Instance.SetupCoinsCounter(coins);
        hexGrid.GenerateGrid(gridSizeX, gridSizeZ, gridChanceToSpawnExtraHexes);
        StartGame();
    }

    public void SetupStartingHex(GridCell cell) {
        Vector3 pos = new(cell.transform.position.x, 0, cell.transform.position.z);
        startingHexObjects.transform.position = pos;
    }

    public void StartGame() {
        playerController.transform.position = playerStartPos.position;
        playerController.StartGame();
        SwitchToPlayMode();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Tab) && canTerraform) {
            if (isInPlayMode) SwitchToHexMode();
            else SwitchToPlayMode();
        }
        if (!isInPlayMode) {
            if (currentOperationType == HexGrid.HexOperationType.NewHex) {
                float dist;
                Vector3 mousePos = Vector3.zero;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out dist)) {
                    mousePos = ray.GetPoint(dist);
                }
                if (hexGrid.GetPositionOnGrid(mousePos) != Vector3.zero) {
                    if (hexToShow) {
                        hexToShow.gameObject.SetActive(true);
                        hexToShow.transform.position = hexGrid.GetPositionOnGrid(mousePos);
                    }
                } else {
                    if(hexToShow) hexToShow.gameObject.SetActive(false);
                }
                if (Input.GetMouseButtonDown(0)) {
                    if (CanPlaceNewHex) {
                        hexGrid.CreateHexOnMousePosition(mousePos, nextHexToPlace);
                    }
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentOperationType = HexGrid.HexOperationType.RotateSingle;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentOperationType = HexGrid.HexOperationType.RotateAround;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentOperationType = HexGrid.HexOperationType.SwitchPlace;
        
    }

    public void MovePlayerToOtherMine(MineEntry enter) {
        if(mines.Count > 1) {
            List<MineEntry> m = mines.FindAll(x => x != enter);
            MineEntry mine = m[UnityEngine.Random.Range(0, m.Count)];
            mine.IgnoreTrigger();
            Vector3 pos = mine.spawnPos.position;
            playerController.transform.position = pos;
        }
    }

    public void GenerateNextHex() {
        //nextHexToPlace = null;
        nextHexToPlace = hexCollection.GetHex();
    }

    public void StartShowingNewHex() {
        if (hexToShow) Destroy(hexToShow.gameObject);
        hexToShow = Instantiate(nextHexToPlace, new Vector3(0,-1000,0), Quaternion.identity);
        hexToShow.SetupOnlyForShow();
    }
    public void StopShowingNewHex() {
        //nextHexToPlace = null;
        if(hexToShow) Destroy(hexToShow.gameObject);
    }

    public void SwitchToPlayMode() {
        if (hexToShow) Destroy(hexToShow.gameObject);
        isInPlayMode = true;
        playerCam.Priority = 10;
        hexCam.Priority = 0;
        playMode?.Invoke();
        //Cursor.visible = false;
    }
    public void SwitchToHexMode() {
        GenerateNextHex();
        isInPlayMode = false;
        playerCam.Priority = 0;
        hexCam.Priority = 10;
        hexMode?.Invoke();
        currentOperationType = HexGrid.HexOperationType.Nothing;
        //Cursor.visible = true;
    }

    public void ChangeOperationMode(HexGrid.HexOperationType type) {
        currentOperationType = type;
        if (type == HexGrid.HexOperationType.NewHex) StartShowingNewHex();
        else StopShowingNewHex();
    }
}
