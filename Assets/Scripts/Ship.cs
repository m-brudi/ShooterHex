using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Ship : MonoBehaviour
{
    [SerializeField] Transform terraformSphere;
    [SerializeField] Transform partOfShip;
    void Start()
    {
        partOfShip.DOLocalMoveY(.05f, 0);
        terraformSphere.DOScale(1, 0);
        Controller.hexMode += HexModeActive;
        Controller.playMode += PlayModeActive;
        terraformSphere.DOScale(1, 0);
        partOfShip.DOLocalMoveY(.05f, 0);
    }

    private void PlayModeActive() {
        terraformSphere.DOScale(4, .5f).SetEase(Ease.InOutBack).SetDelay(.5f);
        partOfShip.DOLocalMoveY(.5f, 0.5f).SetEase(Ease.InOutBack).SetDelay(1);
    }

    private void HexModeActive() {
        terraformSphere.DOScale(20, .5f).SetEase(Ease.InOutBack);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            Controller.Instance.CanTerraform = true;
            terraformSphere.DOScale(4, .5f).SetEase(Ease.InOutBack);
            partOfShip.DOLocalMoveY(.5f, 0.5f).SetEase(Ease.InOutBack);
            UiManager.Instance.ShowOrHideMapInfo(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            Controller.Instance.CanTerraform = false;
            terraformSphere.DOScale(1, .5f).SetEase(Ease.InBack);
            partOfShip.DOLocalMoveY(.05f, .5f).SetEase(Ease.InBack);
            UiManager.Instance.ShowOrHideMapInfo(false);
        }
    }
    
}
