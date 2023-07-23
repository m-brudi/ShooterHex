using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bulletImpact;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Rigidbody rb;
    [SerializeField] Material shotByPlayerMat;
    [SerializeField] Material shotByEnemyMat;
    [SerializeField] float speed = 10;
    [SerializeField] int damage;
    SphereCollider coll;
    [SerializeField] LayerMask layerToNotCollide;
    bool allSet = false;
    private void Start() {
        Controller.hexMode += DeleteOnHexView;
    }

    public void Setup(Vector3 dir, Vector3 origin, int dmg, bool shotByPlayer) {
        StartCoroutine(DestroyAfterSomeTime(5));
        damage = dmg;
        coll = GetComponent<SphereCollider>();
        transform.DOScale(1, 0.2f);
        sr.material = shotByPlayer ? shotByPlayerMat : shotByEnemyMat;
        Vector3 direction = (dir - origin).normalized;
        Vector3 force = direction * speed;
        rb.AddForce(force, ForceMode.VelocityChange);
        transform.rotation = Quaternion.LookRotation(direction);
        if (shotByPlayer) {
            layerToNotCollide = 7; //playerLayer
        } else {
            layerToNotCollide = 8; //enemyLayer
        }
        allSet = true;
    }

    private void OnCollisionEnter(Collision other) {
        if (allSet) {
            if (other.gameObject.layer != layerToNotCollide) {
                coll.enabled = false;
                sr.enabled = false;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
                damageable?.Damage(damage, transform.position);
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy() {
        Controller.hexMode -= DeleteOnHexView;
    }

    void DeleteOnHexView() {
        StopAllCoroutines();
        
        if (gameObject) Destroy(gameObject);
    }

    IEnumerator DestroyAfterSomeTime(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}

