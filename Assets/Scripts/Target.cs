using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour, IDamageable
{
    private float health = 100f;
    public void Damage(float damage, Vector3 pos) {
        transform.DOPunchScale(0.3f * Vector3.one, 0.3f);
        health -= damage;
        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}
