using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : SingletonMonoBehaviour <UiManager>{

    [SerializeField] HexModePanel hexModePanel;
    [SerializeField] TextMeshProUGUI coinsCounter;

    void Start()
    {
        Controller.hexMode += ShowHexModeUI;
        Controller.playMode += ShowPlayModeUI;
    }

    public void SetupCoinsCounter(int value) {
        coinsCounter.text = value.ToString();
    }

    void ShowHexModeUI() {
        hexModePanel.gameObject.SetActive(true);
        hexModePanel.Setup();
    }

    void ShowPlayModeUI() {
        hexModePanel.gameObject.SetActive(false);

    }
}
