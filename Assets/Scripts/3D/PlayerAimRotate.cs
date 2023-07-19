using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimRotate : MonoBehaviour
{
    [Range(0, 1f)]
    [SerializeField] float weight = 1f;
    [Range(0, 1f)]
    [SerializeField] float bodyWeight = .5f;
    [Range(0, 1f)]
    [SerializeField] float headWeight = 1f;
    [Range(0, 1f)]
    [SerializeField] float eyesWeight = .5f;
    [Range(0, 1f)]
    [SerializeField] float clampWeight = .5f;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) {
        animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, Mathf.Infinity)) {
            Vector3 target = new Vector3(hitData.point.x, transform.position.y, hitData.point.z);
            animator.SetLookAtPosition(target);
        }
    }
}
