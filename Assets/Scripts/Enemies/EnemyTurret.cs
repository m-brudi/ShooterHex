using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyTurret : Enemy
{
    [SerializeField] Transform shootingPointsParent;
    [SerializeField] Transform[] shootingPoints;
    [SerializeField] float fireRate = 200;
    [SerializeField] float timeSinceLastShot;
    [SerializeField] int bulletDmg = 2;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float rotateSpeed = 50;
    [SerializeField] int numOfShotsBeforeRotate = 4;
    [SerializeField] int rotationDegrees = 45;
    bool rotating;
    int shots;
    public bool CanShoot() => timeSinceLastShot > 1f / (fireRate / 60f);
    void Start()
    {
        shots = 0;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        dead = false;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = Controller.Instance.PlayerTransform;
        myScale = sprite.transform.localScale.x;
    }

    private void Shoot() {
        shootingPointsParent.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        timeSinceLastShot += Time.deltaTime;
        if (CanShoot() && !rotating) {
            foreach (var item in shootingPoints) {
                Bullet bull = Instantiate(bulletPrefab, item.position, Quaternion.identity).GetComponent<Bullet>();
                bull.transform.localScale = Vector3.zero;
                bull.Setup(item.position, shootingPointsParent.position, bulletDmg, false, bulletSpeed);
            }
            timeSinceLastShot = 0;
            shots++;
            //if (shots == numOfShotsBeforeRotate) Rotate();
        }
    }

    public void Rotate() {
        rotating = true;
        Quaternion rot = shootingPointsParent.rotation * Quaternion.Euler(0, rotationDegrees, 0);
        shootingPointsParent.DORotateQuaternion(rot, 0.5f).OnComplete(() => {
            shots = 0;
            rotating = false;
        });
    }

    public override void AttackPlayer() {
        Shoot();
    }

    }
