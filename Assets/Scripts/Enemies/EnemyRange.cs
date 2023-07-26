using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRange : Enemy {
    [Header("Shooting")]
    [SerializeField] float shootingRange; //lets use sight range for now
    [SerializeField] float fireRate = 200;
    [SerializeField] float timeSinceLastShot;
    [SerializeField] int bulletDmg = 2;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bulletPrefab;
    public bool CanShoot() => timeSinceLastShot > 1f / (fireRate / 60f);

    void Start() {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        dead = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = Controller.Instance.PlayerTransform;
        myScale = sprite.transform.localScale.x;
    }

    public override void AttackPlayer() {
        Shoot();
    }

    private void Shoot() {
        //StartCoroutine(GoBackToChasing());
        //navMeshAgent.SetDestination(transform.position);
       timeSinceLastShot += Time.deltaTime;
        if (CanShoot()) {
            Bullet bull = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bull.transform.localScale = Vector3.zero;
            bull.Speed = bulletSpeed;
            bull.Setup(Controller.Instance.PlayerTransform.position, transform.position, bulletDmg, false);
            timeSinceLastShot = 0;
        }
    }

    IEnumerator GoBackToChasing() {
        yield return new WaitForSeconds(.5f);
        navMeshAgent.SetDestination(player.position);
    }
}
