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

    public void Setup(Vector3 dir) {
        transform.DOScale(1, 0.2f);
        Vector3 direction = (dir - Controller.Instance.PlayerTransform.position).normalized;
        Vector3 force = direction * speed;
        rb.AddForce(force, ForceMode.VelocityChange);
        transform.rotation = Quaternion.LookRotation(direction);
        StartCoroutine(DestroyAfterSomeTime(5));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Player") {
            sr.enabled = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            GameObject imp = Instantiate(bulletImpact, sr.transform.position, transform.rotation);
            Destroy(imp, 1);
            StartCoroutine(DestroyAfterSomeTime(1));
        }
    }

    IEnumerator DestroyAfterSomeTime(float time) {
        yield return new WaitForSeconds(time);
      
        Destroy(gameObject);
    }

}

