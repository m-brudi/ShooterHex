using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bulletImpact;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10;
    [SerializeField] int damage;
    SphereCollider coll;

    public void Setup(Vector3 dir) {
        coll = GetComponent<SphereCollider>();
        transform.DOScale(1, 0.2f);
        Vector3 direction = (dir - Controller.Instance.PlayerTransform.position).normalized;
        Vector3 force = direction * speed;
        rb.AddForce(force, ForceMode.VelocityChange);
        transform.rotation = Quaternion.LookRotation(direction);
        StartCoroutine(DestroyAfterSomeTime(5));
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag != "Player") {
            coll.enabled = false;
            sr.enabled = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable?.Damage(damage, transform.position);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterSomeTime(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}

