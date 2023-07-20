using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] Bullet bullet;
    [SerializeField] private KeyCode reloadKey;



    private void Update() {

        if (Input.GetMouseButtonDown(0) && Controller.Instance.IsInPlayerMode) {
            Plane plane = new Plane(Vector3.up, 0);
            shootInput?.Invoke();
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance)) {
                ShootBullet(ray.GetPoint(distance));
            }
        }
        if (Input.GetKeyDown(reloadKey)) {
            reloadInput?.Invoke();
        }
    }

    void ShootBullet(Vector3 pos) {
        Vector3 target = new(pos.x, 0, pos.z);
        Bullet bull = Instantiate(bullet, transform.position, Quaternion.identity);
        bull.transform.localScale = Vector3.zero;
        bull.Setup(target);
    }
}
