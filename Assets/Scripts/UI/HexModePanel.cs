using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HexModePanel : MonoBehaviour
{
    [SerializeField] Button newHexBtn;
    [SerializeField] Button rotateSingleBtn;
    [SerializeField] Button rotateAroundBtn;
    [SerializeField] Button switchPlaceBtn;

    [SerializeField] TextMeshProUGUI newHexPrice;
    [SerializeField] TextMeshProUGUI singleRotatePrice;
    [SerializeField] TextMeshProUGUI rotateAroundPrice;
    [SerializeField] TextMeshProUGUI switchPlacePrice;

    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;
    public void Setup() {
        newHexBtn.onClick.RemoveAllListeners();
        rotateAroundBtn.onClick.RemoveAllListeners();
        rotateSingleBtn.onClick.RemoveAllListeners();
        switchPlaceBtn.onClick.RemoveAllListeners();

        newHexBtn.onClick.AddListener(() => {
            Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.NewHex);

        });
        rotateAroundBtn.onClick.AddListener(() => {
            Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.RotateAround);

        });
        rotateSingleBtn.onClick.AddListener(() => {
            Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.RotateSingle);

        });
        switchPlaceBtn.onClick.AddListener(() => {
            Controller.Instance.ChangeOperationMode(HexGrid.HexOperationType.SwitchPlace);
           
        });
    }

    private void Update() {
        CheckBtns();
    }

    void CheckBtns() {
        newHexBtn.interactable = Controller.Instance.CanPlaceNewHex;
        rotateAroundBtn.interactable = Controller.Instance.CanRotateAround;
        rotateSingleBtn.interactable = Controller.Instance.CanSingleRotate;
        switchPlaceBtn.interactable = Controller.Instance.CanSwitchPlace;

        newHexPrice.color = newHexBtn.interactable ? activeColor : inactiveColor;
        singleRotatePrice.color = rotateAroundBtn.interactable ? activeColor : inactiveColor;
        rotateAroundPrice.color = rotateSingleBtn.interactable ? activeColor : inactiveColor;
        switchPlacePrice.color = switchPlaceBtn.interactable ? activeColor : inactiveColor;

        newHexPrice.text = Controller.Instance.CostOfNewHex.ToString();
        rotateAroundPrice.text = Controller.Instance.CostOfRotateAround.ToString();
        singleRotatePrice.text = Controller.Instance.CostOfSingleRotate.ToString();
        switchPlacePrice.text = Controller.Instance.CostOfSwitchPlace.ToString();
    }
}
