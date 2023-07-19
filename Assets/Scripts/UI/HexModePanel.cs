using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexModePanel : MonoBehaviour
{
    [SerializeField] Button rotateSingleBtn;
    [SerializeField] Button rotateAroundBtn;
    [SerializeField] Button switchPlaceBtn;

    public void Setup() {
        rotateAroundBtn.onClick.RemoveAllListeners();
        rotateSingleBtn.onClick.RemoveAllListeners();
        switchPlaceBtn.onClick.RemoveAllListeners();

        rotateAroundBtn.onClick.AddListener(() => Controller.Instance.currentOperationType = HexGrid.HexOperationType.RotateAround);
        rotateSingleBtn.onClick.AddListener(() => Controller.Instance.currentOperationType = HexGrid.HexOperationType.RotateSingle);
        switchPlaceBtn.onClick.AddListener(() => Controller.Instance.currentOperationType = HexGrid.HexOperationType.SwitchPlace);
    }
}
