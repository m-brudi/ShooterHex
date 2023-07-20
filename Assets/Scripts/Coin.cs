using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDamageable
{
    [SerializeField] Transform sprite;
    [SerializeField] Animator anim;
    [SerializeField] CapsuleCollider coll;
    Rigidbody rb;
    bool collected;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        //StartCoroutine(TurnOnCollider());
    }
    private void OnTriggerEnter(Collider collision) {
        if (collision.GetComponent<PlayerController>()) {
            if (!collected) {
                coll.enabled = false;
                anim.SetTrigger("collected");
                Controller.Instance.Coins++;
                StartCoroutine(DelayDestroy());
            }
        }
    }  

    IEnumerator TurnOnCollider() {
        yield return new WaitForSeconds(.5f);
        coll.enabled = true;
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
