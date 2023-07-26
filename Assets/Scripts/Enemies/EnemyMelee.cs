using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyMelee : Enemy{

    [Header("Attacking")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] int damage;
    bool alreadyAttacked;

    void Start() {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        dead = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = Controller.Instance.PlayerTransform;
        myScale = sprite.transform.localScale.x;
    }

    public override void AttackPlayer() {
        navMeshAgent.SetDestination(transform.position);
        if (player.position.x > transform.position.x) {
            sprite.localScale = myScale * Vector3.one;
        } else {
            sprite.localScale = myScale * new Vector3(-1, 1, 1);
        }
        if (!alreadyAttacked) {
            //attack
            anim.SetTrigger("Attack");
            StartCoroutine(DelayAttack());
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    IEnumerator DelayAttack() {
        yield return new WaitForSeconds(.5f);
        if (playerInSightRange && playerInAttackRange)
            Controller.Instance.Player.Damage(damage, transform.position);
    }
    private void ResetAttack() {
        alreadyAttacked = false;
    }
}
