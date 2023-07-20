using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour, IDamageable
{
    [Space]
    [Header("Enemy data")]
    [SerializeField] int patrollingSpeed;
    [SerializeField] int chasingSpeed;
    [SerializeField] int health;
    [SerializeField] int damage;
    [SerializeField] int numOfCoinsToDrop;
    [SerializeField] float forceMultiplier = 1;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject shadowSphere;
    CapsuleCollider coll;

    Rigidbody rb;
    [Space]
    [SerializeField] Transform sprite;
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] LayerMask playerMask,groundMask;
    Transform player;

    [Space]
    [Header("Patrolling")]
    [SerializeField] float walkPointRange;
    Vector3 walkPoint;
    bool walkPointSet;

    [Space]
    [Header("Attacking")]
    [SerializeField] float timeBetweenAttacks;
    bool alreadyAttacked;

    [Space]
    [Header("States")]
    [SerializeField] float sightRange; 
    [SerializeField] float  attackRange;
    bool playerInSightRange, playerInAttackRange;

    float myScale;
    bool dead;
    public int Health {
        get {
            return health;
        }
        set {
            health = value;
            if (health <= 0) {
                dead = true;
                SpawnCoins();
                anim.SetFloat("Move", 0);
                anim.SetTrigger("Death");
                StartCoroutine(Death());
            } else {
                anim.SetTrigger("TakeHit");
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        dead = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = Controller.Instance.PlayerTransform;
        myScale = sprite.transform.localScale.x;

    }

    IEnumerator Death() {
        shadowSphere.SetActive(false);
        coll.enabled = false;
        rb.isKinematic = true;
        sprite.GetComponent<SpriteRenderer>().DOFade(0,2);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    void SpawnCoins() {
        for (int i = 0; i < numOfCoinsToDrop; i++) {
            Vector3 force = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));
            GameObject c = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            c.GetComponent<Rigidbody>().AddForce(force * forceMultiplier,ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (!dead) {
            if (Controller.Instance.IsInPlayerMode) {
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

                Vector3 normalizedMovement = navMeshAgent.desiredVelocity.normalized;
                anim.SetFloat("Move", normalizedMovement.magnitude);

                if (navMeshAgent.destination.x > transform.position.x) {
                    sprite.localScale = myScale * Vector3.one;
                } else {
                    sprite.localScale = myScale * new Vector3(-1, 1, 1);
                }

                if (!playerInAttackRange && !playerInSightRange) Patroling();
                if (playerInSightRange && !playerInAttackRange) ChasePlayer();
                if (playerInSightRange && playerInAttackRange) AttackPlayer();
            }
        } else {
            navMeshAgent.SetDestination(transform.position);
        }
        
    }

    private void Patroling() {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) {
            navMeshAgent.SetDestination(walkPoint);
            navMeshAgent.speed = patrollingSpeed;
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, walkPointRange, groundMask))
            walkPointSet = true;
    }

    private void ChasePlayer() {
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = chasingSpeed;
    }

    private void AttackPlayer() {
        navMeshAgent.SetDestination(transform.position);
            if (player.position.x > transform.position.x) {
                sprite.localScale = myScale * Vector3.one;
            } else {
                sprite.localScale = myScale * new Vector3(-1, 1, 1);
            }
        if (!alreadyAttacked) {
            //attack
            anim.SetTrigger("Attack");
            Controller.Instance.Player.TakeDamage(damage);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, sightRange);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, walkPointRange);
    //}

    public void Damage(float damage, Vector3 pos) {
        if (!dead) {
            Health -= (int)damage;
            Vector3 dir = transform.position - pos;
            dir.Normalize();
            rb.AddForce(dir * 4, ForceMode.Impulse);
        }
    }
}
