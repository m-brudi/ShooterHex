using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEditor;

public class Controller : SingletonMonoBehaviour<Controller>
{
    public HexGrid hexGrid;
    public HexGrid.HexOperationType currentOperationType;
    public HexCollection hexCollection;
    [SerializeField] CinemachineVirtualCamera playerCam;
    [SerializeField] CinemachineVirtualCamera hexCam;
    [SerializeField] PlayerController playerController;

    public PlayerController Player {
        get { return playerController; }
    }

    #region events
    public static Action hexMode;
    public static Action playMode;
    #endregion

    public Transform PlayerTransform {
        get { return playerController.transform; }
    }
    public HexGrid.HexOperationType CurrentOperation {
        get { return currentOperationType; }
        set { currentOperationType = value; }
    }
    bool isInPlayMode;
    Plane plane = new Plane(Vector3.up, 0);

    public bool IsInPlayerMode {
        get { return isInPlayMode; }
    }
    void Start()
    {
        SwitchToPlayMode();
#if UNITY_EDITOR
        Cursor.SetCursor(PlayerSettings.defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
#endif
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (isInPlayMode) SwitchToHexMode();
            else SwitchToPlayMode();
        }
        if (!isInPlayMode) {
            if (Input.GetMouseButtonDown(0)) {
                float distance;
                Vector3 clickPos = Vector3.zero;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(plane.Raycast(ray,out distance)) {
                    clickPos = ray.GetPoint(distance);
                }
                //clickPos = new Vector3(clickPos.x, 0, clickPos.z);
                hexGrid.CreateHexOnMousePosition(clickPos);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) currentOperationType = HexGrid.HexOperationType.RotateSingle;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentOperationType = HexGrid.HexOperationType.RotateAround;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentOperationType = HexGrid.HexOperationType.SwitchPlace;
        
    }



    void SwitchToPlayMode() {
        isInPlayMode = true;
        playerCam.Priority = 10;
        hexCam.Priority = 0;
        playMode?.Invoke();
        //Cursor.visible = false;
    }
    void SwitchToHexMode() {
        isInPlayMode = false;
        playerCam.Priority = 0;
        hexCam.Priority = 10;
        hexMode?.Invoke();
        //Cursor.visible = true;
    }
}
