using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : SingletonMonoBehaviour <UiManager>{

    [SerializeField] HexModePanel hexModePanel;

    void Start()
    {
        Controller.hexMode += ShowHexModeUI;
        Controller.playMode += ShowPlayModeUI;
    }

    void ShowHexModeUI() {
        hexModePanel.gameObject.SetActive(true);
        hexModePanel.Setup();
    }

    void ShowPlayModeUI() {
        hexModePanel.gameObject.SetActive(false);

    }
}
