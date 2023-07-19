using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunPanel : MonoBehaviour
{
    [SerializeField] DynamicFilledImage gunFill;
    [SerializeField] GunData currentGun;

    private void Start() {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
        gunFill.SetFill((float)currentGun.currentAmmo / currentGun.magSize);
    }

    void Shoot() {
        if(!currentGun.reloading)
        gunFill.SetFill((float)currentGun.currentAmmo / currentGun.magSize,0);
    }

    void StartReload() {
        gunFill.SetFill(1, currentGun.reloadTime, true);
    }
}
