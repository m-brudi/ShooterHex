using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexModePanel : MonoBehaviour
{
    [SerializeField] Button newHexBtn;
    [SerializeField] Button rotateSingleBtn;
    [SerializeField] Button rotateAroundBtn;
    [SerializeField] Button switchPlaceBtn;

    public void Setup() {
        newHexBtn.onClick.RemoveAllListeners();
        rotateAroundBtn.onClick.RemoveAllListeners();
        rotateSingleBtn.onClick.RemoveAllListeners();
        switchPlaceBtn.onClick.RemoveAllListeners();


        newHexBtn.onClick.AddListener(() => Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.NewHex));
        rotateAroundBtn.onClick.AddListener(() => Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.RotateAround));
        rotateSingleBtn.onClick.AddListener(() => Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.RotateSingle));
        switchPlaceBtn.onClick.AddListener(() => Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.SwitchPlace));


    }
    private void Update() {
        newHexBtn.interactable = Controller.Instance.CanPlaceNewHex;
        rotateAroundBtn.interactable = Controller.Instance.CanManipulateHex;
        rotateSingleBtn.interactable = Controller.Instance.CanManipulateHex;
        switchPlaceBtn.interactable = Controller.Instance.CanManipulateHex;
    }
}
