using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDamageable
{
    [SerializeField] Transform sprite;
    [SerializeField] Animator anim;
    Rigidbody rb;
    bool collected;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            if (!collected) {
                anim.SetTrigger("collected");
                StartCoroutine(DelayDestroy());
            }
        }
    }

    IEnumerator DelayDestroy() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void Damage(float damage, Vector3 pos) {
        Vector3 dir = transform.position - pos;
        dir.Normalize();
        rb.AddForce(dir * damage, ForceMode.Impulse);
    }
}
