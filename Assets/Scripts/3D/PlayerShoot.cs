using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] GunData gunData;
    [SerializeField] Bullet bullet;
    [SerializeField] private KeyCode reloadKey;
    float timeSinceLastShot;
    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Start() {
        gunData.reloading = false;
    }

    public void StartReload() {
        if (!gunData.reloading) {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        yield return new WaitForSeconds(.2f);
        reloadInput?.Invoke();
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private void Update() {
        timeSinceLastShot += Time.deltaTime;
        if (Input.GetMouseButton(0) && Controller.Instance.IsInPlayerMode) {
            if (gunData.currentAmmo > 0) {
                if (CanShoot()) {
                    Plane plane = new Plane(Vector3.up, 0);
                    float distance;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (plane.Raycast(ray, out distance)) {
                        ShootBullet(ray.GetPoint(distance));
                    }

                }
            }
        }
        if (Input.GetKeyDown(reloadKey)) {
            reloadInput?.Invoke();
        }
    }

    void ShootBullet(Vector3 pos) {
        gunData.currentAmmo--;
        if (gunData.currentAmmo == 0) StartReload();
        timeSinceLastShot = 0;
        shootInput?.Invoke();
        Vector3 target = new(pos.x, 0, pos.z);
        Bullet bull = Instantiate(bullet, transform.position, Quaternion.identity);
        bull.transform.localScale = Vector3.zero;
        bull.Setup(target,transform.position, gunData.damage, true);
    }
}
