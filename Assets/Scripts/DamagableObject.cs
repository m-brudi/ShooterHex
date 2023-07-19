using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamagableObject : MonoBehaviour, IDamageable {
    Rigidbody rb;
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    public void Damage(float damage, Vector3 pos) {
        Vector3 dir = transform.position - pos;
        dir.Normalize();
        rb.AddForce(dir * damage, ForceMode.Impulse);
    }
}
