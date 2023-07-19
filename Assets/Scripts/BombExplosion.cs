using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    [SerializeField] Transform sprite;
    void Start()
    {
        //sprite.LookAt(Camera.main.transform, transform.up);
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy() {
        //yield return new WaitForSeconds(.5f);
        DoDamage();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void DoDamage() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4);
        foreach (var hitCollider in hitColliders) {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            damageable?.Damage(30, transform.position);
        }
    }
}
