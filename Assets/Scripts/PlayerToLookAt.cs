using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToLookAt : MonoBehaviour
{
    [SerializeField] Transform player;
    private void LateUpdate() {
        transform.position = player.transform.position;
    }
}
