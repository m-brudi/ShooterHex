using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEntry : MonoBehaviour
{
    public Transform spawnPos;

    bool collide = true;

    private void Start() {
        Controller.Instance.Mines.Add(this);
    }

    public void IgnoreTrigger() {
        collide = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>()){
            if (collide) Controller.Instance.MovePlayerToOtherMine(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<PlayerController>()) {
            collide = true;
        }
    }
}
