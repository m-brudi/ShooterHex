using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FloatingText : MonoBehaviour
{
    Vector3 offset = new(0, 10, 0);
    float destroyTime = .5f;
    float randomize = .3f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.position += offset;
        transform.DORotate(new(54, 0, 0),0);
        transform.DOScale(0, destroyTime).SetEase(Ease.InQuint);

        transform.localPosition += new Vector3(
            Random.Range(-randomize,randomize),
            Random.Range(-randomize,randomize),
            Random.Range(-randomize,randomize)
            );
    }
}
