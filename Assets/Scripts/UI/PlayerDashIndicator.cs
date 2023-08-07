using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashIndicator : MonoBehaviour
{
    [SerializeField] DynamicFilledImage fill;

    public void Setup(float time) {
        fill.FillAmount = 0;
        fill.image.enabled = true;
        fill.SetFill(1, time,()=> { fill.image.enabled = false; });
    }
}
