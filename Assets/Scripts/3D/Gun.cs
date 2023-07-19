using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitMark;
    float timeSinceLastShot;
    
    private void Start() {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    public void StartReload() {
        if (!gunData.reloading) {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    public void Shoot() {
        if(gunData.currentAmmo > 0) {
            if (CanShoot()) {
                if (Physics.Raycast(muzzle.position, transform.right,out RaycastHit hitInfo, gunData.maxDistance)) {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    
                    Instantiate(hitMark, hitInfo.point,Quaternion.identity);
                    damageable?.Damage(gunData.damage, Vector3.zero);
                }
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private void Update() {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position, transform.right,Color.red);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hitData;
        //if (Physics.Raycast(ray, out hitData, Mathf.Infinity)) {
        //    Vector3 target = new Vector3(hitData.point.x, transform.position.y, hitData.point.z);
        //    transform.LookAt(target);
        //}
    }
    private void OnGunShot() {
             muzzleFlash.Play();
    }
}
