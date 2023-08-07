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
    [SerializeField] int numOfCoinsToDrop;
    [SerializeField] float coinSpreadForceMultiplier;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject shadowSphere;
    

    [Space]
    public CapsuleCollider coll;
    public Rigidbody rb;
    public Transform sprite;
    public Animator anim;
    public NavMeshAgent navMeshAgent;
    public LayerMask playerMask,groundMask;
    public Transform player;
    public float myScale;
    public bool dead;
    public bool playerInSightRange, playerInAttackRange;

    [Space]
    [Header("Patrolling")]
    [SerializeField] float walkPointRange;
    Vector3 walkPoint;
    bool walkPointSet;

    [Space]
    [Header("States")]
    [SerializeField] float sightRange; 
    [SerializeField] float  attackRange;

    public int Health {
        get {
            return health;
        }
        set {
            health = value;
            if (health <= 0) {
                StartCoroutine(Death());
            } else {
                anim.SetTrigger("TakeHit");
            }
        }
    }

    void Start()
    {
    }

    IEnumerator Death() {
        dead = true;
        SpawnCoins();
        anim.SetFloat("Move", 0);
        anim.SetTrigger("Death");
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
            c.GetComponent<Rigidbody>().AddForce(force * coinSpreadForceMultiplier,ForceMode.Impulse);
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
                if (!Controller.Instance.IsPlayerDead) {
                    if (playerInSightRange && !playerInAttackRange) ChasePlayer();
                    if (playerInSightRange && playerInAttackRange) AttackPlayer();
                }
            }
        } else {
            navMeshAgent.SetDestination(transform.position);
        }
        
    }

    public virtual void Patroling() {
        if (!walkPointSet) SearchWalkPoint();
        if(navMeshAgent.speed < 0.1f) SearchWalkPoint();
        if (walkPointSet) {
            navMeshAgent.SetDestination(walkPoint);
            navMeshAgent.speed = patrollingSpeed;
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    public virtual void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, walkPointRange, groundMask))
            walkPointSet = true;
    }



    public virtual void ChasePlayer() {
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = chasingSpeed;
    }

    public virtual void AttackPlayer() {

    }

    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, sightRange);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, walkPointRange);
    //}

    public void Damage(int damage, Vector3 pos) {
        if (!dead) {
            Health -= (int)damage;
            Vector3 dir = transform.position - pos;
            dir.Normalize();
            rb.AddForce(dir * 4, ForceMode.Impulse);
        }
    }
}
