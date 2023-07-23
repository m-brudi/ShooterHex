using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour,IDamageable
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject explosion;
    [SerializeField] Transform sprite;
    Rigidbody rb;
    public bool collected = false;
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            if (!collected) {
                Destroy(gameObject);
            }
        }
    }

    public void Plant() {
        collected = true;
        anim.SetTrigger("explode");
        StartCoroutine(Explode());
    }
    IEnumerator Explode() {
        yield return new WaitForSeconds(2);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Damage(int damage, Vector3 pos) {
        Vector3 dir = transform.position - pos;
        dir.Normalize();
        rb.AddForce(dir * damage, ForceMode.Impulse);
    }
}
