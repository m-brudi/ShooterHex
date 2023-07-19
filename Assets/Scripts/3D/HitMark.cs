using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMark : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destory());   
    }
    IEnumerator Destory() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
