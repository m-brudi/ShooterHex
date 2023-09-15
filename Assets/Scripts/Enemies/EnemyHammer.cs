using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHammer : Enemy
{
    [Header("Shooting")]
    [SerializeField] float attackRate = 10;
    [SerializeField] float timeSinceLastAttack;
    [SerializeField] int attackDmg = 5;
    //[SerializeField] float 
    [Header("Effects")]
    [SerializeField] ParticleSystem attackEffect;
    [SerializeField] ParticleSystem walkEffect;

    public bool CanAttack() => timeSinceLastAttack > 1f / (attackRate / 60f);

    void Start() {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        dead = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = Controller.Instance.PlayerTransform;
        myScale = sprite.transform.localScale.x;
    }
    public override void Patroling() {
        //pick a walkPoint
        //jump to its location
        //repeat


        //if (!walkPointSet) SearchWalkPoint();
        //if (navMeshAgent.speed < 0.1f) SearchWalkPoint();
        //if (walkPointSet) {
        //    navMeshAgent.SetDestination(walkPoint);
        //    navMeshAgent.speed = patrollingSpeed;
        //}
        //Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    public override void AttackPlayer() {
        Attack();
    }

    public override void ChasePlayer() {
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = base.chasingSpeed;
        Attack();
    }

    private void Attack() {
        timeSinceLastAttack += Time.deltaTime;
        if (CanAttack()) {
            //jump to player position
            //show particles and screen shake and such
            //add force to all the objects in attack radius

            timeSinceLastAttack = 0;
        }
    }
}
