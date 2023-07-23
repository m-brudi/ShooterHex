using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : SingletonMonoBehaviour <UiManager>{

    [SerializeField] HexModePanel hexModePanel;
    [SerializeField] TextMeshProUGUI coinsCounter;
    [SerializeField] GameObject hpPanel;
    [SerializeField] DynamicFilledImage hpFill;

    void Start()
    {
        Controller.hexMode += ShowHexModeUI;
        Controller.playMode += ShowPlayModeUI;
    }

    public void SetupCoinsCounter(int value) {
        coinsCounter.text = value.ToString();
    }

    public void SetupHpFill(int hp, int maxHp) {
        float e = (float)hp / maxHp;
        hpFill.SetFill((float)hp / maxHp);
    }
    void ShowHexModeUI() {
        hexModePanel.gameObject.SetActive(true);
        hexModePanel.Setup();
        hpPanel.SetActive(false);
    }

    void ShowPlayModeUI() {
        hexModePanel.gameObject.SetActive(false);
        hpPanel.SetActive(true);

    }
}
